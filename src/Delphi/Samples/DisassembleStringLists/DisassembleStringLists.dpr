program DisassembleStringLists;

{$APPTYPE CONSOLE}

uses
  SysUtils,
  Classes,

  uIced.Types in '..\..\uIced.Types.pas',
  uIced.Imports in '..\..\uIced.Imports.pas',
  uIced in '..\..\uIced.pas';

{
This example produces the following output:
00007FFAC46ACDA4 48895C2410           mov       [rsp+10h],rbx
00007FFAC46ACDA9 4889742418           mov       [rsp+18h],rsi
00007FFAC46ACDAE 55                   push      rbp
00007FFAC46ACDAF 57                   push      rdi
00007FFAC46ACDB0 4156                 push      r14
00007FFAC46ACDB2 488DAC2400FFFFFF     lea       rbp,[rsp-100h]
00007FFAC46ACDBA 4881EC00020000       sub       rsp,200h
00007FFAC46ACDC1 488B0518570A00       mov       rax,[rel 7FFA`C475`24E0h]
00007FFAC46ACDC8 4833C4               xor       rax,rsp
00007FFAC46ACDCB 488985F0000000       mov       [rbp+0F0h],rax
00007FFAC46ACDD2 4C8B052F240A00       mov       r8,[rel 7FFA`C474`F208h]
00007FFAC46ACDD9 488D05787C0400       lea       rax,[rel 7FFA`C46F`4A58h]
00007FFAC46ACDE0 33FF                 xor       edi,edi
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
var
  Assembly    : TStringList;
  Hex         : TStringList;
  Len         : Byte;
  Offset      : UInt64;
  i, j        : Integer;
  sOutput     : String;
begin
  if NOT IsInitDLL then
    begin
    WriteLn( 'Library not loaded.' );
    WriteLn( 'Press enter to exit.' );
    ReadLn;
    Exit;
    end;

  Iced.Decoder.Bitness := EXAMPLE_CODE_BITNESS;
  // Formatters: Masm*, Nasm*, Gas* (AT&T) and Intel* (XED).
  // For fastest code, see `SpecializedFormatter` which is ~3.3x faster. Use it if formatting
  // speed is more important than being able to re-assemble formatted instructions.
  Iced.Formatter.FormatterType := ftNasm;

  // Change some options, there are many more
  Iced.Formatter.DigitSeparator        := '`';
  Iced.Formatter.FirstOperandCharIndex := 10;

  Assembly := TStringList.Create;
  Hex      := TStringList.Create;
  {$IF CompilerVersion < 23}{$RANGECHECKS OFF}{$IFEND} // RangeCheck might cause Internal-Error C1118
  if ( Iced.Decode( PByte( @EXAMPLE_CODE[ 0 ] ), Cardinal( Length( EXAMPLE_CODE ) ), Assembly, EXAMPLE_CODE_RIP, Hex, doNONE ) = 0 ) then
  {$IF CompilerVersion < 23}{$RANGECHECKS ON}{$IFEND} // RangeCheck might cause Internal-Error C1118
    begin
    WriteLn( 'Decoding failed.' );
    WriteLn( 'Press enter to exit.' );
    ReadLn;
    Exit;
    end;

  {$IF CompilerVersion < 23}{$RANGECHECKS OFF}{$IFEND} // RangeCheck might cause Internal-Error C1118
  Offset := EXAMPLE_CODE_RIP;
  {$IF CompilerVersion < 23}{$RANGECHECKS ON}{$IFEND} // RangeCheck might cause Internal-Error C1118
  for i := 0 to Hex.Count-1 do
    begin
    sOutput := Hex[ i ];
    Len     := Length( sOutput ) div 2;
    if ( Len < HEXBYTES_COLUMN_BYTE_LENGTH ) then
      begin
      for j := 0 to HEXBYTES_COLUMN_BYTE_LENGTH-Len-1 do
        sOutput := sOutput + '  '
      end;

    // Eg. "00007FFAC46ACDB2 488DAC2400FFFFFF     lea       rbp,[rsp-100h]"
    sOutput := Format( '%.16x ', [ Offset ] ) + sOutput + Assembly[ i ];
    Inc( Offset, Len );

    WriteLn( sOutput );
    end;
  Assembly.Free;
  Hex.Free;

  WriteLn( 'Press enter to exit.' );
  ReadLn;

end.
