program DisassembleGlobal;

{$APPTYPE CONSOLE}

uses
  madExcept,
  madLinkDisAsm,
  madListHardware,
  madListProcesses,
  madListModules,
  SysUtils,

  uIced.Types in '..\..\uIced.Types.pas',
  uIced.Imports in '..\..\uIced.Imports.pas',
  uIced in '..\..\uIced.pas';

{
This example produces the following output:
00001248FC840000 55                push    %rbp
00001248FC840001 57                push    %rdi
00001248FC840002 56                push    %rsi
00001248FC840003 4881EC50000000    sub     $0x50,%rsp
00001248FC84000A C5F877            vzeroupper
00001248FC84000D 488D6C2460        lea     0x60(%rsp),%rbp
00001248FC840012 488BF1            mov     %rcx,%rsi
00001248FC840015 488D7DC8          lea     -0x38(%rbp),%rdi
00001248FC840019 B90A000000        mov     $0xA,%ecx
00001248FC84001E 33C0              xor     %eax,%eax
00001248FC840020 F3AB              rep stos %eax,(%rdi)
00001248FC840022 4881FE78563412    cmp     $0x12345678,%rsi
00001248FC840029 7501              jne     0x00001248FC84002C
00001248FC84002B 90                nop
00001248FC84002C 4533FF            xor     %r15d,%r15d
00001248FC84002F 4C8D3501000000    lea     0x1248FC840037,%r14
00001248FC840036 90                nop
00001248FC840037 12345678          .byte   0x12,0x34,0x56,0x78
}
const
  HEXBYTES_COLUMN_BYTE_LENGTH       = 16;
  EXAMPLE_CODE_BITNESS              = bt64;
  EXAMPLE_CODE_RIP                  = $00001248FC840000;
  raw_data : Array [ 0..3 ] of Byte = ( $12, $34, $56, $78 );
var
  Instruction   : TInstruction;
  MemoryOperand : TMemoryOperand;
  Instructions  : TInstructionList;
  Results       : TBlockEncoderResult;
  Data          : PByte;
  S             : String;
  i             : Integer;
  label1        : UInt64;
  data1         : UInt64;
  pInstruction  : PAnsiChar;
begin
  if NOT IsInitDLL then
    begin
    WriteLn( 'Library not loaded.' );
    WriteLn( 'Press enter to exit.' );
    ReadLn;
    Exit;
    end;

  Instructions := TInstructionList.Create;
  // All created instructions get an IP of 0. The label id is just an IP.
  // The branch instruction's *target* IP should be equal to the IP of the
  // target instruction.
  {$IF CompilerVersion < 23}{$RANGECHECKS OFF}{$IFEND} // RangeCheck might cause Internal-Error C1118
  label1 := Instructions.CreateLabel;
  {$IF CompilerVersion < 23}{$RANGECHECKS ON}{$IFEND} // RangeCheck might cause Internal-Error C1118

  Instructions.Add( Instruction.with1( Push_r64, RBP ) );
  Instructions.Add( Instruction.with1( Push_r64, RDI ) );
  Instructions.Add( Instruction.with1( Push_r64, RSI ) );
  Instructions.Add( Instruction.with2( Sub_rm64_imm32, RSP, Cardinal( $50 ) ) );
  Instructions.Add( Instruction.with_( VEX_Vzeroupper ) );
  Instructions.Add( Instruction.with2(
      Lea_r64_m,
      RBP,
      MemoryOperand.with_base_displ( RSP, $60 )
   ) );
  Instructions.Add( Instruction.with2( Mov_r64_rm64, RSI, RCX ) );
  Instructions.Add( Instruction.with2(
      Lea_r64_m,
      RDI,
      MemoryOperand.with_base_displ( RBP, -$38 )
   ) );
  Instructions.Add( Instruction.with2( Mov_r32_imm32, ECX, Cardinal( $0A ) ) );
  Instructions.Add( Instruction.with2( Xor_r32_rm32, EAX, EAX ) );
  Instructions.Add( Instruction.with_rep_stosd( Cardinal( EXAMPLE_CODE_BITNESS ) ) );
  Instructions.Add( Instruction.with2( Cmp_rm64_imm32, RSI, Cardinal( $12345678 ) ) );
  // Create a branch instruction that references label1
  Instructions.Add( Instruction.with_branch( Jne_rel32_64, label1 ) );
  Instructions.Add( Instruction.with_( Nopd ) );
  // Add the instruction that is the target of the branch
  Instructions.Add( Instruction.with2( Xor_r32_rm32, R15D, R15D ), label1 );

  // Create an instruction that accesses some data using an RIP relative memory operand
  {$IF CompilerVersion < 23}{$RANGECHECKS OFF}{$IFEND} // RangeCheck might cause Internal-Error C1118
  data1 := Instructions.CreateLabel;
  {$IF CompilerVersion < 23}{$RANGECHECKS ON}{$IFEND} // RangeCheck might cause Internal-Error C1118
  Instructions.Add( Instruction.with2(
        Lea_r64_m,
        R14,
        MemoryOperand.with_base_displ( RIP, Int64( data1 ) )
   ) );
  Instructions.Add( Instruction.with_( Nopd ) );
  Instructions.Add( Instruction.with_declare_byte( @raw_data[ 0 ], Length( raw_data ) ), data1 );

  // Use BlockEncoder to encode a block of instructions. This block can contain any
  // number of branches and any number of instructions. It does support encoding more
  // than one block but it's rarely needed.
  // It uses Encoder to encode all instructions.
  // If the target of a branch is too far away, it can fix it to use a longer branch.
  // This can be disabled by enabling some BlockEncoderOptions flags.

  Iced.BlockEncoder.Bitness := EXAMPLE_CODE_BITNESS;
  {$IF CompilerVersion < 23}{$RANGECHECKS OFF}{$IFEND} // RangeCheck might cause Internal-Error C1118
  Iced.BlockEncoder.Encode( EXAMPLE_CODE_RIP, Instructions, Results );
  {$IF CompilerVersion < 23}{$RANGECHECKS ON}{$IFEND} // RangeCheck might cause Internal-Error C1118
  Instructions.Free;

  // Now disassemble the encoded instructions. Note that the 'jmp near'
  // instruction was turned into a 'jmp short' instruction because we
  // didn't disable branch optimizations.
  Iced.Decoder.Bitness := EXAMPLE_CODE_BITNESS;

  Data := PByte( Results.code_buffer );
  {$IF CompilerVersion < 23}{$RANGECHECKS OFF}{$IFEND} // RangeCheck might cause Internal-Error C1118
  Iced.Decoder.SetData( PByte( Results.code_buffer ), Results.code_buffer_len-Length( raw_data ), EXAMPLE_CODE_RIP{Results.RIP}, doNONE );
  {$IF CompilerVersion < 23}{$RANGECHECKS ON}{$IFEND} // RangeCheck might cause Internal-Error C1118

  Iced.Formatter.FormatterType         := ftGas;
  Iced.Formatter.FirstOperandCharIndex := 8;

  while Iced.Decoder.CanDecode do
    begin
//    Iced.Decoder.Decode( Instruction );
    Iced.DecodeFormat( Instruction, pInstruction );

    // Assembly
    S := Format( '%.16x ', [ Instruction.RIP ] );

    for i := 0 to Instruction.len-1 do
      begin
      S := S + Format( '%.2x', [ Data^ ] );
      Inc( Data );
      end;

    for i := 0 to HEXBYTES_COLUMN_BYTE_LENGTH-Instruction.len*2+1 do
      S := S + ' ';

//    WriteLn( S + Iced.Formatter.FormatToString( Instruction ) );
    WriteLn( S + string( pInstruction ) );
    end;

//  Data := PByte( Results.code_buffer ); // @raw_data[ 0 ];
//  Inc( Data, Results.code_buffer_len-Length( raw_data ) );
  Instruction.from_declare_byte( Data, Length( raw_data ) );
  Instruction.len := Length( raw_data );
  S := Format( '%.16x ', [ Iced.Decoder.IP ] );
  for i := 0 to Instruction.len-1 do
    begin
    S := S + Format( '%.2x', [ Data^ ] );
    Inc( Data );
    end;
  for i := 0 to HEXBYTES_COLUMN_BYTE_LENGTH-Instruction.len*2+1 do
    S := S + ' ';
  WriteLn( S + Iced.Formatter.FormatToString( Instruction ) );

//  Iced.Decoder.SetIP( Decoder, EXAMPLE_RIP );
//  Iced.Decoder.SetPosition( Decoder, 0 );

  WriteLn( 'Press enter to exit.' );
  ReadLn;

end.
