program FormatterOutput;

uses
  Forms,

  uIced.Types in '..\..\uIced.Types.pas',
  uIced.Imports in '..\..\uIced.Imports.pas',
  uIced in '..\..\uIced.pas',

  Main in 'Main.pas' {FrmFormatterOutput};

{$R *.res}

begin
  Application.Initialize;
  Application.CreateForm(TFrmFormatterOutput, FrmFormatterOutput);
  Application.Run;
end.
