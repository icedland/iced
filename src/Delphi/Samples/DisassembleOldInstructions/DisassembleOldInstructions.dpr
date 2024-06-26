program DisassembleOldInstructions;

{$APPTYPE CONSOLE}

uses
  SysUtils,

  uIced.Types in '..\..\uIced.Types.pas',
  uIced.Imports in '..\..\uIced.Imports.pas',
  uIced in '..\..\uIced.pas'; // MS

{
This example produces the following output:
00000000731E0A03 660F1A08  bndmov bnd1,[eax]
00000000731E0A07 0F26DE    mov tr3,esi
00000000731E0A0A 0F3600    rdshr [eax]
00000000731E0A0D 0F39      dmint
00000000731E0A0F 0F7808    svdc [eax],cs
00000000731E0A12 0F3D      cpu_read
00000000731E0A14 0F5808    pmvzb mm1,[eax]
00000000731E0A17 DFFC      frinear
00000000731E0A19 0F3F      altinst
}

const
  HEXBYTES_COLUMN_BYTE_LENGTH            = 5;
  EXAMPLE_CODE_BITNESS                   = bt32;
  EXAMPLE_CODE_RIP                       = $731E0A03;
  EXAMPLE_CODE : Array [ 0..23 ] of Byte = (
                                            // bndmov bnd1,[eax]
                                            $66, $0F, $1A, $08,
                                            // mov tr3,esi
                                            $0F, $26, $DE,
                                            // rdshr [eax]
                                            $0F, $36, $00,
                                            // dmint
                                            $0F, $39,
                                            // svdc [eax],cs
                                            $0F, $78, $08,
                                            // cpu_read
                                            $0F, $3D,
                                            // pmvzb mm1,[eax]
                                            $0F, $58, $08,
                                            // frinear
                                            $DF, $FC,
                                            // altinst
                                            $0F, $3F
                                           );
var
  Instruction : TInstruction;
  sOutput     : String;
  start_index : Integer;
  i           : Integer;
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
  Iced.Decoder.SetData( @EXAMPLE_CODE[ 0 ], Length( EXAMPLE_CODE ), EXAMPLE_CODE_RIP, doMPX OR doMOV_TR OR doCYRIX OR doCYRIX_DMI OR doALTINST );
  {$IF CompilerVersion < 23}{$RANGECHECKS ON}{$IFEND} // RangeCheck might cause Internal-Error C1118

  // Formatters: Masm*, Nasm*, Gas* (AT&T) and Intel* (XED).
  // For fastest code, see `SpecializedFormatter` which is ~3.3x faster. Use it if formatting
  // speed is more important than being able to re-assemble formatted instructions.
  Iced.Formatter.FormatterType := ftNasm;

  while Iced.Decoder.CanDecode do
    begin
    Iced.Decoder.Decode( Instruction );

    // Eg. "00007FFAC46ACDB2 488DAC2400FFFFFF     lea       rbp,[rsp-100h]"

    // Hex
    {$IF CompilerVersion < 23}{$RANGECHECKS OFF}{$IFEND} // RangeCheck might cause Internal-Error C1118
    start_index := instruction.RIP-EXAMPLE_CODE_RIP;
    {$IF CompilerVersion < 23}{$RANGECHECKS ON}{$IFEND} // RangeCheck might cause Internal-Error C1118
    sOutput := '';
    for i := start_index to start_index+Instruction.len-1 do
      sOutput := sOutput + IntToHex( EXAMPLE_CODE[ i ], 2 );

    if ( Instruction.len < HEXBYTES_COLUMN_BYTE_LENGTH ) then
      begin
      for i := 0 to HEXBYTES_COLUMN_BYTE_LENGTH-Instruction.len-1 do
        sOutput := sOutput + '  '
      end;

    sOutput := Format( '%.16x ', [ instruction.RIP ] ) + sOutput + Iced.Formatter.FormatToString( Instruction );

    WriteLn( sOutput );
    end;

  WriteLn( 'Press enter to exit.' );
  ReadLn;

end.
