program GetInstructionInfo;

{$APPTYPE CONSOLE}

uses
  SysUtils,

  uIced.Types in '..\..\uIced.Types.pas',
  uIced.Imports in '..\..\uIced.Imports.pas',
  uIced in '..\..\uIced.pas';

{
This example produces the following output:
00007FFAC46ACDA4 48895C2410 mov [rsp+10h],rbx
    OpCode: o64 89 /r
    Instruction: MOV r/m64, r64
    Encoding: Legacy
    Mnemonic: Mov
    Code: Mov_rm64_r64
    CpuidFeature: X64
    FlowControl: Next
    Displacement offset = 4, size = 1
    Memory Size: 8
    Op0Access: Write
    Op1Access: Read
    Op0: r64_or_mem
    Op1: r64_reg
    Used reg: RSP:Read
    Used reg: RBX:Read
    Used mem: SS:RSP+0x10:None:1:UInt64:Write:Code64:0
00007FFAC46ACDA9 4889742418 mov [rsp+18h],rsi
    OpCode: o64 89 /r
    Instruction: MOV r/m64, r64
    Encoding: Legacy
    Mnemonic: Mov
    Code: Mov_rm64_r64
    CpuidFeature: X64
    FlowControl: Next
    Displacement offset = 4, size = 1
    Memory Size: 8
    Op0Access: Write
    Op1Access: Read
    Op0: r64_or_mem
    Op1: r64_reg
    Used reg: RSP:Read
    Used reg: RSI:Read
    Used mem: SS:RSP+0x18:None:1:UInt64:Write:Code64:0
00007FFAC46ACDAE 55 push rbp
    OpCode: o64 50+ro
    Instruction: PUSH r64
    Encoding: Legacy
    Mnemonic: Push
    Code: Push_r64
    CpuidFeature: X64
    FlowControl: Next
    SP Increment: -8
    Op0Access: Read
    Op0: r64_opcode
    Used reg: RBP:Read
    Used reg: RSP:ReadWrite
    Used mem: SS:RSP+0xFFFFFFFFFFFFFFF8:None:1:UInt64:Write:Code64:0
00007FFAC46ACDAF 57 push rdi
    OpCode: o64 50+ro
    Instruction: PUSH r64
    Encoding: Legacy
    Mnemonic: Push
    Code: Push_r64
    CpuidFeature: X64
    FlowControl: Next
    SP Increment: -8
    Op0Access: Read
    Op0: r64_opcode
    Used reg: RDI:Read
    Used reg: RSP:ReadWrite
    Used mem: SS:RSP+0xFFFFFFFFFFFFFFF8:None:1:UInt64:Write:Code64:0
00007FFAC46ACDB0 4156 push r14
    OpCode: o64 50+ro
    Instruction: PUSH r64
    Encoding: Legacy
    Mnemonic: Push
    Code: Push_r64
    CpuidFeature: X64
    FlowControl: Next
    SP Increment: -8
    Op0Access: Read
    Op0: r64_opcode
    Used reg: R14:Read
    Used reg: RSP:ReadWrite
    Used mem: SS:RSP+0xFFFFFFFFFFFFFFF8:None:1:UInt64:Write:Code64:0
00007FFAC46ACDB2 488DAC2400FFFFFF lea rbp,[rsp-100h]
    OpCode: o64 8D /r
    Instruction: LEA r64, m
    Encoding: Legacy
    Mnemonic: Lea
    Code: Lea_r64_m
    CpuidFeature: X64
    FlowControl: Next
    Displacement offset = 4, size = 4
    Op0Access: Write
    Op1Access: NoMemAccess
    Op0: r64_reg
    Op1: mem
    Used reg: RBP:Write
    Used reg: RSP:Read
00007FFAC46ACDBA 4881EC00020000 sub rsp,200h
    OpCode: o64 81 /5 id
    Instruction: SUB r/m64, imm32
    Encoding: Legacy
    Mnemonic: Sub
    Code: Sub_rm64_imm32
    CpuidFeature: X64
    FlowControl: Next
    Immediate offset = 3, size = 4
    RFLAGS Written: OF, SF, ZF, AF, CF, PF
    RFLAGS Modified: OF, SF, ZF, AF, CF, PF
    Op0Access: ReadWrite
    Op1Access: Read
    Op0: r64_or_mem
    Op1: imm32sex64
    Used reg: RSP:ReadWrite
00007FFAC46ACDC1 488B0518570A00 mov rax,[7FFAC47524E0h]
    OpCode: o64 8B /r
    Instruction: MOV r64, r/m64
    Encoding: Legacy
    Mnemonic: Mov
    Code: Mov_r64_rm64
    CpuidFeature: X64
    FlowControl: Next
    Displacement offset = 3, size = 4
    Memory Size: 8
    Op0Access: Write
    Op1Access: Read
    Op0: r64_reg
    Op1: r64_or_mem
    Used reg: RAX:Write
    Used mem: DS:None+0x7FFAC47524E0:None:1:UInt64:Read:Code64:0
00007FFAC46ACDC8 4833C4 xor rax,rsp
    OpCode: o64 33 /r
    Instruction: XOR r64, r/m64
    Encoding: Legacy
    Mnemonic: Xor
    Code: Xor_r64_rm64
    CpuidFeature: X64
    FlowControl: Next
    RFLAGS Written: SF, ZF, PF
    RFLAGS Cleared: OF, CF
    RFLAGS Undefined: AF
    RFLAGS Modified: OF, SF, ZF, AF, CF, PF
    Op0Access: ReadWrite
    Op1Access: Read
    Op0: r64_reg
    Op1: r64_or_mem
    Used reg: RAX:ReadWrite
    Used reg: RSP:Read
00007FFAC46ACDCB 488985F0000000 mov [rbp+0F0h],rax
    OpCode: o64 89 /r
    Instruction: MOV r/m64, r64
    Encoding: Legacy
    Mnemonic: Mov
    Code: Mov_rm64_r64
    CpuidFeature: X64
    FlowControl: Next
    Displacement offset = 3, size = 4
    Memory Size: 8
    Op0Access: Write
    Op1Access: Read
    Op0: r64_or_mem
    Op1: r64_reg
    Used reg: RBP:Read
    Used reg: RAX:Read
    Used mem: SS:RBP+0xF0:None:1:UInt64:Write:Code64:0
00007FFAC46ACDD2 4C8B052F240A00 mov r8,[7FFAC474F208h]
    OpCode: o64 8B /r
    Instruction: MOV r64, r/m64
    Encoding: Legacy
    Mnemonic: Mov
    Code: Mov_r64_rm64
    CpuidFeature: X64
    FlowControl: Next
    Displacement offset = 3, size = 4
    Memory Size: 8
    Op0Access: Write
    Op1Access: Read
    Op0: r64_reg
    Op1: r64_or_mem
    Used reg: R8:Write
    Used mem: DS:None+0x7FFAC474F208:None:1:UInt64:Read:Code64:0
00007FFAC46ACDD9 488D05787C0400 lea rax,[7FFAC46F4A58h]
    OpCode: o64 8D /r
    Instruction: LEA r64, m
    Encoding: Legacy
    Mnemonic: Lea
    Code: Lea_r64_m
    CpuidFeature: X64
    FlowControl: Next
    Displacement offset = 3, size = 4
    Op0Access: Write
    Op1Access: NoMemAccess
    Op0: r64_reg
    Op1: mem
    Used reg: RAX:Write
00007FFAC46ACDE0 33FF xor edi,edi
    OpCode: o32 33 /r
    Instruction: XOR r32, r/m32
    Encoding: Legacy
    Mnemonic: Xor
    Code: Xor_r32_rm32
    CpuidFeature: INTEL386
    FlowControl: Next
    RFLAGS Cleared: OF, SF, CF
    RFLAGS Set: ZF, PF
    RFLAGS Undefined: AF
    RFLAGS Modified: OF, SF, ZF, AF, CF, PF
    Op0Access: Write
    Op1Access: None
    Op0: r32_reg
    Op1: r32_or_mem
    Used reg: RDI:Write
}

const
//  HEXBYTES_COLUMN_BYTE_LENGTH            = 16; // 30;
  EXAMPLE_CODE_BITNESS                   = bt64;
  EXAMPLE_CODE_RIP                       = $00007FFAC46ACDA4;
  EXAMPLE_CODE : Array [ 0..61 ] of Byte = ( $48, $89, $5C, $24, $10, $48, $89, $74, $24, $18, $55, $57, $41, $56, $48, $8D,
                                             $AC, $24, $00, $FF, $FF, $FF, $48, $81, $EC, $00, $02, $00, $00, $48, $8B, $05,
                                             $18, $57, $0A, $00, $48, $33, $C4, $48, $89, $85, $F0, $00, $00, $00, $4C, $8B,
                                             $05, $2F, $24, $0A, $00, $48, $8D, $05, $78, $7C, $04, $00, $33, $FF
                                           );
var
  Instruction   : TInstruction;
  Offsets       : TConstantOffsets;
  FPU_Info      : TFpuStackIncrementInfo;
  CPUIDFeatures : TCPUIDFeaturesArray;
  OPKinds       : TOPKindsArray;
  Encoding      : TEncodingKind;
  Mnemonic      : TMnemonic;
  FlowControl   : TFlowControl;
  OPKind        : TOpCodeOperandKind;
  MemorySize    : TMemorySize;
//  OPKinds_      : TOPCodeOperandKindArray;
  Info          : TInstructionInfo;
  CC            : TConditionCode;
  RFlags        : TRFlags;
  tOutput       : PAnsiChar;

  S             : String;
  C             : UInt64;
  i             : Integer;
  Data          : PByte;
begin
  if NOT IsInitDLL then
    begin
    WriteLn( 'Library not loaded.' );
    WriteLn( 'Press enter to exit.' );
    ReadLn;
    Exit;
    end;

  Iced.Decoder.Bitness := EXAMPLE_CODE_BITNESS;

  Data := @EXAMPLE_CODE[ 0 ];
  {$IF CompilerVersion < 23}{$RANGECHECKS OFF}{$IFEND} // RangeCheck might cause Internal-Error C1118
  Iced.Decoder.SetData( @EXAMPLE_CODE[ 0 ], Length( EXAMPLE_CODE ), EXAMPLE_CODE_RIP, doNONE );
  {$IF CompilerVersion < 23}{$RANGECHECKS ON}{$IFEND} // RangeCheck might cause Internal-Error C1118

  // Formatters: Masm*, Nasm*, Gas* (AT&T) and Intel* (XED).
  // For fastest code, see `SpecializedFormatter` which is ~3.3x faster. Use it if formatting
  // speed is more important than being able to re-assemble formatted instructions.
  Iced.Formatter.FormatterType := ftMasm;

  while Iced.Decoder.CanDecode do
    begin
    Iced.DecodeFormat( Instruction, tOutput );

    // Assembly
    {$IF CompilerVersion < 23}{$RANGECHECKS OFF}{$IFEND} // RangeCheck might cause Internal-Error C1118
    C := Instruction.next_rip-Instruction.len;
    {$IF CompilerVersion < 23}{$RANGECHECKS ON}{$IFEND} // RangeCheck might cause Internal-Error C1118
    S := Format( '%.16x ', [ C ] );

    for i := 0 to Instruction.len-1 do
      begin
      S := S + Format( '%.2x', [ Data^ ] );
      Inc( Data );
      end;

//    for i := 0 to HEXBYTES_COLUMN_BYTE_LENGTH-Instruction.len*2+1 do
      S := S + ' ';

    // For quick hacks, it's fine to use the Display trait to format an instruction,
    // but for real code, use a formatter, eg. MasmFormatter. See other examples.
//    WriteLn( S + Instruction.Format );

//    Iced.Formatter.Format( Instruction, tOutput );
    WriteLn( S + tOutput );

    // Gets offsets in the instruction of the displacement and immediates and their sizes.
    // This can be useful if there are relocations in the binary. The encoder has a similar
    // method. This method must be called after decode() and you must pass in the last
    // instruction decode() returned.
    Iced.Decoder.GetConstantOffsets( Instruction, Offsets );

//    FillChar( OPKinds_, SizeOf( OPKinds_ ), 0 );
//    Instruction.OpCodeInfo_OPKinds( OPKinds_ );

    WriteLn( '    OpCode: ' + Instruction.OpCodeString );
    WriteLn( '    Instruction: ' + Instruction.InstructionString );
    Encoding := Instruction.Encoding;
    WriteLn( '    Encoding: ' + Encoding.AsString );
    Mnemonic := Instruction.Mnemonic;
    WriteLn( '    Mnemonic: ' + Mnemonic.AsString );
    WriteLn( '    Code: ' + Instruction.code.AsString );

    CPUIDFeatures := Instruction.CPUIDFeatures;
    S := '';
    for i := 0 to CPUIDFeatures.Count-1 do
      begin
      if ( i > 0 ) then
        S := S + 'AND ' + CPUIDFeatures.Entries[ i ].AsString
      else
        S := CPUIDFeatures.Entries[ i ].AsString;
      end;
    WriteLn( '    CpuidFeature: ' + S );

    FlowControl := Instruction.FlowControl;
    WriteLn( '    FlowControl: ' + FlowControl.AsString );

    FPU_Info := Instruction.FPU_StackIncrementInfo;
    if FPU_Info.writes_top then
      begin
      if ( FPU_Info.increment = 0 ) then
        WriteLn( '    FPU TOP: the instruction overwrites TOP' )
      else
        WriteLn( Format( '    FPU TOP inc: %d', [ FPU_Info.increment ] ) );

      if FPU_Info.conditional then
        WriteLn( '    FPU TOP cond write: true' )
      else
        WriteLn( '    FPU TOP cond write: false' );
      end;

    if ( Offsets.displacement_size <> 0 ) then
      WriteLn( Format( '    Displacement offset = %d, size = %d', [ Offsets.displacement_offset, Offsets.displacement_size ] ) );
    if ( Offsets.immediate_size <> 0 ) then
      WriteLn( Format( '    Immediate offset = %d, size = %d', [ Offsets.immediate_offset, Offsets.immediate_size ] ) );
    if ( Offsets.immediate_size2 <> 0 ) then
      WriteLn( Format( '    Immediate #2 offset = %d, size = %d', [ Offsets.immediate_offset2, Offsets.immediate_size2 ] ) );

    if Instruction.IsStackInstruction then
      WriteLn( Format( '    SP Increment: %d', [ Instruction.StackPointerIncrement ] ) );

    CC := Instruction.ConditionCode;
    RFlags := Instruction.RFlags;
    if ( CC.ConditionCode <> cc_None ) then
      WriteLn( Format( '    Condition code: %s', [ CC.AsString ] ) );

    if ( NOT RFlags.Read.IsNone ) OR ( NOT RFlags.Written.IsNone ) OR ( NOT RFlags.Cleared.IsNone ) OR ( NOT RFlags.Set_.IsNone ) OR ( NOT RFlags.Undefined.IsNone ) OR ( NOT RFlags.Modified.IsNone ) then
      begin
      if ( NOT RFlags.Read.IsNone ) then
        WriteLn( '    RFLAGS Read: ' + RFlags.Read.AsString );
      if ( NOT RFlags.Written.IsNone ) then
        WriteLn( '    RFLAGS Written: ' + RFlags.Written.AsString );
      if ( NOT RFlags.Cleared.IsNone ) then
        WriteLn( '    RFLAGS Cleared: ' + RFlags.Cleared.AsString );
      if ( NOT RFlags.Set_.IsNone ) then
        WriteLn( '    RFLAGS Set: ' + RFlags.Set_.AsString );
      if ( NOT RFlags.Undefined.IsNone ) then
        WriteLn( '    RFLAGS Undefined: ' + RFlags.Undefined.AsString );
      if ( NOT RFlags.Modified.IsNone ) then
        WriteLn( '    RFLAGS Modified: ' + RFlags.Modified.AsString );
      end;

    FillChar( OPKinds, SizeOf( OPKinds ), 0 );
    Instruction.OPKinds( OPKinds );
    for i := 0 to OPKinds.Count-1 do
      begin
      if ( OPKinds.Entries[ i ].OpKind = okMemory ) then
        begin
        MemorySize := Instruction.MemorySize;
        if ( MemorySize.Size <> 0 ) then
          WriteLn( '    Memory Size: ' + IntToStr( MemorySize.Size ) );
        break;
        end;
      end;

    Iced.InfoFactory.Info( Instruction, Info );
    for i := 0 to Instruction.OPCount-1 do
      WriteLn( Format( '    Op%dAccess: %s', [ i, Info.op_accesses[ i ].AsString ] ) );

    for i := 0 to Instruction.OpCodeInfo_OPCount-1 do
      begin
      OPKind := Instruction.OpCodeInfo.op_kinds[ i ];
      WriteLn( Format( '    Op%d: %s', [ i, OPKind.AsString ] ) );
      end;

    for i := 0 to Info.used_registers.Count-1 do
      WriteLn( Format( '    Used reg: %s:%s', [ Info.used_registers.Entries[ i ].register_.AsString, Info.used_registers.Entries[ i ].access.AsString ] ) );

    for i := 0 to Info.used_memory_locations.Count-1 do
//      WriteLn( '    Used mem: ' + Info.used_memory_locations.Entries[ i ].AsString );
      WriteLn( Format( '    Used mem: %s:%s+0x%.2x:%s:%d:%s:%s:%s:%d', [
                                                                        Info.used_memory_locations.Entries[ i ].segment.AsString,
                                                                        Info.used_memory_locations.Entries[ i ].base.AsString,
                                                                        Info.used_memory_locations.Entries[ i ].displacement,
                                                                        Info.used_memory_locations.Entries[ i ].index.AsString,
                                                                        Info.used_memory_locations.Entries[ i ].scale,
                                                                        Info.used_memory_locations.Entries[ i ].memory_size.AsString,
                                                                        Info.used_memory_locations.Entries[ i ].access.AsString,
                                                                        Info.used_memory_locations.Entries[ i ].address_size.AsString,
                                                                        Info.used_memory_locations.Entries[ i ].vsib_size
                                                                      ] ) );
    end;
//  Iced.Decoder.SetIP( Decoder, EXAMPLE_RIP );
//  Iced.Decoder.SetPosition( Decoder, 0 );

  WriteLn( 'Press enter to exit.' );
  ReadLn;

end.
