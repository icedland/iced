program HookCode;

{$APPTYPE CONSOLE}

uses
  SysUtils,

  uIced.Types in '..\..\uIced.Types.pas',
  uIced.Imports in '..\..\uIced.Imports.pas',
  uIced in '..\..\uIced.pas';

{
This example produces the following output:
Original code:
00007FFAC46ACDA4 48895C2410              mov       [rsp+10h],rbx
00007FFAC46ACDA9 4889742418              mov       [rsp+18h],rsi
00007FFAC46ACDAE 55                      push      rbp
00007FFAC46ACDAF 57                      push      rdi
00007FFAC46ACDB0 4156                    push      r14
00007FFAC46ACDB2 488DAC2400FFFFFF        lea       rbp,[rsp-100h]
00007FFAC46ACDBA 4881EC00020000          sub       rsp,200h
00007FFAC46ACDC1 488B0518570A00          mov       rax,[rel 7FFAC47524E0h]
00007FFAC46ACDC8 4833C4                  xor       rax,rsp
00007FFAC46ACDCB 488985F0000000          mov       [rbp+0F0h],rax
00007FFAC46ACDD2 4C8B052F240A00          mov       r8,[rel 7FFAC474F208h]
00007FFAC46ACDD9 488D05787C0400          lea       rax,[rel 7FFAC46F4A58h]
00007FFAC46ACDE0 33FF                    xor       edi,edi

Original + patched code:
00007FFAC46ACDA4 48B8F0DEBC9A78563412    mov       rax,123456789ABCDEF0h
00007FFAC46ACDAE FFE0                    jmp       rax
00007FFAC46ACDB0 4156                    push      r14
00007FFAC46ACDB2 488DAC2400FFFFFF        lea       rbp,[rsp-100h]
00007FFAC46ACDBA 4881EC00020000          sub       rsp,200h
00007FFAC46ACDC1 488B0518570A00          mov       rax,[rel 7FFAC47524E0h]
00007FFAC46ACDC8 4833C4                  xor       rax,rsp
00007FFAC46ACDCB 488985F0000000          mov       [rbp+0F0h],rax
00007FFAC46ACDD2 4C8B052F240A00          mov       r8,[rel 7FFAC474F208h]
00007FFAC46ACDD9 488D05787C0400          lea       rax,[rel 7FFAC46F4A58h]
00007FFAC46ACDE0 33FF                    xor       edi,edi

Moved code:
00007FFAC48ACDA4 48895C2410              mov       [rsp+10h],rbx
00007FFAC48ACDA9 4889742418              mov       [rsp+18h],rsi
00007FFAC48ACDAE 55                      push      rbp
00007FFAC48ACDAF 57                      push      rdi
00007FFAC48ACDB0 E9FBFFDFFF              jmp       00007FFAC46ACDB0h
}

const
  EXAMPLE_CODE_BITNESS        = bt64;
  HEXBYTES_COLUMN_BYTE_LENGTH = 12;

procedure Disassemble( Data : PByte; Size : Cardinal; RIP : UInt64 );
var
  Instruction : TInstruction;
  sOutput     : String;
  start_index : Integer;
  i           : Integer;
begin
  Iced.Decoder.Bitness := EXAMPLE_CODE_BITNESS;

  {$IF CompilerVersion < 23}{$RANGECHECKS OFF}{$IFEND} // RangeCheck might cause Internal-Error C1118
  Iced.Decoder.SetData( Data, Size, RIP, doNONE );
  {$IF CompilerVersion < 23}{$RANGECHECKS ON}{$IFEND} // RangeCheck might cause Internal-Error C1118

  // Formatters: Masm*, Nasm*, Gas* (AT&T) and Intel* (XED).
  // For fastest code, see `SpecializedFormatter` which is ~3.3x faster. Use it if formatting
  // speed is more important than being able to re-assemble formatted instructions.
  Iced.Formatter.FormatterType := ftNasm;

  // Change some options, there are many more
//  Iced.Formatter.DigitSeparator        := '`';
  Iced.Formatter.FirstOperandCharIndex := 10;

  while Iced.Decoder.CanDecode do
    begin
    Iced.Decoder.Decode( Instruction );

    // Eg. "00007FFAC46ACDB2 488DAC2400FFFFFF     lea       rbp,[rsp-100h]"

    // Hex
    {$IF CompilerVersion < 23}{$RANGECHECKS OFF}{$IFEND} // RangeCheck might cause Internal-Error C1118
    start_index := instruction.RIP-RIP;
    {$IF CompilerVersion < 23}{$RANGECHECKS ON}{$IFEND} // RangeCheck might cause Internal-Error C1118
    sOutput := '';
    for i := start_index to start_index+Instruction.len-1 do
      begin
      sOutput := sOutput + IntToHex( Data^, 2 );
      Inc( Data );
      end;

    if ( Instruction.len < HEXBYTES_COLUMN_BYTE_LENGTH ) then
      begin
      for i := 0 to HEXBYTES_COLUMN_BYTE_LENGTH-Instruction.len-1 do
        sOutput := sOutput + '  '
      end;

    sOutput := Format( '%.16x ', [ instruction.RIP ] ) + sOutput + Iced.Formatter.FormatToString( Instruction );

    WriteLn( sOutput );
    end;

  WriteLn( '' );    
end;

const
  EXAMPLE_CODE_RIP                       = $00007FFAC46ACDA4;
var
  EXAMPLE_CODE : Array [ 0..61 ] of Byte = ( $48, $89, $5C, $24, $10, $48, $89, $74, $24, $18, $55, $57, $41, $56, $48, $8D,
                                             $AC, $24, $00, $FF, $FF, $FF, $48, $81, $EC, $00, $02, $00, $00, $48, $8B, $05,
                                             $18, $57, $0A, $00, $48, $33, $C4, $48, $89, $85, $F0, $00, $00, $00, $4C, $8B,
                                             $05, $2F, $24, $0A, $00, $48, $8D, $05, $78, $7C, $04, $00, $33, $FF
                                           );
const
  // In 64-bit mode, we need 12 bytes to jump to any address:
  //      mov rax,imm64   // 10
  //      jmp rax         // 2
  // We overwrite rax because it's probably not used by the called function.
  // In 32-bit mode, a normal JMP is just 5 bytes
  required_bytes = 10+2;
var
  YOUR_FUNC : UInt64 = UInt64( $123456789ABCDEF0 ); // Address of your code
var
  Instruction            : TInstruction;
  Instructions           : TInstructionList;
  sOutput                : String;
  start_index            : Integer;
  i                      : Integer;
  total_bytes            : Cardinal;
  relocated_base_address : UInt64;
  Results                : TBlockEncoderResult;
  _target                : UInt64;
begin
  if NOT IsInitDLL then
    begin
    WriteLn( 'Library not loaded.' );
    WriteLn( 'Press enter to exit.' );
    ReadLn;
    Exit;
    end;

  WriteLn( 'Original code:' );
  {$IF CompilerVersion < 23}{$RANGECHECKS OFF}{$IFEND} // RangeCheck might cause Internal-Error C1118
  Disassemble( @EXAMPLE_CODE[ 0 ], Length( EXAMPLE_CODE ), EXAMPLE_CODE_RIP );
  {$IF CompilerVersion < 23}{$RANGECHECKS ON}{$IFEND} // RangeCheck might cause Internal-Error C1118

  Instructions := TInstructionList.Create;

  {$IF CompilerVersion < 23}{$RANGECHECKS OFF}{$IFEND} // RangeCheck might cause Internal-Error C1118
  Iced.Decoder.SetData( @EXAMPLE_CODE[ 0 ], Length( EXAMPLE_CODE ), EXAMPLE_CODE_RIP, doNONE );
  {$IF CompilerVersion < 23}{$RANGECHECKS ON}{$IFEND} // RangeCheck might cause Internal-Error C1118

  total_bytes := 0;
  while Iced.Decoder.CanDecode do
    begin
    Iced.Decoder.Decode( Instruction );
    if NOT Instruction.IsValid then
      begin
      WriteLn( 'Library not loaded.' );
      WriteLn( 'Press enter to exit.' );
      ReadLn;
      Instructions.Free;
      Exit;
      end;

    Instructions.Add( Instruction );
    Inc( total_bytes, Instruction.len );

    if ( total_bytes >= required_bytes ) then
      break;

    case Instruction.FlowControl.FlowControl of
      fcNext : ;

      fcUnconditionalBranch : begin
                              // You could check if it's just jumping forward a few bytes and follow it
                              // but this is a simple example so we'll fail.
                              if ( Instruction.op_kinds[ 0 ].OpKind = okNearBranch64 ) then
                                {$IF CompilerVersion < 23}{$RANGECHECKS OFF}{$IFEND} // RangeCheck might cause Internal-Error C1118
                                  _target := Instruction.NearBranchTarget;
                                {$IF CompilerVersion < 23}{$RANGECHECKS ON}{$IFEND} // RangeCheck might cause Internal-Error C1118

                              WriteLn( 'Not supported by this simple example.' );
                              WriteLn( 'Press enter to exit.' );
                              ReadLn;
                              Instructions.Free;
                              Exit;
                              end;

      fcIndirectBranch,
      fcConditionalBranch,
      fcReturn,
      fcCall,
      fcIndirectCall,
      fcInterrupt,
      fcXbeginXabortXend,
      fcException           : begin
                              WriteLn( 'Not supported by this simple example.' );
                              WriteLn( 'Press enter to exit.' );
                              ReadLn;
                              Instructions.Free;
                              Exit;
                              end;
    end;
    end;

  if ( total_bytes < required_bytes ) OR ( Instructions.Count < 1 ) then
    begin
    WriteLn( 'Not enough bytes!' );
    WriteLn( 'Press enter to exit.' );
    ReadLn;
    Exit;
    end;

  // Create a JMP instruction that branches to the original code, except those instructions
  // that we'll re-encode. We don't need to do it if it already ends in 'ret'
  Instruction := Instructions[ Instructions.Count-1 ];
  if ( Instruction.FlowControl.FlowControl <> fcReturn ) then
    Instructions.Add( Instruction.with_branch( Jmp_rel32_64, Instruction.next_rip ) );

  // Relocate the code to some new location. It can fix short/near branches and
  // convert them to short/near/long forms if needed. This also works even if it's a
  // jrcxz/loop/loopcc instruction which only have short forms.
  //
  // It can currently only fix RIP relative operands if the new location is within 2GB
  // of the target data location.
  //
  // Note that a block is not the same thing as a basic block. A block can contain any
  // number of instructions, including any number of branch instructions. One block
  // should be enough unless you must relocate different blocks to different locations.
  {$IF CompilerVersion < 23}{$RANGECHECKS OFF}{$IFEND} // RangeCheck might cause Internal-Error C1118
  relocated_base_address := EXAMPLE_CODE_RIP + $200000;
  {$IF CompilerVersion < 23}{$RANGECHECKS ON}{$IFEND} // RangeCheck might cause Internal-Error C1118

  Iced.BlockEncoder.Encode( relocated_base_address, Instructions, Results );
  Instructions.free;

  // Patch the original code. Pretend that we use some OS API to write to memory...
  // We could use the BlockEncoder/Encoder for this but it's easy to do yourself too.
  // This is 'mov rax,imm64; jmp rax'
  EXAMPLE_CODE[ 0 ] := $48; // \ 'MOV RAX,imm64'
  EXAMPLE_CODE[ 1 ] := $B8;
  Move( YOUR_FUNC, EXAMPLE_CODE[ 2 ], SizeOf( YOUR_FUNC ) );
  EXAMPLE_CODE[ 10 ] := $FF; // \ JMP RAX
  EXAMPLE_CODE[ 11 ] := $E0;

  // Disassemble it
  WriteLn( 'Original + patched code:' );
  {$IF CompilerVersion < 23}{$RANGECHECKS OFF}{$IFEND} // RangeCheck might cause Internal-Error C1118
  Disassemble( @EXAMPLE_CODE[ 0 ], Length( EXAMPLE_CODE ), EXAMPLE_CODE_RIP );
  {$IF CompilerVersion < 23}{$RANGECHECKS ON}{$IFEND} // RangeCheck might cause Internal-Error C1118

  // Disassemble the moved code
  WriteLn( 'Moved code:' );
  {$IF CompilerVersion < 23}{$RANGECHECKS OFF}{$IFEND} // RangeCheck might cause Internal-Error C1118
  Disassemble( PByte( Results.code_buffer ), Results.code_buffer_len, relocated_base_address );
  {$IF CompilerVersion < 23}{$RANGECHECKS ON}{$IFEND} // RangeCheck might cause Internal-Error C1118

  WriteLn( 'Press enter to exit.' );
  ReadLn;

end.
