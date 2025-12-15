// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#include "iced_x86/block_encoder.hpp"
#include "iced_x86/instruction_info.hpp"
#include <algorithm>
#include <unordered_map>
#include <optional>

namespace iced_x86 {

// ============================================================================
// BlockEncoder Implementation
// ============================================================================

BlockEncoder::BlockEncoder( uint32_t bitness, BlockEncoderOptions::Value options ) noexcept
	: bitness_( bitness )
	, options_( options )
	, encoder_( bitness )
{
}

std::expected<BlockEncoderResult, std::string> BlockEncoder::encode(
	uint32_t bitness,
	std::span<const Instruction> instructions,
	uint64_t rip,
	BlockEncoderOptions::Value options
) noexcept {
	BlockEncoder encoder( bitness, options );
	return encoder.encode_block( instructions, rip );
}

std::expected<BlockEncoderResult, std::string> BlockEncoder::encode(
	uint32_t bitness,
	const InstructionBlock& block,
	BlockEncoderOptions::Value options
) noexcept {
	return encode( bitness, block.instructions, block.rip, options );
}

std::expected<std::vector<BlockEncoderResult>, std::string> BlockEncoder::encode(
	uint32_t bitness,
	std::span<const InstructionBlock> blocks,
	BlockEncoderOptions::Value options
) noexcept {
	std::vector<BlockEncoderResult> results;
	results.reserve( blocks.size() );
	
	for ( const auto& block : blocks ) {
		auto result = encode( bitness, block, options );
		if ( !result ) {
			return std::unexpected( result.error() );
		}
		results.push_back( std::move( *result ) );
	}
	
	return results;
}

std::expected<BlockEncoderResult, std::string> BlockEncoder::encode_block(
	std::span<const Instruction> instructions,
	uint64_t rip
) noexcept {
	BlockEncoderResult result;
	result.rip = rip;
	
	bool return_relocs = ( options_ & BlockEncoderOptions::RETURN_RELOC_INFOS ) != 0;
	bool return_offsets = ( options_ & BlockEncoderOptions::RETURN_NEW_INSTRUCTION_OFFSETS ) != 0;
	bool return_constants = ( options_ & BlockEncoderOptions::RETURN_CONSTANT_OFFSETS ) != 0;
	bool dont_fix_branches = ( options_ & BlockEncoderOptions::DONT_FIX_BRANCHES ) != 0;
	
	if ( return_offsets ) {
		result.new_instruction_offsets.reserve( instructions.size() );
	}
	if ( return_constants ) {
		result.constant_offsets.reserve( instructions.size() );
	}
	
	// Copy instructions so we can modify them
	std::vector<Instruction> working_instructions( instructions.begin(), instructions.end() );
	
	// Build a map of label IDs to instruction indices
	// Instructions use ip() to store their label ID (if labeled)
	std::unordered_map<uint64_t, std::size_t> label_to_index;
	for ( std::size_t i = 0; i < working_instructions.size(); ++i ) {
		uint64_t label_id = working_instructions[i].ip();
		if ( label_id != 0 ) {
			label_to_index[label_id] = i;
		}
	}
	
	// Calculate initial instruction sizes and positions
	struct InstrInfo {
		uint64_t ip;
		std::size_t size;
		std::size_t buffer_offset;
		bool is_branch;
		bool needs_fixup;
	};
	std::vector<InstrInfo> instr_infos;
	instr_infos.reserve( working_instructions.size() );
	
	// Helper to check if an instruction is a branch
	auto is_branch_instr = []( const Instruction& instr ) {
		return InstructionExtensions::is_jcc_short_or_near( instr ) ||
		       InstructionExtensions::is_jmp_short_or_near( instr ) ||
		       InstructionExtensions::is_call_near( instr );
	};
	
	// Helper to resolve a label ID to an instruction index
	auto resolve_label = [&label_to_index]( uint64_t label_id ) -> std::optional<std::size_t> {
		auto it = label_to_index.find( label_id );
		if ( it != label_to_index.end() ) {
			return it->second;
		}
		return std::nullopt;
	};
	
	// First pass - estimate sizes (without accurate branch targets)
	// We use a dummy encoder to estimate sizes
	Encoder size_encoder( bitness_ );
	uint64_t current_ip = rip;
	
	for ( std::size_t i = 0; i < working_instructions.size(); ++i ) {
		auto& instr = working_instructions[i];
		InstrInfo info;
		info.ip = current_ip;
		info.buffer_offset = 0;
		info.is_branch = is_branch_instr( instr );
		info.needs_fixup = false;
		
		// For branch instructions, we need to temporarily set a target to get size
		// Use the current IP + some offset to get a reasonable size estimate
		if ( info.is_branch ) {
			// This is just for size estimation - we'll fix the actual target later
			instr.set_near_branch64( current_ip + 32 ); // Arbitrary nearby target
		}
		
		auto encode_result = size_encoder.encode( instr, current_ip );
		if ( !encode_result ) {
			// If encoding fails here, it might be due to the dummy target
			// Use a default size for branches
			info.size = info.is_branch ? 5 : 1;
		} else {
			info.size = *encode_result;
		}
		
		current_ip += info.size;
		instr_infos.push_back( info );
	}
	
	// Now we have estimated positions for all instructions
	// Resolve branch targets to actual addresses
	for ( std::size_t i = 0; i < working_instructions.size(); ++i ) {
		if ( !instr_infos[i].is_branch ) continue;
		
		auto& instr = working_instructions[i];
		uint64_t target_label = instr.near_branch_target();
		
		// Check if this looks like a label ID (small number) vs actual address
		// Label IDs are typically 1, 2, 3, etc. while addresses are large
		if ( target_label > 0 && target_label < 0x10000 ) {
			auto target_idx = resolve_label( target_label );
			if ( target_idx.has_value() ) {
				// Resolve to the instruction's actual IP
				uint64_t resolved_target = instr_infos[*target_idx].ip;
				instr.set_near_branch64( resolved_target );
			}
			// If label not found, leave target as-is (will fail encoding with helpful error)
		}
	}
	
	// Second pass - check if short branches need to become near branches
	if ( !dont_fix_branches ) {
		bool sizes_changed = true;
		int iteration = 0;
		const int max_iterations = 10; // Prevent infinite loops
		
		while ( sizes_changed && iteration < max_iterations ) {
			sizes_changed = false;
			++iteration;
			
			for ( std::size_t i = 0; i < working_instructions.size(); ++i ) {
				if ( !instr_infos[i].is_branch ) continue;
				
				auto& instr = working_instructions[i];
				
				if ( InstructionExtensions::is_jcc_short( instr ) || 
				     InstructionExtensions::is_jmp_short( instr ) ) {
					
					uint64_t target = instr.near_branch_target();
					uint64_t next_ip = instr_infos[i].ip + instr_infos[i].size;
					int64_t displacement = static_cast<int64_t>( target ) - static_cast<int64_t>( next_ip );
					
					if ( displacement < -128 || displacement > 127 ) {
						// Convert short to near
						if ( InstructionExtensions::to_near_branch( instr ) ) {
							instr_infos[i].needs_fixup = true;
							sizes_changed = true;
							
							// Recalculate sizes from this point on
							Encoder resize_encoder( bitness_ );
							uint64_t new_ip = instr_infos[i].ip;
							for ( std::size_t j = i; j < working_instructions.size(); ++j ) {
								instr_infos[j].ip = new_ip;
								auto result = resize_encoder.encode( working_instructions[j], new_ip );
								if ( result ) {
									instr_infos[j].size = *result;
								}
								new_ip += instr_infos[j].size;
							}
							
							// Update branch targets after size changes
							for ( std::size_t j = 0; j < working_instructions.size(); ++j ) {
								if ( !instr_infos[j].is_branch ) continue;
								auto& br_instr = working_instructions[j];
								uint64_t br_target = br_instr.near_branch_target();
								
								// Re-resolve labels since positions changed
								if ( br_target > 0 && br_target < 0x10000 ) {
									auto target_idx = resolve_label( br_target );
									if ( target_idx.has_value() ) {
										br_instr.set_near_branch64( instr_infos[*target_idx].ip );
									}
								}
							}
							
							break; // Restart the loop
						}
					}
				}
			}
		}
	}
	
	// Final pass - actual encoding
	encoder_.set_buffer( {} );
	current_ip = rip;
	
	for ( std::size_t i = 0; i < working_instructions.size(); ++i ) {
		const auto& instr = working_instructions[i];
		instr_infos[i].ip = current_ip;
		instr_infos[i].buffer_offset = encoder_.buffer().size();
		
		auto encode_result = encoder_.encode( instr, current_ip );
		if ( !encode_result ) {
			return std::unexpected( encode_result.error().message );
		}
		
		instr_infos[i].size = *encode_result;
		current_ip += *encode_result;
	}
	
	// Collect results
	result.code_buffer = encoder_.take_buffer();
	
	if ( return_offsets ) {
		for ( std::size_t i = 0; i < instr_infos.size(); ++i ) {
			if ( instr_infos[i].needs_fixup ) {
				result.new_instruction_offsets.push_back( UINT32_MAX );
			} else {
				result.new_instruction_offsets.push_back( 
					static_cast<uint32_t>( instr_infos[i].buffer_offset ) 
				);
			}
		}
	}
	
	if ( return_constants ) {
		for ( std::size_t i = 0; i < instructions.size(); ++i ) {
			result.constant_offsets.push_back( ConstantOffsets{} );
		}
	}
	
	if ( return_relocs && bitness_ == 64 ) {
		// Check for 64-bit absolute addresses that need relocation
		// This is a simplified implementation
	}
	
	return result;
}

bool BlockEncoder::try_encode_instruction( const Instruction& instr, uint64_t ip ) noexcept {
	auto result = encoder_.encode( instr, ip );
	if ( !result ) {
		error_message_ = result.error().message;
		return false;
	}
	return true;
}

} // namespace iced_x86
