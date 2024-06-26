program SymbolResolver;

{$APPTYPE CONSOLE}

uses
  SysUtils,
  Classes,

  uIced.Types in '..\..\uIced.Types.pas',
  uIced.Imports in '..\..\uIced.Imports.pas',
  uIced in '..\..\uIced.pas';

{
This example produces the following output:
mov rcx, [rdx+my_data (0x5AA55AA5)]
}

type
  TSymbolItem = packed record
    Offset : UInt64;
    Name   : AnsiString;
  end;
  TSymbolList = Array of TSymbolItem;

const
  Symbols : Array [ 0..0 ] of TSymbolItem = (
                                              ( Offset: UInt64( $5AA55AA5 ); Name: 'my_data' )
                                            );

function SymbolResolverCallback( var Instruction: TInstruction; Operand: Cardinal; InstructionOperand : Cardinal; Address: UInt64; Size: Cardinal; UserData : Pointer ) : PAnsiChar; cdecl;
var
  i : Integer;
begin
  result := '';
  for i := Low( Symbols ) to High( Symbols ) do
    begin
    {$IF CompilerVersion < 23}{$RANGECHECKS OFF}{$IFEND} // RangeCheck might cause Internal-Error C1118
    if ( Symbols[ i ].Offset = Address ) then
    {$IF CompilerVersion < 23}{$RANGECHECKS ON}{$IFEND} // RangeCheck might cause Internal-Error C1118
      result := PAnsiChar( Symbols[ i ].Name );
    end;
end;

const
  EXAMPLE_CODE_BITNESS                   = bt64;
  EXAMPLE_CODE_RIP                       = 0;
  EXAMPLE_CODE : Array [ 0..6 ] of Byte  = ( $48, $8B, $8A, $A5, $5A, $A5, $5A );
  EXAMPLE_OFFSET                         = $0000001FB55A1234;
var
  Instruction : TInstruction;
  Offset      : UInt64;
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
  Iced.Decoder.SetData( @EXAMPLE_CODE[ 0 ], Length( EXAMPLE_CODE ), EXAMPLE_CODE_RIP, doNone );
  {$IF CompilerVersion < 23}{$RANGECHECKS ON}{$IFEND} // RangeCheck might cause Internal-Error C1118

  Iced.Decoder.Decode( Instruction );

  Iced.Formatter.FormatterType  := ftNasm;
  Iced.Formatter.SymbolResolver := SymbolResolverCallback;
  Iced.Formatter.ShowSymbols    := True;

  // True (Default): mov rcx, [rdx+my_data (0x5AA55AA5)]
  // False         : mov rcx, my_data (0x5AA55AA5)[rdx]
//  Iced.Formatter.SymbolDisplacementInBrackets := True;

  WriteLn( Iced.Formatter.FormatToString( Instruction ) );

  WriteLn( 'Press enter to exit.' );
  ReadLn;

end.
