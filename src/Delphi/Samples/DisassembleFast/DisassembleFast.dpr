program DisassembleFast;

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
00007FFAC46ACDA4 48895C2410          mov [rsp+0x10],rbx
00007FFAC46ACDA9 4889742418          mov [rsp+0x18],rsi
00007FFAC46ACDAE 55                  push rbp
00007FFAC46ACDAF 57                  push rdi
00007FFAC46ACDB0 4156                push r14
00007FFAC46ACDB2 488DAC2400FFFFFF    lea rbp,[rsp-0x100]
00007FFAC46ACDBA 4881EC00020000      sub rsp,0x200
00007FFAC46ACDC1 488B0518570A00      mov rax,[0x7FFAC47524E0]
00007FFAC46ACDC8 4833C4              xor rax,rsp
00007FFAC46ACDCB 488985F0000000      mov [rbp+0xF0],rax
00007FFAC46ACDD2 4C8B052F240A00      mov r8,[0x7FFAC474F208]
00007FFAC46ACDD9 488D05787C0400      lea rax,[0x7FFAC46F4A58]
00007FFAC46ACDE0 33FF                xor edi,edi
}

const
  HEXBYTES_COLUMN_BYTE_LENGTH            = 10;
  EXAMPLE_CODE_BITNESS                   = bt64;
  EXAMPLE_CODE_RIP                       = $00007FFAC46ACDA4;
  EXAMPLE_CODE : Array [ 0..61 ] of Byte = ( $48, $89, $5C, $24, $10, $48, $89, $74, $24, $18, $55, $57, $41, $56, $48, $8D,
                                             $AC, $24, $00, $FF, $FF, $FF, $48, $81, $EC, $00, $02, $00, $00, $48, $8B, $05,
                                             $18, $57, $0A, $00, $48, $33, $C4, $48, $89, $85, $F0, $00, $00, $00, $4C, $8B,
                                             $05, $2F, $24, $0A, $00, $48, $8D, $05, $78, $7C, $04, $00, $33, $FF
                                           );

procedure DecodeFormatCallback( const Instruction: TInstruction; Formatted : PAnsiChar; Size : NativeUInt; var Stop : Boolean; UserData : Pointer ); cdecl;
type
  PPByte = ^PByte;
var
  sOutput     : String;
  i           : Integer;
begin
  // Eg. "00007FFAC46ACDB2 488DAC2400FFFFFF     lea       rbp,[rsp-100h]"

  // Hex
  sOutput := '';
  for i := 0 to Instruction.len-1 do
    begin
    sOutput := sOutput + IntToHex( PPByte( UserData )^^, 2 );
    Inc( PPByte( UserData )^ );
    end;

  if ( Instruction.len < HEXBYTES_COLUMN_BYTE_LENGTH ) then
    begin
    for i := 0 to HEXBYTES_COLUMN_BYTE_LENGTH-Instruction.len-1 do
      sOutput := sOutput + '  '
    end;

  WriteLn( Format( '%.16x ', [ instruction.RIP ] ) + sOutput + Formatted );
end;

var
  pData       : PByte;
//  Instruction : TInstruction;
//  pInstruction: PAnsiChar;
//  sOutput     : String;
//  i           : Integer;
begin
  if NOT IsInitDLL then
    begin
    WriteLn( 'Library not loaded.' );
    WriteLn( 'Press enter to exit.' );
    ReadLn;
    Exit;
    end;

  Iced.Decoder.Bitness := EXAMPLE_CODE_BITNESS;

  {$IF CompilerVersion < 23}{$RANGECHECKS OFF}{$IFEND} // RangeCheck might cause Internal-Error C1118
  Iced.Decoder.SetData( @EXAMPLE_CODE[ 0 ], Length( EXAMPLE_CODE ), EXAMPLE_CODE_RIP, doNONE );
  {$IF CompilerVersion < 23}{$RANGECHECKS ON}{$IFEND} // RangeCheck might cause Internal-Error C1118

  // Formatters: Masm*, Nasm*, Gas* (AT&T) and Intel* (XED).
  // For fastest code, see `SpecializedFormatter` which is ~3.3x faster. Use it if formatting
  // speed is more important than being able to re-assemble formatted instructions.

  // If you never create a db/dw/dd/dq 'instruction', we don't need this feature (default).
  Iced.Formatter.Enable_DB_DW_DD_DQ := False;

  // For a few percent faster code, you can also disable this... (default).
  Iced.Formatter.VerifyOutputHasEnoughBytesLeft := False;

  Iced.Formatter.FormatterType := ftSpecialized;

  // Change some options, there are many more
  Iced.Formatter.UseHexPrefix := True;

  pData := @EXAMPLE_CODE[ 0 ];
  Iced.DecodeFormatToEnd( DecodeFormatCallback, @pData );
{
  while Iced.Decoder.CanDecode do
    begin
//    Iced.Decoder.Decode( Instruction );
    Iced.DecodeFormat( Instruction, pInstruction );

    // Eg. "00007FFAC46ACDB2 488DAC2400FFFFFF     lea       rbp,[rsp-100h]"

    // Hex
    sOutput := '';
    for i := 0 to Instruction.len-1 do
      begin
      sOutput := sOutput + IntToHex( pData^, 2 );
      Inc( pData );
      end;

    if ( Instruction.len < HEXBYTES_COLUMN_BYTE_LENGTH ) then
      begin
      for i := 0 to HEXBYTES_COLUMN_BYTE_LENGTH-Instruction.len-1 do
        sOutput := sOutput + '  '
      end;

//    sOutput := Format( '%.16x ', [ Instruction.RIP ] ) + sOutput + Iced.Formatter.FormatToString( Instruction );
    sOutput := Format( '%.16x ', [ Instruction.RIP ] ) + sOutput + string( pInstruction );

    WriteLn( sOutput );
    end;
}

  WriteLn( 'Press enter to exit.' );
  ReadLn;

end.
