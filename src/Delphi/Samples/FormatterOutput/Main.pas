unit Main;

{
This example produces the following output:
00007FFAC46ACDA4 48895C2410          mov     [rsp+10h],rbx
00007FFAC46ACDA9 4889742418          mov     [rsp+18h],rsi
00007FFAC46ACDAE 55                  push    rbp
00007FFAC46ACDAF 57                  push    rdi
00007FFAC46ACDB0 4156                push    r14
00007FFAC46ACDB2 488DAC2400FFFFFF    lea     rbp,[rsp-100h]
00007FFAC46ACDBA 4881EC00020000      sub     rsp,200h
00007FFAC46ACDC1 488B0518570A00      mov     rax,[7FFAC47524E0h]
00007FFAC46ACDC8 4833C4              xor     rax,rsp
00007FFAC46ACDCB 488985F0000000      mov     [rbp+0F0h],rax
00007FFAC46ACDD2 4C8B052F240A00      mov     r8,[7FFAC474F208h]
00007FFAC46ACDD9 488D05787C0400      lea     rax,[7FFAC46F4A58h]
00007FFAC46ACDE0 33FF                xor     edi,edi
}

interface

uses
  Classes, Controls, Forms, StdCtrls, ComCtrls;

type
  TFrmFormatterOutput = class(TForm)
    rEdtOutput: TRichEdit;
    procedure FormShow(Sender: TObject);
  private
    { Private-Deklarationen }
    procedure DisassembleWithFormatterCallback;
  public
    { Public-Deklarationen }
  end;

var
  FrmFormatterOutput: TFrmFormatterOutput;

// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
implementation

{$R *.dfm}

uses
  Graphics, SysUtils,
  uIced.Types,
  uIced.Imports,
  uIced;

// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
procedure TFrmFormatterOutput.FormShow(Sender: TObject);
begin
  DisassembleWithFormatterCallback;
end;

// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
procedure FormatterOutputCallback( Text : PAnsiChar; Kind : TFormatterTextKind; UserData : Pointer ); cdecl;
begin
  // Determine Color
  case Kind.FormatterTextKind of
    ftkDirective,
    ftkKeyword         : FrmFormatterOutput.rEdtOutput.SelAttributes.Color := clYellow;

    ftkPrefix,
    ftkMnemonic        : FrmFormatterOutput.rEdtOutput.SelAttributes.Color := clRed;

    ftkRegister        : FrmFormatterOutput.rEdtOutput.SelAttributes.Color := clMaroon;
    ftkNumber          : FrmFormatterOutput.rEdtOutput.SelAttributes.Color := clGray;

//    ftkText            : FrmFormatterOutput.rEdtOutput.SelAttributes.Color := ;
//    ftkOperator        : FrmFormatterOutput.rEdtOutput.SelAttributes.Color := ;
//    ftkPunctuation     : FrmFormatterOutput.rEdtOutput.SelAttributes.Color := ;
//
//    ftkDecorator       : FrmFormatterOutput.rEdtOutput.SelAttributes.Color := ;
//    ftkSelectorValue   : FrmFormatterOutput.rEdtOutput.SelAttributes.Color := ;
//    ftkLabelAddress    : FrmFormatterOutput.rEdtOutput.SelAttributes.Color := ;
//    ftkFunctionAddress : FrmFormatterOutput.rEdtOutput.SelAttributes.Color := ;
//    ftkData            : FrmFormatterOutput.rEdtOutput.SelAttributes.Color := ;
//    ftkLabel           : FrmFormatterOutput.rEdtOutput.SelAttributes.Color := ;
//    ftkFunction        : FrmFormatterOutput.rEdtOutput.SelAttributes.Color := ;
  else
    FrmFormatterOutput.rEdtOutput.SelAttributes.Color := clWindowText;
  end;

  // Append Text
  FrmFormatterOutput.rEdtOutput.SelText := Text;
end;

procedure TFrmFormatterOutput.DisassembleWithFormatterCallback;
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
  Instruction : TInstruction;
  sOutput     : String;
  start_index : Integer;
  i           : Integer;
begin
  rEdtOutput.Lines.Clear;
  if NOT IsInitDLL then
    begin
    rEdtOutput.Lines.Add( 'Library not loaded.' );
    Exit;
    end;

  Iced.Decoder.Bitness := EXAMPLE_CODE_BITNESS;

  {$IF CompilerVersion < 23}{$RANGECHECKS OFF}{$IFEND} // RangeCheck might cause Internal-Error C1118
  Iced.Decoder.SetData( @EXAMPLE_CODE[ 0 ], Length( EXAMPLE_CODE ), EXAMPLE_CODE_RIP, doNONE );
  {$IF CompilerVersion < 23}{$RANGECHECKS ON}{$IFEND} // RangeCheck might cause Internal-Error C1118

  // Formatters: Masm*, Nasm*, Gas* (AT&T) and Intel* (XED).
  // NOT for Fast and Specialized
  Iced.Formatter.FormatterType         := ftIntel;
  Iced.Formatter.FirstOperandCharIndex := 8;
  Iced.Formatter.Callback              := FormatterOutputCallback;

  while Iced.Decoder.CanDecode do
    begin
    Iced.Decoder.Decode( Instruction );

    // Eg. "00007FFAC46ACDB2 488DAC2400FFFFFF     "

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

    // Set Color for offset and Hex
    FrmFormatterOutput.rEdtOutput.SelAttributes.Color := clWindowText;
    rEdtOutput.SelText := Format( '%.16x ', [ instruction.RIP ] ) + sOutput;

    // Formatter callback gets called with each TextKind
    // Eg. "lea rbp,[rsp-100h]"
    Iced.Formatter.Format( Instruction );

    // Start new line
    rEdtOutput.SelText := #13#10;
    end;
end;

end.
