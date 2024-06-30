unit uIced;

{
  Iced (Dis)Assembler

  TetzkatLipHoka 2022-2024
}

// MS Finish details ..

interface

{$WARN UNSAFE_CODE OFF}
{$WARN COMPARING_SIGNED_UNSIGNED OFF}

{$IF CompilerVersion >= 22}
  {$LEGACYIFEND ON}
{$IFEND}

{$WARN UNSAFE_TYPE OFF}
{$WARN UNSAFE_CAST OFF}

{.$DEFINE DECODER_LOCAL_POSITION} // Buffer-Position is saved in Local-Variable, updated each Decode

{.$DEFINE SynUnicode}
{$DEFINE AssemblyTools} // MS test again in the end
{.$DEFINE TEST_FUNCTIONS}

uses
  Classes, SysUtils,
  {$IFDEF TEST_FUNCTIONS}Windows,{$ENDIF}
  {$IF NOT Defined( UNICODE ) AND Defined( SynUnicode )}SynUnicode,{$IFEND}
  {$IFDEF AssemblyTools}uAssemblyTools,{$ENDIF}
  uIced.Imports, uIced.Types;

type
  TIcedDecoder = class
  private
    fHandle   : Pointer;
    fBitness  : TIcedBitness;
    fData     : PByte;
    fSize     : NativeUInt;
    fIPosition: NativeUInt;
    {$IFDEF DECODER_LOCAL_POSITION}
    fPosition : NativeUInt;
    {$ENDIF DECODER_LOCAL_POSITION}
    function    GetHandle : Pointer;
    function    GetBitness : TIcedBitness;
    procedure   SetBitness( Value : TIcedBitness );
    function    GetIP : UInt64;
    procedure   SetIP( Value : UInt64 );
    function    GetPosition : NativeUInt;
    procedure   SetPosition( Value : NativeUInt );
    function    GetMaxPosition : NativeUInt;
    function    GetInstructionFirstByte : PByte;
    function    GetCurrentByte : PByte;
    function    GetLastError : TDecoderError;
  public
    constructor Create( Bitness : TIcedBitness = bt64 ); reintroduce;
    destructor  Destroy; override;
    procedure   SetData( Data : PByte; Size : NativeUInt; IP : UInt64 = UInt64( 0 ); Options : Cardinal = doNONE ); {$IF CompilerVersion >= 23}inline;{$IFEND}
    procedure   Decode( var Instruction : TInstruction ); overload; {$IF CompilerVersion >= 23}inline;{$IFEND}
    {$IFDEF AssemblyTools}
    procedure   Decode( var Instruction : TInstruction; var Details : TIcedDetails ); overload; // MS {$IF CompilerVersion >= 23}inline;{$IFEND}
    {$ENDIF AssemblyTools}
    procedure   GetConstantOffsets( var Instruction : TInstruction; var ConstantOffsets : TConstantOffsets ); {$IF CompilerVersion >= 23}inline;{$IFEND}

    property    Handle               : Pointer       read GetHandle;
    property    Bitness              : TIcedBitness  read GetBitness      write SetBitness;
    function    CanDecode : Boolean;
    property    IP                   : UInt64        read GetIP           write SetIP;
    property    Position             : NativeUInt    read GetPosition     write SetPosition;
    property    MaxPosition          : NativeUInt    read GetMaxPosition;

    property    InstructionFirstByte : PByte         read GetInstructionFirstByte;
    property    CurrentByte          : PByte         read GetCurrentByte;

    property    LastError            : TDecoderError read GetLastError;
  end;

// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  TIcedEncoder = class
  private
    fHandle   : Pointer;
    fBitness  : TIcedBitness;
    fCapacity : NativeUInt;
    function    GetHandle : Pointer;
    function    GetBitness : TIcedBitness;
    procedure   SetBitness( Value : TIcedBitness );
    function    GetCapacity : NativeUInt;
    procedure   SetCapacity( Value : NativeUInt );
    function    GetPreventVex2 : Boolean;
    procedure   SetPreventVex2( Value : Boolean );
    function    GetVexWig : Cardinal;
    procedure   SetVexWig( Value : Cardinal );
    function    GetVexLig : Cardinal;
    procedure   SetVexLig( Value : Cardinal );
    function    GetEvexWig : Cardinal;
    procedure   SetEvexWig( Value : Cardinal );
    function    GetEvexLig : Cardinal;
    procedure   SetEvexLig( Value : Cardinal );
    function    GetMvexWig : Cardinal;
    procedure   SetMvexWig( Value : Cardinal );
  public
    constructor Create( Bitness : TIcedBitness = bt64; Capacity : NativeUInt = 0 ); reintroduce;
    destructor  Destroy; override;
    procedure   Encode( var Instruction : TInstruction ); {$IF CompilerVersion >= 23}inline;{$IFEND}
    procedure   Write( Byte : Byte ); {$IF CompilerVersion >= 23}inline;{$IFEND}

    function    GetBuffer( Buffer : PByte; Size : NativeUInt ) : Boolean;
//    function    SetBuffer( Buffer : PByte; Size : NativeUInt ) : Boolean;

    procedure   GetConstantOffsets( var ConstantOffsets : TConstantOffsets ); {$IF CompilerVersion >= 23}inline;{$IFEND}

    property    Handle      : Pointer       read GetHandle;
    property    Bitness     : TIcedBitness  read GetBitness     write SetBitness;
    property    Capacity    : NativeUInt    read GetCapacity    write SetCapacity;

    property    PreventVex2 : Boolean       read GetPreventVex2 write SetPreventVex2;
    property    VexWig      : Cardinal      read GetVexWig      write SetVexWig;
    property    VexLig      : Cardinal      read GetVexLig      write SetVexLig;
    property    EvexWig     : Cardinal      read GetEvexWig     write SetEvexWig;
    property    EvexLig     : Cardinal      read GetEvexLig     write SetEvexLig;
    property    MvexWig     : Cardinal      read GetMvexWig     write SetMvexWig;
  end;

// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  TIcedBlockEncoder = class
  private
    fBitness : TIcedBitness;
    fOptions : Cardinal;
    fMemory  : Pointer;
    function    GetBitness : TIcedBitness;
    procedure   SetBitness( Value : TIcedBitness );

    function    GetOptions : Cardinal;
    procedure   SetOptions( Value : Cardinal );
  public
    constructor Create( Bitness : TIcedBitness = bt64; Options : Cardinal = beoNONE ); reintroduce;
    destructor  Destroy; override;

    property    Bitness : TIcedBitness read GetBitness write SetBitness;
    property    Options : Cardinal     read GetOptions write SetOptions;
    procedure   Encode( RIP : UInt64; var Instructions : TInstruction; Count : NativeUInt; var Results : TBlockEncoderResult ); overload; {$IF CompilerVersion >= 23}inline;{$IFEND}
    procedure   Encode( RIP : UInt64; var Instructions : TInstructionArray; var Results : TBlockEncoderResult ); overload; {$IF CompilerVersion >= 23}inline;{$IFEND}
    procedure   Encode( RIP : UInt64; var Instructions : TInstructionList; var Results : TBlockEncoderResult ); overload; {$IF CompilerVersion >= 23}inline;{$IFEND}
    procedure   Clear; {$IF CompilerVersion >= 23}inline;{$IFEND}
  end;

// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  TInstructionInfoFactory = class
  private
    fHandle  : Pointer;
    fOptions : Cardinal;
    function    GetHandle : Pointer;
    function    GetOptions : Cardinal;
    procedure   SetOptions( Value : Cardinal );
  public
    constructor Create( Options : Cardinal = iioNone ); reintroduce;
    destructor  Destroy; override;
    procedure   Info( var Instruction: TInstruction; var InstructionInfo : TInstructionInfo ); {$IF CompilerVersion >= 23}inline;{$IFEND}

    property    Handle  : Pointer  read GetHandle;
    property    Options : Cardinal read GetOptions write SetOptions;
  end;

// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  TIcedFormatterSettings = record
    // Options
    // Masm
    AddDsPrefix32                   : Boolean;
    SymbolDisplacementInBrackets    : Boolean;
    DisplacementInBrackets          : Boolean;
    // Nasm
    ShowSignExtendedImmediateSize   : Boolean;
    // Gas
    NakedRegisters                  : Boolean;
    ShowMnemonicSizeSuffix          : Boolean;
    SpaceAfterMemoryOperandComma    : Boolean;
    // Specialized
    UseHexPrefix                    : Boolean;
    AlwaysShowMemorySize            : Boolean;
    // Common
    SpaceAfterOperandSeparator      : Boolean;
    AlwaysShowSegmentRegister       : Boolean;
    UsePseudoOps                    : Boolean;
    RipRelativeAddresses            : Boolean;
    ShowSymbolAddress               : Boolean;
    UpperCaseHex                    : Boolean;
    // Common (All but Fast/Specialized)
    UpperCasePrefixes               : Boolean;
    UpperCaseMnemonics              : Boolean;
    UpperCaseRegisters              : Boolean;
    UpperCaseKeyWords               : Boolean;
    UpperCaseDecorators             : Boolean;
    UpperCaseEverything             : Boolean;
    FirstOperandCharIndex           : Cardinal;
    TabSize                         : Cardinal;
    SpaceAfterMemoryBracket         : Boolean;
    SpaceBetweenMemoryAddOperators  : Boolean;
    SpaceBetweenMemoryMulOperators  : Boolean;
    ScaleBeforeIndex                : Boolean;
    AlwaysShowScale                 : Boolean;
    ShowZeroDisplacements           : Boolean;
    HexPrefix                       : AnsiString;
    HexSuffix                       : AnsiString;
    HexDigitGroupSize               : Cardinal;
    DecimalPrefix                   : AnsiString;
    DecimalSuffix                   : AnsiString;
    DecimalDigitGroupSize           : Cardinal;
    OctalPrefix                     : AnsiString;
    OctalSuffix                     : AnsiString;
    OctalDigitGroupSize             : Cardinal;
    BinaryPrefix                    : AnsiString;
    BinarySuffix                    : AnsiString;
    BinaryDigitGroupSize            : Cardinal;
    DigitSeparator                  : AnsiString;
    LeadingZeros                    : Boolean;
    SmallHexNumbersInDecimal        : Boolean;
    AddLeadingZeroToHexNumbers      : Boolean;
    NumberBase                      : TNumberBase;
    BranchLeadingZeros              : Boolean;
    SignedImmediateOperands         : Boolean;
    SignedMemoryDisplacements       : Boolean;
    DisplacementLeadingZeros        : Boolean;
    MemorySizeOptions               : TMemorySizeOptions;
    ShowBranchSize                  : Boolean;
    PreferST0                       : Boolean;
    ShowUselessPrefixes             : Boolean;
    CC_b                            : TCC_b;
    CC_ae                           : TCC_ae;
    CC_e                            : TCC_e;
    CC_ne                           : TCC_ne;
    CC_be                           : TCC_be;
    CC_a                            : TCC_a;
    CC_p                            : TCC_p;
    CC_np                           : TCC_np;
    CC_l                            : TCC_l;
    CC_ge                           : TCC_ge;
    CC_le                           : TCC_le;
    CC_g                            : TCC_g;
  end;
  PIcedFormatterSettings = ^TIcedFormatterSettings;

  TIcedFormatter = class
  private
    fType            : TIcedFormatterType;
    fHandle          : Pointer;
    fOptions         : TIcedFormatterSettings;
    fSpecialized     : TIcedSpecializedFormatterOptions;
    fSymbolResolver  : TSymbolResolverCallback;
    fOptionsProvider : TFormatterOptionsProviderCallback;
    fFormatterOutput : TFormatterOutputCallback;
    fOutputHandle    : Pointer;
    fShowSymbols     : Boolean;
    {$IFDEF AssemblyTools}
    fSymbolHandler   : TSymbolHandler;
    {$ENDIF AssemblyTools}
    procedure   CreateHandle( KeepConfiguration : Boolean = False );
    function    GetHandle : Pointer;
    procedure   SetType( Value : TIcedFormatterType );
    function    GetSymbolResolver : TSymbolResolverCallback;
    procedure   SetSymbolResolver( Value : TSymbolResolverCallback );
    function    GetOptionsProvider : TFormatterOptionsProviderCallback;
    procedure   SetOptionsProvider( Value : TFormatterOptionsProviderCallback );
    function    GetFormatterOutput : TFormatterOutputCallback;
    procedure   SetFormatterOutput( Value : TFormatterOutputCallback );

    procedure   SetShowSymbols( Value : Boolean );
    {$IFDEF AssemblyTools}
    procedure   SetSymbolHandler( Value : TSymbolHandler );
    {$ENDIF AssemblyTools}

    // Options
    function    GetOptions : TIcedFormatterSettings;
    procedure   SetOptions( Value : TIcedFormatterSettings );
    procedure   SetCapstoneOptions;

    // Masm
    function    GetAddDsPrefix32 : Boolean;
    procedure   SetAddDsPrefix32( Value : Boolean );
    function    GetSymbolDisplacementInBrackets : Boolean;
    procedure   SetSymbolDisplacementInBrackets( Value : Boolean );
    function    GetDisplacementInBrackets : Boolean;
    procedure   SetDisplacementInBrackets( Value : Boolean );
    // Nasm
    function    GetShowSignExtendedImmediateSize : Boolean;
    procedure   SetShowSignExtendedImmediateSize( Value : Boolean );
    // Gas
    function    GetNakedRegisters : Boolean;
    procedure   SetNakedRegisters( Value : Boolean );
    function    GetShowMnemonicSizeSuffix : Boolean;
    procedure   SetShowMnemonicSizeSuffix( Value : Boolean );
    function    GetSpaceAfterMemoryOperandComma : Boolean;
    procedure   SetSpaceAfterMemoryOperandComma( Value : Boolean );
    // Specialized
    function    GetUseHexPrefix : Boolean;
    procedure   SetUseHexPrefix( Value : Boolean );
    function    GetAlwaysShowMemorySize : Boolean;
    procedure   SetAlwaysShowMemorySize( Value : Boolean );
    function    GetEnable_DB_DW_DD_DQ : Boolean;
    procedure   SetEnable_DB_DW_DD_DQ( Value : Boolean );
    function    GetVerifyOutputHasEnoughBytesLeft : Boolean;
    procedure   SetVerifyOutputHasEnoughBytesLeft( Value : Boolean );

    // Common
    function    GetSpaceAfterOperandSeparator : Boolean;
    procedure   SetSpaceAfterOperandSeparator( Value : Boolean );
    function    GetAlwaysShowSegmentRegister : Boolean;
    procedure   SetAlwaysShowSegmentRegister( Value : Boolean );
    function    GetUsePseudoOps : Boolean;
    procedure   SetUsePseudoOps( Value : Boolean );
    function    GetRipRelativeAddresses : Boolean;
    procedure   SetRipRelativeAddresses( Value : Boolean );
    function    GetShowSymbolAddress : Boolean;
    procedure   SetShowSymbolAddress( Value : Boolean );
    function    GetUpperCaseHex : Boolean;
    procedure   SetUpperCaseHex( Value : Boolean );
    // Common (All but Fast/Specialized)
    function    GetUpperCasePrefixes : Boolean;
    procedure   SetUpperCasePrefixes( Value : Boolean );
    function    GetUpperCaseMnemonics : Boolean;
    procedure   SetUpperCaseMnemonics( Value : Boolean );
    function    GetUpperCaseRegisters : Boolean;
    procedure   SetUpperCaseRegisters( Value : Boolean );
    function    GetUpperCaseKeyWords : Boolean;
    procedure   SetUpperCaseKeyWords( Value : Boolean );
    function    GetUpperCaseDecorators : Boolean;
    procedure   SetUpperCaseDecorators( Value : Boolean );
    function    GetUpperCaseEverything : Boolean;
    procedure   SetUpperCaseEverything( Value : Boolean );
    function    GetFirstOperandCharIndex : Cardinal;
    procedure   SetFirstOperandCharIndex( Value : Cardinal );
    function    GetTabSize : Cardinal;
    procedure   SetTabSize( Value : Cardinal );
    function    GetSpaceAfterMemoryBracket : Boolean;
    procedure   SetSpaceAfterMemoryBracket( Value : Boolean );
    function    GetSpaceBetweenMemoryAddOperators : Boolean;
    procedure   SetSpaceBetweenMemoryAddOperators( Value : Boolean );
    function    GetSpaceBetweenMemoryMulOperators : Boolean;
    procedure   SetSpaceBetweenMemoryMulOperators( Value : Boolean );
    function    GetScaleBeforeIndex : Boolean;
    procedure   SetScaleBeforeIndex( Value : Boolean );
    function    GetAlwaysShowScale : Boolean;
    procedure   SetAlwaysShowScale( Value : Boolean );
    function    GetShowZeroDisplacements : Boolean;
    procedure   SetShowZeroDisplacements( Value : Boolean );
    function    GetHexPrefix : AnsiString;
    procedure   SetHexPrefix( Value : AnsiString );
    function    GetHexSuffix : AnsiString;
    procedure   SetHexSuffix( Value : AnsiString );
    function    GetHexDigitGroupSize : Cardinal;
    procedure   SetHexDigitGroupSize( Value : Cardinal );
    function    GetDecimalPrefix : AnsiString;
    procedure   SetDecimalPrefix( Value : AnsiString );
    function    GetDecimalSuffix : AnsiString;
    procedure   SetDecimalSuffix( Value : AnsiString );
    function    GetDecimalDigitGroupSize : Cardinal;
    procedure   SetDecimalDigitGroupSize( Value : Cardinal );
    function    GetOctalPrefix : AnsiString;
    procedure   SetOctalPrefix( Value : AnsiString );
    function    GetOctalSuffix : AnsiString;
    procedure   SetOctalSuffix( Value : AnsiString );
    function    GetOctalDigitGroupSize : Cardinal;
    procedure   SetOctalDigitGroupSize( Value : Cardinal );
    function    GetBinaryPrefix : AnsiString;
    procedure   SetBinaryPrefix( Value : AnsiString );
    function    GetBinarySuffix : AnsiString;
    procedure   SetBinarySuffix( Value : AnsiString );
    function    GetBinaryDigitGroupSize : Cardinal;
    procedure   SetBinaryDigitGroupSize( Value : Cardinal );
    function    GetDigitSeparator : AnsiString;
    procedure   SetDigitSeparator( Value : AnsiString );
    function    GetLeadingZeros : Boolean;
    procedure   SetLeadingZeros( Value : Boolean );
    function    GetSmallHexNumbersInDecimal : Boolean;
    procedure   SetSmallHexNumbersInDecimal( Value : Boolean );
    function    GetAddLeadingZeroToHexNumbers : Boolean;
    procedure   SetAddLeadingZeroToHexNumbers( Value : Boolean );
    function    GetNumberBase : TNumberBase;
    procedure   SetNumberBase( Value : TNumberBase );
    function    GetBranchLeadingZeros : Boolean;
    procedure   SetBranchLeadingZeros( Value : Boolean );
    function    GetSignedImmediateOperands : Boolean;
    procedure   SetSignedImmediateOperands( Value : Boolean );
    function    GetSignedMemoryDisplacements : Boolean;
    procedure   SetSignedMemoryDisplacements( Value : Boolean );
    function    GetDisplacementLeadingZeros : Boolean;
    procedure   SetDisplacementLeadingZeros( Value : Boolean );
    function    GetMemorySizeOptions : TMemorySizeOptions;
    procedure   SetMemorySizeOptions( Value : TMemorySizeOptions );
    function    GetShowBranchSize : Boolean;
    procedure   SetShowBranchSize( Value : Boolean );
    function    GetPreferST0 : Boolean;
    procedure   SetPreferST0( Value : Boolean );
    function    GetShowUselessPrefixes : Boolean;
    procedure   SetShowUselessPrefixes( Value : Boolean );
    function    GetCC_b : TCC_b;
    procedure   SetCC_b( Value : TCC_b );
    function    GetCC_ae : TCC_ae;
    procedure   SetCC_ae( Value : TCC_ae );
    function    GetCC_e : TCC_e;
    procedure   SetCC_e( Value : TCC_e );
    function    GetCC_ne : TCC_ne;
    procedure   SetCC_ne( Value : TCC_ne );
    function    GetCC_be : TCC_be;
    procedure   SetCC_be( Value : TCC_be );
    function    GetCC_a : TCC_a;
    procedure   SetCC_a( Value : TCC_a );
    function    GetCC_p : TCC_p;
    procedure   SetCC_p( Value : TCC_p );
    function    GetCC_np : TCC_np;
    procedure   SetCC_np( Value : TCC_np );
    function    GetCC_l : TCC_l;
    procedure   SetCC_l( Value : TCC_l );
    function    GetCC_ge : TCC_ge;
    procedure   SetCC_ge( Value : TCC_ge );
    function    GetCC_le : TCC_le;
    procedure   SetCC_le( Value : TCC_le );
    function    GetCC_g : TCC_g;
    procedure   SetCC_g( Value : TCC_g );
  public
    constructor Create( FormatterType : TIcedFormatterType = DEFAULT_FORMATTER; SymbolResolver : TSymbolResolverCallback = nil; OptionsProvider : TFormatterOptionsProviderCallback = nil ); reintroduce;
    destructor  Destroy; override;
    procedure   DefaultSettings;

    function    FormatToString( var Instruction: TInstruction ) : AnsiString; overload; {$IF CompilerVersion >= 23}inline;{$IFEND}
    procedure   Format( var Instruction: TInstruction; AOutput: PAnsiChar; Size : NativeUInt ); overload; {$IF CompilerVersion >= 23}inline;{$IFEND}
    procedure   Format( var Instruction: TInstruction ); overload; {$IF CompilerVersion >= 23}inline;{$IFEND} // TFormatterOutputCallback

    property    Handle                          : Pointer                            read GetHandle;
    property    FormatterType                   : TIcedFormatterType                 read fType                             write SetType;
    property    SymbolResolver                  : TSymbolResolverCallback            read GetSymbolResolver                 write SetSymbolResolver;
    property    OptionsProvider                 : TFormatterOptionsProviderCallback  read GetOptionsProvider                write SetOptionsProvider;
    property    Callback                        : TFormatterOutputCallback           read GetFormatterOutput                write SetFormatterOutput;

    property    ShowSymbols                     : Boolean                            read fShowSymbols                      write SetShowSymbols;
    {$IFDEF AssemblyTools}
    property    SymbolHandler                   : TSymbolHandler                     read fSymbolHandler                    write SetSymbolHandler;
    {$ENDIF AssemblyTools}

    // Options
    property    Options                         : TIcedFormatterSettings             read GetOptions                        write SetOptions;
    procedure   SettingsToStringList( Settings : TIcedFormatterSettings; var StrL : TStringList );
    function    StringListToSettings( StrL : TStringList; Settings : PIcedFormatterSettings = nil ) : boolean;

    // Masm
    property    AddDsPrefix32                   : Boolean                            read GetAddDsPrefix32                  write SetAddDsPrefix32;
    property    SymbolDisplacementInBrackets    : Boolean                            read GetSymbolDisplacementInBrackets   write SetSymbolDisplacementInBrackets;
    property    DisplacementInBrackets          : Boolean                            read GetDisplacementInBrackets         write SetDisplacementInBrackets;
    // Nasm
    property    ShowSignExtendedImmediateSize   : Boolean                            read GetShowSignExtendedImmediateSize  write SetShowSignExtendedImmediateSize;
    // Gas
    property    NakedRegisters                  : Boolean                            read GetNakedRegisters                 write SetNakedRegisters;
    property    ShowMnemonicSizeSuffix          : Boolean                            read GetShowMnemonicSizeSuffix         write SetShowMnemonicSizeSuffix;
    property    SpaceAfterMemoryOperandComma    : Boolean                            read GetSpaceAfterMemoryOperandComma   write SetSpaceAfterMemoryOperandComma;
    // Specialized
    property    UseHexPrefix                    : Boolean                            read GetUseHexPrefix                   write SetUseHexPrefix;
    property    AlwaysShowMemorySize            : Boolean                            read GetAlwaysShowMemorySize           write SetAlwaysShowMemorySize;
    property    Enable_DB_DW_DD_DQ              : Boolean                            read GetEnable_DB_DW_DD_DQ             write SetEnable_DB_DW_DD_DQ;
    property    VerifyOutputHasEnoughBytesLeft  : Boolean                            read GetVerifyOutputHasEnoughBytesLeft write SetVerifyOutputHasEnoughBytesLeft;

    // Common
    property    SpaceAfterOperandSeparator      : Boolean                            read GetSpaceAfterOperandSeparator     write SetSpaceAfterOperandSeparator;
    property    AlwaysShowSegmentRegister       : Boolean                            read GetAlwaysShowSegmentRegister      write SetAlwaysShowSegmentRegister;
    property    UsePseudoOps                    : Boolean                            read GetUsePseudoOps                   write SetUsePseudoOps;
    property    RipRelativeAddresses            : Boolean                            read GetRipRelativeAddresses           write SetRipRelativeAddresses;
    property    ShowSymbolAddress               : Boolean                            read GetShowSymbolAddress              write SetShowSymbolAddress;
    property    UpperCaseHex                    : Boolean                            read GetUpperCaseHex                   write SetUpperCaseHex;
    // Common (All but Fast/Specialized)
    property    UpperCasePrefixes               : Boolean                            read GetUpperCasePrefixes              write SetUpperCasePrefixes;
    property    UpperCaseMnemonics              : Boolean                            read GetUpperCaseMnemonics             write SetUpperCaseMnemonics;
    property    UpperCaseRegisters              : Boolean                            read GetUpperCaseRegisters             write SetUpperCaseRegisters;
    property    UpperCaseKeyWords               : Boolean                            read GetUpperCaseKeyWords              write SetUpperCaseKeyWords;
    property    UpperCaseDecorators             : Boolean                            read GetUpperCaseDecorators            write SetUpperCaseDecorators;
    property    UpperCaseEverything             : Boolean                            read GetUpperCaseEverything            write SetUpperCaseEverything;
    property    FirstOperandCharIndex           : Cardinal                           read GetFirstOperandCharIndex          write SetFirstOperandCharIndex;
    property    TabSize                         : Cardinal                           read GetTabSize                        write SetTabSize;
    property    SpaceAfterMemoryBracket         : Boolean                            read GetSpaceAfterMemoryBracket        write SetSpaceAfterMemoryBracket;
    property    SpaceBetweenMemoryAddOperators  : Boolean                            read GetSpaceBetweenMemoryAddOperators write SetSpaceBetweenMemoryAddOperators;
    property    SpaceBetweenMemoryMulOperators  : Boolean                            read GetSpaceBetweenMemoryMulOperators write SetSpaceBetweenMemoryMulOperators;
    property    ScaleBeforeIndex                : Boolean                            read GetScaleBeforeIndex               write SetScaleBeforeIndex;
    property    AlwaysShowScale                 : Boolean                            read GetAlwaysShowScale                write SetAlwaysShowScale;
    property    ShowZeroDisplacements           : Boolean                            read GetShowZeroDisplacements          write SetShowZeroDisplacements;
    property    HexPrefix                       : AnsiString                         read GetHexPrefix                      write SetHexPrefix;
    property    HexSuffix                       : AnsiString                         read GetHexSuffix                      write SetHexSuffix;
    property    HexDigitGroupSize               : Cardinal                           read GetHexDigitGroupSize              write SetHexDigitGroupSize;
    property    DecimalPrefix                   : AnsiString                         read GetDecimalPrefix                  write SetDecimalPrefix;
    property    DecimalSuffix                   : AnsiString                         read GetDecimalSuffix                  write SetDecimalSuffix;
    property    DecimalDigitGroupSize           : Cardinal                           read GetDecimalDigitGroupSize          write SetDecimalDigitGroupSize;
    property    OctalPrefix                     : AnsiString                         read GetOctalPrefix                    write SetOctalPrefix;
    property    OctalSuffix                     : AnsiString                         read GetOctalSuffix                    write SetOctalSuffix;
    property    OctalDigitGroupSize             : Cardinal                           read GetOctalDigitGroupSize            write SetOctalDigitGroupSize;
    property    BinaryPrefix                    : AnsiString                         read GetBinaryPrefix                   write SetBinaryPrefix;
    property    BinarySuffix                    : AnsiString                         read GetBinarySuffix                   write SetBinarySuffix;
    property    BinaryDigitGroupSize            : Cardinal                           read GetBinaryDigitGroupSize           write SetBinaryDigitGroupSize;
    property    DigitSeparator                  : AnsiString                         read GetDigitSeparator                 write SetDigitSeparator;
    property    LeadingZeros                    : Boolean                            read GetLeadingZeros                   write SetLeadingZeros;
    property    SmallHexNumbersInDecimal        : Boolean                            read GetSmallHexNumbersInDecimal       write SetSmallHexNumbersInDecimal;
    property    AddLeadingZeroToHexNumbers      : Boolean                            read GetAddLeadingZeroToHexNumbers     write SetAddLeadingZeroToHexNumbers;
    property    NumberBase                      : TNumberBase                        read GetNumberBase                     write SetNumberBase;
    property    BranchLeadingZeros              : Boolean                            read GetBranchLeadingZeros             write SetBranchLeadingZeros;
    property    SignedImmediateOperands         : Boolean                            read GetSignedImmediateOperands        write SetSignedImmediateOperands;
    property    SignedMemoryDisplacements       : Boolean                            read GetSignedMemoryDisplacements      write SetSignedMemoryDisplacements;
    property    DisplacementLeadingZeros        : Boolean                            read GetDisplacementLeadingZeros       write SetDisplacementLeadingZeros;
    property    MemorySizeOptions               : TMemorySizeOptions                 read GetMemorySizeOptions              write SetMemorySizeOptions;
    property    ShowBranchSize                  : Boolean                            read GetShowBranchSize                 write SetShowBranchSize;
    property    PreferST0                       : Boolean                            read GetPreferST0                      write SetPreferST0;
    property    ShowUselessPrefixes             : Boolean                            read GetShowUselessPrefixes            write SetShowUselessPrefixes;
    property    CC_b                            : TCC_b                              read GetCC_b                           write SetCC_b;
    property    CC_ae                           : TCC_ae                             read GetCC_ae                          write SetCC_ae;
    property    CC_e                            : TCC_e                              read GetCC_e                           write SetCC_e;
    property    CC_ne                           : TCC_ne                             read GetCC_ne                          write SetCC_ne;
    property    CC_be                           : TCC_be                             read GetCC_be                          write SetCC_be;
    property    CC_a                            : TCC_a                              read GetCC_a                           write SetCC_a;
    property    CC_p                            : TCC_p                              read GetCC_p                           write SetCC_p;
    property    CC_np                           : TCC_np                             read GetCC_np                          write SetCC_np;
    property    CC_l                            : TCC_l                              read GetCC_l                           write SetCC_l;
    property    CC_ge                           : TCC_ge                             read GetCC_ge                          write SetCC_ge;
    property    CC_le                           : TCC_le                             read GetCC_le                          write SetCC_le;
    property    CC_g                            : TCC_g                              read GetCC_g                           write SetCC_g;
  end;

// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  tIcedPointerScanResult = record
    Origin      : UInt64;
    Instruction : string;
    vPointer    : UInt64;
    Count       : Word;
  end;
  tIcedPointerScanResults = Array of tIcedPointerScanResult;

  tIcedReferenceScanResult = record
    Origin      : UInt64;
    Instruction : string;
  end;
  tIcedReferenceScanResults = Array of tIcedReferenceScanResult;

  tIcedAssemblyScanMode = (
    asmEqual, asmSimiliar, asmWildcard
    {$IF CompilerVersion >= 22}, asmRegExp{$IFEND} // XE
  );

  tIcedPointerScanProcessEvent = procedure( Current, Total : Cardinal; var Cancel : Boolean ) of object;

  tIcedAssemblyCompareMode = ( acmCenter, acmForward, acmBackward );

{$IF NOT Declared( TByteInstruction )} // IFNDEF AssemblyTools
const
  MAX_INSTRUCTION_LEN_ = 15;
  MASKING_OFFSET = $10000;  
type
  TByteInstruction = Array [ 0..MAX_INSTRUCTION_LEN_-1 ] of Byte;
  pByteInstruction = ^TByteInstruction;
{$IFEND}

// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  TIced = class
  private
    fDecoder      : TIcedDecoder;
    fEncoder      : TIcedEncoder;
    fBlockEncoder : TIcedBlockEncoder;
    fInfoFactory  : TInstructionInfoFactory;
    fFormatter    : TIcedFormatter;
  public
    constructor Create( Bitness : TIcedBitness = bt64 ); reintroduce;
    destructor  Destroy; override;
    property    Decoder      : TIcedDecoder            read fDecoder;
    property    Encoder      : TIcedEncoder            read fEncoder;
    property    BlockEncoder : TIcedBlockEncoder       read fBlockEncoder;
    property    InfoFactory  : TInstructionInfoFactory read fInfoFactory;
    property    Formatter    : TIcedFormatter          read fFormatter;

    {$IF Declared( SynUnicode )}
    function    DecodeFromFile( FileName : String; Size : Cardinal; Offset : UInt64; StrL_Assembly : TUnicodeStrings; CodeOffset : UInt64 = UInt64( 0 ); StrL_Hex : TUnicodeStrings = nil; {$IFDEF AssemblyTools}Details : pIcedDetails = nil;{$ENDIF} DecoderSettings : Cardinal = doNONE; {$IFDEF AssemblyTools}CalcJumpLines : Boolean = True;{$ENDIF} CombineNOP : boolean = False ) : Cardinal; overload;
    function    DecodeFromStream( Data : TMemoryStream; Size : Cardinal; StrL_Assembly : TUnicodeStrings; CodeOffset : UInt64 = UInt64( 0 ); StrL_Hex : TUnicodeStrings = nil; {$IFDEF AssemblyTools}Details : pIcedDetails = nil;{$ENDIF} DecoderSettings : Cardinal = doNONE; {$IFDEF AssemblyTools}CalcJumpLines : Boolean = True;{$ENDIF} CombineNOP : boolean = False ) : Cardinal; overload;

    function    Decode( Data : PByte; Size : Cardinal; StrL_Assembly : TUnicodeStrings; CodeOffset : UInt64 = UInt64( 0 ); StrL_Hex : TUnicodeStrings = nil; {$IFDEF AssemblyTools}Details : pIcedDetails = nil;{$ENDIF} DecoderSettings : Cardinal = doNONE{$IFDEF AssemblyTools}; CalcJumpLines : Boolean = True{$ENDIF} ) : Cardinal; overload;
    function    Decode( Data : PByte; Size : Cardinal; Count : Cardinal; StrL_Assembly : TUnicodeStrings; CodeOffset : UInt64 = UInt64( 0 ); StrL_Hex : TUnicodeStrings = nil; {$IFDEF AssemblyTools}Details : pIcedDetails = nil;{$ENDIF} DecoderSettings : Cardinal = doNONE{$IFDEF AssemblyTools}; CalcJumpLines : Boolean = True{$ENDIF} ) : Cardinal; overload;
    function    Decode( Data : string; StrL_Assembly : TUnicodeStrings; CodeOffset : UInt64 = UInt64( 0 ); StrL_Hex : TUnicodeStrings = nil; {$IFDEF AssemblyTools}Details : pIcedDetails = nil;{$ENDIF} DecoderSettings : Cardinal = doNONE; {$IFDEF AssemblyTools}CalcJumpLines : Boolean = True;{$ENDIF} CombineNOP : boolean = False ) : boolean; overload;
    function    Decode( Data : TStrings; StrL_Assembly : TUnicodeStrings; CodeOffset : UInt64 = UInt64( 0 ); StrL_Hex : TUnicodeStrings = nil; {$IFDEF AssemblyTools}Details : pIcedDetails = nil;{$ENDIF} DecoderSettings : Cardinal = doNONE; {$IFDEF AssemblyTools}CalcJumpLines : Boolean = True;{$ENDIF} CombineNOP : boolean = False ) : boolean; overload;

    function    DecodeCombineNOP( Data : PByte; Size : Cardinal; StrL_Assembly : TUnicodeStrings; CodeOffset : UInt64 = UInt64( 0 ); StrL_Hex : TUnicodeStrings = nil; {$IFDEF AssemblyTools}Details : pIcedDetails = nil;{$ENDIF} DecoderSettings : Cardinal = doNONE{$IFDEF AssemblyTools}; CalcJumpLines : Boolean = True{$ENDIF} ) : Cardinal; overload;
    function    DecodeCombineNOP( Data : PByte; Size : Cardinal; Count : Cardinal; StrL_Assembly : TUnicodeStrings; CodeOffset : UInt64 = UInt64( 0 ); StrL_Hex : TUnicodeStrings = nil; {$IFDEF AssemblyTools}Details : pIcedDetails = nil;{$ENDIF} DecoderSettings : Cardinal = doNONE{$IFDEF AssemblyTools}; CalcJumpLines : Boolean = True{$ENDIF} ) : Cardinal; overload;
    {$IFEND UNICODE}

    function    DecodeFromFile( FileName : String; Size : Cardinal; Offset : UInt64; StrL_Assembly : TStrings; CodeOffset : UInt64 = UInt64( 0 ); StrL_Hex : TStrings = nil; {$IFDEF AssemblyTools}Details : pIcedDetails = nil;{$ENDIF} DecoderSettings : Cardinal = doNONE; {$IFDEF AssemblyTools}CalcJumpLines : Boolean = True;{$ENDIF} CombineNOP : boolean = False ) : Cardinal; overload;
    function    DecodeFromStream( Data : TMemoryStream; Size : Cardinal; StrL_Assembly : TStrings; CodeOffset : UInt64 = UInt64( 0 ); StrL_Hex : TStrings = nil; {$IFDEF AssemblyTools}Details : pIcedDetails = nil;{$ENDIF} DecoderSettings : Cardinal = doNONE; {$IFDEF AssemblyTools}CalcJumpLines : Boolean = True;{$ENDIF} CombineNOP : boolean = False ) : Cardinal; overload;

    function    Decode( Data : PByte; Size : Cardinal; StrL_Assembly : TStrings; CodeOffset : UInt64 = UInt64( 0 ); StrL_Hex : TStrings = nil; {$IFDEF AssemblyTools}Details : pIcedDetails = nil;{$ENDIF} DecoderSettings : Cardinal = doNONE{$IFDEF AssemblyTools}; CalcJumpLines : Boolean = True{$ENDIF} ) : Cardinal; overload;
    function    Decode( Data : PByte; Size : Cardinal; Count : Cardinal; StrL_Assembly : TStrings; CodeOffset : UInt64 = UInt64( 0 ); StrL_Hex : TStrings = nil; {$IFDEF AssemblyTools}Details : pIcedDetails = nil;{$ENDIF} DecoderSettings : Cardinal = doNONE{$IFDEF AssemblyTools}; CalcJumpLines : Boolean = True{$ENDIF} ) : Cardinal; overload;
    function    Decode( Data : PByte; Size : Cardinal; var Instruction: String; CodeOffset : UInt64 = UInt64( 0 ); InstructionBytes : pString = nil; Offset : PUInt64 = nil; {$IFDEF AssemblyTools}Detail : pIcedDetail = nil;{$ENDIF AssemblyTools} DecoderSettings : Cardinal = doNONE; {$IFDEF AssemblyTools}CalcJumpLines : Boolean = True;{$ENDIF} CombineNOP : boolean = False ) : Cardinal; overload;
    function    Decode( Data : string; StrL_Assembly : TStrings; CodeOffset : UInt64 = UInt64( 0 ); StrL_Hex : TStrings = nil; {$IFDEF AssemblyTools}Details : pIcedDetails = nil;{$ENDIF} DecoderSettings : Cardinal = doNONE; {$IFDEF AssemblyTools}CalcJumpLines : Boolean = True;{$ENDIF} CombineNOP : boolean = False ) : boolean; overload;
    function    Decode( Data : TStrings; StrL_Assembly : TStrings; CodeOffset : UInt64 = UInt64( 0 ); StrL_Hex : TStrings = nil; {$IFDEF AssemblyTools}Details : pIcedDetails = nil;{$ENDIF} DecoderSettings : Cardinal = doNONE; {$IFDEF AssemblyTools}CalcJumpLines : Boolean = True;{$ENDIF} CombineNOP : boolean = False ) : boolean; overload;

    function    DecodeCombineNOP( Data : PByte; Size : Cardinal; StrL_Assembly : TStrings; CodeOffset : UInt64 = UInt64( 0 ); StrL_Hex : TStrings = nil; {$IFDEF AssemblyTools}Details : pIcedDetails = nil;{$ENDIF} DecoderSettings : Cardinal = doNONE{$IFDEF AssemblyTools}; CalcJumpLines : Boolean = True{$ENDIF} ) : Cardinal; overload;
    function    DecodeCombineNOP( Data : PByte; Size : Cardinal; Count : Cardinal; StrL_Assembly : TStrings; CodeOffset : UInt64 = UInt64( 0 ); StrL_Hex : TStrings = nil; {$IFDEF AssemblyTools}Details : pIcedDetails = nil;{$ENDIF} DecoderSettings : Cardinal = doNONE{$IFDEF AssemblyTools}; CalcJumpLines : Boolean = True{$ENDIF} ) : Cardinal; overload;

    {$IFDEF AssemblyTools}
    function    DecodeAddress( var Data : TByteInstruction; Address : UInt64; Instruction : PString = nil; {$IFDEF AssemblyTools}Detail : pIcedDetail = nil;{$ENDIF} DecoderSettings : Cardinal = doNONE ) : Cardinal; overload;
    function    DecodeAddress( Data : PByte; Size : Cardinal; Address : UInt64; InstructionBytes : pByteInstruction = nil; Instruction : PString = nil; {$IFDEF AssemblyTools}Detail : pIcedDetail = nil;{$ENDIF} DecoderSettings : Cardinal = doNONE ) : Cardinal; overload;
    {$ENDIF AssemblyTools}
    function    Compare( Data : PByte; Size : Cardinal; Offset : UInt64; DataCompare : PByte; SizeCompare : Cardinal; OffsetCompare : UInt64; Count : Word; CodeOffset : UInt64 = UInt64( 0 ); Mode : tIcedAssemblyCompareMode = acmCenter ) : Cardinal; overload;
    function    Compare( Data : TMemoryStream; Offset : UInt64; DataCompare : TMemoryStream; OffsetCompare : UInt64; Count : Word; CodeOffset : UInt64 = UInt64( 0 ); Mode : tIcedAssemblyCompareMode = acmCenter ) : Cardinal; overload;

    function    PointerScan( Data : PByte; Size : Cardinal; var results : tIcedPointerScanResults; CodeOffset : UInt64 = UInt64( 0 ); ProcessEvent : tIcedPointerScanProcessEvent = nil ) : Cardinal; overload;
    function    PointerScan( Data : TMemoryStream; var results : tIcedPointerScanResults; CodeOffset : UInt64 = UInt64( 0 ); ProcessEvent : tIcedPointerScanProcessEvent = nil ) : Cardinal; overload;
    function    ReferenceScan( Data : PByte; Size : Cardinal; Reference : UInt64; var results : tIcedReferenceScanResults; CodeOffset : UInt64 = UInt64( 0 ); ProcessEvent : tIcedPointerScanProcessEvent = nil ) : Cardinal; overload;
    function    ReferenceScan( Data : TMemoryStream; Reference : UInt64; var results : tIcedReferenceScanResults; CodeOffset : UInt64 = UInt64( 0 ); ProcessEvent : tIcedPointerScanProcessEvent = nil ) : Cardinal; overload;

    function    AssemblyScan( Data : PByte; Size : Cardinal; Assembly : String; var results : tIcedReferenceScanResults; CodeOffset : UInt64 = UInt64( 0 ); Mode : tIcedAssemblyScanMode = asmEqual; ProcessEvent : tIcedPointerScanProcessEvent = nil ) : Cardinal; overload;
    function    AssemblyScan( Data : TMemoryStream; Assembly : String; var results : tIcedReferenceScanResults; CodeOffset : UInt64 = UInt64( 0 ); Mode : tIcedAssemblyScanMode = asmEqual; ProcessEvent : tIcedPointerScanProcessEvent = nil ) : Cardinal; overload;

    function    AssemblyScan( Data : PByte; Size : Cardinal; Assembly : PInstruction; AssemblyCount : Cardinal; var results : tIcedReferenceScanResults; CodeOffset : UInt64 = UInt64( 0 ); Mode : tIcedAssemblyScanMode = asmEqual; ProcessEvent : tIcedPointerScanProcessEvent = nil ) : Cardinal; overload;
    function    AssemblyScan( Data : TMemoryStream; Assembly : PInstruction; AssemblyCount : Cardinal; var results : tIcedReferenceScanResults; CodeOffset : UInt64 = UInt64( 0 ); Mode : tIcedAssemblyScanMode = asmEqual; ProcessEvent : tIcedPointerScanProcessEvent = nil ) : Cardinal; overload;
    function    AssemblyScan( Data : PByte; Size : Cardinal; var Assembly : TInstructionArray; var results : tIcedReferenceScanResults; CodeOffset : UInt64 = UInt64( 0 ); Mode : tIcedAssemblyScanMode = asmEqual; ProcessEvent : tIcedPointerScanProcessEvent = nil ) : Cardinal; overload;
    function    AssemblyScan( Data : TMemoryStream; var Assembly : TInstructionArray; var results : tIcedReferenceScanResults; CodeOffset : UInt64 = UInt64( 0 ); Mode : tIcedAssemblyScanMode = asmEqual; ProcessEvent : tIcedPointerScanProcessEvent = nil ) : Cardinal; overload;

    function    FindInstruction( Data : PByte; Size : Cardinal; Offset : UInt64; CodeOffset : UInt64 = UInt64( 0 ); {$IFDEF AssemblyTools}Details : pIcedDetails = nil;{$ENDIF} DecoderSettings : Cardinal = doNONE ) : Cardinal; overload;
    function    FindInstruction( Data : TMemoryStream; Offset : UInt64; CodeOffset : UInt64 = UInt64( 0 ); {$IFDEF AssemblyTools}Details : pIcedDetails = nil;{$ENDIF} DecoderSettings : Cardinal = doNONE ) : Cardinal; overload;

    function    FindNextInstruction( Data : TMemoryStream; CodeOffset : UInt64; Address : UInt64; var Offset : UInt64; InstructionCount : Word = 1; InstructionBytes : pByteInstruction = nil; Instruction : PString = nil; {$IFDEF AssemblyTools}Details : pIcedDetails = nil;{$ENDIF} DecoderSettings : Cardinal = doNONE ) : Cardinal; overload;
    function    FindNextInstruction( Data : PByte; Size : Cardinal; CodeOffset : UInt64; Address : UInt64; var Offset : UInt64; InstructionCount : Word = 1; InstructionBytes : pByteInstruction = nil; Instruction : PString = nil; {$IFDEF AssemblyTools}Details : pIcedDetails = nil;{$ENDIF} DecoderSettings : Cardinal = doNONE ) : Cardinal; overload;

// MS
//    function    FindPreviousInstructionCount( Data : TMemoryStream; CodeOffset : UInt64; Address : UInt64; var Offset : UInt64; InstructionCount : Word = 1; InstructionBytes : pByteInstruction = nil; Instruction : PString = nil; {$IFDEF AssemblyTools}Details : pIcedDetails = nil;{$ENDIF} DecoderSettings : Cardinal = doNONE ) : Cardinal; overload;
//    function    FindPreviousInstructionCount( Data : PByte; Size : Cardinal; CodeOffset : UInt64; Address : UInt64; var Offset : UInt64; InstructionCount : Word = 1; InstructionBytes : pByteInstruction = nil; Instruction : PString = nil; {$IFDEF AssemblyTools}Details : pIcedDetails = nil;{$ENDIF} DecoderSettings : Cardinal = doNONE ) : Cardinal; overload;
//    function    FindPreviousInstruction( Data : TMemoryStream; CodeOffset : UInt64; Address : UInt64; var Offset : UInt64; DesiredInstructionOffset : UInt64; InstructionBytes : pByteInstruction = nil; Instruction : PString = nil; {$IFDEF AssemblyTools}Details : pIcedDetails = nil;{$ENDIF} DecoderSettings : Cardinal = doNONE ) : Cardinal; overload;
//    function    FindPreviousInstruction( Data : PByte; Size : Cardinal; CodeOffset : UInt64; Address : UInt64; var Offset : UInt64; DesiredInstructionOffset : UInt64; InstructionBytes : pByteInstruction = nil; Instruction : PString = nil; {$IFDEF AssemblyTools}Details : pIcedDetails = nil;{$ENDIF} DecoderSettings : Cardinal = doNONE ) : Cardinal; overload;
  end;

var
  Iced : TIced;

// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
{$IFDEF TEST_FUNCTIONS}
const
  EXAMPLE_RIP: UInt64 = UInt64( $7FFAC46ACDA4 );
  EXAMPLE_CODE: Array [ 0..61 ] of Byte = (
    $48, $89, $5C, $24, $10, $48, $89, $74, $24, $18, $55, $57, $41, $56, $48, $8D,
    $AC, $24, $00, $FF, $FF, $FF, $48, $81, $EC, $00, $02, $00, $00, $48, $8B, $05,
    $18, $57, $0A, $00, $48, $33, $C4, $48, $89, $85, $F0, $00, $00, $00, $4C, $8B,
    $05, $2F, $24, $0A, $00, $48, $8D, $05, $78, $7C, $04, $00, $33, $FF
  );

function Test( StrL : TStringList ) : Boolean;
function Test_Decode( Data : PByte; Size : Cardinal; ARIP : UInt64; AOutput : TStringList; FormatterType : TIcedFormatterType = ftMasm;
                      SymbolResolver : TSymbolResolverCallback = nil;
                      FormatterOptionsProviderCallback : TFormatterOptionsProviderCallback = nil;
                      FormatterOutputCallback : TFormatterOutputCallback = nil;
                      Assembly : Boolean = True; DecodeInfos : Boolean = True; Infos : Boolean = True ) : Boolean;
function  Test_ReEncode( Data : PByte; Size : Cardinal; ARIP : UInt64; {LocalBuffer : Boolean = False;} BlockEncode : Boolean = False; AOutput : TStringList = nil ) : Boolean;
procedure Test_Assemble( AOutput : TStringList; ARIP : UInt64 = UInt64( $00001248FC840000 ) );

function  Benchmark( Data : PByte; Size : Cardinal; Bitness : TIcedBitness; AFormat : Boolean = False ) : Double; overload;
function  Benchmark( FileName : String; Bitness : TIcedBitness; AFormat : Boolean = False ) : Double; overload;
{$ENDIF TEST_FUNCTIONS}

// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
implementation

{$IF CompilerVersion >= 22} // XE
uses
  RegularExpressions;
{$IFEND}

// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
constructor TIcedDecoder.Create( Bitness : TIcedBitness = bt64 );
begin
  inherited Create;
  fHandle   := nil;
  fBitness  := Bitness;
  fData     := nil;
  fSize     := 0;
  {$IF CompilerVersion < 23}{$RANGECHECKS OFF}{$IFEND} // RangeCheck might cause Internal-Error C1118
  fIPosition:= 0;
  {$IFDEF DECODER_LOCAL_POSITION}
  fPosition := 0;
  {$ENDIF DECODER_LOCAL_POSITION}
  {$IF CompilerVersion < 23}{$RANGECHECKS ON}{$IFEND} // RangeCheck might cause Internal-Error C1118
end;

destructor TIcedDecoder.Destroy;
begin
  if ( fHandle <> nil ) then
    IcedFreeMemory( fHandle );

  inherited;
end;

function TIcedDecoder.GetHandle : Pointer;
begin
  result := nil;
  if ( self = nil ) then
    Exit;
  result := fHandle;
end;

function TIcedDecoder.GetBitness : TIcedBitness;
begin
  result := bt16;
  if ( self = nil ) then
    Exit;
  result := fBitness;
//  result := TIcedBitness( Decoder_GetBitness( fHandle ) );
end;

procedure TIcedDecoder.SetBitness( Value : TIcedBitness );
begin
  if ( self = nil ) then
    Exit;
  fBitness := Value;

  if ( fHandle <> nil ) then
    begin
    IcedFreeMemory( fHandle );
    fHandle := nil;
    end;
end;

procedure TIcedDecoder.SetData( Data : PByte; Size : NativeUInt; IP : UInt64 = UInt64( 0 ); Options : Cardinal = doNONE );
begin
  if ( self = nil ) then
    Exit;
  if NOT uIced.Imports.IsInitDLL then
    Exit;
  if ( fHandle <> nil ) then
    IcedFreeMemory( fHandle );

  if ( fData = Data ) AND ( fSize = 0 ) then
    begin
    Position := 0;
    Exit;
    end;
  fData     := Data;
  fSize     := Size;
  {$IF CompilerVersion < 23}{$RANGECHECKS OFF}{$IFEND} // RangeCheck might cause Internal-Error C1118
  fIPosition:= 0;
  {$IFDEF DECODER_LOCAL_POSITION}
  fPosition := 0;
  {$ENDIF DECODER_LOCAL_POSITION}
  {$IF CompilerVersion < 23}{$RANGECHECKS ON}{$IFEND} // RangeCheck might cause Internal-Error C1118

  fHandle := Decoder_Create( Cardinal( fBitness ), Data, Size, IP, Options );
end;

function TIcedDecoder.CanDecode : Boolean;
begin
  result := False;
  if ( self = nil ) then
    Exit;
  if ( fHandle = nil ) then
    Exit;
  result := Decoder_CanDecode( fHandle );
end;

function TIcedDecoder.GetIP : UInt64;
begin
  {$IF CompilerVersion < 23}{$RANGECHECKS OFF}{$IFEND} // RangeCheck might cause Internal-Error C1118
  result := 0;
  {$IF CompilerVersion < 23}{$RANGECHECKS ON}{$IFEND} // RangeCheck might cause Internal-Error C1118
  if ( self = nil ) then
    Exit;
  if ( fHandle = nil ) then
    Exit;
  {$IF CompilerVersion < 23}{$RANGECHECKS OFF}{$IFEND} // RangeCheck might cause Internal-Error C1118
  result := Decoder_GetIP( fHandle );
  {$IF CompilerVersion < 23}{$RANGECHECKS ON}{$IFEND} // RangeCheck might cause Internal-Error C1118
end;

procedure TIcedDecoder.SetIP( Value : UInt64 );
begin
  if ( self = nil ) then
    Exit;
  if ( fHandle = nil ) then
    Exit;
  Decoder_SetIP( fHandle, Value );
end;

function TIcedDecoder.GetPosition : NativeUInt;
begin
  {$IF CompilerVersion < 23}{$RANGECHECKS OFF}{$IFEND} // RangeCheck might cause Internal-Error C1118
  result := 0;
  {$IF CompilerVersion < 23}{$RANGECHECKS ON}{$IFEND} // RangeCheck might cause Internal-Error C1118
  if ( self = nil ) then
    Exit;
  if ( fHandle = nil ) then
    Exit;
  {$IF CompilerVersion < 23}{$RANGECHECKS OFF}{$IFEND} // RangeCheck might cause Internal-Error C1118
  {$IFDEF DECODER_LOCAL_POSITION}
  result := fPosition;
  {$ELSE}
  result := Decoder_GetPosition( fHandle );
  {$ENDIF DECODER_LOCAL_POSITION}
  {$IF CompilerVersion < 23}{$RANGECHECKS ON}{$IFEND} // RangeCheck might cause Internal-Error C1118
end;

procedure TIcedDecoder.SetPosition( Value : NativeUInt );
begin
  if ( self = nil ) then
    Exit;
  if ( fHandle = nil ) then
    Exit;

  {$IFDEF DECODER_LOCAL_POSITION}
  if ( fPosition = Value ) then
    Exit;
  if ( Value > fSize ) then
    Exit;
  fPosition := Value;
  {$ELSE}
  if ( Value > fSize ) then
    Exit;
  {$ENDIF DECODER_LOCAL_POSITION}

  Decoder_SetPosition( fHandle, Value );
end;

function TIcedDecoder.GetMaxPosition : NativeUInt;
begin
  {$IF CompilerVersion < 23}{$RANGECHECKS OFF}{$IFEND} // RangeCheck might cause Internal-Error C1118
  result := 0;
  {$IF CompilerVersion < 23}{$RANGECHECKS ON}{$IFEND} // RangeCheck might cause Internal-Error C1118
  if ( self = nil ) then
    Exit;
  if ( fHandle = nil ) then
    Exit;
  {$IF CompilerVersion < 23}{$RANGECHECKS OFF}{$IFEND} // RangeCheck might cause Internal-Error C1118
  result := fSize;
//  result := Decoder_GetMaxPosition( fHandle );
  {$IF CompilerVersion < 23}{$RANGECHECKS ON}{$IFEND} // RangeCheck might cause Internal-Error C1118
end;

function TIcedDecoder.GetInstructionFirstByte : PByte;
begin
  result := nil;
  if ( self = nil ) then
    Exit;
  if ( fHandle = nil ) then
    Exit;
  if ( fData = nil ) OR ( fSize = 0 ) then
    Exit;

  {$IFDEF DECODER_LOCAL_POSITION}
  if ( fPosition = 0 ) then
    Exit;
  {$ELSE}
  if ( Position = 0 ) then
    Exit;
  {$ENDIF DECODER_LOCAL_POSITION}

  result := PByte( PAnsiChar( fData ) + fIPosition );
end;

function TIcedDecoder.GetCurrentByte : PByte;
begin
  result := nil;
  if ( self = nil ) then
    Exit;
  if ( fHandle = nil ) then
    Exit;
  if ( fData = nil ) OR ( fSize = 0 ) then
    Exit;

  {$IFDEF DECODER_LOCAL_POSITION}
  result := PByte( PAnsiChar( fData ) + fPosition );
  {$ELSE}
  result := PByte( PAnsiChar( fData ) + Position );
  {$ENDIF DECODER_LOCAL_POSITION}
end;

function TIcedDecoder.GetLastError : TDecoderError;
begin
  result.DecoderError := deNone;
  if ( self = nil ) then
    Exit;
  if ( fHandle = nil ) then
    Exit;
  result := Decoder_GetLastError( fHandle );
end;

procedure TIcedDecoder.Decode( var Instruction : TInstruction );
begin
  if ( self = nil ) then
    begin
    FillChar( Instruction, SizeOf( Instruction ), 0 );
    Exit;
    end;
  if ( fHandle = nil ) then
    begin
    FillChar( Instruction, SizeOf( Instruction ), 0 );
    Exit;
    end;

  {$IF CompilerVersion < 23}{$RANGECHECKS OFF}{$IFEND} // RangeCheck might cause Internal-Error C1118
  {$IFDEF DECODER_LOCAL_POSITION}
  fIPosition := fPosition;
  {$ELSE}
  fIPosition := Position;
  {$ENDIF DECODER_LOCAL_POSITION}
  {$IF CompilerVersion < 23}{$RANGECHECKS ON}{$IFEND} // RangeCheck might cause Internal-Error C1118

  Decoder_Decode( fHandle, Instruction );

  {$IFDEF DECODER_LOCAL_POSITION}
  {$IF CompilerVersion < 23}{$RANGECHECKS OFF}{$IFEND} // RangeCheck might cause Internal-Error C1118
//  fPosition := Position;
  Inc( fPosition, Instruction.len );
  {$IF CompilerVersion < 23}{$RANGECHECKS ON}{$IFEND} // RangeCheck might cause Internal-Error C1118
  {$ENDIF DECODER_LOCAL_POSITION}
end;

{$IFDEF AssemblyTools}
procedure TIcedDecoder.Decode( var Instruction : TInstruction; var Details : TIcedDetails );
const
  JUMPTARGET_BLOCKSIZE = 1000;
var
  Offsets : TConstantOffsets;
begin
  if ( self = nil ) then
    begin
    FillChar( Instruction, SizeOf( Instruction ), 0 );
//    FillChar( Detail, SizeOf( Detail ), 0 );
    Exit;
    end;
  if ( fHandle = nil ) then
    begin
    FillChar( Instruction, SizeOf( Instruction ), 0 );
//    FillChar( Detail, SizeOf( Detail ), 0 );
    Exit;
    end;

  {$IF CompilerVersion < 23}{$RANGECHECKS OFF}{$IFEND} // RangeCheck might cause Internal-Error C1118
  {$IFDEF DECODER_LOCAL_POSITION}
  fIPosition := fPosition;
  {$ELSE}
  fIPosition := Position;
  {$ENDIF DECODER_LOCAL_POSITION}
  {$IF CompilerVersion < 23}{$RANGECHECKS ON}{$IFEND} // RangeCheck might cause Internal-Error C1118

  Decoder_Decode( fHandle, Instruction );

  Details.Items[ Details.Count ].Code                 := Instruction.code;
  Move( Instruction.Regs[ 0 ], Details.Items[ Details.Count ].Registers[ 0 ], Length( Details.Items[ Details.Count ].Registers )*SizeOf( Details.Items[ Details.Count ].Registers[ 0 ] ) );
  {$IF CompilerVersion < 23}{$RANGECHECKS OFF}{$IFEND} // RangeCheck might cause Internal-Error C1118
  Details.Items[ Details.Count ].Offset               := Instruction.RIP;
  {$IF CompilerVersion < 23}{$RANGECHECKS ON}{$IFEND} // RangeCheck might cause Internal-Error C1118
  Details.Items[ Details.Count ].Size                 := Instruction.len;

  FillChar( Details.Items[ Details.Count ].Registers_, SizeOf( Details.Items[ Details.Count ].Registers_ ), 0 ); // MS ?

//  Details.Items[ Details.Count ].Registers_.Target    := Instruction. // MS working for all but lea
//  Details.Items[ Details.Count ].Registers_.Source    := Instruction.
//  Details.Items[ Details.Count ].Registers_.Parameter := Instruction.

{
LEA
Multiplier is always Byte 4 before displacement
If Length = 3 or Displacement starts at 3 its not used
488D84 1A 00010000 = 1A = 1
488D84 5A 00010000 = 1A = 2
488D84 9A 00010000 = 1A = 4
488D84 DA 00010000 = 1A = 8

488D841A00010000 lea rax, [rdx+rbx+0x100]
488D845A00010000 lea rax, [rdx+rbx*2+0x100]
488D849A00010000 lea rax, [rdx+rbx*4+0x100]
488D84DA00010000 lea rax, [rdx+rbx*8+0x100]

488D041A lea rax, [rdx+rbx*1]
488D045A lea rax, [rdx+rbx*2]
488D049A lea rax, [rdx+rbx*4]
488D04DA lea rax, [rdx+rbx*8]

678D041A lea eax, [edx+ebx*1]
678D045A lea eax, [edx+ebx*2]
678D049A lea eax, [edx+ebx*4]
678D04DA lea eax, [edx+ebx*8]
}

  Decoder_GetConstantOffsets( fHandle, Instruction, Offsets );

  {if ForceMask AND ( Offsets.displacement_offset <> 0 ) then
    Details.Items[ Details.Count ].Mask := True
  else} if ( Offsets.displacement_offset <> 0 ) AND ( Offsets.displacement_size = 4 ) then
    Details.Items[ Details.Count ].Mask := ( ABS( Integer( Instruction.mem_displ-Instruction.Rip-Instruction.Len ) ) >= MASKING_OFFSET ); // value is bigger than 65535 (positive and negative)

  if Details.Items[ Details.Count ].IsJump AND ( ( Instruction.op_kinds[ 0 ] <> okRegister_ ) OR ( Instruction.mem_displ <> 0 ) ) then
    begin
    Details.Items[ Details.Count ].JumpTargetID := Details.TargetCount;
    Inc( Details.TargetCount );
    if ( Details.TargetCount >= Length( Details.JumpTargets ) ) then
      SetLength( Details.JumpTargets, Details.TargetCount+JUMPTARGET_BLOCKSIZE );

    FillChar( Details.JumpTargets[ Details.Items[ Details.Count ].JumpTargetID ], SizeOf( Details.JumpTargets[ Details.Items[ Details.Count ].JumpTargetID ] ), 0 );
//    Details.JumpTargets[ Details.Items[ Details.Count ].JumpTargetID ].IsRegJump     := Instruction.IsRegJump;
    {$IF CompilerVersion < 23}{$RANGECHECKS OFF}{$IFEND} // RangeCheck might cause Internal-Error C1118
    Details.JumpTargets[ Details.Items[ Details.Count ].JumpTargetID ].Target        := Instruction.mem_displ;
    {$IF CompilerVersion < 23}{$RANGECHECKS ON}{$IFEND} // RangeCheck might cause Internal-Error C1118
    end
  else
    Details.Items[ Details.Count ].JumpTargetID := 0;

  {$IFDEF DECODER_LOCAL_POSITION}
//  fPosition := Position;
  Inc( fPosition, Instruction.len );
  {$ENDIF DECODER_LOCAL_POSITION}
end;
{$ENDIF AssemblyTools}

procedure TIcedDecoder.GetConstantOffsets( var Instruction : TInstruction; var ConstantOffsets : TConstantOffsets );
begin
  FillChar( ConstantOffsets, SizeOf( ConstantOffsets ), 0 );
  if ( self = nil ) then
    Exit;
  if ( fHandle = nil ) then
    Exit;
  Decoder_GetConstantOffsets( fHandle, Instruction, ConstantOffsets );
end;

// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
constructor TIcedEncoder.Create( Bitness : TIcedBitness = bt64; Capacity : NativeUInt = 0 );
begin
  inherited Create;
  fHandle  := nil;
  fBitness := Bitness;
  {$IF CompilerVersion < 23}{$RANGECHECKS OFF}{$IFEND} // RangeCheck might cause Internal-Error C1118
  fCapacity := Capacity;
  {$IF CompilerVersion < 23}{$RANGECHECKS ON}{$IFEND} // RangeCheck might cause Internal-Error C1118
end;

destructor TIcedEncoder.Destroy;
begin
  if ( fHandle <> nil ) then
    IcedFreeMemory( fHandle );

  inherited;
end;

function TIcedEncoder.GetHandle : Pointer;
begin
  result := nil;
  if ( self = nil ) then
    Exit;
  result := fHandle;
end;

function TIcedEncoder.GetBitness : TIcedBitness;
begin
  result := bt16;
  if ( self = nil ) then
    Exit;
  result := fBitness;
//  result := TIcedBitness( Encoder_GetBitness( fHandle ) );
end;

procedure TIcedEncoder.SetBitness( Value : TIcedBitness );
begin
  if ( self = nil ) then
    Exit;
  fBitness := Value;

  if ( fHandle <> nil ) then
    begin
    IcedFreeMemory( fHandle );
    fHandle := nil;
    end;
end;

function TIcedEncoder.GetCapacity : NativeUInt;
begin
  {$IF CompilerVersion < 23}{$RANGECHECKS OFF}{$IFEND} // RangeCheck might cause Internal-Error C1118
  result := 0;
  {$IF CompilerVersion < 23}{$RANGECHECKS ON}{$IFEND} // RangeCheck might cause Internal-Error C1118
  if ( self = nil ) then
    Exit;
  {$IF CompilerVersion < 23}{$RANGECHECKS OFF}{$IFEND} // RangeCheck might cause Internal-Error C1118
  result := fCapacity;
  {$IF CompilerVersion < 23}{$RANGECHECKS ON}{$IFEND} // RangeCheck might cause Internal-Error C1118
end;

procedure TIcedEncoder.SetCapacity( Value : NativeUInt );
begin
  if ( self = nil ) then
    Exit;
  {$IF CompilerVersion < 23}{$RANGECHECKS OFF}{$IFEND} // RangeCheck might cause Internal-Error C1118
  fCapacity := Value;
  {$IF CompilerVersion < 23}{$RANGECHECKS ON}{$IFEND} // RangeCheck might cause Internal-Error C1118

  if ( fHandle <> nil ) then
    begin
    IcedFreeMemory( fHandle );
    fHandle := nil;
    end;
end;

procedure TIcedEncoder.Encode( var Instruction : TInstruction );
begin
  if ( self = nil ) then
    Exit;
  if NOT uIced.Imports.IsInitDLL then
    Exit;
  if ( fHandle = nil ) then
    fHandle := Encoder_Create( Cardinal( fBitness ), fCapacity );

  Encoder_Encode( fHandle, Instruction );
end;

procedure TIcedEncoder.Write( Byte : Byte );
begin
  if ( self = nil ) then
    Exit;
  if ( fHandle = nil ) then
    Exit;
  Encoder_WriteByte( fHandle, Byte );
end;

function TIcedEncoder.GetBuffer( Buffer : PByte; Size : NativeUInt ) : Boolean;
begin
  result := False;
  FillChar( Buffer^, Size, 0 );
  if ( self = nil ) then
    Exit;
  if ( fHandle = nil ) then
    Exit;
  result := Encoder_GetBuffer( fHandle, Buffer, Size );
end;

{
function TIcedEncoder.SetBuffer( Buffer : PByte; Size : NativeUInt ) : Boolean;
begin
  result := False;
  if ( self = nil ) then
    Exit;
  if ( fHandle = nil ) then
    fHandle := Encoder_Create( Cardinal( fBitness ) );
  FillChar( Buffer^, Size, 0 );
  result := Encoder_SetBuffer( fHandle, Buffer, Size );
end;
}

procedure TIcedEncoder.GetConstantOffsets( var ConstantOffsets : TConstantOffsets );
begin
  FillChar( ConstantOffsets, SizeOf( ConstantOffsets ), 0 );
  if ( self = nil ) then
    Exit;
  if ( fHandle = nil ) then
    Exit;
  Encoder_GetConstantOffsets( fHandle, ConstantOffsets );
end;

function TIcedEncoder.GetPreventVex2 : Boolean;
begin
  result := False;
  if ( self = nil ) then
    Exit;
  result := Encoder_GetPreventVex2( fHandle );
end;

procedure TIcedEncoder.SetPreventVex2( Value : Boolean );
begin
  if ( self = nil ) then
    Exit;
  if ( fHandle = nil ) then
    Exit;
  {result := }Encoder_SetPreventVex2( fHandle, Value );
end;

function TIcedEncoder.GetVexWig : Cardinal;
begin
  result := 0;
  if ( self = nil ) then
    Exit;
  result := Encoder_GetVexWig( fHandle );
end;

procedure TIcedEncoder.SetVexWig( Value : Cardinal );
begin
  if ( self = nil ) then
    Exit;
  if ( fHandle = nil ) then
    Exit;
  {result := }Encoder_SetVexWig( fHandle, Value );
end;

function TIcedEncoder.GetVexLig : Cardinal;
begin
  result := 0;
  if ( self = nil ) then
    Exit;
  result := Encoder_GetVexLig( fHandle );
end;

procedure TIcedEncoder.SetVexLig( Value : Cardinal );
begin
  if ( self = nil ) then
    Exit;
  if ( fHandle = nil ) then
    Exit;
  {result := }Encoder_SetVexLig( fHandle, Value );
end;

function TIcedEncoder.GetEvexWig : Cardinal;
begin
  result := 0;
  if ( self = nil ) then
    Exit;
  result := Encoder_GetEvexWig( fHandle );
end;

procedure TIcedEncoder.SetEvexWig( Value : Cardinal );
begin
  if ( self = nil ) then
    Exit;
  if ( fHandle = nil ) then
    Exit;
  {result := }Encoder_SetEvexWig( fHandle, Value );
end;

function TIcedEncoder.GetEvexLig : Cardinal;
begin
  result := 0;
  if ( self = nil ) then
    Exit;
  result := Encoder_GetEvexLig( fHandle );
end;

procedure TIcedEncoder.SetEvexLig( Value : Cardinal );
begin
  if ( self = nil ) then
    Exit;
  if ( fHandle = nil ) then
    Exit;
  {result := }Encoder_SetEvexLig( fHandle, Value );
end;

function TIcedEncoder.GetMvexWig : Cardinal;
begin
  result := 0;
  if ( self = nil ) then
    Exit;
  result := Encoder_GetMvexWig( fHandle );
end;

procedure TIcedEncoder.SetMvexWig( Value : Cardinal );
begin
  if ( self = nil ) then
    Exit;
  if ( fHandle = nil ) then
    Exit;
  {result := }Encoder_SetMvexWig( fHandle, Value );
end;

// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
constructor TIcedBlockEncoder.Create( Bitness : TIcedBitness = bt64; Options : Cardinal = beoNONE );
begin
  inherited Create;
  fBitness := Bitness;
  fOptions := Options;
  fMemory  := nil;
end;

destructor TIcedBlockEncoder.Destroy;
begin
  if ( fMemory <> nil ) then
    IcedFreeMemory( fMemory );

  inherited;
end;

function TIcedBlockEncoder.GetBitness : TIcedBitness;
begin
  result := bt16;
  if ( self = nil ) then
    Exit;
  result := fBitness;
//  result := TIcedBitness( Decoder_GetBitness( fHandle ) );
end;

function TIcedBlockEncoder.GetOptions : Cardinal;
begin
  result := beoNONE;
  if ( self = nil ) then
    Exit;
  result := fOptions;
end;

procedure TIcedBlockEncoder.SetOptions( Value : Cardinal );
begin
  if ( self = nil ) then
    Exit;
  fOptions := Value;
end;

procedure TIcedBlockEncoder.SetBitness( Value : TIcedBitness );
begin
  if ( self = nil ) then
    Exit;
  fBitness := Value;
end;

procedure TIcedBlockEncoder.Encode( RIP : UInt64; var Instructions : TInstruction; Count : NativeUInt; var Results : TBlockEncoderResult );
begin
  if ( self = nil ) then
    Exit;
  if NOT uIced.Imports.IsInitDLL then
    Exit;

  if ( fMemory <> nil ) then
    IcedFreeMemory( fMemory );
  fMemory := BlockEncoder( Cardinal( fBitness ), RIP, Instructions, Count, Results, fOptions );
end;

procedure TIcedBlockEncoder.Encode( RIP : UInt64; var Instructions : TInstructionArray; var Results : TBlockEncoderResult );
begin
  Encode( RIP, Instructions[ 0 ], NativeUInt( Length( Instructions ) ), Results );
end;

procedure TIcedBlockEncoder.Encode( RIP : UInt64; var Instructions : TInstructionList; var Results : TBlockEncoderResult );
begin
  Encode( RIP, Instructions.Pointer^, Instructions.Count, Results );
end;

procedure TIcedBlockEncoder.Clear;
begin
  if ( self = nil ) then
    Exit;
  if ( fMemory <> nil ) then
    begin
    IcedFreeMemory( fMemory );
    fMemory := nil
    end;
end;

// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
constructor TInstructionInfoFactory.Create( Options : Cardinal = iioNone );
begin
  inherited Create;
  if uIced.Imports.IsInitDLL then
    fHandle := InstructionInfoFactory_Create
  else
    fHandle := nil;
  fOptions := Options;
end;

destructor TInstructionInfoFactory.Destroy;
begin
  if ( fHandle <> nil ) then
    IcedFreeMemory( fHandle );

  inherited;
end;

function TInstructionInfoFactory.GetHandle : Pointer;
begin
  result := nil;
  if ( self = nil ) then
    Exit;
  result := fHandle;
end;

function TInstructionInfoFactory.GetOptions : Cardinal;
begin
  result := beoNONE;
  if ( self = nil ) then
    Exit;
  result := fOptions;
end;

procedure TInstructionInfoFactory.SetOptions( Value : Cardinal );
begin
  if ( self = nil ) then
    Exit;
  fOptions := Value;
end;

procedure TInstructionInfoFactory.Info( var Instruction: TInstruction; var InstructionInfo : TInstructionInfo );
begin
  FillChar( InstructionInfo, SizeOf( InstructionInfo ), 0 );
  if ( self = nil ) then
    Exit;
  if ( fHandle = nil ) then
    Exit;
  {result := }InstructionInfoFactory_Info( fHandle, Instruction, InstructionInfo, fOptions );
end;

// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
function SymbolResolverCallback( var Instruction: TInstruction; Operand: Cardinal; InstructionOperand : Cardinal; Address: UInt64; Size: Cardinal; UserData : Pointer ) : PAnsiChar; cdecl;
begin
  if ( userData = nil ) then
    begin
    result := '';
    Exit;
    end;
  {$IFDEF AssemblyTools}
  if ( TIcedFormatter( UserData ).fSymbolHandler <> nil ) then
    begin
    result := PAnsiChar( AnsiString( TIcedFormatter( UserData ).fSymbolHandler.Symbol( Address ) ) );
    if ( result = '' ) AND ( @TIcedFormatter( UserData ).fSymbolResolver <> nil ) then
      result := PAnsiChar( TIcedFormatter( UserData ).fSymbolResolver( Instruction, Operand, InstructionOperand, Address, Size, UserData ) );
    end
  else
  {$ENDIF AssemblyTools}
  if ( @TIcedFormatter( UserData ).fSymbolResolver <> nil ) then
    result := PAnsiChar( TIcedFormatter( UserData ).fSymbolResolver( Instruction, Operand, InstructionOperand, Address, Size, UserData ) )
  else
    result := '';
end;

constructor TIcedFormatter.Create( FormatterType : TIcedFormatterType = DEFAULT_FORMATTER; SymbolResolver : TSymbolResolverCallback = nil; OptionsProvider : TFormatterOptionsProviderCallback = nil );
begin
  inherited Create;
  fType            := FormatterType;
  fHandle          := nil;
  FillChar( fOptions, SizeOf( fOptions ), 0 );
  fSymbolResolver  := SymbolResolver;
  fOptionsProvider := OptionsProvider;
  fFormatterOutput := nil;
  fOutputHandle    := nil;

  fSpecialized.ENABLE_SYMBOL_RESOLVER              := False; // Internally Handled
  fSpecialized.ENABLE_DB_DW_DD_DQ                  := False;
  fSpecialized.verify_output_has_enough_bytes_left := True;

  fShowSymbols     := True;
  {$IFDEF AssemblyTools}
  fSymbolHandler   := nil;
  {$ENDIF AssemblyTools}

  CreateHandle;
end;

destructor TIcedFormatter.Destroy;
begin
  if ( fOutputHandle <> nil ) then
    IcedFreeMemory( fOutputHandle );
  if ( fHandle <> nil ) then
    IcedFreeMemory( fHandle );

  inherited;
end;

procedure TIcedFormatter.CreateHandle( KeepConfiguration : Boolean = False );
var
  SymbolResolver : TSymbolResolverCallback;
  tOptions       : TIcedFormatterSettings;
begin
  if ( self = nil ) then
    Exit;
  if NOT uIced.Imports.IsInitDLL then
    Exit;

  if ( fHandle <> nil ) then
    IcedFreeMemory( fHandle );

  if fShowSymbols AND ( ( @fSymbolResolver <> nil ){$IFDEF AssemblyTools} OR ( fSymbolHandler <> nil ){$ENDIF} ) then
    SymbolResolver := SymbolResolverCallback
  else
    SymbolResolver := nil;

  fSpecialized.ENABLE_SYMBOL_RESOLVER := ( @SymbolResolver <> nil );

  case fType of
//    ftMasm        : fHandle := MasmFormatter_Create( SymbolResolver, fOptionsProvider, Pointer( self ) );
    ftNasm        : fHandle := NasmFormatter_Create( SymbolResolver, fOptionsProvider, Pointer( self ) );
    ftGas         : fHandle := GasFormatter_Create( SymbolResolver, fOptionsProvider, Pointer( self ) );
    ftIntel       : fHandle := IntelFormatter_Create( SymbolResolver, fOptionsProvider, Pointer( self ) );
    ftFast        : fHandle := FastFormatter_Create( SymbolResolver, Pointer( self ) );
    ftSpecialized : fHandle := SpecializedFormatter_Create( SymbolResolver, Pointer( self ) );
  else
    fHandle := MasmFormatter_Create( SymbolResolver, fOptionsProvider, Pointer( self ) );
  end;

  if KeepConfiguration then
    begin
    tOptions  := fOptions;
    fOptions := GetOptions;
    SetOptions( tOptions );
    end
  else
    begin
    fOptions := GetOptions;

    if ( fType = ftCapstone ) then
      SetCapstoneOptions;
    end;
end;

procedure TIcedFormatter.DefaultSettings;
begin
  if ( self = nil ) then
    Exit;
  CreateHandle;
end;

function TIcedFormatter.GetHandle : Pointer;
begin
  result := nil;
  if ( self = nil ) then
    Exit;
  result := fHandle;
end;

procedure TIcedFormatter.SetType( Value : TIcedFormatterType );
begin
  if ( self = nil ) then
    Exit;
  if ( fType = Value ) then
    Exit;

  fType := Value;
  CreateHandle;
end;

function TIcedFormatter.GetSymbolResolver : TSymbolResolverCallback;
begin
  result := nil;
  if ( self = nil ) then
    Exit;
  result := fSymbolResolver;
end;

procedure TIcedFormatter.SetSymbolResolver( Value : TSymbolResolverCallback );
begin
  if ( self = nil ) then
    Exit;
  if ( @fSymbolResolver = @Value ) then
    Exit;

  fSymbolResolver := Value;
  CreateHandle( True{KeepConfiguration} );
end;

function TIcedFormatter.GetOptionsProvider : TFormatterOptionsProviderCallback;
begin
  result := nil;
  if ( self = nil ) then
    Exit;
  result := fOptionsProvider;
end;

procedure TIcedFormatter.SetOptionsProvider( Value : TFormatterOptionsProviderCallback );
begin
  if ( self = nil ) then
    Exit;
  if ( @fOptionsProvider = @Value ) then
    Exit;

  if ( fType in [ ftFast, ftSpecialized ] ) then
    Exit;

  fOptionsProvider := Value;
  CreateHandle( True{KeepConfiguration} );
end;

function TIcedFormatter.GetFormatterOutput : TFormatterOutputCallback;
begin
  result := nil;
  if ( self = nil ) then
    Exit;
  result := fFormatterOutput;
end;

procedure TIcedFormatter.SetFormatterOutput( Value : TFormatterOutputCallback );
begin
  if ( self = nil ) then
    Exit;
  if ( @fFormatterOutput = @Value ) then
    Exit;

  if ( fType in [ ftFast, ftSpecialized ] ) then
    Exit;

  fFormatterOutput := Value;
  if ( fOutputHandle <> nil ) then
    IcedFreeMemory( fOutputHandle );
  if ( @Value <> nil ) then
    fOutputHandle := FormatterOutput_Create( Value, Pointer( self ) )
  else
    fOutputHandle := nil;
end;

function TIcedFormatter.FormatToString( var Instruction: TInstruction ) : AnsiString;
var
  tOutput : Array [ 0..255 ] of AnsiChar;
begin
  result := '';
  if ( self = nil ) then
    Exit;

  if ( fHandle = nil ) then
    Exit;

  FillChar( tOutput[ 0 ], Length( tOutput ), 0 );
  case fType of
//    ftMasm        : MasmFormatter_Format( fHandle, Instruction, @tOutput[ 0 ], Length( tOutput ) );
    ftNasm        : NasmFormatter_Format( fHandle, Instruction, @tOutput[ 0 ], Length( tOutput ) );
    ftGas         : GasFormatter_Format( fHandle, Instruction, @tOutput[ 0 ], Length( tOutput ) );
    ftIntel       : IntelFormatter_Format( fHandle, Instruction, @tOutput[ 0 ], Length( tOutput ) );
    ftFast        : FastFormatter_Format( fHandle, Instruction, @tOutput[ 0 ], Length( tOutput ) );
    ftSpecialized : SpecializedFormatter_Format( fHandle, fSpecialized.Options, Instruction, @tOutput[ 0 ], Length( tOutput ) ); // M
  else
    MasmFormatter_Format( fHandle, Instruction, @tOutput[ 0 ], Length( tOutput ) );
  end;
  result := AnsiString( tOutput )
end;

procedure TIcedFormatter.Format( var Instruction: TInstruction; AOutput: PAnsiChar; Size : NativeUInt );
begin
  if ( AOutput = nil ) then
    Exit;
  if ( Size = 0 ) then
    Exit;

  if ( self = nil ) then
    begin
    FillChar( AOutput^, Size, 0 );
    Exit;
    end;

  if ( fHandle = nil ) then
    begin
    FillChar( AOutput^, Size, 0 );
    Exit;
    end;

  case fType of
//    ftMasm        : MasmFormatter_Format( fHandle, Instruction, AOutput, Size );
    ftNasm        : NasmFormatter_Format( fHandle, Instruction, AOutput, Size );
    ftGas         : GasFormatter_Format( fHandle, Instruction, AOutput, Size );
    ftIntel       : IntelFormatter_Format( fHandle, Instruction, AOutput, Size );
    ftFast        : FastFormatter_Format( fHandle, Instruction, AOutput, Size );
    ftSpecialized : SpecializedFormatter_Format( fHandle, fSpecialized.Options, Instruction, AOutput, Size ); // M
  else
    MasmFormatter_Format( fHandle, Instruction, AOutput, Size );
  end;
end;

procedure TIcedFormatter.Format( var Instruction: TInstruction ); // TFormatterOutputCallback
begin
  if ( self = nil ) then
    Exit;

  if ( fHandle = nil ) then
    Exit;

  if ( fOutputHandle = nil ) then
    Exit;

  case fType of
    ftMasm        : MasmFormatter_FormatCallback( fHandle, Instruction, fOutputHandle );
    ftNasm        : NasmFormatter_FormatCallback( fHandle, Instruction, fOutputHandle );
    ftGas         : GasFormatter_FormatCallback( fHandle, Instruction, fOutputHandle );
    ftIntel       : IntelFormatter_FormatCallback( fHandle, Instruction, fOutputHandle );
  else
    Exit;
  end;
end;

// Options
function TIcedFormatter.GetOptions : TIcedFormatterSettings;
var
  C : Array [ 0..255 ] of AnsiChar;
begin
  FillChar( result, SizeOf( result ), 0 );
  if ( self = nil ) then
    Exit;
  if NOT uIced.Imports.IsInitDLL then
    Exit;

  if ( fType in [ ftMasm, ftCapstone ] ) then
    begin
    // Masm
    result.AddDsPrefix32                   := MasmFormatter_GetAddDsPrefix32( fHandle );
    result.SymbolDisplacementInBrackets    := MasmFormatter_GetSymbolDisplacementInBrackets( fHandle );
    result.DisplacementInBrackets          := MasmFormatter_GetDisplacementInBrackets( fHandle );
    end;

  if ( fType = ftNasm ) then
    // Nasm
    result.ShowSignExtendedImmediateSize   := NasmFormatter_GetShowSignExtendedImmediateSize( fHandle );

  if ( fType = ftGas ) then
    begin
    // Gas
    result.NakedRegisters                  := GasFormatter_GetNakedRegisters( fHandle );
    result.ShowMnemonicSizeSuffix          := GasFormatter_GetShowMnemonicSizeSuffix( fHandle );
    result.SpaceAfterMemoryOperandComma    := GasFormatter_GetSpaceAfterMemoryOperandComma( fHandle );
    end;

  if ( fType = ftSpecialized ) then
    begin
    // Specialized
    result.UseHexPrefix                    := SpecializedFormatter_GetUseHexPrefix( fHandle );
    result.AlwaysShowMemorySize            := SpecializedFormatter_GetAlwaysShowMemorySize( fHandle );
    end;

  // Common
  result.SpaceAfterOperandSeparator      := Formatter_GetSpaceAfterOperandSeparator( fHandle, fType );
  result.AlwaysShowSegmentRegister       := Formatter_GetAlwaysShowSegmentRegister( fHandle, fType );
  result.UsePseudoOps                    := Formatter_GetUsePseudoOps( fHandle, fType );
  result.RipRelativeAddresses            := Formatter_GetRipRelativeAddresses( fHandle, fType );
  result.ShowSymbolAddress               := Formatter_GetShowSymbolAddress( fHandle, fType );
  result.UpperCaseHex                    := Formatter_GetUpperCaseHex( fHandle, fType );

  if NOT ( fType in [ ftFast, ftSpecialized ] ) then
    begin
    // Common (All but Fast/Specialized)
    result.UpperCasePrefixes               := Formatter_GetUpperCasePrefixes( fHandle, fType );
    result.UpperCaseMnemonics              := Formatter_GetUpperCaseMnemonics( fHandle, fType );
    result.UpperCaseRegisters              := Formatter_GetUpperCaseRegisters( fHandle, fType );
    result.UpperCaseKeyWords               := Formatter_GetUpperCaseKeyWords( fHandle, fType );
    result.UpperCaseDecorators             := Formatter_GetUpperCaseDecorators( fHandle, fType );
    result.UpperCaseEverything             := Formatter_GetUpperCaseEverything( fHandle, fType );
    result.FirstOperandCharIndex           := Formatter_GetFirstOperandCharIndex( fHandle, fType );
    result.TabSize                         := Formatter_GetTabSize( fHandle, fType );
    result.SpaceAfterMemoryBracket         := Formatter_GetSpaceAfterMemoryBracket( fHandle, fType );
    result.SpaceBetweenMemoryAddOperators  := Formatter_GetSpaceBetweenMemoryAddOperators( fHandle, fType );
    result.SpaceBetweenMemoryMulOperators  := Formatter_GetSpaceBetweenMemoryMulOperators( fHandle, fType );
    result.ScaleBeforeIndex                := Formatter_GetScaleBeforeIndex( fHandle, fType );
    result.AlwaysShowScale                 := Formatter_GetAlwaysShowScale( fHandle, fType );
    result.ShowZeroDisplacements           := Formatter_GetShowZeroDisplacements( fHandle, fType );
    Formatter_GetHexPrefix( fHandle, fType, @C[ 0 ], Length( C ) );
    result.HexPrefix                       := C;
    Formatter_GetHexSuffix( fHandle, fType, @C[ 0 ], Length( C ) );
    result.HexSuffix                       := C;
    result.HexDigitGroupSize               := Formatter_GetHexDigitGroupSize( fHandle, fType );
    Formatter_GetDecimalPrefix( fHandle, fType, @C[ 0 ], Length( C ) );
    result.DecimalPrefix                   := C;
    Formatter_GetDecimalSuffix( fHandle, fType, @C[ 0 ], Length( C ) );
    result.DecimalSuffix                   := C;
    result.DecimalDigitGroupSize           := Formatter_GetDecimalDigitGroupSize( fHandle, fType );
    Formatter_GetOctalPrefix( fHandle, fType, @C[ 0 ], Length( C ) );
    result.OctalPrefix                     := C;
    Formatter_GetOctalSuffix( fHandle, fType, @C[ 0 ], Length( C ) );
    result.OctalSuffix                     := C;
    result.OctalDigitGroupSize             := Formatter_GetOctalDigitGroupSize( fHandle, fType );
    Formatter_GetBinaryPrefix( fHandle, fType, @C[ 0 ], Length( C ) );
    result.BinaryPrefix                    := C;
    Formatter_GetBinarySuffix( fHandle, fType, @C[ 0 ], Length( C ) );
    result.BinarySuffix                    := C;
    result.BinaryDigitGroupSize            := Formatter_GetBinaryDigitGroupSize( fHandle, fType );
    Formatter_GetDigitSeparator( fHandle, fType, @C[ 0 ], Length( C ) );
    result.DigitSeparator                  := C;
    result.LeadingZeros                    := Formatter_GetLeadingZeros( fHandle, fType );
    result.SmallHexNumbersInDecimal        := Formatter_GetSmallHexNumbersInDecimal( fHandle, fType );
    result.AddLeadingZeroToHexNumbers      := Formatter_GetAddLeadingZeroToHexNumbers( fHandle, fType );
    result.NumberBase                      := Formatter_GetNumberBase( fHandle, fType );
    result.BranchLeadingZeros              := Formatter_GetBranchLeadingZeros( fHandle, fType );
    result.SignedImmediateOperands         := Formatter_GetSignedImmediateOperands( fHandle, fType );
    result.SignedMemoryDisplacements       := Formatter_GetSignedMemoryDisplacements( fHandle, fType );
    result.DisplacementLeadingZeros        := Formatter_GetDisplacementLeadingZeros( fHandle, fType );
    result.MemorySizeOptions               := Formatter_GetMemorySizeOptions( fHandle, fType );
    result.ShowBranchSize                  := Formatter_GetShowBranchSize( fHandle, fType );
    result.PreferST0                       := Formatter_GetPreferST0( fHandle, fType );
    result.ShowUselessPrefixes             := Formatter_GetShowUselessPrefixes( fHandle, fType );
    result.CC_b                            := Formatter_GetCC_b( fHandle, fType );
    result.CC_ae                           := Formatter_GetCC_ae( fHandle, fType );
    result.CC_e                            := Formatter_GetCC_e( fHandle, fType );
    result.CC_ne                           := Formatter_GetCC_ne( fHandle, fType );
    result.CC_be                           := Formatter_GetCC_be( fHandle, fType );
    result.CC_a                            := Formatter_GetCC_a( fHandle, fType );
    result.CC_p                            := Formatter_GetCC_p( fHandle, fType );
    result.CC_np                           := Formatter_GetCC_np( fHandle, fType );
    result.CC_l                            := Formatter_GetCC_l( fHandle, fType );
    result.CC_ge                           := Formatter_GetCC_ge( fHandle, fType );
    result.CC_le                           := Formatter_GetCC_le( fHandle, fType );
    result.CC_g                            := Formatter_GetCC_g( fHandle, fType );
    end;
end;

procedure TIcedFormatter.SetOptions( Value : TIcedFormatterSettings );
begin
  if ( self = nil ) then
    Exit;

  // Masm
  AddDsPrefix32                  := Value.AddDsPrefix32;
  SymbolDisplacementInBrackets   := Value.SymbolDisplacementInBrackets;
  DisplacementInBrackets         := Value.DisplacementInBrackets;

  // Nasm
  ShowSignExtendedImmediateSize  := Value.ShowSignExtendedImmediateSize;
  // Gas
  NakedRegisters                 := Value.NakedRegisters;
  ShowMnemonicSizeSuffix         := Value.ShowMnemonicSizeSuffix;
  SpaceAfterMemoryOperandComma   := Value.SpaceAfterMemoryOperandComma;
  // Specialized
  UseHexPrefix                   := Value.UseHexPrefix;
  AlwaysShowMemorySize           := Value.AlwaysShowMemorySize;
  // Common
  SpaceAfterOperandSeparator     := Value.SpaceAfterOperandSeparator;
  AlwaysShowSegmentRegister      := Value.AlwaysShowSegmentRegister;
  UsePseudoOps                   := Value.UsePseudoOps;
  RipRelativeAddresses           := Value.RipRelativeAddresses;
  ShowSymbolAddress              := Value.ShowSymbolAddress;
  UpperCaseHex                   := Value.UpperCaseHex;

  // Common (All but Fast/Specialized)
  UpperCasePrefixes              := Value.UpperCasePrefixes;
  UpperCaseMnemonics             := Value.UpperCaseMnemonics;
  UpperCaseRegisters             := Value.UpperCaseRegisters;
  UpperCaseKeyWords              := Value.UpperCaseKeyWords;
  UpperCaseDecorators            := Value.UpperCaseDecorators;
  UpperCaseEverything            := Value.UpperCaseEverything;
  FirstOperandCharIndex          := Value.FirstOperandCharIndex;
  TabSize                        := Value.TabSize;
  SpaceAfterMemoryBracket        := Value.SpaceAfterMemoryBracket;
  SpaceBetweenMemoryAddOperators := Value.SpaceBetweenMemoryAddOperators;
  SpaceBetweenMemoryMulOperators := Value.SpaceBetweenMemoryMulOperators;
  ScaleBeforeIndex               := Value.ScaleBeforeIndex;
  AlwaysShowScale                := Value.AlwaysShowScale;
  ShowZeroDisplacements          := Value.ShowZeroDisplacements;
  HexPrefix                      := Value.HexPrefix;
  HexSuffix                      := Value.HexSuffix;
  HexDigitGroupSize              := Value.HexDigitGroupSize;
  DecimalPrefix                  := Value.DecimalPrefix;
  DecimalSuffix                  := Value.DecimalSuffix;
  DecimalDigitGroupSize          := Value.DecimalDigitGroupSize;
  OctalPrefix                    := Value.OctalPrefix;
  OctalSuffix                    := Value.OctalSuffix;
  OctalDigitGroupSize            := Value.OctalDigitGroupSize;
  BinaryPrefix                   := Value.BinaryPrefix;
  BinarySuffix                   := Value.BinarySuffix;
  BinaryDigitGroupSize           := Value.BinaryDigitGroupSize;
  DigitSeparator                 := Value.DigitSeparator;
  LeadingZeros                   := Value.LeadingZeros;
  SmallHexNumbersInDecimal       := Value.SmallHexNumbersInDecimal;
  AddLeadingZeroToHexNumbers     := Value.AddLeadingZeroToHexNumbers;
  NumberBase                     := Value.NumberBase;
  BranchLeadingZeros             := Value.BranchLeadingZeros;
  SignedImmediateOperands        := Value.SignedImmediateOperands;
  SignedMemoryDisplacements      := Value.SignedMemoryDisplacements;
  DisplacementLeadingZeros       := Value.DisplacementLeadingZeros;
  MemorySizeOptions              := Value.MemorySizeOptions;
  ShowBranchSize                 := Value.ShowBranchSize;
  PreferST0                      := Value.PreferST0;
  ShowUselessPrefixes            := Value.ShowUselessPrefixes;
  CC_b                           := Value.CC_b;
  CC_ae                          := Value.CC_ae;
  CC_e                           := Value.CC_e;
  CC_ne                          := Value.CC_ne;
  CC_be                          := Value.CC_be;
  CC_a                           := Value.CC_a;
  CC_p                           := Value.CC_p;
  CC_np                          := Value.CC_np;
  CC_l                           := Value.CC_l;
  CC_ge                          := Value.CC_ge;
  CC_le                          := Value.CC_le;
  CC_g                           := Value.CC_g;
end;

procedure TIcedFormatter.SettingsToStringList( Settings : TIcedFormatterSettings; var StrL : TStringList );
begin
  if ( self = nil ) then
    Exit;

  if ( StrL = nil ) then
    Exit;
  StrL.Clear;

  StrL.Add( SysUtils.Format( '%s=%d', [ 'AddDsPrefix32', Byte( Settings.AddDsPrefix32 ) ] ) );
  StrL.Add( SysUtils.Format( '%s=%d', [ 'SymbolDisplacementInBrackets', Byte( Settings.SymbolDisplacementInBrackets ) ] ) );
  StrL.Add( SysUtils.Format( '%s=%d', [ 'DisplacementInBrackets', Byte( Settings.DisplacementInBrackets ) ] ) );
  StrL.Add( SysUtils.Format( '%s=%d', [ 'ShowSignExtendedImmediateSize', Byte( Settings.ShowSignExtendedImmediateSize ) ] ) );
  StrL.Add( SysUtils.Format( '%s=%d', [ 'NakedRegisters', Byte( Settings.NakedRegisters ) ] ) );
  StrL.Add( SysUtils.Format( '%s=%d', [ 'ShowMnemonicSizeSuffix', Byte( Settings.ShowMnemonicSizeSuffix ) ] ) );
  StrL.Add( SysUtils.Format( '%s=%d', [ 'SpaceAfterMemoryOperandComma', Byte( Settings.SpaceAfterMemoryOperandComma ) ] ) );
  StrL.Add( SysUtils.Format( '%s=%d', [ 'UseHexPrefix', Byte( Settings.UseHexPrefix ) ] ) );
  StrL.Add( SysUtils.Format( '%s=%d', [ 'AlwaysShowMemorySize', Byte( Settings.AlwaysShowMemorySize ) ] ) );
  StrL.Add( SysUtils.Format( '%s=%d', [ 'SpaceAfterOperandSeparator', Byte( Settings.SpaceAfterOperandSeparator ) ] ) );
  StrL.Add( SysUtils.Format( '%s=%d', [ 'AlwaysShowSegmentRegister', Byte( Settings.AlwaysShowSegmentRegister ) ] ) );
  StrL.Add( SysUtils.Format( '%s=%d', [ 'UsePseudoOps', Byte( Settings.UsePseudoOps ) ] ) );
  StrL.Add( SysUtils.Format( '%s=%d', [ 'RipRelativeAddresses', Byte( Settings.RipRelativeAddresses ) ] ) );
  StrL.Add( SysUtils.Format( '%s=%d', [ 'ShowSymbolAddress', Byte( Settings.ShowSymbolAddress ) ] ) );
  StrL.Add( SysUtils.Format( '%s=%d', [ 'UpperCaseHex', Byte( Settings.UpperCaseHex ) ] ) );

  StrL.Add( SysUtils.Format( '%s=%d', [ 'UpperCasePrefixes', Byte( Settings.UpperCasePrefixes ) ] ) );
  StrL.Add( SysUtils.Format( '%s=%d', [ 'UpperCaseMnemonics', Byte( Settings.UpperCaseMnemonics ) ] ) );
  StrL.Add( SysUtils.Format( '%s=%d', [ 'UpperCaseRegisters', Byte( Settings.UpperCaseRegisters ) ] ) );
  StrL.Add( SysUtils.Format( '%s=%d', [ 'UpperCaseKeyWords', Byte( Settings.UpperCaseKeyWords ) ] ) );
  StrL.Add( SysUtils.Format( '%s=%d', [ 'UpperCaseDecorators', Byte( Settings.UpperCaseDecorators ) ] ) );
  StrL.Add( SysUtils.Format( '%s=%d', [ 'UpperCaseEverything', Byte( Settings.UpperCaseEverything ) ] ) );
  StrL.Add( SysUtils.Format( '%s=%d', [ 'FirstOperandCharIndex', Settings.FirstOperandCharIndex ] ) );
  StrL.Add( SysUtils.Format( '%s=%d', [ 'TabSize', Settings.TabSize ] ) );
  StrL.Add( SysUtils.Format( '%s=%d', [ 'SpaceAfterMemoryBracket', Byte( Settings.SpaceAfterMemoryBracket ) ] ) );
  StrL.Add( SysUtils.Format( '%s=%d', [ 'SpaceBetweenMemoryAddOperators', Byte( Settings.SpaceBetweenMemoryAddOperators ) ] ) );
  StrL.Add( SysUtils.Format( '%s=%d', [ 'SpaceBetweenMemoryMulOperators', Byte( Settings.SpaceBetweenMemoryMulOperators ) ] ) );
  StrL.Add( SysUtils.Format( '%s=%d', [ 'ScaleBeforeIndex', Byte( Settings.ScaleBeforeIndex ) ] ) );
  StrL.Add( SysUtils.Format( '%s=%d', [ 'AlwaysShowScale', Byte( Settings.AlwaysShowScale ) ] ) );
  StrL.Add( SysUtils.Format( '%s=%d', [ 'ShowZeroDisplacements', Byte( Settings.ShowZeroDisplacements ) ] ) );
  StrL.Add( SysUtils.Format( '%s=%s', [ 'HexPrefix', Settings.HexPrefix ] ) );
  StrL.Add( SysUtils.Format( '%s=%s', [ 'HexSuffix', Settings.HexSuffix ] ) );
  StrL.Add( SysUtils.Format( '%s=%d', [ 'HexDigitGroupSize', Settings.HexDigitGroupSize ] ) );
  StrL.Add( SysUtils.Format( '%s=%s', [ 'DecimalPrefix', Settings.DecimalPrefix ] ) );
  StrL.Add( SysUtils.Format( '%s=%s', [ 'DecimalSuffix', Settings.DecimalSuffix ] ) );
  StrL.Add( SysUtils.Format( '%s=%d', [ 'DecimalDigitGroupSize', Settings.DecimalDigitGroupSize ] ) );
  StrL.Add( SysUtils.Format( '%s=%s', [ 'OctalPrefix', Settings.OctalPrefix ] ) );
  StrL.Add( SysUtils.Format( '%s=%s', [ 'OctalSuffix', Settings.OctalSuffix ] ) );
  StrL.Add( SysUtils.Format( '%s=%d', [ 'OctalDigitGroupSize', Settings.OctalDigitGroupSize ] ) );
  StrL.Add( SysUtils.Format( '%s=%s', [ 'BinaryPrefix', Settings.BinaryPrefix ] ) );
  StrL.Add( SysUtils.Format( '%s=%s', [ 'BinarySuffix', Settings.BinarySuffix ] ) );
  StrL.Add( SysUtils.Format( '%s=%d', [ 'BinaryDigitGroupSize', Settings.BinaryDigitGroupSize ] ) );
  StrL.Add( SysUtils.Format( '%s=%s', [ 'DigitSeparator', Settings.DigitSeparator ] ) );
  StrL.Add( SysUtils.Format( '%s=%d', [ 'LeadingZeros', Byte( Settings.LeadingZeros ) ] ) );
  StrL.Add( SysUtils.Format( '%s=%d', [ 'SmallHexNumbersInDecimal', Byte( Settings.SmallHexNumbersInDecimal ) ] ) );
  StrL.Add( SysUtils.Format( '%s=%d', [ 'AddLeadingZeroToHexNumbers', Byte( Settings.AddLeadingZeroToHexNumbers ) ] ) );
  StrL.Add( SysUtils.Format( '%s=%d', [ 'NumberBase', Byte( Settings.NumberBase ) ] ) );
  StrL.Add( SysUtils.Format( '%s=%d', [ 'BranchLeadingZeros', Byte( Settings.BranchLeadingZeros ) ] ) );
  StrL.Add( SysUtils.Format( '%s=%d', [ 'SignedImmediateOperands', Byte( Settings.SignedImmediateOperands ) ] ) );
  StrL.Add( SysUtils.Format( '%s=%d', [ 'SignedMemoryDisplacements', Byte( Settings.SignedMemoryDisplacements ) ] ) );
  StrL.Add( SysUtils.Format( '%s=%d', [ 'DisplacementLeadingZeros', Byte( Settings.DisplacementLeadingZeros ) ] ) );
  StrL.Add( SysUtils.Format( '%s=%d', [ 'MemorySizeOptions', Byte( Settings.MemorySizeOptions )  ] ) );
  StrL.Add( SysUtils.Format( '%s=%d', [ 'ShowBranchSize', Byte( Settings.ShowBranchSize ) ] ) );
  StrL.Add( SysUtils.Format( '%s=%d', [ 'PreferST0', Byte( Settings.PreferST0 ) ] ) );
  StrL.Add( SysUtils.Format( '%s=%d', [ 'ShowUselessPrefixes', Byte( Settings.ShowUselessPrefixes ) ] ) );
  StrL.Add( SysUtils.Format( '%s=%d', [ 'CC_b', Byte( Settings.CC_b ) ] ) );
  StrL.Add( SysUtils.Format( '%s=%d', [ 'CC_ae', Byte( Settings.CC_ae ) ] ) );
  StrL.Add( SysUtils.Format( '%s=%d', [ 'CC_e', Byte( Settings.CC_e ) ] ) );
  StrL.Add( SysUtils.Format( '%s=%d', [ 'CC_ne', Byte( Settings.CC_ne ) ] ) );
  StrL.Add( SysUtils.Format( '%s=%d', [ 'CC_be', Byte( Settings.CC_be ) ] ) );
  StrL.Add( SysUtils.Format( '%s=%d', [ 'CC_a', Byte( Settings.CC_a ) ] ) );
  StrL.Add( SysUtils.Format( '%s=%d', [ 'CC_p', Byte( Settings.CC_p ) ] ) );
  StrL.Add( SysUtils.Format( '%s=%d', [ 'CC_np', Byte( Settings.CC_np ) ] ) );
  StrL.Add( SysUtils.Format( '%s=%d', [ 'CC_l', Byte( Settings.CC_l ) ] ) );
  StrL.Add( SysUtils.Format( '%s=%d', [ 'CC_ge', Byte( Settings.CC_ge ) ] ) );
  StrL.Add( SysUtils.Format( '%s=%d', [ 'CC_le', Byte( Settings.CC_le ) ] ) );
  StrL.Add( SysUtils.Format( '%s=%d', [ 'CC_g', Byte( Settings.CC_g ) ] ) );
end;

function TIcedFormatter.StringListToSettings( StrL : TStringList; Settings : PIcedFormatterSettings = nil ) : boolean;
var
  tmp : TIcedFormatterSettings;
begin
  result := False;
  if ( self = nil ) then
    Exit;

  if ( StrL = nil ) then
    Exit;
  StrL.Delimiter := '=';

  if ( Settings = nil ) then
    Settings := @tmp;

  Settings^.AddDsPrefix32 := Boolean( StrToIntDef( StrL.Values[ 'AddDsPrefix32' ], Byte( fOptions.AddDsPrefix32 ) ) );
  Settings^.SymbolDisplacementInBrackets := Boolean( StrToIntDef( StrL.Values[ 'SymbolDisplacementInBrackets' ], Byte( fOptions.SymbolDisplacementInBrackets ) ) );
  Settings^.DisplacementInBrackets := Boolean( StrToIntDef( StrL.Values[ 'DisplacementInBrackets' ], Byte( fOptions.DisplacementInBrackets ) ) );
  Settings^.ShowSignExtendedImmediateSize := Boolean( StrToIntDef( StrL.Values[ 'ShowSignExtendedImmediateSize' ], Byte( fOptions.ShowSignExtendedImmediateSize ) ) );
  Settings^.NakedRegisters := Boolean( StrToIntDef( StrL.Values[ 'NakedRegisters' ], Byte( fOptions.NakedRegisters ) ) );
  Settings^.ShowMnemonicSizeSuffix := Boolean( StrToIntDef( StrL.Values[ 'ShowMnemonicSizeSuffix' ], Byte( fOptions.ShowMnemonicSizeSuffix ) ) );
  Settings^.SpaceAfterMemoryOperandComma := Boolean( StrToIntDef( StrL.Values[ 'SpaceAfterMemoryOperandComma' ], Byte( fOptions.SpaceAfterMemoryOperandComma ) ) );
  Settings^.UseHexPrefix := Boolean( StrToIntDef( StrL.Values[ 'UseHexPrefix' ], Byte( fOptions.UseHexPrefix ) ) );
  Settings^.AlwaysShowMemorySize := Boolean( StrToIntDef( StrL.Values[ 'AlwaysShowMemorySize' ], Byte( fOptions.AlwaysShowMemorySize ) ) );
  Settings^.SpaceAfterOperandSeparator := Boolean( StrToIntDef( StrL.Values[ 'SpaceAfterOperandSeparator' ], Byte( fOptions.SpaceAfterOperandSeparator ) ) );
  Settings^.AlwaysShowSegmentRegister := Boolean( StrToIntDef( StrL.Values[ 'AlwaysShowSegmentRegister' ], Byte( fOptions.AlwaysShowSegmentRegister ) ) );
  Settings^.UsePseudoOps := Boolean( StrToIntDef( StrL.Values[ 'UsePseudoOps' ], Byte( fOptions.UsePseudoOps ) ) );
  Settings^.RipRelativeAddresses := Boolean( StrToIntDef( StrL.Values[ 'RipRelativeAddresses' ], Byte( fOptions.RipRelativeAddresses ) ) );
  Settings^.ShowSymbolAddress := Boolean( StrToIntDef( StrL.Values[ 'ShowSymbolAddress' ], Byte( fOptions.ShowSymbolAddress ) ) );
  Settings^.UpperCaseHex := Boolean( StrToIntDef( StrL.Values[ 'UpperCaseHex' ], Byte( fOptions.UpperCaseHex ) ) );

  Settings^.UpperCasePrefixes := Boolean( StrToIntDef( StrL.Values[ 'UpperCasePrefixes' ], Byte( fOptions.UpperCasePrefixes ) ) );
  Settings^.UpperCaseMnemonics := Boolean( StrToIntDef( StrL.Values[ 'UpperCaseMnemonics' ], Byte( fOptions.UpperCaseMnemonics ) ) );
  Settings^.UpperCaseRegisters := Boolean( StrToIntDef( StrL.Values[ 'UpperCaseRegisters' ], Byte( fOptions.UpperCaseRegisters ) ) );
  Settings^.UpperCaseKeyWords := Boolean( StrToIntDef( StrL.Values[ 'UpperCaseKeyWords' ], Byte( fOptions.UpperCaseKeyWords ) ) );
  Settings^.UpperCaseDecorators := Boolean( StrToIntDef( StrL.Values[ 'UpperCaseDecorators' ], Byte( fOptions.UpperCaseDecorators ) ) );
  Settings^.UpperCaseEverything := Boolean( StrToIntDef( StrL.Values[ 'UpperCaseEverything' ], Byte( fOptions.UpperCaseEverything ) ) );
  Settings^.FirstOperandCharIndex := StrToIntDef( StrL.Values[ 'FirstOperandCharIndex' ], 0 );
  Settings^.TabSize := StrToIntDef( StrL.Values[ 'TabSize' ], 0 );
  Settings^.SpaceAfterMemoryBracket := Boolean( StrToIntDef( StrL.Values[ 'SpaceAfterMemoryBracket' ], Byte( fOptions.SpaceAfterMemoryBracket ) ) );
  Settings^.SpaceBetweenMemoryAddOperators := Boolean( StrToIntDef( StrL.Values[ 'SpaceBetweenMemoryAddOperators' ], Byte( fOptions.SpaceBetweenMemoryAddOperators ) ) );
  Settings^.SpaceBetweenMemoryMulOperators := Boolean( StrToIntDef( StrL.Values[ 'SpaceBetweenMemoryMulOperators' ], Byte( fOptions.SpaceBetweenMemoryMulOperators ) ) );
  Settings^.ScaleBeforeIndex := Boolean( StrToIntDef( StrL.Values[ 'ScaleBeforeIndex' ], Byte( fOptions.ScaleBeforeIndex ) ) );
  Settings^.AlwaysShowScale := Boolean( StrToIntDef( StrL.Values[ 'AlwaysShowScale' ], Byte( fOptions.AlwaysShowScale ) ) );
  Settings^.ShowZeroDisplacements := Boolean( StrToIntDef( StrL.Values[ 'ShowZeroDisplacements' ], Byte( fOptions.ShowZeroDisplacements ) ) );
  Settings^.HexPrefix := AnsiString( StrL.Values[ 'HexPrefix' ] );
  Settings^.HexSuffix := AnsiString( StrL.Values[ 'HexSuffix' ] );
  Settings^.HexDigitGroupSize := StrToIntDef( StrL.Values[ 'HexDigitGroupSize' ], 0 );
  Settings^.DecimalPrefix := AnsiString( StrL.Values[ 'DecimalPrefix' ] );
  Settings^.DecimalSuffix := AnsiString( StrL.Values[ 'DecimalSuffix' ] );
  Settings^.DecimalDigitGroupSize := StrToIntDef( StrL.Values[ 'DecimalDigitGroupSize' ], 0 );
  Settings^.OctalPrefix := AnsiString( StrL.Values[ 'OctalPrefix' ] );
  Settings^.OctalSuffix := AnsiString( StrL.Values[ 'OctalSuffix' ] );
  Settings^.OctalDigitGroupSize := StrToIntDef( StrL.Values[ 'OctalDigitGroupSize' ], 0 );
  Settings^.BinaryPrefix := AnsiString( StrL.Values[ 'BinaryPrefix' ] );
  Settings^.BinarySuffix := AnsiString( StrL.Values[ 'BinarySuffix' ] );
  Settings^.BinaryDigitGroupSize := StrToIntDef( StrL.Values[ 'BinaryDigitGroupSize' ], 0 );
  Settings^.DigitSeparator := AnsiString( StrL.Values[ 'DigitSeparator' ] );
  Settings^.LeadingZeros := Boolean( StrToIntDef( StrL.Values[ 'LeadingZeros' ], Byte( fOptions.LeadingZeros ) ) );
  Settings^.SmallHexNumbersInDecimal := Boolean( StrToIntDef( StrL.Values[ 'SmallHexNumbersInDecimal' ], Byte( fOptions.SmallHexNumbersInDecimal ) ) );
  Settings^.AddLeadingZeroToHexNumbers := Boolean( StrToIntDef( StrL.Values[ 'AddLeadingZeroToHexNumbers' ], Byte( fOptions.AddLeadingZeroToHexNumbers ) ) );
  Settings^.NumberBase.NumberBase := TNumberBaseType( StrToIntDef( StrL.Values[ 'NumberBase' ], Byte( fOptions.NumberBase.NumberBase ) ) );
  Settings^.BranchLeadingZeros := Boolean( StrToIntDef( StrL.Values[ 'BranchLeadingZeros' ], Byte( fOptions.BranchLeadingZeros ) ) );
  Settings^.SignedImmediateOperands := Boolean( StrToIntDef( StrL.Values[ 'SignedImmediateOperands' ], Byte( fOptions.SignedImmediateOperands ) ) );
  Settings^.SignedMemoryDisplacements := Boolean( StrToIntDef( StrL.Values[ 'SignedMemoryDisplacements' ], Byte( fOptions.SignedMemoryDisplacements ) ) );
  Settings^.DisplacementLeadingZeros := Boolean( StrToIntDef( StrL.Values[ 'DisplacementLeadingZeros' ], Byte( fOptions.DisplacementLeadingZeros ) ) );
  Settings^.MemorySizeOptions.MemorySizeOptions := TMemorySizeOptionsType( StrToIntDef( StrL.Values[ 'MemorySizeOptions' ], Byte( fOptions.MemorySizeOptions.MemorySizeOptions ) ) );
  Settings^.ShowBranchSize := Boolean( StrToIntDef( StrL.Values[ 'ShowBranchSize' ], Byte( fOptions.ShowBranchSize ) ) );
  Settings^.PreferST0 := Boolean( StrToIntDef( StrL.Values[ 'PreferST0' ], Byte( fOptions.PreferST0 ) ) );
  Settings^.ShowUselessPrefixes := Boolean( StrToIntDef( StrL.Values[ 'ShowUselessPrefixes' ], Byte( fOptions.ShowUselessPrefixes ) ) );
  Settings^.CC_b := TCC_b( StrToIntDef( StrL.Values[ 'CC_b' ], Byte( fOptions.CC_b ) ) );
  Settings^.CC_ae := TCC_ae( StrToIntDef( StrL.Values[ 'CC_ae' ], Byte( fOptions.CC_ae ) ) );
  Settings^.CC_e := TCC_e( StrToIntDef( StrL.Values[ 'CC_e' ], Byte( fOptions.CC_e ) ) );
  Settings^.CC_ne := TCC_ne( StrToIntDef( StrL.Values[ 'CC_ne' ], Byte( fOptions.CC_ne ) ) );
  Settings^.CC_be := TCC_be( StrToIntDef( StrL.Values[ 'CC_be' ], Byte( fOptions.CC_be ) ) );
  Settings^.CC_a := TCC_a( StrToIntDef( StrL.Values[ 'CC_a' ], Byte( fOptions.CC_a ) ) );
  Settings^.CC_p := TCC_p( StrToIntDef( StrL.Values[ 'CC_p' ], Byte( fOptions.CC_p ) ) );
  Settings^.CC_np := TCC_np( StrToIntDef( StrL.Values[ 'CC_np' ], Byte( fOptions.CC_np ) ) );
  Settings^.CC_l := TCC_l( StrToIntDef( StrL.Values[ 'CC_l' ], Byte( fOptions.CC_l ) ) );
  Settings^.CC_ge := TCC_ge( StrToIntDef( StrL.Values[ 'CC_ge' ], Byte( fOptions.CC_ge ) ) );
  Settings^.CC_le := TCC_le( StrToIntDef( StrL.Values[ 'CC_le' ], Byte( fOptions.CC_le ) ) );
  Settings^.CC_g := TCC_g( StrToIntDef( StrL.Values[ 'CC_g' ], Byte( fOptions.CC_g ) ) );

  if ( Settings = @tmp ) then
    Options := tmp;
end;

procedure TIcedFormatter.SetCapstoneOptions;
begin
  if ( self = nil ) then
    Exit;
  if ( fHandle = nil ) then
    Exit;

  ShowSymbolAddress                := True;
  SpaceAfterMemoryOperandComma     := True;
  SpaceAfterOperandSeparator       := True;
  RipRelativeAddresses             := True;
  ShowBranchSize                   := False;
  HexPrefix                        := '0x';
  HexSuffix                        := '';
end;

// Masm
function TIcedFormatter.GetAddDsPrefix32 : Boolean;
begin
  result := false;
  if ( self = nil ) then
    Exit;
  if ( fHandle = nil ) then
    Exit;
  if NOT ( fType in [ ftMasm, ftCapstone ] ) then
    Exit;

  result := fOptions.AddDsPrefix32;
//  result := MasmFormatter_GetAddDsPrefix32( fHandle );
end;

procedure TIcedFormatter.SetAddDsPrefix32( Value : Boolean );
begin
  if ( self = nil ) then
    Exit;
  if ( fHandle = nil ) then
    Exit;
  if NOT ( fType in [ ftMasm, ftCapstone ] ) then
    Exit;
  if ( fOptions.AddDsPrefix32 = Value ) then
    Exit;
  fOptions.AddDsPrefix32 := Value;

  {result := }MasmFormatter_SetAddDsPrefix32( fHandle, Value );
end;

function TIcedFormatter.GetSymbolDisplacementInBrackets : Boolean;
begin
  result := false;
  if ( self = nil ) then
    Exit;
  if ( fHandle = nil ) then
    Exit;
  if NOT ( fType in [ ftMasm, ftCapstone ] ) then
    Exit;

  result := fOptions.SymbolDisplacementInBrackets;
//  result := MasmFormatter_GetSymbolDisplacementInBrackets( fHandle );
end;

procedure TIcedFormatter.SetSymbolDisplacementInBrackets( Value : Boolean );
begin
  if ( self = nil ) then
    Exit;
  if ( fHandle = nil ) then
    Exit;
  if NOT ( fType in [ ftMasm, ftCapstone ] ) then
    Exit;
  if ( fOptions.SymbolDisplacementInBrackets = Value ) then
    Exit;
  fOptions.SymbolDisplacementInBrackets := Value;

  {result := }MasmFormatter_SetSymbolDisplacementInBrackets( fHandle, Value );
end;

function TIcedFormatter.GetDisplacementInBrackets : Boolean;
begin
  result := false;
  if ( self = nil ) then
    Exit;
  if ( fHandle = nil ) then
    Exit;
  if NOT ( fType in [ ftMasm, ftCapstone ] ) then
    Exit;

  result := fOptions.DisplacementInBrackets;
//  result := MasmFormatter_GetDisplacementInBrackets( fHandle );
end;

procedure TIcedFormatter.SetDisplacementInBrackets( Value : Boolean );
begin
  if ( self = nil ) then
    Exit;
  if ( fHandle = nil ) then
    Exit;
  if NOT ( fType in [ ftMasm, ftCapstone ] ) then
    Exit;
  if ( fOptions.DisplacementInBrackets = Value ) then
    Exit;
  fOptions.DisplacementInBrackets := Value;

  {result := }MasmFormatter_SetDisplacementInBrackets( fHandle, Value );
end;

// Nasm
function TIcedFormatter.GetShowSignExtendedImmediateSize : Boolean;
begin
  result := false;
  if ( self = nil ) then
    Exit;
  if ( fHandle = nil ) then
    Exit;
  if ( fType <> ftNasm ) then
    Exit;

  result := fOptions.ShowSignExtendedImmediateSize;
//  result := NasmFormatter_GetShowSignExtendedImmediateSize( fHandle );
end;

procedure TIcedFormatter.SetShowSignExtendedImmediateSize( Value : Boolean );
begin
  if ( self = nil ) then
    Exit;
  if ( fHandle = nil ) then
    Exit;
  if ( fType <> ftNasm ) then
    Exit;
  if ( fOptions.ShowSignExtendedImmediateSize = Value ) then
    Exit;
  fOptions.ShowSignExtendedImmediateSize := Value;

  {result := }NasmFormatter_SetShowSignExtendedImmediateSize( fHandle, Value );
end;

// Gas
function TIcedFormatter.GetNakedRegisters : Boolean;
begin
  result := false;
  if ( self = nil ) then
    Exit;
  if ( fHandle = nil ) then
    Exit;
  if ( fType <> ftGas ) then
    Exit;

  result := fOptions.NakedRegisters;
//  result := GasFormatter_GetNakedRegisters( fHandle );
end;

procedure TIcedFormatter.SetNakedRegisters( Value : Boolean );
begin
  if ( self = nil ) then
    Exit;
  if ( fHandle = nil ) then
    Exit;
  if ( fType <> ftGas ) then
    Exit;
  if ( fOptions.NakedRegisters = Value ) then
    Exit;
  fOptions.NakedRegisters := Value;

  {result := }GasFormatter_SetNakedRegisters( fHandle, Value );
end;

function TIcedFormatter.GetShowMnemonicSizeSuffix : Boolean;
begin
  result := false;
  if ( self = nil ) then
    Exit;
  if ( fHandle = nil ) then
    Exit;
  if ( fType <> ftGas ) then
    Exit;

  result := fOptions.ShowMnemonicSizeSuffix;
//  result := GasFormatter_GetShowMnemonicSizeSuffix( fHandle );
end;

procedure TIcedFormatter.SetShowMnemonicSizeSuffix( Value : Boolean );
begin
  if ( self = nil ) then
    Exit;
  if ( fHandle = nil ) then
    Exit;
  if ( fType <> ftGas ) then
    Exit;
  if ( fOptions.ShowMnemonicSizeSuffix = Value ) then
    Exit;
  fOptions.ShowMnemonicSizeSuffix := Value;

  {result := }GasFormatter_SetShowMnemonicSizeSuffix( fHandle, Value );
end;

function TIcedFormatter.GetSpaceAfterMemoryOperandComma : Boolean;
begin
  result := false;
  if ( self = nil ) then
    Exit;
  if ( fHandle = nil ) then
    Exit;
  if ( fType <> ftGas ) then
    Exit;

  result := fOptions.SpaceAfterMemoryOperandComma;
//  result := GasFormatter_GetSpaceAfterMemoryOperandComma( fHandle );
end;

procedure TIcedFormatter.SetSpaceAfterMemoryOperandComma( Value : Boolean );
begin
  if ( self = nil ) then
    Exit;
  if ( fHandle = nil ) then
    Exit;
  if ( fType <> ftGas ) then
    Exit;
  if ( fOptions.SpaceAfterMemoryOperandComma = Value ) then
    Exit;
  fOptions.SpaceAfterMemoryOperandComma := Value;

  {result := }GasFormatter_SetSpaceAfterMemoryOperandComma( fHandle, Value );
end;

// Specialized
function TIcedFormatter.GetUseHexPrefix : Boolean;
begin
  result := false;
  if ( self = nil ) then
    Exit;
  if ( fHandle = nil ) then
    Exit;
  if ( fType <> ftSpecialized ) then
    Exit;

  result := fOptions.UseHexPrefix;
//  result := SpecializedFormatter_GetUseHexPrefix( fHandle );
end;

procedure TIcedFormatter.SetUseHexPrefix( Value : Boolean );
begin
  if ( self = nil ) then
    Exit;
  if ( fHandle = nil ) then
    Exit;
  if ( fType <> ftSpecialized ) then
    Exit;
  if ( fOptions.UseHexPrefix = Value ) then
    Exit;
  fOptions.UseHexPrefix := Value;

  {result := }SpecializedFormatter_SetUseHexPrefix( fHandle, Value );
end;

function TIcedFormatter.GetAlwaysShowMemorySize : Boolean;
begin
  result := false;
  if ( self = nil ) then
    Exit;
  if ( fHandle = nil ) then
    Exit;
  if ( fType <> ftSpecialized ) then
    Exit;

  result := fOptions.AlwaysShowMemorySize;
//  result := SpecializedFormatter_GetAlwaysShowMemorySize( fHandle );
end;

procedure TIcedFormatter.SetAlwaysShowMemorySize( Value : Boolean );
begin
  if ( self = nil ) then
    Exit;
  if ( fHandle = nil ) then
    Exit;
  if ( fType <> ftSpecialized ) then
    Exit;
  if ( fOptions.AlwaysShowMemorySize = Value ) then
    Exit;
  fOptions.AlwaysShowMemorySize := Value;

  {result := }SpecializedFormatter_SetAlwaysShowMemorySize( fHandle, Value );
end;

function TIcedFormatter.GetEnable_DB_DW_DD_DQ : Boolean;
begin
  result := false;
  if ( self = nil ) then
    Exit;
  if ( fHandle = nil ) then
    Exit;
//  if ( fType <> ftSpecialized ) then
//    Exit;
  result := fSpecialized.ENABLE_DB_DW_DD_DQ;
end;

procedure TIcedFormatter.SetEnable_DB_DW_DD_DQ( Value : Boolean );
begin
  if ( self = nil ) then
    Exit;
  if ( fHandle = nil ) then
    Exit;

  fSpecialized.ENABLE_DB_DW_DD_DQ := Value;

  if ( fType <> ftSpecialized ) then
    Exit;
  CreateHandle;
end;

function TIcedFormatter.GetVerifyOutputHasEnoughBytesLeft : Boolean;
begin
  result := false;
  if ( self = nil ) then
    Exit;
  if ( fHandle = nil ) then
    Exit;
//  if ( fType <> ftSpecialized ) then
//    Exit;
  result := fSpecialized.verify_output_has_enough_bytes_left;
end;

procedure TIcedFormatter.SetVerifyOutputHasEnoughBytesLeft( Value : Boolean );
begin
  if ( self = nil ) then
    Exit;
  if ( fHandle = nil ) then
    Exit;

  fSpecialized.verify_output_has_enough_bytes_left := Value;

  if ( fType <> ftSpecialized ) then
    Exit;
  CreateHandle;
end;

// Common
function TIcedFormatter.GetSpaceAfterOperandSeparator : Boolean;
begin
  result := false;
  if ( self = nil ) then
    Exit;
  if ( fHandle = nil ) then
    Exit;

  result := fOptions.SpaceAfterOperandSeparator;
//  result := Formatter_GetSpaceAfterOperandSeparator( fHandle );
end;

procedure TIcedFormatter.SetSpaceAfterOperandSeparator( Value : Boolean );
begin
  if ( self = nil ) then
    Exit;
  if ( fHandle = nil ) then
    Exit;

  if ( fOptions.SpaceAfterOperandSeparator = Value ) then
    Exit;
  fOptions.SpaceAfterOperandSeparator := Value;

  {result := }Formatter_SetSpaceAfterOperandSeparator( fHandle, fType, Value );
end;

function TIcedFormatter.GetAlwaysShowSegmentRegister : Boolean;
begin
  result := false;
  if ( self = nil ) then
    Exit;
  if ( fHandle = nil ) then
    Exit;

  result := fOptions.AlwaysShowSegmentRegister;
//  result := Formatter_GetAlwaysShowSegmentRegister( fHandle );
end;

procedure TIcedFormatter.SetAlwaysShowSegmentRegister( Value : Boolean );
begin
  if ( self = nil ) then
    Exit;
  if ( fHandle = nil ) then
    Exit;
  if ( fOptions.AlwaysShowSegmentRegister = Value ) then
    Exit;
  fOptions.AlwaysShowSegmentRegister := Value;

  {result := }Formatter_SetAlwaysShowSegmentRegister( fHandle, fType, Value );
end;

function TIcedFormatter.GetUsePseudoOps : Boolean;
begin
  result := false;
  if ( self = nil ) then
    Exit;
  if ( fHandle = nil ) then
    Exit;

  result := fOptions.UsePseudoOps;
//  result := Formatter_GetUsePseudoOps( fHandle );
end;

procedure TIcedFormatter.SetUsePseudoOps( Value : Boolean );
begin
  if ( self = nil ) then
    Exit;
  if ( fHandle = nil ) then
    Exit;
  if ( fOptions.UsePseudoOps = Value ) then
    Exit;
  fOptions.UsePseudoOps := Value;

  {result := }Formatter_SetUsePseudoOps( fHandle, fType, Value );
end;

function TIcedFormatter.GetRipRelativeAddresses : Boolean;
begin
  result := false;
  if ( self = nil ) then
    Exit;
  if ( fHandle = nil ) then
    Exit;

  result := fOptions.RipRelativeAddresses;
//  result := Formatter_GetRipRelativeAddresses( fHandle );
end;

procedure TIcedFormatter.SetRipRelativeAddresses( Value : Boolean );
begin
  if ( self = nil ) then
    Exit;
  if ( fHandle = nil ) then
    Exit;
  if ( fOptions.RipRelativeAddresses = Value ) then
    Exit;
  fOptions.RipRelativeAddresses := Value;

  {result := }Formatter_SetRipRelativeAddresses( fHandle, fType, Value );
end;

function TIcedFormatter.GetShowSymbolAddress : Boolean;
begin
  result := false;
  if ( self = nil ) then
    Exit;
  if ( fHandle = nil ) then
    Exit;

  result := fOptions.ShowSymbolAddress;
//  result := Formatter_GetShowSymbolAddress( fHandle );
end;

procedure TIcedFormatter.SetShowSymbolAddress( Value : Boolean );
begin
  if ( self = nil ) then
    Exit;
  if ( fHandle = nil ) then
    Exit;
  if ( fOptions.ShowSymbolAddress = Value ) then
    Exit;
  fOptions.ShowSymbolAddress := Value;

  {result := }Formatter_SetShowSymbolAddress( fHandle, fType, Value );
end;

function TIcedFormatter.GetUpperCaseHex : Boolean;
begin
  result := false;
  if ( self = nil ) then
    Exit;
  if ( fHandle = nil ) then
    Exit;

  result := fOptions.UpperCaseHex;
//  result := Formatter_GetUpperCaseHex( fHandle );
end;

procedure TIcedFormatter.SetUpperCaseHex( Value : Boolean );
begin
  if ( self = nil ) then
    Exit;
  if ( fHandle = nil ) then
    Exit;
  if ( fOptions.UpperCaseHex = Value ) then
    Exit;
  fOptions.UpperCaseHex := Value;

  {result := }Formatter_SetUpperCaseHex( fHandle, fType, Value );
end;

// Common (All but Fast/Specialized)
function TIcedFormatter.GetUpperCasePrefixes : Boolean;
begin
  result := false;
  if ( self = nil ) then
    Exit;
  if ( fHandle = nil ) then
    Exit;
  if ( fType in [ ftFast, ftSpecialized ] ) then
    Exit;

  result := fOptions.UpperCasePrefixes;
//  result := Formatter_GetUpperCasePrefixes( fHandle );
end;

procedure TIcedFormatter.SetUpperCasePrefixes( Value : Boolean );
begin
  if ( self = nil ) then
    Exit;
  if ( fHandle = nil ) then
    Exit;
  if ( fType in [ ftFast, ftSpecialized ] ) then
    Exit;
  if ( fOptions.UpperCasePrefixes = Value ) then
    Exit;
  fOptions.UpperCasePrefixes := Value;

  {result := }Formatter_SetUpperCasePrefixes( fHandle, fType, Value );
end;

function TIcedFormatter.GetUpperCaseMnemonics : Boolean;
begin
  result := false;
  if ( self = nil ) then
    Exit;
  if ( fHandle = nil ) then
    Exit;
  if ( fType in [ ftFast, ftSpecialized ] ) then
    Exit;

  result := fOptions.UpperCaseMnemonics;
//  result := Formatter_GetUpperCaseMnemonics( fHandle );
end;

procedure TIcedFormatter.SetUpperCaseMnemonics( Value : Boolean );
begin
  if ( self = nil ) then
    Exit;
  if ( fHandle = nil ) then
    Exit;
  if ( fType in [ ftFast, ftSpecialized ] ) then
    Exit;
  if ( fOptions.UpperCaseMnemonics = Value ) then
    Exit;
  fOptions.UpperCaseMnemonics := Value;

  {result := }Formatter_SetUpperCaseMnemonics( fHandle, fType, Value );
end;

function TIcedFormatter.GetUpperCaseRegisters : Boolean;
begin
  result := false;
  if ( self = nil ) then
    Exit;
  if ( fHandle = nil ) then
    Exit;
  if ( fType in [ ftFast, ftSpecialized ] ) then
    Exit;

  result := fOptions.UpperCaseRegisters;
//  result := Formatter_GetUpperCaseRegisters( fHandle );
end;

procedure TIcedFormatter.SetUpperCaseRegisters( Value : Boolean );
begin
  if ( self = nil ) then
    Exit;
  if ( fHandle = nil ) then
    Exit;
  if ( fType in [ ftFast, ftSpecialized ] ) then
    Exit;
  if ( fOptions.UpperCaseRegisters = Value ) then
    Exit;
  fOptions.UpperCaseRegisters := Value;

  {result := }Formatter_SetUpperCaseRegisters( fHandle, fType, Value );
end;

function TIcedFormatter.GetUpperCaseKeyWords : Boolean;
begin
  result := false;
  if ( self = nil ) then
    Exit;
  if ( fHandle = nil ) then
    Exit;
  if ( fType in [ ftFast, ftSpecialized ] ) then
    Exit;

  result := fOptions.UpperCaseKeyWords;
//  result := Formatter_GetUpperCaseKeyWords( fHandle );
end;

procedure TIcedFormatter.SetUpperCaseKeyWords( Value : Boolean );
begin
  if ( self = nil ) then
    Exit;
  if ( fHandle = nil ) then
    Exit;
  if ( fType in [ ftFast, ftSpecialized ] ) then
    Exit;
  if ( fOptions.UpperCaseKeyWords = Value ) then
    Exit;
  fOptions.UpperCaseKeyWords := Value;

  {result := }Formatter_SetUpperCaseKeyWords( fHandle, fType, Value );
end;

function TIcedFormatter.GetUpperCaseDecorators : Boolean;
begin
  result := false;
  if ( self = nil ) then
    Exit;
  if ( fHandle = nil ) then
    Exit;
  if ( fType in [ ftFast, ftSpecialized ] ) then
    Exit;

  result := fOptions.UpperCaseDecorators;
//  result := Formatter_GetUpperCaseDecorators( fHandle );
end;

procedure TIcedFormatter.SetUpperCaseDecorators( Value : Boolean );
begin
  if ( self = nil ) then
    Exit;
  if ( fHandle = nil ) then
    Exit;
  if ( fType in [ ftFast, ftSpecialized ] ) then
    Exit;
  if ( fOptions.UpperCaseDecorators = Value ) then
    Exit;
  fOptions.UpperCaseDecorators := Value;

  {result := }Formatter_SetUpperCaseDecorators( fHandle, fType, Value );
end;

function TIcedFormatter.GetUpperCaseEverything : Boolean;
begin
  result := false;
  if ( self = nil ) then
    Exit;
  if ( fHandle = nil ) then
    Exit;
  if ( fType in [ ftFast, ftSpecialized ] ) then
    Exit;

  result := fOptions.UpperCaseEverything;
//  result := Formatter_GetUpperCaseEverything( fHandle );
end;

procedure TIcedFormatter.SetUpperCaseEverything( Value : Boolean );
begin
  if ( self = nil ) then
    Exit;
  if ( fHandle = nil ) then
    Exit;
  if ( fType in [ ftFast, ftSpecialized ] ) then
    Exit;
  if ( fOptions.UpperCaseEverything = Value ) then
    Exit;
  fOptions.UpperCaseEverything := Value;

  {result := }Formatter_SetUpperCaseEverything( fHandle, fType, Value );
end;

function TIcedFormatter.GetFirstOperandCharIndex : Cardinal;
begin
  result := 0;
  if ( self = nil ) then
    Exit;
  if ( fHandle = nil ) then
    Exit;
  if ( fType in [ ftFast, ftSpecialized ] ) then
    Exit;

  result := fOptions.FirstOperandCharIndex;
//  result := Formatter_GetFirstOperandCharIndex( fHandle );
end;

procedure TIcedFormatter.SetFirstOperandCharIndex( Value : Cardinal );
begin
  if ( self = nil ) then
    Exit;
  if ( fHandle = nil ) then
    Exit;
  if ( fType in [ ftFast, ftSpecialized ] ) then
    Exit;
  if ( fOptions.FirstOperandCharIndex = Value ) then
    Exit;
  fOptions.FirstOperandCharIndex := Value;

  {result := }Formatter_SetFirstOperandCharIndex( fHandle, fType, Value );
end;

function TIcedFormatter.GetTabSize : Cardinal;
begin
  result := 0;
  if ( self = nil ) then
    Exit;
  if ( fHandle = nil ) then
    Exit;
  if ( fType in [ ftFast, ftSpecialized ] ) then
    Exit;

  result := fOptions.TabSize;
//  result := Formatter_GetTabSize( fHandle );
end;

procedure TIcedFormatter.SetTabSize( Value : Cardinal );
begin
  if ( self = nil ) then
    Exit;
  if ( fHandle = nil ) then
    Exit;
  if ( fType in [ ftFast, ftSpecialized ] ) then
    Exit;
  if ( fOptions.TabSize = Value ) then
    Exit;
  fOptions.TabSize := Value;

  {result := }Formatter_SetTabSize( fHandle, fType, Value );
end;

function TIcedFormatter.GetSpaceAfterMemoryBracket : Boolean;
begin
  result := false;
  if ( self = nil ) then
    Exit;
  if ( fHandle = nil ) then
    Exit;
  if ( fType in [ ftFast, ftSpecialized ] ) then
    Exit;

  result := fOptions.SpaceAfterMemoryBracket;
//  result := Formatter_GetSpaceAfterMemoryBracket( fHandle );
end;

procedure TIcedFormatter.SetSpaceAfterMemoryBracket( Value : Boolean );
begin
  if ( self = nil ) then
    Exit;
  if ( fHandle = nil ) then
    Exit;
  if ( fType in [ ftFast, ftSpecialized ] ) then
    Exit;
  if ( fOptions.SpaceAfterMemoryBracket = Value ) then
    Exit;
  fOptions.SpaceAfterMemoryBracket := Value;

  {result := }Formatter_SetSpaceAfterMemoryBracket( fHandle, fType, Value );
end;

function TIcedFormatter.GetSpaceBetweenMemoryAddOperators : Boolean;
begin
  result := false;
  if ( self = nil ) then
    Exit;
  if ( fHandle = nil ) then
    Exit;
  if ( fType in [ ftFast, ftSpecialized ] ) then
    Exit;

  result := fOptions.SpaceBetweenMemoryAddOperators;
//  result := Formatter_GetSpaceBetweenMemoryAddOperators( fHandle );
end;

procedure TIcedFormatter.SetSpaceBetweenMemoryAddOperators( Value : Boolean );
begin
  if ( self = nil ) then
    Exit;
  if ( fHandle = nil ) then
    Exit;
  if ( fType in [ ftFast, ftSpecialized ] ) then
    Exit;
  if ( fOptions.SpaceBetweenMemoryAddOperators = Value ) then
    Exit;
  fOptions.SpaceBetweenMemoryAddOperators := Value;

  {result := }Formatter_SetSpaceBetweenMemoryAddOperators( fHandle, fType, Value );
end;

function TIcedFormatter.GetSpaceBetweenMemoryMulOperators : Boolean;
begin
  result := false;
  if ( self = nil ) then
    Exit;
  if ( fHandle = nil ) then
    Exit;
  if ( fType in [ ftFast, ftSpecialized ] ) then
    Exit;

  result := fOptions.SpaceBetweenMemoryMulOperators;
//  result := Formatter_GetSpaceBetweenMemoryMulOperators( fHandle );
end;

procedure TIcedFormatter.SetSpaceBetweenMemoryMulOperators( Value : Boolean );
begin
  if ( self = nil ) then
    Exit;
  if ( fHandle = nil ) then
    Exit;
  if ( fType in [ ftFast, ftSpecialized ] ) then
    Exit;
  if ( fOptions.SpaceBetweenMemoryMulOperators = Value ) then
    Exit;
  fOptions.SpaceBetweenMemoryMulOperators := Value;

  {result := }Formatter_SetSpaceBetweenMemoryMulOperators( fHandle, fType, Value );
end;

function TIcedFormatter.GetScaleBeforeIndex : Boolean;
begin
  result := false;
  if ( self = nil ) then
    Exit;
  if ( fHandle = nil ) then
    Exit;
  if ( fType in [ ftFast, ftSpecialized ] ) then
    Exit;

  result := fOptions.ScaleBeforeIndex;
//  result := Formatter_GetScaleBeforeIndex( fHandle );
end;

procedure TIcedFormatter.SetScaleBeforeIndex( Value : Boolean );
begin
  if ( self = nil ) then
    Exit;
  if ( fHandle = nil ) then
    Exit;
  if ( fType in [ ftFast, ftSpecialized ] ) then
    Exit;
  if ( fOptions.ScaleBeforeIndex = Value ) then
    Exit;
  fOptions.ScaleBeforeIndex := Value;

  {result := }Formatter_SetScaleBeforeIndex( fHandle, fType, Value );
end;

function TIcedFormatter.GetAlwaysShowScale : Boolean;
begin
  result := false;
  if ( self = nil ) then
    Exit;
  if ( fHandle = nil ) then
    Exit;
  if ( fType in [ ftFast, ftSpecialized ] ) then
    Exit;

  result := fOptions.AlwaysShowScale;
//  result := Formatter_GetAlwaysShowScale( fHandle );
end;

procedure TIcedFormatter.SetAlwaysShowScale( Value : Boolean );
begin
  if ( self = nil ) then
    Exit;
  if ( fHandle = nil ) then
    Exit;
  if ( fType in [ ftFast, ftSpecialized ] ) then
    Exit;
  if ( fOptions.AlwaysShowScale = Value ) then
    Exit;
  fOptions.AlwaysShowScale := Value;

  {result := }Formatter_SetAlwaysShowScale( fHandle, fType, Value );
end;

function TIcedFormatter.GetShowZeroDisplacements : Boolean;
begin
  result := False;
  if ( self = nil ) then
    Exit;
  if ( fHandle = nil ) then
    Exit;
  if ( fType in [ ftFast, ftSpecialized ] ) then
    Exit;

  result := fOptions.ShowZeroDisplacements;
//  result := Formatter_GetShowZeroDisplacements( fHandle );
end;

procedure TIcedFormatter.SetShowZeroDisplacements( Value : Boolean );
begin
  if ( self = nil ) then
    Exit;
  if ( fHandle = nil ) then
    Exit;
  if ( fType in [ ftFast, ftSpecialized ] ) then
    Exit;
  if ( fOptions.ShowZeroDisplacements = Value ) then
    Exit;
  fOptions.ShowZeroDisplacements := Value;

  {result := }Formatter_SetShowZeroDisplacements( fHandle, fType, Value );
end;

function TIcedFormatter.GetHexPrefix : AnsiString;
//var
//  C : Array [ 0..255 ] of AnsiChar;
begin
  result := '';
  if ( self = nil ) then
    Exit;
  if ( fHandle = nil ) then
    Exit;
  if ( fType in [ ftFast, ftSpecialized ] ) then
    Exit;

  result := fOptions.HexPrefix;
//  Formatter_GetHexPrefix( fHandle, @C[ 0 ], Length( C ) );
//  result := C;
end;

procedure TIcedFormatter.SetHexPrefix( Value : AnsiString );
begin
  if ( self = nil ) then
    Exit;
  if ( fHandle = nil ) then
    Exit;
  if ( fType in [ ftFast, ftSpecialized ] ) then
    Exit;
  if ( fOptions.HexPrefix = Value ) then
    Exit;
  fOptions.HexPrefix := Value;

  {result := }Formatter_SetHexPrefix( fHandle, fType, PAnsiChar( Value ) );
end;

function TIcedFormatter.GetHexSuffix : AnsiString;
//var
//  C : Array [ 0..255 ] of AnsiChar;
begin
  result := '';
  if ( self = nil ) then
    Exit;
  if ( fHandle = nil ) then
    Exit;
  if ( fType in [ ftFast, ftSpecialized ] ) then
    Exit;

  result := fOptions.HexSuffix;
//  Formatter_GetHexSuffix( fHandle, @C[ 0 ], Length( C ) );
//  result := C;
end;

procedure TIcedFormatter.SetHexSuffix( Value : AnsiString );
begin
  if ( self = nil ) then
    Exit;
  if ( fHandle = nil ) then
    Exit;
  if ( fType in [ ftFast, ftSpecialized ] ) then
    Exit;
  if ( fOptions.HexSuffix = Value ) then
    Exit;
  fOptions.HexSuffix := Value;

  {result := }Formatter_SetHexSuffix( fHandle, fType, PAnsiChar( Value ) );
end;

function TIcedFormatter.GetHexDigitGroupSize : Cardinal;
begin
  result := 0;
  if ( self = nil ) then
    Exit;
  if ( fHandle = nil ) then
    Exit;
  if ( fType in [ ftFast, ftSpecialized ] ) then
    Exit;

  result := fOptions.HexDigitGroupSize;
//  result := Formatter_GetHexDigitGroupSize( fHandle );
end;

procedure TIcedFormatter.SetHexDigitGroupSize( Value : Cardinal );
begin
  if ( self = nil ) then
    Exit;
  if ( fHandle = nil ) then
    Exit;
  if ( fType in [ ftFast, ftSpecialized ] ) then
    Exit;
  if ( fOptions.HexDigitGroupSize = Value ) then
    Exit;
  fOptions.HexDigitGroupSize := Value;

  {result := }Formatter_SetHexDigitGroupSize( fHandle, fType, Value );
end;

function TIcedFormatter.GetDecimalPrefix : AnsiString;
//var
//  C : Array [ 0..255 ] of AnsiChar;
begin
  result := '';
  if ( self = nil ) then
    Exit;
  if ( fHandle = nil ) then
    Exit;
  if ( fType in [ ftFast, ftSpecialized ] ) then
    Exit;

  result := fOptions.DecimalPrefix;
//  Formatter_GetDecimalPrefix( fHandle, @C[ 0 ], Length( C ) );
//  result := C;
end;

procedure TIcedFormatter.SetDecimalPrefix( Value : AnsiString );
begin
  if ( self = nil ) then
    Exit;
  if ( fHandle = nil ) then
    Exit;
  if ( fType in [ ftFast, ftSpecialized ] ) then
    Exit;
  if ( fOptions.DecimalPrefix = Value ) then
    Exit;
  fOptions.DecimalPrefix := Value;

  {result := }Formatter_SetDecimalPrefix( fHandle, fType, PAnsiChar( Value ) );
end;

function TIcedFormatter.GetDecimalSuffix : AnsiString;
//var
//  C : Array [ 0..255 ] of AnsiChar;
begin
  result := '';
  if ( self = nil ) then
    Exit;
  if ( fHandle = nil ) then
    Exit;
  if ( fType in [ ftFast, ftSpecialized ] ) then
    Exit;

  result := fOptions.DecimalSuffix;
//  Formatter_GetDecimalSuffix( fHandle, @C[ 0 ], Length( C ) );
//  result := C;
end;

procedure TIcedFormatter.SetDecimalSuffix( Value : AnsiString );
begin
  if ( self = nil ) then
    Exit;
  if ( fHandle = nil ) then
    Exit;
  if ( fType in [ ftFast, ftSpecialized ] ) then
    Exit;
  if ( fOptions.DecimalSuffix = Value ) then
    Exit;
  fOptions.DecimalSuffix := Value;

  {result := }Formatter_SetDecimalSuffix( fHandle, fType, PAnsiChar( Value ) );
end;

function TIcedFormatter.GetDecimalDigitGroupSize : Cardinal;
begin
  result := 0;
  if ( self = nil ) then
    Exit;
  if ( fHandle = nil ) then
    Exit;
  if ( fType in [ ftFast, ftSpecialized ] ) then
    Exit;

  result := fOptions.DecimalDigitGroupSize;
//  result := Formatter_GetDecimalDigitGroupSize( fHandle );
end;

procedure TIcedFormatter.SetDecimalDigitGroupSize( Value : Cardinal );
begin
  if ( self = nil ) then
    Exit;
  if ( fHandle = nil ) then
    Exit;
  if ( fType in [ ftFast, ftSpecialized ] ) then
    Exit;
  if ( fOptions.DecimalDigitGroupSize = Value ) then
    Exit;
  fOptions.DecimalDigitGroupSize := Value;

  {result := }Formatter_SetDecimalDigitGroupSize( fHandle, fType, Value );
end;

function TIcedFormatter.GetOctalPrefix : AnsiString;
//var
//  C : Array [ 0..255 ] of AnsiChar;
begin
  result := '';
  if ( self = nil ) then
    Exit;
  if ( fHandle = nil ) then
    Exit;
  if ( fType in [ ftFast, ftSpecialized ] ) then
    Exit;

  result := fOptions.OctalPrefix;
//  Formatter_GetOctalPrefix( fHandle, @C[ 0 ], Length( C ) );
//  result := C;
end;

procedure TIcedFormatter.SetOctalPrefix( Value : AnsiString );
begin
  if ( self = nil ) then
    Exit;
  if ( fHandle = nil ) then
    Exit;
  if ( fType in [ ftFast, ftSpecialized ] ) then
    Exit;
  if ( fOptions.OctalPrefix = Value ) then
    Exit;
  fOptions.OctalPrefix := Value;

  {result := }Formatter_SetOctalPrefix( fHandle, fType, PAnsiChar( Value ) );
end;

function TIcedFormatter.GetOctalSuffix : AnsiString;
//var
//  C : Array [ 0..255 ] of AnsiChar;
begin
  result := '';
  if ( self = nil ) then
    Exit;
  if ( fHandle = nil ) then
    Exit;
  if ( fType in [ ftFast, ftSpecialized ] ) then
    Exit;

  result := fOptions.OctalSuffix;
//  Formatter_GetOctalSuffix( fHandle, @C[ 0 ], Length( C ) );
//  result := C;
end;

procedure TIcedFormatter.SetOctalSuffix( Value : AnsiString );
begin
  if ( self = nil ) then
    Exit;
  if ( fHandle = nil ) then
    Exit;
  if ( fType in [ ftFast, ftSpecialized ] ) then
    Exit;
  if ( fOptions.OctalSuffix = Value ) then
    Exit;
  fOptions.OctalSuffix := Value;

  {result := }Formatter_SetOctalSuffix( fHandle, fType, PAnsiChar( Value ) );
end;

function TIcedFormatter.GetOctalDigitGroupSize : Cardinal;
begin
  result := 0;
  if ( self = nil ) then
    Exit;
  if ( fHandle = nil ) then
    Exit;
  if ( fType in [ ftFast, ftSpecialized ] ) then
    Exit;

  result := fOptions.OctalDigitGroupSize;
//  result := Formatter_GetOctalDigitGroupSize( fHandle );
end;

procedure TIcedFormatter.SetOctalDigitGroupSize( Value : Cardinal );
begin
  if ( self = nil ) then
    Exit;
  if ( fHandle = nil ) then
    Exit;
  if ( fType in [ ftFast, ftSpecialized ] ) then
    Exit;
  if ( fOptions.OctalDigitGroupSize = Value ) then
    Exit;
  fOptions.OctalDigitGroupSize := Value;

  {result := }Formatter_SetOctalDigitGroupSize( fHandle, fType, Value );
end;

function TIcedFormatter.GetBinaryPrefix : AnsiString;
//var
//  C : Array [ 0..255 ] of AnsiChar;
begin
  result := '';
  if ( self = nil ) then
    Exit;
  if ( fHandle = nil ) then
    Exit;
  if ( fType in [ ftFast, ftSpecialized ] ) then
    Exit;

  result := fOptions.BinaryPrefix;
//  Formatter_GetBinaryPrefix( fHandle, @C[ 0 ], Length( C ) );
//  result := C;
end;

procedure TIcedFormatter.SetBinaryPrefix( Value : AnsiString );
begin
  if ( self = nil ) then
    Exit;
  if ( fHandle = nil ) then
    Exit;
  if ( fType in [ ftFast, ftSpecialized ] ) then
    Exit;
  if ( fOptions.BinaryPrefix = Value ) then
    Exit;
  fOptions.BinaryPrefix := Value;

  {result := }Formatter_SetBinaryPrefix( fHandle, fType, PAnsiChar( Value ) );
end;

function TIcedFormatter.GetBinarySuffix : AnsiString;
//var
//  C : Array [ 0..255 ] of AnsiChar;
begin
  result := '';
  if ( self = nil ) then
    Exit;
  if ( fHandle = nil ) then
    Exit;
  if ( fType in [ ftFast, ftSpecialized ] ) then
    Exit;

  result := fOptions.BinarySuffix;
//  Formatter_GetBinarySuffix( fHandle, @C[ 0 ], Length( C ) );
//  result := C;
end;

procedure TIcedFormatter.SetBinarySuffix( Value : AnsiString );
begin
  if ( self = nil ) then
    Exit;
  if ( fHandle = nil ) then
    Exit;
  if ( fType in [ ftFast, ftSpecialized ] ) then
    Exit;
  if ( fOptions.BinarySuffix = Value ) then
    Exit;
  fOptions.BinarySuffix := Value;

  {result := }Formatter_SetBinarySuffix( fHandle, fType, PAnsiChar( Value ) );
end;

function TIcedFormatter.GetBinaryDigitGroupSize : Cardinal;
begin
  result := 0;
  if ( self = nil ) then
    Exit;
  if ( fHandle = nil ) then
    Exit;
  if ( fType in [ ftFast, ftSpecialized ] ) then
    Exit;

  result := fOptions.BinaryDigitGroupSize;
//  result := Formatter_GetBinaryDigitGroupSize( fHandle );
end;

procedure TIcedFormatter.SetBinaryDigitGroupSize( Value : Cardinal );
begin
  if ( self = nil ) then
    Exit;
  if ( fHandle = nil ) then
    Exit;
  if ( fType in [ ftFast, ftSpecialized ] ) then
    Exit;
  if ( fOptions.BinaryDigitGroupSize = Value ) then
    Exit;
  fOptions.BinaryDigitGroupSize := Value;

  {result := }Formatter_SetBinaryDigitGroupSize( fHandle, fType, Value );
end;

function TIcedFormatter.GetDigitSeparator : AnsiString;
//var
//  C : Array [ 0..255 ] of AnsiChar;
begin
  result := '';
  if ( self = nil ) then
    Exit;
  if ( fHandle = nil ) then
    Exit;
  if ( fType in [ ftFast, ftSpecialized ] ) then
    Exit;

  result := fOptions.DigitSeparator;
//  Formatter_GetDigitSeparator( fHandle, @C[ 0 ], Length( C ) );
//  result := C;
end;

procedure TIcedFormatter.SetDigitSeparator( Value : AnsiString );
begin
  if ( self = nil ) then
    Exit;
  if ( fHandle = nil ) then
    Exit;
  if ( fType in [ ftFast, ftSpecialized ] ) then
    Exit;
  if ( fOptions.DigitSeparator = Value ) then
    Exit;
  fOptions.DigitSeparator := Value;

  {result := }Formatter_SetDigitSeparator( fHandle, fType, PAnsiChar( Value ) );
end;

function TIcedFormatter.GetLeadingZeros : Boolean;
begin
  result := False;
  if ( self = nil ) then
    Exit;
  if ( fHandle = nil ) then
    Exit;
  if ( fType in [ ftFast, ftSpecialized ] ) then
    Exit;

  result := fOptions.LeadingZeros;
//  result := Formatter_GetLeadingZeros( fHandle );
end;

procedure TIcedFormatter.SetLeadingZeros( Value : Boolean );
begin
  if ( self = nil ) then
    Exit;
  if ( fHandle = nil ) then
    Exit;
  if ( fType in [ ftFast, ftSpecialized ] ) then
    Exit;
  if ( fOptions.LeadingZeros = Value ) then
    Exit;
  fOptions.LeadingZeros := Value;

  {result := }Formatter_SetLeadingZeros( fHandle, fType, Value );
end;

function TIcedFormatter.GetSmallHexNumbersInDecimal : Boolean;
begin
  result := false;
  if ( self = nil ) then
    Exit;
  if ( fHandle = nil ) then
    Exit;
  if ( fType in [ ftFast, ftSpecialized ] ) then
    Exit;

  result := fOptions.SmallHexNumbersInDecimal;
//  result := Formatter_GetSmallHexNumbersInDecimal( fHandle );
end;

procedure TIcedFormatter.SetSmallHexNumbersInDecimal( Value : Boolean );
begin
  if ( self = nil ) then
    Exit;
  if ( fHandle = nil ) then
    Exit;
  if ( fType in [ ftFast, ftSpecialized ] ) then
    Exit;
  if ( fOptions.SmallHexNumbersInDecimal = Value ) then
    Exit;
  fOptions.SmallHexNumbersInDecimal := Value;

  {result := }Formatter_SetSmallHexNumbersInDecimal( fHandle, fType, Value );
end;

function TIcedFormatter.GetAddLeadingZeroToHexNumbers : Boolean;
begin
  result := false;
  if ( self = nil ) then
    Exit;
  if ( fHandle = nil ) then
    Exit;
  if ( fType in [ ftFast, ftSpecialized ] ) then
    Exit;

  result := fOptions.AddLeadingZeroToHexNumbers;
//  result := Formatter_GetAddLeadingZeroToHexNumbers( fHandle );
end;

procedure TIcedFormatter.SetAddLeadingZeroToHexNumbers( Value : Boolean );
begin
  if ( self = nil ) then
    Exit;
  if ( fHandle = nil ) then
    Exit;
  if ( fType in [ ftFast, ftSpecialized ] ) then
    Exit;
  if ( fOptions.AddLeadingZeroToHexNumbers = Value ) then
    Exit;
  fOptions.AddLeadingZeroToHexNumbers := Value;

  {result := }Formatter_SetAddLeadingZeroToHexNumbers( fHandle, fType, Value );
end;

function TIcedFormatter.GetNumberBase : TNumberBase;
begin
  result.NumberBase := nbHexadecimal;
  if ( self = nil ) then
    Exit;
  if ( fHandle = nil ) then
    Exit;
  if ( fType in [ ftFast, ftSpecialized ] ) then
    Exit;

  result := fOptions.NumberBase;
//  result := Formatter_GetNumberBase( fHandle );
end;

procedure TIcedFormatter.SetNumberBase( Value : TNumberBase );
begin
  if ( self = nil ) then
    Exit;
  if ( fHandle = nil ) then
    Exit;
  if ( fType in [ ftFast, ftSpecialized ] ) then
    Exit;
  if ( fOptions.NumberBase.NumberBase = Value.NumberBase ) then
    Exit;
  fOptions.NumberBase := Value;

  {result := }Formatter_SetNumberBase( fHandle, fType, Value );
end;

function TIcedFormatter.GetBranchLeadingZeros : Boolean;
begin
  result := false;
  if ( self = nil ) then
    Exit;
  if ( fHandle = nil ) then
    Exit;
  if ( fType in [ ftFast, ftSpecialized ] ) then
    Exit;

  result := fOptions.BranchLeadingZeros;
//  result := Formatter_GetBranchLeadingZeros( fHandle );
end;

procedure TIcedFormatter.SetBranchLeadingZeros( Value : Boolean );
begin
  if ( self = nil ) then
    Exit;
  if ( fHandle = nil ) then
    Exit;
  if ( fType in [ ftFast, ftSpecialized ] ) then
    Exit;
  if ( fOptions.BranchLeadingZeros = Value ) then
    Exit;
  fOptions.BranchLeadingZeros := Value;

  {result := }Formatter_SetBranchLeadingZeros( fHandle, fType, Value );
end;

function TIcedFormatter.GetSignedImmediateOperands : Boolean;
begin
  result := false;
  if ( self = nil ) then
    Exit;
  if ( fHandle = nil ) then
    Exit;
  if ( fType in [ ftFast, ftSpecialized ] ) then
    Exit;

  result := fOptions.SignedImmediateOperands;
//  result := Formatter_GetSignedImmediateOperands( fHandle );
end;

procedure TIcedFormatter.SetSignedImmediateOperands( Value : Boolean );
begin
  if ( self = nil ) then
    Exit;
  if ( fHandle = nil ) then
    Exit;
  if ( fType in [ ftFast, ftSpecialized ] ) then
    Exit;
  if ( fOptions.SignedImmediateOperands = Value ) then
    Exit;
  fOptions.SignedImmediateOperands := Value;

  {result := }Formatter_SetSignedImmediateOperands( fHandle, fType, Value );
end;

function TIcedFormatter.GetSignedMemoryDisplacements : Boolean;
begin
  result := false;
  if ( self = nil ) then
    Exit;
  if ( fHandle = nil ) then
    Exit;
  if ( fType in [ ftFast, ftSpecialized ] ) then
    Exit;

  result := fOptions.SignedMemoryDisplacements;
//  result := Formatter_GetSignedMemoryDisplacements( fHandle );
end;

procedure TIcedFormatter.SetSignedMemoryDisplacements( Value : Boolean );
begin
  if ( self = nil ) then
    Exit;
  if ( fHandle = nil ) then
    Exit;
  if ( fType in [ ftFast, ftSpecialized ] ) then
    Exit;
  if ( fOptions.SignedMemoryDisplacements = Value ) then
    Exit;
  fOptions.SignedMemoryDisplacements := Value;

  {result := }Formatter_SetSignedMemoryDisplacements( fHandle, fType, Value );
end;

function TIcedFormatter.GetDisplacementLeadingZeros : Boolean;
begin
  result := false;
  if ( self = nil ) then
    Exit;
  if ( fHandle = nil ) then
    Exit;
  if ( fType in [ ftFast, ftSpecialized ] ) then
    Exit;

  result := fOptions.DisplacementLeadingZeros;
//  result := Formatter_GetDisplacementLeadingZeros( fHandle );
end;

procedure TIcedFormatter.SetDisplacementLeadingZeros( Value : Boolean );
begin
  if ( self = nil ) then
    Exit;
  if ( fHandle = nil ) then
    Exit;
  if ( fType in [ ftFast, ftSpecialized ] ) then
    Exit;
  if ( fOptions.DisplacementLeadingZeros = Value ) then
    Exit;
  fOptions.DisplacementLeadingZeros := Value;

  {result := }Formatter_SetDisplacementLeadingZeros( fHandle, fType, Value );
end;

function TIcedFormatter.GetMemorySizeOptions : TMemorySizeOptions;
begin
  result.MemorySizeOptions := msoDefault;
  if ( self = nil ) then
    Exit;
  if ( fHandle = nil ) then
    Exit;
  if ( fType in [ ftFast, ftSpecialized ] ) then
    Exit;

  result := fOptions.MemorySizeOptions;
//  result := Formatter_GetMemorySizeOptions( fHandle );
end;

procedure TIcedFormatter.SetMemorySizeOptions( Value : TMemorySizeOptions );
begin
  if ( self = nil ) then
    Exit;
  if ( fHandle = nil ) then
    Exit;
  if ( fType in [ ftFast, ftSpecialized ] ) then
    Exit;
  if ( fOptions.MemorySizeOptions.MemorySizeOptions = Value.MemorySizeOptions ) then
    Exit;
  fOptions.MemorySizeOptions := Value;

  {result := }Formatter_SetMemorySizeOptions( fHandle, fType, Value );
end;

function TIcedFormatter.GetShowBranchSize : Boolean;
begin
  result := false;
  if ( self = nil ) then
    Exit;
  if ( fHandle = nil ) then
    Exit;
  if ( fType in [ ftFast, ftSpecialized ] ) then
    Exit;

  result := fOptions.ShowBranchSize;
//  result := Formatter_GetShowBranchSize( fHandle );
end;

procedure TIcedFormatter.SetShowBranchSize( Value : Boolean );
begin
  if ( self = nil ) then
    Exit;
  if ( fHandle = nil ) then
    Exit;
  if ( fType in [ ftFast, ftSpecialized ] ) then
    Exit;
  if ( fOptions.ShowBranchSize = Value ) then
    Exit;
  fOptions.ShowBranchSize := Value;

  {result := }Formatter_SetShowBranchSize( fHandle, fType, Value );
end;

function TIcedFormatter.GetPreferST0 : Boolean;
begin
  result := false;
  if ( self = nil ) then
    Exit;
  if ( fHandle = nil ) then
    Exit;
  if ( fType in [ ftFast, ftSpecialized ] ) then
    Exit;

  result := fOptions.PreferST0;
//  result := Formatter_GetPreferST0( fHandle );
end;

procedure TIcedFormatter.SetPreferST0( Value : Boolean );
begin
  if ( self = nil ) then
    Exit;
  if ( fHandle = nil ) then
    Exit;
  if ( fType in [ ftFast, ftSpecialized ] ) then
    Exit;
  if ( fOptions.PreferST0 = Value ) then
    Exit;
  fOptions.PreferST0 := Value;

  {result := }Formatter_SetPreferST0( fHandle, fType, Value );
end;

function TIcedFormatter.GetShowUselessPrefixes : Boolean;
begin
  result := false;
  if ( self = nil ) then
    Exit;
  if ( fHandle = nil ) then
    Exit;
  if ( fType in [ ftFast, ftSpecialized ] ) then
    Exit;

  result := fOptions.ShowUselessPrefixes;
//  result := Formatter_GetShowUselessPrefixes( fHandle );
end;

procedure TIcedFormatter.SetShowUselessPrefixes( Value : Boolean );
begin
  if ( self = nil ) then
    Exit;
  if ( fHandle = nil ) then
    Exit;
  if ( fType in [ ftFast, ftSpecialized ] ) then
    Exit;
  if ( fOptions.ShowUselessPrefixes = Value ) then
    Exit;
  fOptions.ShowUselessPrefixes := Value;

  {result := }Formatter_SetShowUselessPrefixes( fHandle, fType, Value );
end;

function TIcedFormatter.GetCC_b : TCC_b;
begin
  result := b;
  if ( self = nil ) then
    Exit;
  if ( fHandle = nil ) then
    Exit;
  if ( fType in [ ftFast, ftSpecialized ] ) then
    Exit;

  result := fOptions.CC_b;
//  result := Formatter_GetCC_b( fHandle );
end;

procedure TIcedFormatter.SetCC_b( Value : TCC_b );
begin
  if ( self = nil ) then
    Exit;
  if ( fHandle = nil ) then
    Exit;
  if ( fType in [ ftFast, ftSpecialized ] ) then
    Exit;
  if ( fOptions.CC_b = Value ) then
    Exit;
  fOptions.CC_b := Value;

  {result := }Formatter_SetCC_b( fHandle, fType, Value );
end;

function TIcedFormatter.GetCC_ae : TCC_ae;
begin
  result := ae;
  if ( self = nil ) then
    Exit;
  if ( fHandle = nil ) then
    Exit;
  if ( fType in [ ftFast, ftSpecialized ] ) then
    Exit;

  result := fOptions.CC_ae;
//  result := Formatter_GetCC_ae( fHandle );
end;

procedure TIcedFormatter.SetCC_ae( Value : TCC_ae );
begin
  if ( self = nil ) then
    Exit;
  if ( fHandle = nil ) then
    Exit;
  if ( fType in [ ftFast, ftSpecialized ] ) then
    Exit;
  if ( fOptions.CC_ae = Value ) then
    Exit;
  fOptions.CC_ae := Value;

  {result := }Formatter_SetCC_ae( fHandle, fType, Value );
end;

function TIcedFormatter.GetCC_e : TCC_e;
begin
  result := e;
  if ( self = nil ) then
    Exit;
  if ( fHandle = nil ) then
    Exit;
  if ( fType in [ ftFast, ftSpecialized ] ) then
    Exit;

  result := fOptions.CC_e;
//  result := Formatter_GetCC_e( fHandle );
end;

procedure TIcedFormatter.SetCC_e( Value : TCC_e );
begin
  if ( self = nil ) then
    Exit;
  if ( fHandle = nil ) then
    Exit;
  if ( fType in [ ftFast, ftSpecialized ] ) then
    Exit;
  if ( fOptions.CC_e = Value ) then
    Exit;
  fOptions.CC_e := Value;

  {result := }Formatter_SetCC_e( fHandle, fType, Value );
end;

function TIcedFormatter.GetCC_ne : TCC_ne;
begin
  result := ne;
  if ( self = nil ) then
    Exit;
  if ( fHandle = nil ) then
    Exit;
  if ( fType in [ ftFast, ftSpecialized ] ) then
    Exit;

  result := fOptions.CC_ne;
//  result := Formatter_GetCC_ne( fHandle );
end;

procedure TIcedFormatter.SetCC_ne( Value : TCC_ne );
begin
  if ( self = nil ) then
    Exit;
  if ( fHandle = nil ) then
    Exit;
  if ( fType in [ ftFast, ftSpecialized ] ) then
    Exit;
  if ( fOptions.CC_ne = Value ) then
    Exit;
  fOptions.CC_ne := Value;

  {result := }Formatter_SetCC_ne( fHandle, fType, Value );
end;

function TIcedFormatter.GetCC_be : TCC_be;
begin
  result := be;
  if ( self = nil ) then
    Exit;
  if ( fHandle = nil ) then
    Exit;
  if ( fType in [ ftFast, ftSpecialized ] ) then
    Exit;

  result := fOptions.CC_be;
//  result := Formatter_GetCC_be( fHandle );
end;

procedure TIcedFormatter.SetCC_be( Value : TCC_be );
begin
  if ( self = nil ) then
    Exit;
  if ( fHandle = nil ) then
    Exit;
  if ( fType in [ ftFast, ftSpecialized ] ) then
    Exit;
  if ( fOptions.CC_be = Value ) then
    Exit;
  fOptions.CC_be := Value;

  {result := }Formatter_SetCC_be( fHandle, fType, Value );
end;

function TIcedFormatter.GetCC_a : TCC_a;
begin
  result := a;
  if ( self = nil ) then
    Exit;
  if ( fHandle = nil ) then
    Exit;
  if ( fType in [ ftFast, ftSpecialized ] ) then
    Exit;

  result := fOptions.CC_a;
//  result := Formatter_GetCC_a( fHandle );
end;

procedure TIcedFormatter.SetCC_a( Value : TCC_a );
begin
  if ( self = nil ) then
    Exit;
  if ( fHandle = nil ) then
    Exit;
  if ( fType in [ ftFast, ftSpecialized ] ) then
    Exit;
  if ( fOptions.CC_a = Value ) then
    Exit;
  fOptions.CC_a := Value;

  {result := }Formatter_SetCC_a( fHandle, fType, Value );
end;

function TIcedFormatter.GetCC_p : TCC_p;
begin
  result := p;
  if ( self = nil ) then
    Exit;
  if ( fHandle = nil ) then
    Exit;
  if ( fType in [ ftFast, ftSpecialized ] ) then
    Exit;

  result := fOptions.CC_p;
//  result := Formatter_GetCC_p( fHandle );
end;

procedure TIcedFormatter.SetCC_p( Value : TCC_p );
begin
  if ( self = nil ) then
    Exit;
  if ( fHandle = nil ) then
    Exit;
  if ( fType in [ ftFast, ftSpecialized ] ) then
    Exit;
  if ( fOptions.CC_p = Value ) then
    Exit;
  fOptions.CC_p := Value;

  {result := }Formatter_SetCC_p( fHandle, fType, Value );
end;

function TIcedFormatter.GetCC_np : TCC_np;
begin
  result := np;
  if ( self = nil ) then
    Exit;
  if ( fHandle = nil ) then
    Exit;
  if ( fType in [ ftFast, ftSpecialized ] ) then
    Exit;

  result := fOptions.CC_np;
//  result := Formatter_GetCC_np( fHandle );
end;

procedure TIcedFormatter.SetCC_np( Value : TCC_np );
begin
  if ( self = nil ) then
    Exit;
  if ( fHandle = nil ) then
    Exit;
  if ( fType in [ ftFast, ftSpecialized ] ) then
    Exit;
  if ( fOptions.CC_np = Value ) then
    Exit;
  fOptions.CC_np := Value;

  {result := }Formatter_SetCC_np( fHandle, fType, Value );
end;

function TIcedFormatter.GetCC_l : TCC_l;
begin
  result := l;
  if ( self = nil ) then
    Exit;
  if ( fHandle = nil ) then
    Exit;
  if ( fType in [ ftFast, ftSpecialized ] ) then
    Exit;

  result := fOptions.CC_l;
//  result := Formatter_GetCC_l( fHandle );
end;

procedure TIcedFormatter.SetCC_l( Value : TCC_l );
begin
  if ( self = nil ) then
    Exit;
  if ( fHandle = nil ) then
    Exit;
  if ( fType in [ ftFast, ftSpecialized ] ) then
    Exit;
  if ( fOptions.CC_l = Value ) then
    Exit;
  fOptions.CC_l := Value;

  {result := }Formatter_SetCC_l( fHandle, fType, Value );
end;

function TIcedFormatter.GetCC_ge : TCC_ge;
begin
  result := ge;
  if ( self = nil ) then
    Exit;
  if ( fHandle = nil ) then
    Exit;
  if ( fType in [ ftFast, ftSpecialized ] ) then
    Exit;

  result := fOptions.CC_ge;
//  result := Formatter_GetCC_ge( fHandle );
end;

procedure TIcedFormatter.SetCC_ge( Value : TCC_ge );
begin
  if ( self = nil ) then
    Exit;
  if ( fHandle = nil ) then
    Exit;
  if ( fType in [ ftFast, ftSpecialized ] ) then
    Exit;
  if ( fOptions.CC_ge = Value ) then
    Exit;
  fOptions.CC_ge := Value;

  {result := }Formatter_SetCC_ge( fHandle, fType, Value );
end;

function TIcedFormatter.GetCC_le : TCC_le;
begin
  result := le;
  if ( self = nil ) then
    Exit;
  if ( fHandle = nil ) then
    Exit;
  if ( fType in [ ftFast, ftSpecialized ] ) then
    Exit;

  result := fOptions.CC_le;
//  result := Formatter_GetCC_le( fHandle );
end;

procedure TIcedFormatter.SetCC_le( Value : TCC_le );
begin
  if ( self = nil ) then
    Exit;
  if ( fHandle = nil ) then
    Exit;
  if ( fType in [ ftFast, ftSpecialized ] ) then
    Exit;
  if ( fOptions.CC_le = Value ) then
    Exit;
  fOptions.CC_le := Value;

  {result := }Formatter_SetCC_le( fHandle, fType, Value );
end;

function TIcedFormatter.GetCC_g : TCC_g;
begin
  result := g;
  if ( self = nil ) then
    Exit;
  if ( fHandle = nil ) then
    Exit;
  if ( fType in [ ftFast, ftSpecialized ] ) then
    Exit;

  result := fOptions.CC_g;
//  result := Formatter_GetCC_g( fHandle );
end;

procedure TIcedFormatter.SetCC_g( Value : TCC_g );
begin
  if ( self = nil ) then
    Exit;
  if ( fHandle = nil ) then
    Exit;
  if ( fType in [ ftFast, ftSpecialized ] ) then
    Exit;
  if ( fOptions.CC_g = Value ) then
    Exit;
  fOptions.CC_g := Value;

  {result := }Formatter_SetCC_g( fHandle, fType, Value );
end;

procedure TIcedFormatter.SetShowSymbols( Value : Boolean );
begin
  if ( self = nil ) then
    Exit;
  if ( fShowSymbols = Value ) then
    Exit;
  if ( fType in [ ftFast, ftSpecialized ] ) then
    Exit;
  if Value AND ( ( @fSymbolResolver = nil ){$IFDEF AssemblyTools} AND ( fSymbolHandler = nil ){$ENDIF} ) then
    Exit;
  fShowSymbols := Value;    
  CreateHandle( True{KeepConfiguration} );
end;

{$IFDEF AssemblyTools}
procedure TIcedFormatter.SetSymbolHandler( Value : TSymbolHandler );
begin
  if ( self = nil ) then
    Exit;
  if ( fSymbolHandler = Value ) then
    Exit;

  if ( fType in [ ftFast, ftSpecialized ] ) then
    Exit;

  fSymbolHandler := Value;
  CreateHandle( True{KeepConfiguration} );
end;
{$ENDIF AssemblyTools}

// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
constructor TIced.Create( Bitness : TIcedBitness = bt64 );
begin
  inherited Create;
  fDecoder      := TIcedDecoder.Create( Bitness );
  fEncoder      := TIcedEncoder.Create( Bitness );
  fBlockEncoder := TIcedBlockEncoder.Create( Bitness );
  fInfoFactory  := TInstructionInfoFactory.Create;
  fFormatter    := TIcedFormatter.Create;
end;

destructor TIced.Destroy;
begin
  fDecoder.Free;
  fEncoder.Free;
  fBlockEncoder.Free;
  fInfoFactory.Free;
  fFormatter.Free;

  inherited;
end;

// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// Decoder
function TIced.DecodeFromFile( FileName : String; Size : Cardinal; Offset : UInt64; StrL_Assembly : TStrings; CodeOffset : UInt64 = UInt64( 0 ); StrL_Hex : TStrings = nil; {$IFDEF AssemblyTools}Details : pIcedDetails = nil;{$ENDIF} DecoderSettings : Cardinal = doNONE; {$IFDEF AssemblyTools}CalcJumpLines : Boolean = True;{$ENDIF} CombineNOP : boolean = False ) : Cardinal;
var
  S : TMemoryStream;
begin
  result := 0;
  if ( self = nil ) then
    Exit;
  if ( FileName = '' ) then
    Exit;

  if NOT FileExists( FileName ) then
    Exit;

  S := TMemoryStream.Create;
  S.LoadFromFile( FileName );

  if ( Offset <= S.Size ) then
    S.Position := Offset;
  result := DecodeFromStream( S, Size, StrL_Assembly, CodeOffset, StrL_Hex, {$IFDEF AssemblyTools}Details,{$ENDIF} DecoderSettings, {$IFDEF AssemblyTools}CalcJumpLines,{$ENDIF} CombineNOP );
  S.Free;
end;

function TIced.DecodeFromStream( Data : TMemoryStream; Size : Cardinal; StrL_Assembly : TStrings; CodeOffset : UInt64 = UInt64( 0 ); StrL_Hex : TStrings = nil; {$IFDEF AssemblyTools}Details : pIcedDetails = nil;{$ENDIF} DecoderSettings : Cardinal = doNONE; {$IFDEF AssemblyTools}CalcJumpLines : Boolean = True;{$ENDIF} CombineNOP : boolean = False ) : Cardinal;
begin
  if ( Data = nil ) then
    begin
    result := 0;
    Exit;
    end;
  if ( Size = 0 ) then
    Size := Data.Size-Data.Position;
  if ( Size > Data.Size-Data.Position ) then
    Size := Data.Size-Data.Position;

  if CombineNOP then
    result := DecodeCombineNOP( {$IF CompilerVersion < 23}PByte( PAnsiChar{$ELSE}( PByte{$IFEND}( Data.Memory )+Data.Position ), Size, StrL_Assembly, CodeOffset, StrL_Hex, {$IFDEF AssemblyTools}Details,{$ENDIF} DecoderSettings{$IFDEF AssemblyTools}, CalcJumpLines{$ENDIF} )
  else
    result := Decode( {$IF CompilerVersion < 23}PByte( PAnsiChar{$ELSE}( PByte{$IFEND}( Data.Memory )+Data.Position ), Size, StrL_Assembly, CodeOffset, StrL_Hex, {$IFDEF AssemblyTools}Details,{$ENDIF} DecoderSettings{$IFDEF AssemblyTools}, CalcJumpLines{$ENDIF} );
end;

function TIced.Decode( Data : PByte; Size : Cardinal; StrL_Assembly : TStrings; CodeOffset : UInt64 = UInt64( 0 ); StrL_Hex : TStrings = nil; {$IFDEF AssemblyTools}Details : pIcedDetails = nil;{$ENDIF} DecoderSettings : Cardinal = doNONE{$IFDEF AssemblyTools}; CalcJumpLines : Boolean = True{$ENDIF} ) : Cardinal;
const
  DETAILS_BLOCKSIZE = 1000;
var
  Instruction : TInstruction;
  tOutput     : Array [ 0..255 ] of AnsiChar;
  i           : Integer;
  S           : String;
begin
  result := 0;
  if ( self = nil ) then
    Exit;
  if ( Data = nil ) OR ( Size = 0 ) then
    Exit;

//  if ( StrL_Assembly <> nil ) then
//    StrL_Assembly.Clear;

  fDecoder.SetData( Data, Size, CodeOffset, DecoderSettings );

  {$IFDEF AssemblyTools}
  if ( Details <> nil ) then
    begin
    if ( StrL_Assembly <> nil ) AND ( StrL_Hex <> nil ) then
      begin
      while fDecoder.CanDecode do
        begin
        if ( Details^.Count >= Length( Details^.Items ) ) then
          SetLength( Details^.Items, Details^.Count+DETAILS_BLOCKSIZE );

        fDecoder.Decode( Instruction, Details^ );
        Inc( Details^.Count );

        Formatter.Format( Instruction, tOutput, Length( tOutput ) );
        StrL_Assembly.Add( String( tOutput ) );

        S := '';
        for i := 0 to Instruction.len-1 do
          begin
          S := S + IntToHex( Data^{$IFNDEF UNICODE}, 2{$ENDIF} );
          Inc( Data );
          end;
        StrL_Hex.Add( S );

        Inc( result, Instruction.len );
        end;
      end
    else if ( StrL_Assembly <> nil ) then
      begin
      while fDecoder.CanDecode do
        begin
        if ( Details^.Count >= Length( Details^.Items ) ) then
          SetLength( Details^.Items, Details^.Count+DETAILS_BLOCKSIZE );

        fDecoder.Decode( Instruction, Details^ );
        Inc( Details^.Count );

        Formatter.Format( Instruction, tOutput, Length( tOutput ) );
        StrL_Assembly.Add( String( tOutput ) );

        Inc( result, Instruction.len );
        end;
      end
    else if ( StrL_Hex <> nil ) then
      begin
      while fDecoder.CanDecode do
        begin
        if ( Details^.Count >= Length( Details^.Items ) ) then
          SetLength( Details^.Items, Details^.Count+DETAILS_BLOCKSIZE );

        fDecoder.Decode( Instruction, Details^ );
        Inc( Details^.Count );

        S := '';
        for i := 0 to Instruction.len-1 do
          begin
          S := S + IntToHex( Data^{$IFNDEF UNICODE}, 2{$ENDIF} );
          Inc( Data );
          end;
        StrL_Hex.Add( S );

        Inc( result, Instruction.len );
        end;
      end
    else
      begin
      while fDecoder.CanDecode do
        begin
        if ( Details^.Count >= Length( Details^.Items ) ) then
          SetLength( Details^.Items, Details^.Count+DETAILS_BLOCKSIZE );

        fDecoder.Decode( Instruction, Details^ );
        Inc( Details^.Count );

        Inc( result, Instruction.len );
        end;
      end;

    SetLength( Details^.Items, Details^.Count );
    SetLength( Details^.JumpTargets, Details^.TargetCount );
    if CalcJumpLines then
      JumpLines( Details^ );
    end
  else
  {$ENDIF AssemblyTools}
    begin
    if ( StrL_Assembly <> nil ) AND ( StrL_Hex <> nil ) then
      begin
      while fDecoder.CanDecode do
        begin
        fDecoder.Decode( Instruction );

        Formatter.Format( Instruction, tOutput, Length( tOutput ) );
        StrL_Assembly.Add( String( tOutput ) );

        S := '';
        for i := 0 to Instruction.len-1 do
          begin
          S := S + IntToHex( Data^{$IFNDEF UNICODE}, 2{$ENDIF} );
          Inc( Data );
          end;
        StrL_Hex.Add( S );

        Inc( result, Instruction.len );
        end;
      end
    else if ( StrL_Assembly <> nil ) then
      begin
      while fDecoder.CanDecode do
        begin
        fDecoder.Decode( Instruction );

        Formatter.Format( Instruction, tOutput, Length( tOutput ) );
        StrL_Assembly.Add( String( tOutput ) );

        Inc( result, Instruction.len );
        end;
      end
    else if ( StrL_Hex <> nil ) then
      begin
      while fDecoder.CanDecode do
        begin
        fDecoder.Decode( Instruction );

        S := '';
        for i := 0 to Instruction.len-1 do
          begin
          S := S + IntToHex( Data^{$IFNDEF UNICODE}, 2{$ENDIF} );
          Inc( Data );
          end;
        StrL_Hex.Add( S );

        Inc( result, Instruction.len );
        end;
      end;
    end;
end;

function TIced.Decode( Data : PByte; Size : Cardinal; Count : Cardinal; StrL_Assembly : TStrings; CodeOffset : UInt64 = UInt64( 0 ); StrL_Hex : TStrings = nil; {$IFDEF AssemblyTools}Details : pIcedDetails = nil;{$ENDIF} DecoderSettings : Cardinal = doNONE{$IFDEF AssemblyTools}; CalcJumpLines : Boolean = True{$ENDIF} ) : Cardinal;
const
  DETAILS_BLOCKSIZE = 1000;
var
  Instruction : TInstruction;
  tOutput     : Array [ 0..255 ] of AnsiChar;
  i           : Integer;
  S           : String;
begin
  result := 0;
  if ( self = nil ) then
    Exit;
  if ( Data = nil ) OR ( Size = 0 ) then
    Exit;
  if ( Count = 0 ) then
    Exit;

//  if ( StrL_Assembly <> nil ) then
//    StrL_Assembly.Clear;

  fDecoder.SetData( Data, Size, CodeOffset, DecoderSettings );

  {$IFDEF AssemblyTools}
  if ( Details <> nil ) then
    begin
    if ( StrL_Assembly <> nil ) AND ( StrL_Hex <> nil ) then
      begin
      while fDecoder.CanDecode AND ( Count > 0 ) do
        begin
        if ( Details^.Count >= Length( Details^.Items ) ) then
          SetLength( Details^.Items, Details^.Count+DETAILS_BLOCKSIZE );

        fDecoder.Decode( Instruction, Details^ );
        Inc( Details^.Count );

        Formatter.Format( Instruction, tOutput, Length( tOutput ) );
        StrL_Assembly.Add( String( tOutput ) );

        S := '';
        for i := 0 to Instruction.len-1 do
          begin
          S := S + IntToHex( Data^{$IFNDEF UNICODE}, 2{$ENDIF} );
          Inc( Data );
          end;
        StrL_Hex.Add( S );

        Inc( result, Instruction.len );
        Dec( Count );
        end;
      end
    else if ( StrL_Assembly <> nil ) then
      begin
      while fDecoder.CanDecode AND ( Count > 0 ) do
        begin
        if ( Details^.Count >= Length( Details^.Items ) ) then
          SetLength( Details^.Items, Details^.Count+DETAILS_BLOCKSIZE );

        fDecoder.Decode( Instruction, Details^ );
        Inc( Details^.Count );

        Formatter.Format( Instruction, tOutput, Length( tOutput ) );
        StrL_Assembly.Add( String( tOutput ) );

        Inc( result, Instruction.len );
        Dec( Count );
        end;
      end
    else if ( StrL_Hex <> nil ) then
      begin
      while fDecoder.CanDecode AND ( Count > 0 ) do
        begin
        if ( Details^.Count >= Length( Details^.Items ) ) then
          SetLength( Details^.Items, Details^.Count+DETAILS_BLOCKSIZE );

        fDecoder.Decode( Instruction, Details^ );
        Inc( Details^.Count );

        S := '';
        for i := 0 to Instruction.len-1 do
          begin
          S := S + IntToHex( Data^{$IFNDEF UNICODE}, 2{$ENDIF} );
          Inc( Data );
          end;
        StrL_Hex.Add( S );

        Inc( result, Instruction.len );
        Dec( Count );
        end;
      end
    else
      begin
      while fDecoder.CanDecode AND ( Count > 0 ) do
        begin
        if ( Details^.Count >= Length( Details^.Items ) ) then
          SetLength( Details^.Items, Details^.Count+DETAILS_BLOCKSIZE );

        fDecoder.Decode( Instruction, Details^ );
        Inc( Details^.Count );

        Inc( result, Instruction.len );
        Dec( Count );
        end;
      end;

    SetLength( Details^.Items, Details^.Count );
    SetLength( Details^.JumpTargets, Details^.TargetCount );
    if CalcJumpLines then
      JumpLines( Details^ );
    end
  else
  {$ENDIF AssemblyTools}
    begin
    if ( StrL_Assembly <> nil ) AND ( StrL_Hex <> nil ) then
      begin
      while fDecoder.CanDecode AND ( Count > 0 ) do
        begin
        fDecoder.Decode( Instruction );

        Formatter.Format( Instruction, tOutput, Length( tOutput ) );
        StrL_Assembly.Add( String( tOutput ) );

        S := '';
        for i := 0 to Instruction.len-1 do
          begin
          S := S + IntToHex( Data^{$IFNDEF UNICODE}, 2{$ENDIF} );
          Inc( Data );
          end;
        StrL_Hex.Add( S );

        Inc( result, Instruction.len );
        Dec( Count );
        end;
      end
    else if ( StrL_Assembly <> nil ) then
      begin
      while fDecoder.CanDecode AND ( Count > 0 ) do
        begin
        fDecoder.Decode( Instruction );

        Formatter.Format( Instruction, tOutput, Length( tOutput ) );
        StrL_Assembly.Add( String( tOutput ) );

        Inc( result, Instruction.len );
        Dec( Count );
        end;
      end
    else if ( StrL_Hex <> nil ) then
      begin
      while fDecoder.CanDecode AND ( Count > 0 ) do
        begin
        fDecoder.Decode( Instruction );

        S := '';
        for i := 0 to Instruction.len-1 do
          begin
          S := S + IntToHex( Data^{$IFNDEF UNICODE}, 2{$ENDIF} );
          Inc( Data );
          end;
        StrL_Hex.Add( S );

        Inc( result, Instruction.len );
        Dec( Count );
        end;
      end;
    end;
end;

function TIced.Decode( Data : PByte; Size : Cardinal; var Instruction: String; CodeOffset : UInt64 = UInt64( 0 ); InstructionBytes : pString = nil; Offset : PUInt64 = nil; {$IFDEF AssemblyTools}Detail : pIcedDetail = nil;{$ENDIF} DecoderSettings : Cardinal = doNONE; {$IFDEF AssemblyTools}CalcJumpLines : Boolean = True;{$ENDIF} CombineNOP : boolean = False ) : Cardinal;
var
  StrL_Assembly : TStrings;
  StrL_Hex : TStrings;
  {$IFDEF AssemblyTools}
  Details : TIcedDetails;
  {$ENDIF AssemblyTools}
begin
  StrL_Assembly := TStringList.Create;
  StrL_Hex := TStringList.Create;
  {$IFDEF AssemblyTools}
  FillChar( Details, SizeOf( Details ), 0 );
  {$ENDIF AssemblyTools}

  if CombineNOP then
    result := DecodeCombineNOP( Data, Size, 1{Count}, StrL_Assembly, CodeOffset, StrL_Hex, {$IFDEF AssemblyTools}@Details,{$ENDIF} DecoderSettings{$IFDEF AssemblyTools}, CalcJumpLines{$ENDIF} )
  else
    result := Decode( Data, Size, 1{Count}, StrL_Assembly, CodeOffset, StrL_Hex, {$IFDEF AssemblyTools}@Details,{$ENDIF} DecoderSettings{$IFDEF AssemblyTools}, CalcJumpLines{$ENDIF} );
  if ( result > 0 ) then
    begin
    Instruction := StrL_Assembly[ 0 ];
    if ( InstructionBytes <> nil ) then
      InstructionBytes^ := StrL_Hex[ 0 ];
    {$IFDEF AssemblyTools}
    if ( Detail <> nil ) then
      Detail^ := Details.Items[ 0 ];
    {$ENDIF AssemblyTools}
    end
  else
    begin
    Instruction := '';
    if ( InstructionBytes <> nil ) then
      InstructionBytes^ := '';
    {$IFDEF AssemblyTools}
    if ( Detail <> nil ) then
      FillChar( Detail^, SizeOf( Detail ), 0 );
    {$ENDIF AssemblyTools}
    end;
  StrL_Assembly.Free;
  StrL_Hex.Free;
  {$IFDEF AssemblyTools}
  SetLength( Details.Items, 0 );
  SetLength( Details.JumpTargets, 0 );
  {$ENDIF AssemblyTools}
end;

function TIced.Decode( Data : string; StrL_Assembly : TStrings; CodeOffset : UInt64 = UInt64( 0 ); StrL_Hex : TStrings = nil; {$IFDEF AssemblyTools}Details : pIcedDetails = nil;{$ENDIF} DecoderSettings : Cardinal = doNONE; {$IFDEF AssemblyTools}CalcJumpLines : Boolean = True;{$ENDIF} CombineNOP : boolean = False ) : boolean;
var
  StrL : TStringList;
begin
  StrL := TStringList.Create;
  StrL.Text := Data;
  result := Decode( StrL, StrL_Assembly, CodeOffset, StrL_Hex, {$IFDEF AssemblyTools}Details,{$ENDIF} DecoderSettings, {$IFDEF AssemblyTools}CalcJumpLines,{$ENDIF} CombineNOP );
  StrL.Free;
end;

function TIced.Decode( Data : TStrings; StrL_Assembly : TStrings; CodeOffset : UInt64 = UInt64( 0 ); StrL_Hex : TStrings = nil; {$IFDEF AssemblyTools}Details : pIcedDetails = nil;{$ENDIF} DecoderSettings : Cardinal = doNONE; {$IFDEF AssemblyTools}CalcJumpLines : Boolean = True;{$ENDIF} CombineNOP : boolean = False ) : boolean;
  function IsHex( sHex : String ) : Boolean; {$IF CompilerVersion >= 23}inline;{$IFEND}
  var
    i : Integer;
  Begin
    Result := false;
    if ( Trim( sHex ) = '' ) Then
      Exit;

    sHex := StringReplace( sHex, ' ', '', [ rfReplaceAll ] );
    sHex := StringReplace( sHex, '-', '', [ rfReplaceAll ] );
    if ( Copy( sHex, 1, 1 ) = '$' ) then
      sHex := Copy( sHex, 2, Length( sHex )-1 )
    else if ( Copy( sHex, 1, 2 ) = '0x' ) then
      sHex := Copy( sHex, 3, Length( sHex )-2 );

    sHex := UpperCase( sHex );
    if Odd( Length( sHex ) ) then
      sHex := '0' + sHex;

    for i := {$IF CompilerVersion >= 24}Low( sHex ){$ELSE}1{$IFEND} to {$IF CompilerVersion >= 24}High( sHex ){$ELSE}Length( sHex ){$IFEND} do
      begin
  //    if NOT IsHexChar( sHex[ i ], WildCard ) then
  //      Exit;

      if NOT ( {$IF CompilerVersion >= 20}CharInSet( sHex[ i ],{$ELSE}( sHex[ i ] in{$IFEND} [ '0'..'9' ,'a'..'f', 'A'..'F' ] ) ) then
        Exit;
      end;

    Result := true;
  end;
const
  HexTable : Array[ 0..15 ] of Char = ( '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' );
  DETAILS_BLOCKSIZE = 1000;
var
  i, j, k : Integer;
  sHex    : String;
  Bts     : Array of Byte;
  StrL_In : TStringList;
  tStrL   : TStringList;
  sCom    : string;
  iPos    : Integer;
  A, B    : SmallInt;
  IsDATA  : Boolean;
begin
  result := false;
  if ( StrL_Hex <> nil ) then
    StrL_Hex.Clear;
  {$IFDEF AssemblyTools}
  if ( Details <> nil ) then
    FillChar( Details^, SizeOf( Details^ ), 0 );
  {$ENDIF AssemblyTools}
  if ( Data = nil ) then
    Exit;
  if ( Data.Text = '' ) then
    Exit;
  StrL_Assembly.BeginUpdate;
  if ( StrL_Assembly <> nil ) then
    StrL_Assembly.Clear;

  if NOT uIced.Imports.IsInitDLL then
    begin
    if ( StrL_Assembly <> nil ) then
      StrL_Assembly.Add( 'DLL not loaded.' );
    StrL_Assembly.EndUpdate;
    Exit;
    end;

  StrL_In := TStringList.Create;
  StrL_In.Assign( Data );
  tStrL   := TStringList.Create;

  // Merge/Join Lines
  IsDATA := False; // MS !!!?
  i := 0;
  while ( i < StrL_In.Count ) do
    begin
    if ( CompareText( StrL_In[ i ], '[DATA]' ) = 0 ) then
      IsDATA := True;
    if IsHex( StrL_In[ i ] ) then
      begin
      sHex := Trim( StrL_In[ i ] );
      while ( i < StrL_In.Count-1 ) AND IsHex( StrL_In[ i+1 ] ) do
        begin
        sHex := sHex + Trim( StrL_In[ i+1 ] );
        Inc( i );
        end;
      tStrL.Add( sHex );
      end
    else
      tStrL.Add( StrL_In[ i ] );
    if ( CompareText( StrL_In[ i ], '[/DATA]' ) = 0 ) then
      IsDATA := False;
    Inc( i );
    end;
  StrL_In.Assign( tStrL );
  tStrL.free;

  IsDATA := False;
  for i := 0 to StrL_In.Count-1 do
    begin
    StrL_In[ i ] := StringReplace( StrL_In[ i ], #9, #32, [rfReplaceAll] );
    StrL_In[ i ] := Trim( StrL_In[ i ] );

    if ( CompareText( StrL_In[ i ], '[DATA]' ) = 0 ) then
      begin
      IsDATA := True;
      j := 0;
      end
    else if ( CompareText( StrL_In[ i ], '[/DATA]' ) = 0 ) then
      begin
      IsDATA := False;
      j := 1;
      end
    else
      j := -1;

    if ( StrL_In[ i ] = '' ) OR ( j >= 0 ) then
      begin
      if ( StrL_Hex <> nil ) then
        begin
        if ( j >= 0 ) then
          StrL_Hex.Add( StrL_In[ i ] )
        else
          StrL_Hex.Add( '' );
        end;
      if ( StrL_Assembly <> nil ) then
        StrL_Assembly.Add( '' );
      {$IFDEF AssemblyTools}
      if ( Details <> nil ) then
        begin
        if ( Details^.Count >= Length( Details^.Items ) ) then
          SetLength( Details^.Items, Details^.Count+DETAILS_BLOCKSIZE );

        FillChar( Details^.Items[ Details^.Count ], SizeOf( TIcedDetail ), 0 );
        {$IF CompilerVersion < 23}{$RANGECHECKS OFF}{$IFEND}
        Details^.Items[ Details^.Count ].Offset := CodeOffset;
        {$IF CompilerVersion < 23}{$RANGECHECKS ON}{$IFEND}
        Inc( Details^.Count );
        end;
      {$ENDIF AssemblyTools}
      continue;
      end;

    if IsDATA then
      begin
      if ( StrL_Hex <> nil ) then
        StrL_Hex.Add( StrL_In[ i ] );
      if ( StrL_Assembly <> nil ) then
        StrL_Assembly.Add( StrL_In[ i ] );
      {$IFDEF AssemblyTools}
      if ( Details <> nil ) then
        begin
        if ( Details^.Count >= Length( Details^.Items ) ) then
          SetLength( Details^.Items, Details^.Count+DETAILS_BLOCKSIZE );

        FillChar( Details^.Items[ Details^.Count ], SizeOf( TIcedDetail ), 0 );
        {$IF CompilerVersion < 23}{$RANGECHECKS OFF}{$IFEND}
        Details^.Items[ Details^.Count ].Offset := CodeOffset;
        {$IF CompilerVersion < 23}{$RANGECHECKS ON}{$IFEND}
        Inc( Details^.Count );
        end;
      {$ENDIF AssemblyTools}
      continue;
      end;

    sCom := '';
    iPos := -1;
    sHex := StrL_In[ i ];
    for j := 0 to 3 do
      begin
      case j of
        0 : iPos := Pos( '//', StrL_In[ i ] );
        1 : iPos := Pos( '/*', StrL_In[ i ] );
        2 : iPos := Pos( '{', StrL_In[ i ] );
        3 : iPos := Pos( '(*', StrL_In[ i ] );
      end;
      if ( iPos > 0 ) then
        begin
        sHex := Trim( Copy( StrL_In[ i ], 1, iPos-1 ) );
        sCom := ' ' + Copy( StrL_In[ i ], iPos, Length( StrL_In[ i ] )-iPos+1 );
        break;
        end;
      end;

    sHex := StringReplace( sHex, ' ', '', [ rfReplaceAll ] );
    sHex := StringReplace( sHex, '-', '', [ rfReplaceAll ] );
    if ( Copy( sHex, 1, 1 ) = '$' ) then
      sHex := Copy( sHex, 2, Length( sHex )-1 )
    else if ( Copy( sHex, 1, 2 ) = '0x' ) then
      sHex := Copy( sHex, 3, Length( sHex )-2 );

    sHex := UpperCase( sHex );
    if Odd( Length( sHex ) ) then
      sHex := '0' + sHex;

//    if ( Copy( StrL[ i ], 1, 2 ) = '//' ) OR ( Copy( StrL[ i ], 1, 2 ) = '/*' ) OR ( Copy( StrL[ i ], 1, 2 ) = '{' ) OR ( Copy( StrL[ i ], 1, 2 ) = '(*' ) then
    if NOT IsHex( sHex ) then
      begin
      if ( StrL_Hex <> nil ) then
        StrL_Hex.Add( StrL_In[ i ] );
      if ( StrL_Assembly <> nil ) then
        StrL_Assembly.Add( StrL_In[ i ] );
      {$IFDEF AssemblyTools}
      if ( Details <> nil ) then
        begin
        if ( Details^.Count >= Length( Details^.Items ) ) then
          SetLength( Details^.Items, Details^.Count+DETAILS_BLOCKSIZE );

        FillChar( Details^.Items[ Details^.Count ], SizeOf( tIcedDetail ), 0 );
        {$IF CompilerVersion < 23}{$RANGECHECKS OFF}{$IFEND}
        Details^.Items[ Details^.Count ].Offset := CodeOffset;
        {$IF CompilerVersion < 23}{$RANGECHECKS ON}{$IFEND}

        Inc( Details^.Count );
        end;
      {$ENDIF AssemblyTools}
      continue;
      end;

    SetLength( Bts, Length( sHex ) div 2 );
    j := {$IF CompilerVersion >= 24}Low( sHex ){$ELSE}1{$IFEND};
    while ( j < {$IF CompilerVersion >= 24}High( sHex ){$ELSE}Length( sHex ){$IFEND} ) do
      begin
//      HexToBytes( AnsiString( sHex[ j ] + sHex[ j+1 ] ), @Bts[ ( j-1 ) div 2 ], 1 );

      A := -1;
      B := -1;
      for k := Low( HexTable ) to High( HexTable ) do
        begin
        if ( HexTable[ k ] = sHex[ j ] ) then
          A := 16 * k;
        if ( HexTable[ k ] = sHex[ j+1 ] ) then
          B := k;
        end;
      if ( A < 0 ) or ( B < 0 ) then
        Exit;
      Bts[ ( j-1 ) div 2 ] := A + B;
      Inc( j, 2 );
      end;

    if CombineNOP then
      {k := }DecodeCombineNOP( @Bts[ 0 ], Length( Bts ), StrL_Assembly, CodeOffset, StrL_Hex, {$IFDEF AssemblyTools}Details,{$ENDIF} DecoderSettings{$IFDEF AssemblyTools}, CalcJumpLines AND ( i = StrL_In.Count-1 ){$ENDIF} )
    else
      {k := }Decode( @Bts[ 0 ], Length( Bts ), StrL_Assembly, CodeOffset, StrL_Hex, {$IFDEF AssemblyTools}Details,{$ENDIF} DecoderSettings{$IFDEF AssemblyTools}, CalcJumpLines AND ( i = StrL_In.Count-1 ){$ENDIF} );

    if ( sCom <> '' ) then
      begin
      StrL_Assembly[ StrL_Assembly.Count-1 ] := StrL_Assembly[ StrL_Assembly.Count-1 ] + sCom;
      if ( StrL_Hex <> nil ) then
        StrL_Hex[ StrL_Hex.Count-1 ] := StrL_Hex[ StrL_Hex.Count-1 ] + sCom;
      end;

    {$IF CompilerVersion < 23}{$RANGECHECKS OFF}{$IFEND}
    Inc( CodeOffset, Length( Bts ) );
    {$IF CompilerVersion < 23}{$RANGECHECKS ON}{$IFEND}
    SetLength( Bts, 0 );
    end;

  {$IFDEF AssemblyTools}
  if ( Details <> nil ) then
    begin
    SetLength( Details^.Items, Details^.Count );
    SetLength( Details^.JumpTargets, Details^.TargetCount );
    end;
  {$ENDIF AssemblyTools}

  StrL_In.free;
  StrL_Assembly.EndUpdate;
//  if ( Details <> nil ) AND ( Architecture = CS_ARCH_X86 ) then
//    JumpLines( Details^.Items );
  result := True;
end;

function TIced.DecodeCombineNOP( Data : PByte; Size : Cardinal; StrL_Assembly : TStrings; CodeOffset : UInt64 = UInt64( 0 ); StrL_Hex : TStrings = nil; {$IFDEF AssemblyTools}Details : pIcedDetails = nil;{$ENDIF} DecoderSettings : Cardinal = doNONE{$IFDEF AssemblyTools}; CalcJumpLines : Boolean = True{$ENDIF} ) : Cardinal;
const
  DETAILS_BLOCKSIZE = 1000;
  BLOCKSIZE_NOP     = MAX_INSTRUCTION_LEN_;
var
  Instruction : TInstruction;
  tOutput     : Array [ 0..255 ] of AnsiChar;
  i           : Integer;
  S           : String;
  bDecode     : Boolean;
  bNOP        : Byte;
begin
  result := 0;
  if ( self = nil ) then
    Exit;
  if ( Data = nil ) OR ( Size = 0 ) then
    Exit;

//  if ( StrL_Assembly <> nil ) then
//    StrL_Assembly.Clear;

  fDecoder.SetData( Data, Size, CodeOffset, DecoderSettings );

  bNOP := 0;
  {$IFDEF AssemblyTools}
  if ( Details <> nil ) then
    begin
    if ( StrL_Assembly <> nil ) AND ( StrL_Hex <> nil ) then
      begin
      bDecode := fDecoder.CanDecode;
      while bDecode do
        begin
        if ( Details^.Count >= Length( Details^.Items ) ) then
          SetLength( Details^.Items, Details^.Count+DETAILS_BLOCKSIZE );

        fDecoder.Decode( Instruction, Details^ );
        bDecode := fDecoder.CanDecode;

        if ( Instruction.code.Code = Nopd ) AND ( bNOP < BLOCKSIZE_NOP ) then
          begin
          if ( bNOP = 0 ) then
            begin
            Formatter.Format( Instruction, tOutput, Length( tOutput ) );
            StrL_Assembly.Add( String( tOutput ) );
            Inc( Details^.Count );
            end;

          Inc( bNOP );
          if bDecode AND ( bNOP <= BLOCKSIZE_NOP ) then
            Continue;
          end;

        if ( bNOP > 1 ) then
          begin
          Details^.Items[ Details^.Count-1 ].Size := bNOP;
          StrL_Assembly[ StrL_Assembly.Count-1 ] := StrL_Assembly[ StrL_Assembly.Count-1 ] + ':' + IntToStr( bNOP );

          S := '';
          for i := 0 to bNOP-1 do
            begin
            S := S + IntToHex( Data^{$IFNDEF UNICODE}, 2{$ENDIF} );
            Inc( Data );
            end;
          StrL_Hex.Add( S );
          end
        else if ( bNOP = 1 ) then
          begin
          StrL_Hex.Add( IntToHex( Data^{$IFNDEF UNICODE}, 2{$ENDIF} ) );
          Inc( Data );
          end;
        if ( bNOP > 0 ) then
          begin
          Inc( result, bNOP );
          if NOT bDecode then
            break;
          end;
        bNOP := 0;

        Formatter.Format( Instruction, tOutput, Length( tOutput ) );
        StrL_Assembly.Add( String( tOutput ) );

        Inc( Details^.Count );

        S := '';
        for i := 0 to Instruction.len-1 do
          begin
          S := S + IntToHex( Data^{$IFNDEF UNICODE}, 2{$ENDIF} );
          Inc( Data );
          end;
        StrL_Hex.Add( S );

        Inc( result, Instruction.len );
        end;
      end
    else if ( StrL_Assembly <> nil ) then
      begin
      bDecode := fDecoder.CanDecode;
      while bDecode do
        begin
        if ( Details^.Count >= Length( Details^.Items ) ) then
          SetLength( Details^.Items, Details^.Count+DETAILS_BLOCKSIZE );

        fDecoder.Decode( Instruction, Details^ );
        bDecode := fDecoder.CanDecode;

        if ( Instruction.code.Code = Nopd ) AND ( bNOP < BLOCKSIZE_NOP ) then
          begin
          if ( bNOP = 0 ) then
            begin
            Formatter.Format( Instruction, tOutput, Length( tOutput ) );
            StrL_Assembly.Add( String( tOutput ) );
            Inc( Details^.Count );
            end;
          Inc( bNOP );
          if bDecode AND ( bNOP <= BLOCKSIZE_NOP ) then
            Continue;
          end;

        if ( bNOP > 1 ) then
          begin
          Details^.Items[ Details^.Count-1 ].Size := bNOP;
          StrL_Assembly[ StrL_Assembly.Count-1 ] := StrL_Assembly[ StrL_Assembly.Count-1 ] + ':' + IntToStr( bNOP );

//          Inc( Data, bNOP-1 );
          end;
//        else if ( bNOP = 1 ) then
//          Inc( Data );
        if ( bNOP > 0 ) then
          begin
          Inc( result, bNOP );
          if NOT bDecode then
            break;
          end;
        bNOP := 0;

        Formatter.Format( Instruction, tOutput, Length( tOutput ) );
        StrL_Assembly.Add( String( tOutput ) );

        Inc( Details^.Count );
        Inc( result, Instruction.len );
        end;
      end
    else if ( StrL_Hex <> nil ) then
      begin
      bDecode := fDecoder.CanDecode;
      while bDecode do
        begin
        if ( Details^.Count >= Length( Details^.Items ) ) then
          SetLength( Details^.Items, Details^.Count+DETAILS_BLOCKSIZE );

        fDecoder.Decode( Instruction, Details^ );
        bDecode := fDecoder.CanDecode;

        if ( Instruction.code.Code = Nopd ) AND ( bNOP < BLOCKSIZE_NOP ) then
          begin
          if ( bNOP = 0 ) then
            Inc( Details^.Count );
          Inc( bNOP );
          if bDecode AND ( bNOP <= BLOCKSIZE_NOP ) then
            Continue;
          end;

        if ( bNOP > 1 ) then
          begin
          Details^.Items[ Details^.Count-1 ].Size := bNOP;
          S := '';
          for i := 0 to bNOP-1 do
            begin
            S := S + IntToHex( Data^{$IFNDEF UNICODE}, 2{$ENDIF} );
            Inc( Data );
            end;
          StrL_Hex.Add( S );
          end
        else if ( bNOP = 1 ) then
          begin
          StrL_Hex.Add( IntToHex( Data^{$IFNDEF UNICODE}, 2{$ENDIF} ) );
          Inc( Data );
          end;
        if ( bNOP > 0 ) then
          begin
          Inc( result, bNOP );
          if NOT bDecode then
            break;
          end;
        bNOP := 0;

        Inc( Details^.Count );

        S := '';
        for i := 0 to Instruction.len-1 do
          begin
          S := S + IntToHex( Data^{$IFNDEF UNICODE}, 2{$ENDIF} );
          Inc( Data );
          end;
        StrL_Hex.Add( S );

        Inc( result, Instruction.len );
        end;
      end
    else
      begin
      bDecode := fDecoder.CanDecode;
      while bDecode do
        begin
        if ( Details^.Count >= Length( Details^.Items ) ) then
          SetLength( Details^.Items, Details^.Count+DETAILS_BLOCKSIZE );

        fDecoder.Decode( Instruction, Details^ );
        bDecode := fDecoder.CanDecode;

        if ( Instruction.code.Code = Nopd ) AND ( bNOP < BLOCKSIZE_NOP ) then
          begin
          if ( bNOP = 0 ) then
            Inc( Details^.Count );
          Inc( bNOP );
          if bDecode AND ( bNOP <= BLOCKSIZE_NOP ) then
            Continue;
          end;

        if ( bNOP > 1 ) then
          begin
          Details^.Items[ Details^.Count-1 ].Size := bNOP;
//          Inc( Data, bNOP-1 );
          end;
//        else if ( bNOP = 1 ) then
//          Inc( Data );
        if ( bNOP > 0 ) then
          begin
          Inc( result, bNOP );
          if NOT bDecode then
            break;
          end;
        bNOP := 0;

        Inc( Details^.Count );

        Inc( result, Instruction.len );
        end;
      end;

    SetLength( Details^.Items, Details^.Count );
    SetLength( Details^.JumpTargets, Details^.TargetCount );
    if CalcJumpLines then
      JumpLines( Details^ );
    end
  else
  {$ENDIF AssemblyTools}
    begin
    if ( StrL_Assembly <> nil ) AND ( StrL_Hex <> nil ) then
      begin
      bDecode := fDecoder.CanDecode;
      while bDecode do
        begin
        fDecoder.Decode( Instruction );
        bDecode := fDecoder.CanDecode;

        if ( Instruction.code.Code = Nopd ) AND ( bNOP < BLOCKSIZE_NOP ) then
          begin
          Inc( bNOP );

          if ( bNOP = 0 ) then
            begin
            Formatter.Format( Instruction, tOutput, Length( tOutput ) );
            StrL_Assembly.Add( String( tOutput ) );
            end;

          if bDecode AND ( bNOP <= BLOCKSIZE_NOP ) then
            Continue;
          end;

        if ( bNOP > 1 ) then
          begin
          StrL_Assembly[ StrL_Assembly.Count-1 ] := StrL_Assembly[ StrL_Assembly.Count-1 ] + ':' + IntToStr( bNOP );

          S := '';
          for i := 0 to bNOP-1 do
            begin
            S := S + IntToHex( Data^{$IFNDEF UNICODE}, 2{$ENDIF} );
            Inc( Data );
            end;
          StrL_Hex.Add( S );
          end
        else if ( bNOP = 1 ) then
          begin
          StrL_Hex.Add( IntToHex( Data^{$IFNDEF UNICODE}, 2{$ENDIF} ) );
          Inc( Data );
          end;
        if ( bNOP > 0 ) then
          begin
          Inc( result, bNOP );
          if NOT bDecode then
            break;
          end;
        bNOP := 0;

        Formatter.Format( Instruction, tOutput, Length( tOutput ) );
        StrL_Assembly.Add( String( tOutput ) );

        S := '';
        for i := 0 to Instruction.len-1 do
          begin
          S := S + IntToHex( Data^{$IFNDEF UNICODE}, 2{$ENDIF} );
          Inc( Data );
          end;
        StrL_Hex.Add( S );

        Inc( result, Instruction.len );
        end;
      end
    else if ( StrL_Assembly <> nil ) then
      begin
      bDecode := fDecoder.CanDecode;
      while bDecode do
        begin
        fDecoder.Decode( Instruction );
        bDecode := fDecoder.CanDecode;

        if ( Instruction.code.Code = Nopd ) AND ( bNOP < BLOCKSIZE_NOP ) then
          begin
          Inc( bNOP );

          if ( bNOP = 0 ) then
            begin
            Formatter.Format( Instruction, tOutput, Length( tOutput ) );
            StrL_Assembly.Add( String( tOutput ) );
            end;

          if bDecode AND ( bNOP <= BLOCKSIZE_NOP ) then
            Continue;
          end;

        if ( bNOP > 1 ) then
          begin
          StrL_Assembly[ StrL_Assembly.Count-1 ] := StrL_Assembly[ StrL_Assembly.Count-1 ] + ':' + IntToStr( bNOP );

//          Inc( Data, bNOP-1 );
          end;
//        else if ( bNOP = 1 ) then
//          Inc( Data );
        if ( bNOP > 0 ) then
          begin
          Inc( result, bNOP );
          if NOT bDecode then
            break;
          end;
        bNOP := 0;

        Formatter.Format( Instruction, tOutput, Length( tOutput ) );
        StrL_Assembly.Add( String( tOutput ) );

        Inc( result, Instruction.len );
        end;
      end
    else if ( StrL_Hex <> nil ) then
      begin
      bDecode := fDecoder.CanDecode;
      while bDecode do
        begin
        fDecoder.Decode( Instruction );
        bDecode := fDecoder.CanDecode;

        if ( Instruction.code.Code = Nopd ) AND ( bNOP < BLOCKSIZE_NOP ) then
          begin
          Inc( bNOP );
          if bDecode AND ( bNOP <= BLOCKSIZE_NOP ) then
            Continue;
          end;

        if ( bNOP > 1 ) then
          begin
          S := '';
          for i := 0 to bNOP-1 do
            begin
            S := S + IntToHex( Data^{$IFNDEF UNICODE}, 2{$ENDIF} );
            Inc( Data );
            end;
          StrL_Hex.Add( S );
          end
        else if ( bNOP = 1 ) then
          begin
          StrL_Hex.Add( IntToHex( Data^{$IFNDEF UNICODE}, 2{$ENDIF} ) );
          Inc( Data );
          end;
        if ( bNOP > 0 ) then
          begin
          Inc( result, bNOP );
          if NOT bDecode then
            break;
          end;
        bNOP := 0;

        S := '';
        for i := 0 to Instruction.len-1 do
          begin
          S := S + IntToHex( Data^{$IFNDEF UNICODE}, 2{$ENDIF} );
          Inc( Data );
          end;
        StrL_Hex.Add( S );

        Inc( result, Instruction.len );
        end;
      end;
    end;
end;

function TIced.DecodeCombineNOP( Data : PByte; Size : Cardinal; Count : Cardinal; StrL_Assembly : TStrings; CodeOffset : UInt64 = UInt64( 0 ); StrL_Hex : TStrings = nil; {$IFDEF AssemblyTools}Details : pIcedDetails = nil;{$ENDIF} DecoderSettings : Cardinal = doNONE{$IFDEF AssemblyTools}; CalcJumpLines : Boolean = True{$ENDIF} ) : Cardinal;
const
  DETAILS_BLOCKSIZE = 1000;
  BLOCKSIZE_NOP     = 16;
var
  Instruction : TInstruction;
  tOutput     : Array [ 0..255 ] of AnsiChar;
  i           : Integer;
  S           : String;
  bDecode     : Boolean;
  bNOP        : Byte;
begin
  result := 0;
  if ( self = nil ) then
    Exit;
  if ( Data = nil ) OR ( Size = 0 ) then
    Exit;
  if ( Count = 0 ) then
    Exit;

//  if ( StrL_Assembly <> nil ) then
//    StrL_Assembly.Clear;

  fDecoder.SetData( Data, Size, CodeOffset, DecoderSettings );

  bNOP := 0;
  {$IFDEF AssemblyTools}
  if ( Details <> nil ) then
    begin
    if ( StrL_Assembly <> nil ) AND ( StrL_Hex <> nil ) then
      begin
      bDecode := fDecoder.CanDecode;
      while bDecode do
        begin
        if ( Details^.Count >= Length( Details^.Items ) ) then
          SetLength( Details^.Items, Details^.Count+DETAILS_BLOCKSIZE );

        fDecoder.Decode( Instruction, Details^ );
        Dec( Count );
        bDecode := fDecoder.CanDecode AND ( Count > 0 );

        if ( Instruction.code.Code = Nopd ) AND ( bNOP < BLOCKSIZE_NOP ) then
          begin
          if ( bNOP = 0 ) then
            begin
            Formatter.Format( Instruction, tOutput, Length( tOutput ) );
            StrL_Assembly.Add( String( tOutput ) );
            Inc( Details^.Count );
            end;

          Inc( bNOP );
          if bDecode AND ( bNOP <= BLOCKSIZE_NOP ) then
            Continue;
          end;

        if ( bNOP > 1 ) then
          begin
          Details^.Items[ Details^.Count-1 ].Size := bNOP;
          StrL_Assembly[ StrL_Assembly.Count-1 ] := StrL_Assembly[ StrL_Assembly.Count-1 ] + ':' + IntToStr( bNOP );

          S := '';
          for i := 0 to bNOP-1 do
            begin
            S := S + IntToHex( Data^{$IFNDEF UNICODE}, 2{$ENDIF} );
            Inc( Data );
            end;
          StrL_Hex.Add( S );
          end
        else if ( bNOP = 1 ) then
          begin
          StrL_Hex.Add( IntToHex( Data^{$IFNDEF UNICODE}, 2{$ENDIF} ) );
          Inc( Data );
          end;
        if ( bNOP > 0 ) then
          begin
          Inc( result, bNOP );
          if NOT bDecode then
            break;
          end;
        bNOP := 0;

        Formatter.Format( Instruction, tOutput, Length( tOutput ) );
        StrL_Assembly.Add( String( tOutput ) );

        Inc( Details^.Count );

        S := '';
        for i := 0 to Instruction.len-1 do
          begin
          S := S + IntToHex( Data^{$IFNDEF UNICODE}, 2{$ENDIF} );
          Inc( Data );
          end;
        StrL_Hex.Add( S );

        Inc( result, Instruction.len );
        end;
      end
    else if ( StrL_Assembly <> nil ) then
      begin
      bDecode := fDecoder.CanDecode;
      while bDecode do
        begin
        if ( Details^.Count >= Length( Details^.Items ) ) then
          SetLength( Details^.Items, Details^.Count+DETAILS_BLOCKSIZE );

        fDecoder.Decode( Instruction, Details^ );
        Dec( Count );
        bDecode := fDecoder.CanDecode AND ( Count > 0 );

        if ( Instruction.code.Code = Nopd ) AND ( bNOP < BLOCKSIZE_NOP ) then
          begin
          if ( bNOP = 0 ) then
            begin
            Formatter.Format( Instruction, tOutput, Length( tOutput ) );
            StrL_Assembly.Add( String( tOutput ) );
            Inc( Details^.Count );
            end;
          Inc( bNOP );
          if bDecode AND ( bNOP <= BLOCKSIZE_NOP ) then
            Continue;
          end;

        if ( bNOP > 1 ) then
          begin
          Details^.Items[ Details^.Count-1 ].Size := bNOP;
          StrL_Assembly[ StrL_Assembly.Count-1 ] := StrL_Assembly[ StrL_Assembly.Count-1 ] + ':' + IntToStr( bNOP );

//          Inc( Data, bNOP-1 );
          end;
//        else if ( bNOP = 1 ) then
//          Inc( Data );
        if ( bNOP > 0 ) then
          begin
          Inc( result, bNOP );
          if NOT bDecode then
            break;
          end;
        bNOP := 0;

        Formatter.Format( Instruction, tOutput, Length( tOutput ) );
        StrL_Assembly.Add( String( tOutput ) );

        Inc( Details^.Count );
        Inc( result, Instruction.len );
        end;
      end
    else if ( StrL_Hex <> nil ) then
      begin
      bDecode := fDecoder.CanDecode;
      while bDecode do
        begin
        if ( Details^.Count >= Length( Details^.Items ) ) then
          SetLength( Details^.Items, Details^.Count+DETAILS_BLOCKSIZE );

        fDecoder.Decode( Instruction, Details^ );
        Dec( Count );
        bDecode := fDecoder.CanDecode AND ( Count > 0 );

        if ( Instruction.code.Code = Nopd ) AND ( bNOP < BLOCKSIZE_NOP ) then
          begin
          if ( bNOP = 0 ) then
            Inc( Details^.Count );
          Inc( bNOP );
          if bDecode AND ( bNOP <= BLOCKSIZE_NOP ) then
            Continue;
          end;

        if ( bNOP > 1 ) then
          begin
          Details^.Items[ Details^.Count-1 ].Size := bNOP;
          S := '';
          for i := 0 to bNOP-1 do
            begin
            S := S + IntToHex( Data^{$IFNDEF UNICODE}, 2{$ENDIF} );
            Inc( Data );
            end;
          StrL_Hex.Add( S );
          end
        else if ( bNOP = 1 ) then
          begin
          StrL_Hex.Add( IntToHex( Data^{$IFNDEF UNICODE}, 2{$ENDIF} ) );
          Inc( Data );
          end;
        if ( bNOP > 0 ) then
          begin
          Inc( result, bNOP );
          if NOT bDecode then
            break;
          end;
        bNOP := 0;

        Inc( Details^.Count );

        S := '';
        for i := 0 to Instruction.len-1 do
          begin
          S := S + IntToHex( Data^{$IFNDEF UNICODE}, 2{$ENDIF} );
          Inc( Data );
          end;
        StrL_Hex.Add( S );

        Inc( result, Instruction.len );
        end;
      end
    else
      begin
      bDecode := fDecoder.CanDecode;
      while bDecode do
        begin
        if ( Details^.Count >= Length( Details^.Items ) ) then
          SetLength( Details^.Items, Details^.Count+DETAILS_BLOCKSIZE );

        fDecoder.Decode( Instruction, Details^ );
        Dec( Count );
        bDecode := fDecoder.CanDecode AND ( Count > 0 );

        if ( Instruction.code.Code = Nopd ) AND ( bNOP < BLOCKSIZE_NOP ) then
          begin
          if ( bNOP = 0 ) then
            Inc( Details^.Count );
          Inc( bNOP );
          if bDecode AND ( bNOP <= BLOCKSIZE_NOP ) then
            Continue;
          end;

        if ( bNOP > 1 ) then
          begin
          Details^.Items[ Details^.Count-1 ].Size := bNOP;
//          Inc( Data, bNOP-1 );
          end;
//        else if ( bNOP = 1 ) then
//          Inc( Data );
        if ( bNOP > 0 ) then
          begin
          Inc( result, bNOP );
          if NOT bDecode then
            break;
          end;
        bNOP := 0;

        Inc( Details^.Count );

        Inc( result, Instruction.len );
        end;
      end;

    SetLength( Details^.Items, Details^.Count );
    SetLength( Details^.JumpTargets, Details^.TargetCount );
    if CalcJumpLines then
      JumpLines( Details^ );
    end
  else
  {$ENDIF AssemblyTools}
    begin
    if ( StrL_Assembly <> nil ) AND ( StrL_Hex <> nil ) then
      begin
      bDecode := fDecoder.CanDecode;
      while bDecode do
        begin
        fDecoder.Decode( Instruction );
        Dec( Count );
        bDecode := fDecoder.CanDecode AND ( Count > 0 );

        if ( Instruction.code.Code = Nopd ) AND ( bNOP < BLOCKSIZE_NOP ) then
          begin
          Inc( bNOP );

          if ( bNOP = 0 ) then
            begin
            Formatter.Format( Instruction, tOutput, Length( tOutput ) );
            StrL_Assembly.Add( String( tOutput ) );
            end;

          if bDecode AND ( bNOP <= BLOCKSIZE_NOP ) then
            Continue;
          end;

        if ( bNOP > 1 ) then
          begin
          StrL_Assembly[ StrL_Assembly.Count-1 ] := StrL_Assembly[ StrL_Assembly.Count-1 ] + ':' + IntToStr( bNOP );

          S := '';
          for i := 0 to bNOP-1 do
            begin
            S := S + IntToHex( Data^{$IFNDEF UNICODE}, 2{$ENDIF} );
            Inc( Data );
            end;
          StrL_Hex.Add( S );
          end
        else if ( bNOP = 1 ) then
          begin
          StrL_Hex.Add( IntToHex( Data^{$IFNDEF UNICODE}, 2{$ENDIF} ) );
          Inc( Data );
          end;
        if ( bNOP > 0 ) then
          begin
          Inc( result, bNOP );
          if NOT bDecode then
            break;
          end;
        bNOP := 0;

        Formatter.Format( Instruction, tOutput, Length( tOutput ) );
        StrL_Assembly.Add( String( tOutput ) );

        S := '';
        for i := 0 to Instruction.len-1 do
          begin
          S := S + IntToHex( Data^{$IFNDEF UNICODE}, 2{$ENDIF} );
          Inc( Data );
          end;
        StrL_Hex.Add( S );

        Inc( result, Instruction.len );
        end;
      end
    else if ( StrL_Assembly <> nil ) then
      begin
      bDecode := fDecoder.CanDecode;
      while bDecode do
        begin
        fDecoder.Decode( Instruction );
        Dec( Count );
        bDecode := fDecoder.CanDecode AND ( Count > 0 );

        if ( Instruction.code.Code = Nopd ) AND ( bNOP < BLOCKSIZE_NOP ) then
          begin
          Inc( bNOP );

          if ( bNOP = 0 ) then
            begin
            Formatter.Format( Instruction, tOutput, Length( tOutput ) );
            StrL_Assembly.Add( String( tOutput ) );
            end;

          if bDecode AND ( bNOP <= BLOCKSIZE_NOP ) then
            Continue;
          end;

        if ( bNOP > 1 ) then
          begin
          StrL_Assembly[ StrL_Assembly.Count-1 ] := StrL_Assembly[ StrL_Assembly.Count-1 ] + ':' + IntToStr( bNOP );

//          Inc( Data, bNOP-1 );
          end;
//        else if ( bNOP = 1 ) then
//          Inc( Data );
        if ( bNOP > 0 ) then
          begin
          Inc( result, bNOP );
          if NOT bDecode then
            break;
          end;
        bNOP := 0;

        Formatter.Format( Instruction, tOutput, Length( tOutput ) );
        StrL_Assembly.Add( String( tOutput ) );

        Inc( result, Instruction.len );
        end;
      end
    else if ( StrL_Hex <> nil ) then
      begin
      bDecode := fDecoder.CanDecode;
      while bDecode do
        begin
        fDecoder.Decode( Instruction );
        Dec( Count );
        bDecode := fDecoder.CanDecode AND ( Count > 0 );

        if ( Instruction.code.Code = Nopd ) AND ( bNOP < BLOCKSIZE_NOP ) then
          begin
          Inc( bNOP );
          if bDecode AND ( bNOP <= BLOCKSIZE_NOP ) then
            Continue;
          end;

        if ( bNOP > 1 ) then
          begin
          S := '';
          for i := 0 to bNOP-1 do
            begin
            S := S + IntToHex( Data^{$IFNDEF UNICODE}, 2{$ENDIF} );
            Inc( Data );
            end;
          StrL_Hex.Add( S );
          end
        else if ( bNOP = 1 ) then
          begin
          StrL_Hex.Add( IntToHex( Data^{$IFNDEF UNICODE}, 2{$ENDIF} ) );
          Inc( Data );
          end;
        if ( bNOP > 0 ) then
          begin
          Inc( result, bNOP );
          if NOT bDecode then
            break;
          end;
        bNOP := 0;

        S := '';
        for i := 0 to Instruction.len-1 do
          begin
          S := S + IntToHex( Data^{$IFNDEF UNICODE}, 2{$ENDIF} );
          Inc( Data );
          end;
        StrL_Hex.Add( S );

        Inc( result, Instruction.len );
        end;
      end;
    end;
end;

{$IF Declared( SynUnicode )}
function TIced.DecodeFromFile( FileName : String; Size : Cardinal; Offset : UInt64; StrL_Assembly : TUnicodeStrings; CodeOffset : UInt64 = UInt64( 0 ); StrL_Hex : TUnicodeStrings = nil; {$IFDEF AssemblyTools}Details : pIcedDetails = nil;{$ENDIF} DecoderSettings : Cardinal = doNONE; {$IFDEF AssemblyTools}CalcJumpLines : Boolean = True;{$ENDIF} CombineNOP : boolean = False ) : Cardinal;
var
  Assembly,
  Hex      : TStringList;
begin
  if ( StrL_Assembly <> nil ) then
    Assembly := TStringList.Create
  else
    Assembly := nil;

  if ( StrL_Hex <> nil ) then
    Hex := TStringList.Create
  else
    Hex := nil;

  result := DecodeFromFile( FileName, Size, Offset, Assembly, CodeOffset, Hex, {$IFDEF AssemblyTools}Details,{$ENDIF} DecoderSettings, {$IFDEF AssemblyTools}CalcJumpLines, {$ENDIF}CombineNOP );

  if ( Assembly <> nil ) then
    begin
    StrL_Assembly.Text := Assembly.Text;
    Assembly.free;
    end;
  if ( Hex <> nil ) then
    begin
    StrL_Hex.Text := Hex.Text;
    Hex.free;
    end;
end;

function TIced.DecodeFromStream( Data : TMemoryStream; Size : Cardinal; StrL_Assembly : TUnicodeStrings; CodeOffset : UInt64 = UInt64( 0 ); StrL_Hex : TUnicodeStrings = nil; {$IFDEF AssemblyTools}Details : pIcedDetails = nil;{$ENDIF} DecoderSettings : Cardinal = doNONE; {$IFDEF AssemblyTools}CalcJumpLines : Boolean = True;{$ENDIF} CombineNOP : boolean = False ) : Cardinal;
var
  Assembly,
  Hex      : TStringList;
begin
  if ( StrL_Assembly <> nil ) then
    Assembly := TStringList.Create
  else
    Assembly := nil;

  if ( StrL_Hex <> nil ) then
    Hex := TStringList.Create
  else
    Hex := nil;

  result := DecodeFromStream( Data, Size, Assembly, CodeOffset, Hex, {$IFDEF AssemblyTools}Details,{$ENDIF} DecoderSettings, {$IFDEF AssemblyTools}CalcJumpLines, {$ENDIF}CombineNOP );

  if ( Assembly <> nil ) then
    begin
    StrL_Assembly.Text := Assembly.Text;
    Assembly.free;
    end;
  if ( Hex <> nil ) then
    begin
    StrL_Hex.Text := Hex.Text;
    Hex.free;
    end;
end;

function TIced.Decode( Data : PByte; Size : Cardinal; StrL_Assembly : TUnicodeStrings; CodeOffset : UInt64 = UInt64( 0 ); StrL_Hex : TUnicodeStrings = nil; {$IFDEF AssemblyTools}Details : pIcedDetails = nil;{$ENDIF} DecoderSettings : Cardinal = doNONE{$IFDEF AssemblyTools}; CalcJumpLines : Boolean = True{$ENDIF} ) : Cardinal;
var
  Assembly,
  Hex      : TStringList;
begin
  if ( StrL_Assembly <> nil ) then
    Assembly := TStringList.Create
  else
    Assembly := nil;

  if ( StrL_Hex <> nil ) then
    Hex := TStringList.Create
  else
    Hex := nil;

  result := Decode( Data, Size, Assembly, CodeOffset, Hex, {$IFDEF AssemblyTools}Details,{$ENDIF} DecoderSettings{$IFDEF AssemblyTools}, CalcJumpLines{$ENDIF} );

  if ( Assembly <> nil ) then
    begin
    StrL_Assembly.Text := Assembly.Text;
    Assembly.free;
    end;
  if ( Hex <> nil ) then
    begin
    StrL_Hex.Text := Hex.Text;
    Hex.free;
    end;
end;

function TIced.Decode( Data : PByte; Size : Cardinal; Count : Cardinal; StrL_Assembly : TUnicodeStrings; CodeOffset : UInt64 = UInt64( 0 ); StrL_Hex : TUnicodeStrings = nil; {$IFDEF AssemblyTools}Details : pIcedDetails = nil;{$ENDIF} DecoderSettings : Cardinal = doNONE{$IFDEF AssemblyTools}; CalcJumpLines : Boolean = True{$ENDIF} ) : Cardinal;
var
  Assembly,
  Hex      : TStringList;
begin
  if ( StrL_Assembly <> nil ) then
    Assembly := TStringList.Create
  else
    Assembly := nil;

  if ( StrL_Hex <> nil ) then
    Hex := TStringList.Create
  else
    Hex := nil;

  result := Decode( Data, Size, Assembly, CodeOffset, Hex, {$IFDEF AssemblyTools}Details,{$ENDIF} DecoderSettings{$IFDEF AssemblyTools}, CalcJumpLines{$ENDIF} );

  if ( Assembly <> nil ) then
    begin
    StrL_Assembly.Text := Assembly.Text;
    Assembly.free;
    end;
  if ( Hex <> nil ) then
    begin
    StrL_Hex.Text := Hex.Text;
    Hex.free;
    end;
end;

function TIced.Decode( Data : string; StrL_Assembly : TUnicodeStrings; CodeOffset : UInt64 = UInt64( 0 ); StrL_Hex : TUnicodeStrings = nil; {$IFDEF AssemblyTools}Details : pIcedDetails = nil;{$ENDIF} DecoderSettings : Cardinal = doNONE; {$IFDEF AssemblyTools}CalcJumpLines : Boolean = True;{$ENDIF} CombineNOP : boolean = False ) : boolean;
var
  Assembly,
  Hex      : TStringList;
begin
  if ( StrL_Assembly <> nil ) then
    Assembly := TStringList.Create
  else
    Assembly := nil;

  if ( StrL_Hex <> nil ) then
    Hex := TStringList.Create
  else
    Hex := nil;

  result := Decode( Data, Assembly, CodeOffset, Hex, {$IFDEF AssemblyTools}Details,{$ENDIF} DecoderSettings, {$IFDEF AssemblyTools}CalcJumpLines, {$ENDIF}CombineNOP );

  if ( Assembly <> nil ) then
    begin
    StrL_Assembly.Text := Assembly.Text;
    Assembly.free;
    end;
  if ( Hex <> nil ) then
    begin
    StrL_Hex.Text := Hex.Text;
    Hex.free;
    end;
end;

function TIced.Decode( Data : TStrings; StrL_Assembly : TUnicodeStrings; CodeOffset : UInt64 = UInt64( 0 ); StrL_Hex : TUnicodeStrings = nil; {$IFDEF AssemblyTools}Details : pIcedDetails = nil;{$ENDIF} DecoderSettings : Cardinal = doNONE; {$IFDEF AssemblyTools}CalcJumpLines : Boolean = True;{$ENDIF} CombineNOP : boolean = False ) : boolean;
var
  Assembly,
  Hex      : TStringList;
begin
  if ( StrL_Assembly <> nil ) then
    Assembly := TStringList.Create
  else
    Assembly := nil;

  if ( StrL_Hex <> nil ) then
    Hex := TStringList.Create
  else
    Hex := nil;

  result := Decode( Data, Assembly, CodeOffset, Hex, {$IFDEF AssemblyTools}Details,{$ENDIF} DecoderSettings, {$IFDEF AssemblyTools}CalcJumpLines, {$ENDIF}CombineNOP );

  if ( Assembly <> nil ) then
    begin
    StrL_Assembly.Text := Assembly.Text;
    Assembly.free;
    end;
  if ( Hex <> nil ) then
    begin
    StrL_Hex.Text := Hex.Text;
    Hex.free;
    end;
end;

function TIced.DecodeCombineNOP( Data : PByte; Size : Cardinal; StrL_Assembly : TUnicodeStrings; CodeOffset : UInt64 = UInt64( 0 ); StrL_Hex : TUnicodeStrings = nil; {$IFDEF AssemblyTools}Details : pIcedDetails = nil;{$ENDIF} DecoderSettings : Cardinal = doNONE{$IFDEF AssemblyTools}; CalcJumpLines : Boolean = True{$ENDIF} ) : Cardinal;
var
  Assembly,
  Hex      : TStringList;
begin
  if ( StrL_Assembly <> nil ) then
    Assembly := TStringList.Create
  else
    Assembly := nil;

  if ( StrL_Hex <> nil ) then
    Hex := TStringList.Create
  else
    Hex := nil;

  result := DecodeCombineNOP( Data, Size, Assembly, CodeOffset, Hex, {$IFDEF AssemblyTools}Details,{$ENDIF} DecoderSettings{$IFDEF AssemblyTools}, CalcJumpLines{$ENDIF} );

  if ( Assembly <> nil ) then
    begin
    StrL_Assembly.Text := Assembly.Text;
    Assembly.free;
    end;
  if ( Hex <> nil ) then
    begin
    StrL_Hex.Text := Hex.Text;
    Hex.free;
    end;
end;

function TIced.DecodeCombineNOP( Data : PByte; Size : Cardinal; Count : Cardinal; StrL_Assembly : TUnicodeStrings; CodeOffset : UInt64 = UInt64( 0 ); StrL_Hex : TUnicodeStrings = nil; {$IFDEF AssemblyTools}Details : pIcedDetails = nil;{$ENDIF} DecoderSettings : Cardinal = doNONE{$IFDEF AssemblyTools}; CalcJumpLines : Boolean = True{$ENDIF} ) : Cardinal;
var
  Assembly,
  Hex      : TStringList;
begin
  if ( StrL_Assembly <> nil ) then
    Assembly := TStringList.Create
  else
    Assembly := nil;

  if ( StrL_Hex <> nil ) then
    Hex := TStringList.Create
  else
    Hex := nil;

  result := DecodeCombineNOP( Data, Size, Count, Assembly, CodeOffset, Hex, {$IFDEF AssemblyTools}Details,{$ENDIF} DecoderSettings{$IFDEF AssemblyTools}, CalcJumpLines{$ENDIF} );

  if ( Assembly <> nil ) then
    begin
    StrL_Assembly.Text := Assembly.Text;
    Assembly.free;
    end;
  if ( Hex <> nil ) then
    begin
    StrL_Hex.Text := Hex.Text;
    Hex.free;
    end;
end;
{$IFEND UNICODE}

{$IFDEF AssemblyTools}
function TIced.DecodeAddress( var Data : TByteInstruction; Address : UInt64; Instruction : PString = nil; {$IFDEF AssemblyTools}Detail : pIcedDetail = nil;{$ENDIF} DecoderSettings : Cardinal = doNONE ) : Cardinal;
var
  StrL_Assembly : TStrings;
  StrL_Hex : TStrings;
  {$IFDEF AssemblyTools}
  Details : TIcedDetails;
  {$ENDIF AssemblyTools}
begin
  StrL_Assembly := TStringList.Create;
  StrL_Hex := TStringList.Create;
  {$IFDEF AssemblyTools}
  FillChar( Details, SizeOf( Details ), 0 );
  {$ENDIF AssemblyTools}

  result := Decode( @Data[ 0 ], Length( Data ), 1, StrL_Assembly, Address, StrL_Hex, {$IFDEF AssemblyTools}@Details,{$ENDIF} DecoderSettings, False{CalcJumpLines} );
  if ( result > 0 ) then
    begin
    if ( Instruction <> nil ) then
      Instruction^ := StrL_Assembly[ 0 ];
    {$IFDEF AssemblyTools}
    if ( Detail <> nil ) then
      Detail^ := Details.Items[ 0 ];
    {$ENDIF AssemblyTools}
    end
  else
    begin
    if ( Instruction <> nil ) then
      Instruction^ := '';
    {$IFDEF AssemblyTools}
    if ( Detail <> nil ) then
      FillChar( Detail^, SizeOf( Detail ), 0 );
    {$ENDIF AssemblyTools}
    end;

  StrL_Assembly.Free;
  StrL_Hex.Free;
  {$IFDEF AssemblyTools}
  SetLength( Details.Items, 0 );
  SetLength( Details.JumpTargets, 0 );
  {$ENDIF AssemblyTools}
end;

function TIced.DecodeAddress( Data : PByte; Size : Cardinal; Address : UInt64; InstructionBytes : pByteInstruction = nil; Instruction : PString = nil; {$IFDEF AssemblyTools}Detail : pIcedDetail = nil;{$ENDIF} DecoderSettings : Cardinal = doNONE ) : Cardinal;
var
  StrL_Assembly : TStrings;
  StrL_Hex : TStrings;
  {$IFDEF AssemblyTools}
  Details : TIcedDetails;
  {$ENDIF AssemblyTools}
begin
  StrL_Assembly := TStringList.Create;
  StrL_Hex := TStringList.Create;
  {$IFDEF AssemblyTools}
  FillChar( Details, SizeOf( Details ), 0 );
  {$ENDIF AssemblyTools}

  result := Decode( Data, Size, 1, StrL_Assembly, Address, StrL_Hex, {$IFDEF AssemblyTools}@Details,{$ENDIF} DecoderSettings, False{CalcJumpLines} );
  if ( result > 0 ) then
    begin
    if ( Instruction <> nil ) then
      Instruction^ := StrL_Assembly[ 0 ];
    {$IFDEF AssemblyTools}
    if ( Detail <> nil ) then
      Detail^ := Details.Items[ 0 ];
    {$ENDIF AssemblyTools}
    end
  else
    begin
    if ( Instruction <> nil ) then
      Instruction^ := '';
    {$IFDEF AssemblyTools}
    if ( Detail <> nil ) then
      FillChar( Detail^, SizeOf( Detail ), 0 );
    {$ENDIF AssemblyTools}
    end;

  StrL_Assembly.Free;
  StrL_Hex.Free;
  {$IFDEF AssemblyTools}
  SetLength( Details.Items, 0 );
  SetLength( Details.JumpTargets, 0 );
  {$ENDIF AssemblyTools}
end;
{$ENDIF AssemblyTools}

function TIced.Compare( Data : PByte; Size : Cardinal; Offset : UInt64; DataCompare : PByte; SizeCompare : Cardinal; OffsetCompare : UInt64; Count : Word; CodeOffset : UInt64 = UInt64( 0 ); Mode : tIcedAssemblyCompareMode = acmCenter ) : Cardinal;
var
  Decoder2 : TIcedDecoder;
  Inst1    : TInstruction;
  Inst2    : TInstruction;
begin
  result := 0;
  if ( Data = nil ) OR ( Size = 0 ) OR ( DataCompare = nil ) OR ( SizeCompare = 0 ) OR ( Count = 0 ) then
    Exit;

  Decoder2 := TIcedDecoder.Create( fDecoder.Bitness );

  {$IF CompilerVersion < 23}{$RANGECHECKS OFF}{$IFEND} // RangeCheck might cause Internal-Error C1118
  Offset := Offset-CodeOffset;
  OffsetCompare := OffsetCompare-CodeOffset;
  {$IF CompilerVersion < 23}{$RANGECHECKS ON}{$IFEND} // RangeCheck might cause Internal-Error C1118
  Size := Size-( Offset-CodeOffset );
  SizeCompare := SizeCompare-( OffsetCompare-CodeOffset );

  case Mode of // MS FindPreviousInstruction
    acmCenter   : begin
//                  if ( FindPreviousInstructionCount( Data, Size, 0{CodeOffset}, Offset, Offset, Count div 2 ) = 0 ) then
//                    Exit;
//                  if ( FindPreviousInstructionCount( DataCompare, SizeCompare, 0{CodeOffset}, OffsetCompare, OffsetCompare, Count div 2 ) = 0 ) then
//                    Exit;
                  end;
    acmBackward : begin
//                  if ( FindPreviousInstructionCount( Data, Size, 0{CodeOffset}, Offset, Offset, Count ) = 0 ) then
//                    Exit;
//                  if ( FindPreviousInstructionCount( DataCompare, SizeCompare, 0{CodeOffset}, OffsetCompare, OffsetCompare, Count ) = 0 ) then
//                    Exit;
                  end;
  end;

  {$IF CompilerVersion < 23}{$RANGECHECKS OFF}{$IFEND} // RangeCheck might cause Internal-Error C1118
  fDecoder.SetData( {$IF CompilerVersion < 23}PByte( PAnsiChar{$ELSE}( PByte{$IFEND}( Data ) + Offset ), Size, CodeOffset );
  Decoder2.SetData( {$IF CompilerVersion < 23}PByte( PAnsiChar{$ELSE}( PByte{$IFEND}( DataCompare ) + OffsetCompare ), SizeCompare, CodeOffset );
  {$IF CompilerVersion < 23}{$RANGECHECKS ON}{$IFEND} // RangeCheck might cause Internal-Error C1118

  while ( Count > 0 ) AND ( fDecoder.CanDecode ) AND ( Decoder2.CanDecode ) do
    begin
    fDecoder.Decode( Inst1 );
    Decoder2.Decode( Inst2 );

    if Inst1.IsEqual( Inst2 ) then
      Inc( result );

//    Inc( Data, Inst1.len );
//    Inc( DataCompare, Inst2.len );
//    {$IF CompilerVersion < 23}{$RANGECHECKS OFF}{$IFEND} // RangeCheck might cause Internal-Error C1118
//    Inc( CodeOffset, Inst1.len );
//    {$IF CompilerVersion < 23}{$RANGECHECKS ON}{$IFEND} // RangeCheck might cause Internal-Error C1118
    Dec( Count );
    end;
  Decoder2.free;
end;

function TIced.Compare( Data : TMemoryStream; Offset : UInt64; DataCompare : TMemoryStream; OffsetCompare : UInt64; Count : Word; CodeOffset : UInt64 = UInt64( 0 ); Mode : tIcedAssemblyCompareMode = acmCenter ) : Cardinal;
begin
  if ( Data = nil ) OR ( DataCompare = nil ) then
    begin
    result := 0;
    Exit;
    end;

  result := Compare( {$IF CompilerVersion < 23}PByte( PAnsiChar{$ELSE}( PByte{$IFEND}( Data.Memory )+Data.Position ),
                     Data.Size-Data.Position, Offset,
                     {$IF CompilerVersion < 23}PByte( PAnsiChar{$ELSE}( PByte{$IFEND}( DataCompare.Memory )+DataCompare.Position ),
                     DataCompare.Size-DataCompare.Position, OffsetCompare,
                     Count, CodeOffset, Mode );
end;

function TIced.PointerScan( Data : PByte; Size : Cardinal; var results : tIcedPointerScanResults; CodeOffset : UInt64 = UInt64( 0 ); ProcessEvent : tIcedPointerScanProcessEvent = nil ) : Cardinal;
const
  BLOCK_SIZE   = 100000;
  UPDATE_PERCENT = 0.01;
var
  Instruction : TInstruction;
  Offsets     : TConstantOffsets;
  i           : Integer;
  done        : UInt64;
  b           : Boolean;
  Off         : UInt64;
  Disp        : Int64;
  Upd         : Cardinal;
  UpdBytes    : Cardinal;

  Cancel      : Boolean;
begin
  result := 0;
  SetLength( results, 0 );
  if ( Data = nil ) then
    Exit;
  if ( Size = 0 ) then
    Exit;

  fDecoder.SetData( Data, Size, CodeOffset );

  {$IF CompilerVersion < 23}{$RANGECHECKS OFF}{$IFEND} // RangeCheck might cause Internal-Error C1118
  done := 0;
  {$IF CompilerVersion < 23}{$RANGECHECKS ON}{$IFEND} // RangeCheck might cause Internal-Error C1118
  Cancel := False;

  Upd := 0;
  UpdBytes := Trunc( size * UPDATE_PERCENT );
  while ( done < size ) do
    begin
    if ( @ProcessEvent <> nil ) AND ( Upd = 0 ) then
      begin
      ProcessEvent( done, size, Cancel );
      if Cancel then
        Break;
      Upd := UpdBytes;
      end;

    fDecoder.Decode( Instruction );

    if Instruction.IsValid then
      begin
      fDecoder.GetConstantOffsets( Instruction, Offsets );

      if ( Offsets.displacement_size > 0 ) OR ( Offsets.immediate_size > 0 ) OR ( Offsets.immediate_size2 > 0 ) then
        begin
        {$IF CompilerVersion < 23}{$RANGECHECKS OFF}{$IFEND} // RangeCheck might cause Internal-Error C1118
        Off := Instruction.next_rip;
        {$IF CompilerVersion < 23}{$RANGECHECKS ON}{$IFEND} // RangeCheck might cause Internal-Error C1118

        case Offsets.displacement_size of
          1 : Disp := PShortInt( PAnsiChar( Data{fDecoder.InstructionFirstByte} ) + Offsets.displacement_offset )^;
          2 : Disp := PSmallInt( PAnsiChar( Data{fDecoder.InstructionFirstByte} ) + Offsets.displacement_offset )^;
          4 : Disp := PInteger( PAnsiChar( Data{fDecoder.InstructionFirstByte} ) + Offsets.displacement_offset )^;
          8 : Disp := PInt64( PAnsiChar( Data{fDecoder.InstructionFirstByte} ) + Offsets.displacement_offset )^;
        else
          Disp := 0;
        end;

        if ( Disp > 0 ) then
          Inc( Off, Disp )
        else if ( Disp < 0 ) then
          Dec( Off, Abs( Disp ) );

        case Offsets.immediate_size of
          1 : Disp := PShortInt( PAnsiChar( Data{fDecoder.InstructionFirstByte} ) + Offsets.immediate_offset )^;
          2 : Disp := PSmallInt( PAnsiChar( Data{fDecoder.InstructionFirstByte} ) + Offsets.immediate_offset )^;
          4 : Disp := PInteger( PAnsiChar( Data{fDecoder.InstructionFirstByte} ) + Offsets.immediate_offset )^;
          8 : Disp := PInt64( PAnsiChar( Data{fDecoder.InstructionFirstByte} ) + Offsets.immediate_offset )^;
        else
          Disp := 0;
        end;

        if ( Disp > 0 ) then
          Inc( Off, Disp )
        else if ( Disp < 0 ) then
          Dec( Off, Abs( Disp ) );

        case Offsets.immediate_size2 of
          1 : Disp := PShortInt( PAnsiChar( Data{fDecoder.InstructionFirstByte} ) + Offsets.immediate_offset2 )^;
          2 : Disp := PSmallInt( PAnsiChar( Data{fDecoder.InstructionFirstByte} ) + Offsets.immediate_offset2 )^;
          4 : Disp := PInteger( PAnsiChar( Data{fDecoder.InstructionFirstByte} ) + Offsets.immediate_offset2 )^;
          8 : Disp := PInt64( PAnsiChar( Data{fDecoder.InstructionFirstByte} ) + Offsets.immediate_offset2 )^;
        else
          Disp := 0;
        end;

        if ( Disp > 0 ) then
          Inc( Off, Disp )
        else if ( Disp < 0 ) then
          Dec( Off, Abs( Disp ) );

        if ( ABS( Instruction.next_rip-Off ) >= MASKING_OFFSET ) then
          begin
          // Filter duplicates
          b := false;
          for i := Low( Results ) to result-1 do // High( Results ) do
            begin
            if ( Results[ i ].vPointer = Off ) then
              begin
              if ( Results[ i ].Count < High( Results[ i ].Count ) ) then
                Inc( Results[ i ].Count );
              b := True;
              break;
              end;
            end;
          if NOT b then
            begin
            if ( result >= Length( results ) ) then
              SetLength( results, Length( results )+BLOCK_SIZE );
            {$IF CompilerVersion < 23}{$RANGECHECKS OFF}{$IFEND} // RangeCheck might cause Internal-Error C1118
            Results[ result ].Origin      := Instruction.RIP;
            Results[ result ].Instruction := String( Formatter.FormatToString( Instruction ) );
            Results[ result ].vPointer    := Off;
            Results[ result ].Count       := 0;
            {$IF CompilerVersion < 23}{$RANGECHECKS ON}{$IFEND} // RangeCheck might cause Internal-Error C1118
            Inc( result );
            end;
          end;
        end;
      end;

    {$IF CompilerVersion < 23}{$RANGECHECKS OFF}{$IFEND} // RangeCheck might cause Internal-Error C1118
    done := done + Instruction.len;
    Inc( Data, Instruction.len );
//    Inc( CodeOffset, Instruction.len );
    if ( Upd > Instruction.len )  then
      Dec( Upd, Instruction.len )
    else
      Upd := 0;
    {$IF CompilerVersion < 23}{$RANGECHECKS ON}{$IFEND} // RangeCheck might cause Internal-Error C1118
    end;

  SetLength( results, result );
end;

function TIced.PointerScan( Data : TMemoryStream; var results : tIcedPointerScanResults; CodeOffset : UInt64 = UInt64( 0 ); ProcessEvent : tIcedPointerScanProcessEvent = nil ) : Cardinal;
begin
  if ( Data = nil ) then
    begin
    result := 0;
    Exit;
    end;
  result := PointerScan( {$IF CompilerVersion < 23}PByte( PAnsiChar{$ELSE}( PByte{$IFEND}( Data.Memory )+Data.Position ), Data.Size-Data.Position, Results, CodeOffset, ProcessEvent );
end;

function TIced.ReferenceScan( Data : PByte; Size : Cardinal; Reference : UInt64; var results : tIcedReferenceScanResults; CodeOffset : UInt64 = UInt64( 0 ); ProcessEvent : tIcedPointerScanProcessEvent = nil ) : Cardinal;
const
  BLOCK_SIZE     = 10000;
  UPDATE_PERCENT = 0.25;
var
  Instruction : TInstruction;
  Offsets     : TConstantOffsets;
  done        : UInt64;
  Off         : UInt64;
  Disp        : Int64;
  Upd         : Cardinal;
  UpdBytes    : Cardinal;

  Cancel      : Boolean;
begin
  result := 0;
  SetLength( results, 0 );
  if ( Data = nil ) then
    Exit;
  if ( Size = 0 ) then
    Exit;

  fDecoder.SetData( Data, Size, CodeOffset );

  {$IF CompilerVersion < 23}{$RANGECHECKS OFF}{$IFEND} // RangeCheck might cause Internal-Error C1118
  done := 0;
  {$IF CompilerVersion < 23}{$RANGECHECKS ON}{$IFEND} // RangeCheck might cause Internal-Error C1118
  Cancel := False;

  Upd := 0;
  UpdBytes := Trunc( size * UPDATE_PERCENT );
  while ( done < size ) do
    begin
    if ( @ProcessEvent <> nil ) AND ( Upd = 0 ) then
      begin
      ProcessEvent( done, size, Cancel );
      if Cancel then
        Break;
      Upd := UpdBytes;
      end;

    fDecoder.Decode( Instruction );

    if Instruction.IsValid then
      begin
      fDecoder.GetConstantOffsets( Instruction, Offsets );

      if ( Offsets.displacement_size > 0 ) OR ( Offsets.immediate_size > 0 ) OR ( Offsets.immediate_size2 > 0 ) then
        begin
        {$IF CompilerVersion < 23}{$RANGECHECKS OFF}{$IFEND} // RangeCheck might cause Internal-Error C1118
        Off := Instruction.next_rip;
        {$IF CompilerVersion < 23}{$RANGECHECKS ON}{$IFEND} // RangeCheck might cause Internal-Error C1118

        case Offsets.displacement_size of
          1 : Disp := PShortInt( PAnsiChar( Data{fDecoder.InstructionFirstByte} ) + Offsets.displacement_offset )^;
          2 : Disp := PSmallInt( PAnsiChar( Data{fDecoder.InstructionFirstByte} ) + Offsets.displacement_offset )^;
          4 : Disp := PInteger( PAnsiChar( Data{fDecoder.InstructionFirstByte} ) + Offsets.displacement_offset )^;
          8 : Disp := PInt64( PAnsiChar( Data{fDecoder.InstructionFirstByte} ) + Offsets.displacement_offset )^;
        else
          Disp := 0;
        end;

        if ( Disp > 0 ) then
          Inc( Off, Disp )
        else if ( Disp < 0 ) then
          Dec( Off, Abs( Disp ) );

        case Offsets.immediate_size of
          1 : Disp := PShortInt( PAnsiChar( Data{fDecoder.InstructionFirstByte} ) + Offsets.immediate_offset )^;
          2 : Disp := PSmallInt( PAnsiChar( Data{fDecoder.InstructionFirstByte} ) + Offsets.immediate_offset )^;
          4 : Disp := PInteger( PAnsiChar( Data{fDecoder.InstructionFirstByte} ) + Offsets.immediate_offset )^;
          8 : Disp := PInt64( PAnsiChar( Data{fDecoder.InstructionFirstByte} ) + Offsets.immediate_offset )^;
        else
          Disp := 0;
        end;

        if ( Disp > 0 ) then
          Inc( Off, Disp )
        else if ( Disp < 0 ) then
          Dec( Off, Abs( Disp ) );

        case Offsets.immediate_size2 of
          1 : Disp := PShortInt( PAnsiChar( Data{fDecoder.InstructionFirstByte} ) + Offsets.immediate_offset2 )^;
          2 : Disp := PSmallInt( PAnsiChar( Data{fDecoder.InstructionFirstByte} ) + Offsets.immediate_offset2 )^;
          4 : Disp := PInteger( PAnsiChar( Data{fDecoder.InstructionFirstByte} ) + Offsets.immediate_offset2 )^;
          8 : Disp := PInt64( PAnsiChar( Data{fDecoder.InstructionFirstByte} ) + Offsets.immediate_offset2 )^;
        else
          Disp := 0;
        end;

        if ( Disp > 0 ) then
          Inc( Off, Disp )
        else if ( Disp < 0 ) then
          Dec( Off, Abs( Disp ) );

        if ( Reference = Off ) then
          begin
          if ( result >= Length( results ) ) then
            SetLength( results, Length( results )+BLOCK_SIZE );
          {$IF CompilerVersion < 23}{$RANGECHECKS OFF}{$IFEND} // RangeCheck might cause Internal-Error C1118
          Results[ result ].Origin      := Instruction.RIP;
          Results[ result ].Instruction := String( Formatter.FormatToString( Instruction ) );
          {$IF CompilerVersion < 23}{$RANGECHECKS ON}{$IFEND} // RangeCheck might cause Internal-Error C1118
          Inc( result );
          end;
        end;
      end;

    {$IF CompilerVersion < 23}{$RANGECHECKS OFF}{$IFEND} // RangeCheck might cause Internal-Error C1118
    done := done + Instruction.len;
    Inc( Data, Instruction.len );
//    Inc( CodeOffset, Instruction.len );
    if ( Upd > Instruction.len )  then
      Dec( Upd, Instruction.len )
    else
      Upd := 0;
    {$IF CompilerVersion < 23}{$RANGECHECKS ON}{$IFEND} // RangeCheck might cause Internal-Error C1118
    end;

  SetLength( results, result );
end;

function TIced.ReferenceScan( Data : TMemoryStream; Reference : UInt64; var results : tIcedReferenceScanResults; CodeOffset : UInt64 = UInt64( 0 ); ProcessEvent : tIcedPointerScanProcessEvent = nil ) : Cardinal;
begin
  if ( Data = nil ) then
    begin
    result := 0;
    Exit;
    end;
  result := ReferenceScan( {$IF CompilerVersion < 23}PByte( PAnsiChar{$ELSE}( PByte{$IFEND}( Data.Memory )+Data.Position ), Data.Size-Data.Position, Reference, Results, CodeOffset, ProcessEvent );
end;

function TIced.AssemblyScan( Data : PByte; Size : Cardinal; Assembly : String; var results : tIcedReferenceScanResults; CodeOffset : UInt64 = UInt64( 0 ); Mode : tIcedAssemblyScanMode = asmEqual; ProcessEvent : tIcedPointerScanProcessEvent = nil ) : Cardinal;
  function IsWm( Text, MatchText: String; caseSensitive : boolean = false ): Boolean;
  var
    StringPtr: PChar;
    PatternPtr: PChar;
    StringRes: PChar;
    PatternRes: PChar;
    tText,
    tMatchText : String;
  begin
    Result := False;

    if ( Text = '' ) or ( MatchText = '' ) then
      Exit;

    if ( caseSensitive ) then
      begin
      tText := Text;
      tMatchText := MatchText;
      end
    else
      begin
      tText      := {$IFDEF STANDALONE}SysUtils.{$ENDIF}UpperCase( Text );
      tMatchText := {$IFDEF STANDALONE}SysUtils.{$ENDIF}UpperCase( MatchText );
      end;

    StringPtr  := PChar( tText );
    PatternPtr := PChar( tMatchText );
    StringRes  := nil;
    PatternRes := nil;

    repeat
      repeat
        case PatternPtr^ of
          #0:
              begin
              Result := StringPtr^ = #0;
              if Result or ( StringRes = nil ) or ( PatternRes = nil ) then
                Exit;

              StringPtr := StringRes;
              PatternPtr := PatternRes;
              Break;
              end;
          '*':
              begin
              Inc( PatternPtr );
              PatternRes := PatternPtr;
              Break;
              end;
          '?':
              begin
              if StringPtr^ = #0 then
                Exit;
              Inc( StringPtr );
              Inc( PatternPtr );
              end;
        else
            begin
              if StringPtr^ = #0 then
                Exit;
              if StringPtr^ <> PatternPtr^ then
                begin
                if ( StringRes = nil ) or ( PatternRes = nil ) then
                  Exit;
                StringPtr := StringRes;
                PatternPtr := PatternRes;
                Break;
                end
              else
                begin
                Inc( StringPtr );
                Inc( PatternPtr );
                end;
            end;
        end;
      until False;

      repeat
        case PatternPtr^ of
          #0:
              begin
              Result := True;
              Exit;
              end;
          '*':
              begin
              Inc( PatternPtr );
              PatternRes := PatternPtr;
              end;
          '?':
              begin
              if StringPtr^ = #0 then
                Exit;
              Inc( StringPtr );
              Inc( PatternPtr );
              end;
        else
              begin
              repeat
                if StringPtr^ = #0 then
                  Exit;
                if StringPtr^ = PatternPtr^ then
                  Break;
                Inc( StringPtr );
              until False;
              Inc( StringPtr );
              StringRes := StringPtr;
              Inc( PatternPtr );
              Break;
              end;
        end;
      until False;
    until False;
  end;
const
  BLOCK_SIZE     = 10000;
  UPDATE_PERCENT = 0.25;
  NOP            = 'nop';
var
  Instruction : TInstruction;
  done        : UInt64;
  StrL        : TStringList;
  iCnt        : Cardinal;
  sInstruction: TStringList;
  cInstruction: Array [ 0..255 ] of AnsiChar;
  {$IF Declared( RegularExpressions )}
  RegEx       : TRegEx;
  {$IFEND}
  b           : Boolean;
  Offset      : UInt64;
  i, j        : Integer;
  S, ts       : string;

  Upd         : Cardinal;
  UpdBytes    : Cardinal;
  Cancel      : Boolean;
begin
  result := 0;
  SetLength( results, 0 );
  if ( Data = nil ) then
    Exit;
  if ( Size = 0 ) then
    Exit;
  if ( Assembly = '' ) then
    Exit;
  if ( Mode = asmSimiliar ) then
    Exit;

  fDecoder.SetData( Data, Size, CodeOffset );

  {$IF CompilerVersion < 23}{$RANGECHECKS OFF}{$IFEND} // RangeCheck might cause Internal-Error C1118
  done := 0;
  {$IF CompilerVersion < 23}{$RANGECHECKS ON}{$IFEND} // RangeCheck might cause Internal-Error C1118
  Cancel := False;

  StrL := TStringList.Create;
  StrL.Text := LowerCase( Trim( Assembly ) );

  for i := StrL.Count-1 downTo 0 do
    begin
    S := Trim( StrL[ i ] );
    S := StringReplace( S, #9, ' ', [ rfReplaceAll ] );
    if ( S = '' ) then
      StrL.Delete( i )
    else if ( Copy( S, 1, 4 ) = NOP + ':' ) then
      begin
      Upd := StrToIntDef( Copy( S, 5, Length( S )-4 ), 1);
      while ( Upd > 1 ) do
        begin
        StrL.Insert( i+1, NOP );
        Dec( Upd );
        end;
      StrL[ i ] := NOP;
      end
    else
      begin
      tS := '';
      while ( tS <> S ) do
        begin
        tS := S;
        S := StringReplace( S, '  ', ' ', [ rfReplaceAll ] );
        end;

      {$IF Declared( asmRegExp )}
      if ( Mode <> asmRegExp ) then
      {$IFEND}      
        begin
        if fFormatter.fOptions.SpaceAfterOperandSeparator then
          S := StringReplace( S, ',', ', ', [ rfReplaceAll ] )
        else
          S := StringReplace( S, ', ', ',', [ rfReplaceAll ] );        
        
        if fFormatter.fOptions.SpaceAfterMemoryBracket then
          begin
          S := StringReplace( S, '[', '[ ', [ rfReplaceAll ] );
          S := StringReplace( S, ']', ' ]', [ rfReplaceAll ] );
          end
        else
          begin
          S := StringReplace( S, '[ ', '[', [ rfReplaceAll ] );
          S := StringReplace( S, ' ]', ']', [ rfReplaceAll ] );
          end;
          
        if fFormatter.fOptions.SpaceBetweenMemoryAddOperators then
          begin
          S := StringReplace( S, '+', ' + ', [ rfReplaceAll ] );
          S := StringReplace( S, '-', ' - ', [ rfReplaceAll ] );
          end
        else
          begin
          S := StringReplace( S, ' +', '+', [ rfReplaceAll ] );
          S := StringReplace( S, '+ ', '+', [ rfReplaceAll ] );
          S := StringReplace( S, ' -', '-', [ rfReplaceAll ] );
          S := StringReplace( S, '- ', '-', [ rfReplaceAll ] );        
          end;          
        end;
      
      if ( Mode = asmEqual ) then 
        begin
        if fFormatter.fOptions.SpaceBetweenMemoryMulOperators then
          S := StringReplace( S, '*', ' * ', [ rfReplaceAll ] )
        else
          begin
          S := StringReplace( S, ' *', '*', [ rfReplaceAll ] );
          S := StringReplace( S, '* ', '*', [ rfReplaceAll ] );
          end;
        end;
        
      S := StringReplace( S, ' ,', ',', [ rfReplaceAll ] );
      tS := '';
      while ( tS <> S ) do
        begin
        tS := S;
        S := StringReplace( S, '  ', ' ', [ rfReplaceAll ] );
        end;
      StrL[ i ] := S;
      end;
    end;

  iCnt := 0;
  sInstruction := TStringList.Create;
  {$IF CompilerVersion < 23}{$RANGECHECKS OFF}{$IFEND} // RangeCheck might cause Internal-Error C1118
  Offset := CodeOffset;
  {$IF CompilerVersion < 23}{$RANGECHECKS ON}{$IFEND} // RangeCheck might cause Internal-Error C1118

  {$IF Declared( RegularExpressions )}
  if Mode = asmRegExp then
    RegEx := TRegEx.Create( StrL[ 0 ] );
  {$IFEND}

  Upd := 0;
  UpdBytes := Trunc( size * UPDATE_PERCENT );
  while ( done < size ) do
    begin
    if ( @ProcessEvent <> nil ) AND ( Upd = 0 ) then
      begin
      ProcessEvent( done, size, Cancel );
      if Cancel then
        Break;
      Upd := UpdBytes;
      end;

    fDecoder.Decode( Instruction );

    if Instruction.IsValid then
      begin
      Formatter.Format( Instruction, @cInstruction[ 0 ], Length( cInstruction ) );

      case Mode of
        asmEqual    : b := ( CompareText( StrL[ iCnt ], String( cInstruction ) ) = 0 );
        asmWildcard : b := IsWm( String( cInstruction ), StrL[ iCnt ], False );
      {$IF Declared( RegularExpressions )}
        asmRegExp   : b := RegEx.IsMatch( String( cInstruction ) );
      {$IFEND}
      else
        b := False;
      end;

      if b then
        begin
        if ( iCnt = 0 ) then
          {$IF CompilerVersion < 23}{$RANGECHECKS OFF}{$IFEND} // RangeCheck might cause Internal-Error C1118
          Offset := Instruction.RIP;
          {$IF CompilerVersion < 23}{$RANGECHECKS ON}{$IFEND} // RangeCheck might cause Internal-Error C1118

        if ( iCnt = StrL.Count-1 ) then
          begin
          sInstruction.Add( String( cInstruction ) );
          j := 0;
          for i := sInstruction.Count-1 downTo 0 do
            begin
            if ( LowerCase( sInstruction[ i ] ) = NOP ) then
              begin
              sInstruction.Delete( i );
              Inc( j );
              end
            else
              begin
              if ( j > 0 ) then
                sInstruction.Insert( i, NOP + ':' + IntToStr( j ) );
              j := 0;
              end;
            end;
          if ( j > 0 ) then
            sInstruction.Insert( 0, NOP + ':' + IntToStr( j ) );

          if ( result >= Length( results ) ) then
            SetLength( results, Length( results )+BLOCK_SIZE );
          {$IF CompilerVersion < 23}{$RANGECHECKS OFF}{$IFEND} // RangeCheck might cause Internal-Error C1118
          Results[ result ].Origin      := Offset;
          Results[ result ].Instruction := Trim( sInstruction.Text );
          {$IF CompilerVersion < 23}{$RANGECHECKS ON}{$IFEND} // RangeCheck might cause Internal-Error C1118
          Inc( result );

          iCnt := 0;
          sInstruction.Clear;
          end
        else
          begin
          Inc( iCnt );
          sInstruction.Add( String( cInstruction ) );
          end;
        end
      else
        begin
        iCnt := 0;
        sInstruction.Clear;
        end;

      {$IF Declared( RegularExpressions )}
      if Mode = asmRegExp then
        RegEx := TRegEx.Create( StrL[ iCnt ] );
      {$IFEND Declared( RegularExpressions )}
      end
    else
      begin
      iCnt := 0;
      sInstruction.Clear;
      {$IF Declared( RegularExpressions )}
      if Mode = asmRegExp then
        RegEx := TRegEx.Create( StrL[ iCnt ] );
      {$IFEND Declared( RegularExpressions )}
      end;

    {$IF CompilerVersion < 23}{$RANGECHECKS OFF}{$IFEND} // RangeCheck might cause Internal-Error C1118
    done := done + Instruction.len;
//    Inc( Code, Instruction.len );
//    Inc( CodeOffset, Instruction.len );
    if ( Upd > Instruction.len )  then
      Dec( Upd, Instruction.len )
    else
      Upd := 0;
    {$IF CompilerVersion < 23}{$RANGECHECKS ON}{$IFEND} // RangeCheck might cause Internal-Error C1118
    end;
  StrL.free;

  SetLength( results, result );
end;

function TIced.AssemblyScan( Data : TMemoryStream; Assembly : String; var results : tIcedReferenceScanResults; CodeOffset : UInt64 = UInt64( 0 ); Mode : tIcedAssemblyScanMode = asmEqual; ProcessEvent : tIcedPointerScanProcessEvent = nil ) : Cardinal;
begin
  if ( Data = nil ) then
    begin
    result := 0;
    Exit;
    end;
  result := AssemblyScan( {$IF CompilerVersion < 23}PByte( PAnsiChar{$ELSE}( PByte{$IFEND}( Data.Memory )+Data.Position ), Data.Size-Data.Position, Assembly, Results, CodeOffset, Mode, ProcessEvent );
end;

function TIced.AssemblyScan( Data : PByte; Size : Cardinal; Assembly : PInstruction; AssemblyCount : Cardinal; var results : tIcedReferenceScanResults; CodeOffset : UInt64 = UInt64( 0 ); Mode : tIcedAssemblyScanMode = asmEqual; ProcessEvent : tIcedPointerScanProcessEvent = nil ) : Cardinal;
type
  TInstructionArrayStatic = Array [ 0..0 ] of TInstruction;
  PInstructionArrayStatic = ^TInstructionArrayStatic;
const
  BLOCK_SIZE   = 100000;
  UPDATE_PERCENT = 0.25;
var
  aAssembly   : PInstructionArrayStatic absolute Assembly;
  Instruction : TInstruction;
  done        : UInt64;
  Upd         : Cardinal;
  UpdBytes    : Cardinal;
  i           : Integer;
  b           : Boolean;
  Offset      : UInt64;
  sInstruction: String;

  Cancel      : Boolean;
begin
  result := 0;
  SetLength( results, 0 );
  if ( Data = nil ) then
    Exit;
  if ( Size = 0 ) then
    Exit;
  if ( Assembly = nil ) OR ( AssemblyCount = 0 ) then
    Exit;
  if NOT ( Mode in [ asmEqual, asmSimiliar ] ) then
    Exit;

  fDecoder.SetData( Data, Size, CodeOffset );

  {$IF CompilerVersion < 23}{$RANGECHECKS OFF}{$IFEND} // RangeCheck might cause Internal-Error C1118
  done := 0;
  {$IF CompilerVersion < 23}{$RANGECHECKS ON}{$IFEND} // RangeCheck might cause Internal-Error C1118
  Cancel := False;

  Upd := 0;
  UpdBytes := Trunc( size * UPDATE_PERCENT );
  i := 0;
  {$IF CompilerVersion < 23}{$RANGECHECKS OFF}{$IFEND} // RangeCheck might cause Internal-Error C1118
  Offset := Instruction.RIP;
  {$IF CompilerVersion < 23}{$RANGECHECKS ON}{$IFEND} // RangeCheck might cause Internal-Error C1118
  while ( done < size ) do
    begin
    if ( @ProcessEvent <> nil ) AND ( Upd = 0 ) then
      begin
      ProcessEvent( done, size, Cancel );
      if Cancel then
        Break;
      Upd := UpdBytes;
      end;

    fDecoder.Decode( Instruction );

    if Instruction.IsValid then
      begin
      case Mode of
        asmEqual    : b := Instruction.IsEqual( {$R-}aAssembly^[ i ]{$R+} );
        asmSimiliar : b := Instruction.IsSimiliar( {$R-}aAssembly^[ i ]{$R+} );
      else
        b := False;
      end;

      if b then
        begin
        if ( i = 0 ) then
          {$IF CompilerVersion < 23}{$RANGECHECKS OFF}{$IFEND} // RangeCheck might cause Internal-Error C1118
          Offset := Instruction.RIP;
          {$IF CompilerVersion < 23}{$RANGECHECKS ON}{$IFEND} // RangeCheck might cause Internal-Error C1118
        sInstruction := sInstruction + String( Formatter.FormatToString( Instruction ) ) + #13#10;
        Inc( i );

        if ( i >= AssemblyCount ) then
          begin
          if ( result >= Length( results ) ) then
            SetLength( results, Length( results )+BLOCK_SIZE );
          {$IF CompilerVersion < 23}{$RANGECHECKS OFF}{$IFEND} // RangeCheck might cause Internal-Error C1118
          Results[ result ].Origin      := Offset;
          Results[ result ].Instruction := Copy( sInstruction, 1, Length( sInstruction )-2 );
          {$IF CompilerVersion < 23}{$RANGECHECKS ON}{$IFEND} // RangeCheck might cause Internal-Error C1118
          Inc( result );

          i := 0;
          sInstruction := '';
          end;
        end
      else
        begin
        i := 0;
        sInstruction := '';
        end;
      end;

    {$IF CompilerVersion < 23}{$RANGECHECKS OFF}{$IFEND} // RangeCheck might cause Internal-Error C1118
    done := done + Instruction.len;
//    Inc( Data, Instruction.len );
//    Inc( CodeOffset, Instruction.len );
    if ( Upd > Instruction.len )  then
      Dec( Upd, Instruction.len )
    else
      Upd := 0;
    {$IF CompilerVersion < 23}{$RANGECHECKS ON}{$IFEND} // RangeCheck might cause Internal-Error C1118
    end;

  SetLength( results, result );
end;

function TIced.AssemblyScan( Data : TMemoryStream; Assembly : PInstruction; AssemblyCount : Cardinal; var results : tIcedReferenceScanResults; CodeOffset : UInt64 = UInt64( 0 ); Mode : tIcedAssemblyScanMode = asmEqual; ProcessEvent : tIcedPointerScanProcessEvent = nil ) : Cardinal;
begin
  if ( Data = nil ) then
    begin
    result := 0;
    Exit;
    end;
  result := AssemblyScan( {$IF CompilerVersion < 23}PByte( PAnsiChar{$ELSE}( PByte{$IFEND}( Data.Memory )+Data.Position ), Data.Size-Data.Position, Assembly, AssemblyCount, Results, CodeOffset, Mode, ProcessEvent );
end;

function TIced.AssemblyScan( Data : PByte; Size : Cardinal; var Assembly : TInstructionArray; var results : tIcedReferenceScanResults; CodeOffset : UInt64 = UInt64( 0 ); Mode : tIcedAssemblyScanMode = asmEqual; ProcessEvent : tIcedPointerScanProcessEvent = nil ) : Cardinal;
begin
  if ( Length( Assembly ) = 0 ) then
    begin
    result := 0;
    Exit;
    end;
  result := AssemblyScan( Data, Size, @Assembly[ 0 ], Length( Assembly ), Results, CodeOffset, Mode, ProcessEvent );
end;

function TIced.AssemblyScan( Data : TMemoryStream; var Assembly : TInstructionArray; var results : tIcedReferenceScanResults; CodeOffset : UInt64 = UInt64( 0 ); Mode : tIcedAssemblyScanMode = asmEqual; ProcessEvent : tIcedPointerScanProcessEvent = nil ) : Cardinal;
begin
  if ( Data = nil ) OR ( Length( Assembly ) = 0 ) then
    begin
    result := 0;
    Exit;
    end;
  result := AssemblyScan( {$IF CompilerVersion < 23}PByte( PAnsiChar{$ELSE}( PByte{$IFEND}( Data.Memory )+Data.Position ), Data.Size-Data.Position, @Assembly[ 0 ], Length( Assembly ), Results, CodeOffset, Mode, ProcessEvent );
end;

function TIced.FindInstruction( Data : PByte; Size : Cardinal; Offset : UInt64; CodeOffset : UInt64 = UInt64( 0 ); {$IFDEF AssemblyTools}Details : pIcedDetails = nil;{$ENDIF} DecoderSettings : Cardinal = doNONE ) : Cardinal;
const
  BLOCKSIZE = 20;
var
  Instruction : TInstruction;
  i           : Cardinal;
begin
  {$IF CompilerVersion < 23}{$RANGECHECKS OFF}{$IFEND} // RangeCheck might cause Internal-Error C1118
  result := 0;
  if ( self = nil ) then
    Exit;
  if ( Data = nil ) OR ( Size = 0 ) then
    Exit;

  if ( Offset > CodeOffset+Size ) then
    Exit;

  if ( CodeOffset > Offset ) then
    Exit;

  Offset := Offset-CodeOffset;
  if ( Offset > BLOCKSIZE )  then
    begin
    i := Offset-BLOCKSIZE;
    Inc( Data, i );
    end
  else
    i := 0;

  fDecoder.SetData( Data, Size-i, {CodeOffset+}i, DecoderSettings );
  {$IFDEF AssemblyTools}
  if ( Details <> nil ) then
    begin
    while fDecoder.CanDecode do
      begin
      fDecoder.Decode( Instruction, Details^ );
      if ( Instruction.next_rip >= Offset ) then
        begin
        result := Instruction.next_rip;
        Break;
        end;
      end;
    end
  else
  {$ENDIF AssemblyTools}
    begin
    while fDecoder.CanDecode do
      begin
      fDecoder.Decode( Instruction );
      if ( Instruction.next_rip >= Offset ) then
        begin
        result := Instruction.next_rip;
        Break;
        end;
      end;
    end;
  {$IF CompilerVersion < 23}{$RANGECHECKS ON}{$IFEND} // RangeCheck might cause Internal-Error C1118
end;

function TIced.FindInstruction( Data : TMemoryStream; Offset : UInt64; CodeOffset : UInt64 = UInt64( 0 ); {$IFDEF AssemblyTools}Details : pIcedDetails = nil;{$ENDIF} DecoderSettings : Cardinal = doNONE ) : Cardinal;
begin
  if ( Data = nil ) then
    begin
    result := 0;
    Exit;
    end;
  {$IF CompilerVersion < 23}{$RANGECHECKS OFF}{$IFEND} // RangeCheck might cause Internal-Error C1118
  result := FindInstruction( {$IF CompilerVersion < 23}PByte( PAnsiChar{$ELSE}( PByte{$IFEND}( Data.Memory )+Data.Position ), Data.Size-Data.Position, Offset, CodeOffset, {$IFDEF AssemblyTools}Details,{$ENDIF} DecoderSettings );
  {$IF CompilerVersion < 23}{$RANGECHECKS ON}{$IFEND} // RangeCheck might cause Internal-Error C1118
end;

function TIced.FindNextInstruction( Data : TMemoryStream; CodeOffset : UInt64; Address : UInt64; var Offset : UInt64; InstructionCount : Word = 1; InstructionBytes : pByteInstruction = nil; Instruction : PString = nil; {$IFDEF AssemblyTools}Details : pIcedDetails = nil;{$ENDIF} DecoderSettings : Cardinal = doNONE ) : Cardinal;
begin
  if ( Data = nil ) then
    begin
    result := 0;
    Exit;
    end;
  {$IF CompilerVersion < 23}{$RANGECHECKS OFF}{$IFEND} // RangeCheck might cause Internal-Error C1118
  result := FindNextInstruction( {$IF CompilerVersion < 23}PByte( PAnsiChar{$ELSE}( PByte{$IFEND}( Data.Memory )+Data.Position ), Data.Size-Data.Position, Address, Offset, CodeOffset, InstructionCount, InstructionBytes, Instruction, {$IFDEF AssemblyTools}Details,{$ENDIF} DecoderSettings );
  {$IF CompilerVersion < 23}{$RANGECHECKS ON}{$IFEND} // RangeCheck might cause Internal-Error C1118
end;

function TIced.FindNextInstruction( Data : PByte; Size : Cardinal; CodeOffset : UInt64; Address : UInt64; var Offset : UInt64; InstructionCount : Word = 1; InstructionBytes : pByteInstruction = nil; Instruction : PString = nil; {$IFDEF AssemblyTools}Details : pIcedDetails = nil;{$ENDIF} DecoderSettings : Cardinal = doNONE ) : Cardinal;
var
  Inst : TInstruction;
begin
  result := 0;
  if ( self = nil ) then
    Exit;

  if ( Data = nil ) OR ( Size = 0 ) then
    Exit;

  {$IF CompilerVersion < 23}{$RANGECHECKS OFF}{$IFEND} // RangeCheck might cause Internal-Error C1118
  if ( Address > CodeOffset+Size ) then
  {$IF CompilerVersion < 23}{$RANGECHECKS ON}{$IFEND} // RangeCheck might cause Internal-Error C1118  
    Exit;

  if ( CodeOffset > Address ) then
    Exit;

  {$IF CompilerVersion < 23}{$RANGECHECKS OFF}{$IFEND} // RangeCheck might cause Internal-Error C1118
  fDecoder.SetData( {$IF CompilerVersion < 23}PByte( PAnsiChar{$ELSE}( PByte{$IFEND}( Data ) + CodeOffset ), Size-CodeOffset, 0{Address}, DecoderSettings );
  {$IF CompilerVersion < 23}{$RANGECHECKS ON}{$IFEND} // RangeCheck might cause Internal-Error C1118

  Inc( InstructionCount );
  {$IFDEF AssemblyTools}
  if ( Details <> nil ) then
    begin
    while fDecoder.CanDecode AND ( InstructionCount > 0 ) do
      begin
      fDecoder.Decode( Inst, Details^ );
      Dec( InstructionCount );
      end;
    end
  else
  {$ENDIF AssemblyTools}
    begin
    while fDecoder.CanDecode AND ( InstructionCount > 0 ) do
      begin
      fDecoder.Decode( Inst );
      Dec( InstructionCount );
      end;
    end;

  if ( InstructionCount = 0 ) then
    begin
    {$IF CompilerVersion < 23}{$RANGECHECKS OFF}{$IFEND} // RangeCheck might cause Internal-Error C1118
    Offset := Inst.RIP+Address;
    {$IF CompilerVersion < 23}{$RANGECHECKS ON}{$IFEND} // RangeCheck might cause Internal-Error C1118
    result := Inst.len;

    if ( InstructionBytes <> nil ) then
      Move( {$IF CompilerVersion < 23}PByte( PAnsiChar{$ELSE}( PByte{$IFEND}( Data ) + result )^, InstructionBytes^[ 0 ], Inst.len );
    if ( Instruction <> nil ) then
      Instruction^ := String( Formatter.FormatToString( Inst ) );
    end;
end;

(*
function TIced.FindPreviousInstructionCount( Code : TMemoryStream; CodeOffset : UInt64; Address : UInt64; var Offset : UInt64; InstructionCount : Word = 1; InstructionBytes : pByteInstruction = nil; Instruction : PString = nil; Detail : px86AssemblyDetail = nil ) : Cardinal;
begin
  result := 0;
  {$IF CompilerVersion < 23}{$RANGECHECKS OFF}{$IFEND} // RangeCheck might cause Internal-Error C1118
  Offset := 0;
  {$IF CompilerVersion < 23}{$RANGECHECKS ON}{$IFEND} // RangeCheck might cause Internal-Error C1118
  if ( InstructionBytes <> nil ) then
    FillChar( InstructionBytes^, SizeOf( InstructionBytes^ ), 0 );
  if ( Instruction <> nil ) then
    Instruction^ := '';
  if ( Detail <> nil ) then
    FillChar( Detail^, SizeOf( Detail^ ), 0 );
  if ( self = nil ) then
    Exit;
  if ( Code = nil ) then
    Exit;
  result := FindPreviousInstructionCount( {$IF CompilerVersion < 23}PByte( PAnsiChar{$ELSE}( PByte{$IFEND}( Code.Memory )+Code.Position ), Code.Size-Code.Position, CodeOffset, Address, Offset, InstructionCount, InstructionBytes, Instruction, Detail );
end;

function TIced.FindPreviousInstructionCount( Code : PByte; Size : Cardinal; CodeOffset : UInt64; Address : UInt64; var Offset : UInt64; InstructionCount : Word = 1; InstructionBytes : pByteInstruction = nil; Instruction : PString = nil; Detail : px86AssemblyDetail = nil ) : Cardinal;
var
  i, j     : Integer;
  StrL_Out : TStringList;
  CurInst  : String;
  tDetails : tx86AssemblyDetails;
  B        : PByte;
begin
  result := 0;
  {$IF CompilerVersion < 23}{$RANGECHECKS OFF}{$IFEND} // RangeCheck might cause Internal-Error C1118
  Offset := 0;
  {$IF CompilerVersion < 23}{$RANGECHECKS ON}{$IFEND} // RangeCheck might cause Internal-Error C1118
  if ( InstructionBytes <> nil ) then
    FillChar( InstructionBytes^, SizeOf( InstructionBytes^ ), 0 );
  if ( Instruction <> nil ) then
    Instruction^ := '';
  if ( Detail <> nil ) then
    FillChar( Detail^, SizeOf( Detail^ ), 0 );
  if ( self = nil ) then
    Exit;
  if ( Code = nil ) then
    Exit;
  if ( InstructionCount = 0 ) then
    Exit;
  if ( Size < ( InstructionCount+1 )*MAX_INSTRUCTION_LEN_ ) then
    Exit;

  StrL_Out := TStringList.Create;
  {$IF CompilerVersion < 23}{$RANGECHECKS OFF}{$IFEND} // RangeCheck might cause Internal-Error C1118
  if ( Execute( Pointer( {$IF CompilerVersion < 23}PAnsiChar{$ELSE}PByte{$IFEND}( Code ) + CodeOffset ), MAX_INSTRUCTION_LEN_, StrL_Out, Address, nil, nil ) > 0 ) then
  {$IF CompilerVersion < 23}{$RANGECHECKS ON}{$IFEND} // RangeCheck might cause Internal-Error C1118
    CurInst := StrL_Out[ 0 ]
  else
    begin
    StrL_Out.Free;
    Exit;
    end;

  Code := Pointer( {$IF CompilerVersion < 23}PAnsiChar{$ELSE}PByte{$IFEND}( Code ) + CodeOffset - InstructionCount*MAX_INSTRUCTION_LEN_ );
  for i := 0 to MAX_INSTRUCTION_LEN_-1 do
    begin
    if ( result > 0 ) then
      Break;
    StrL_Out.Clear;
    FillChar( tDetails, SizeOf( tDetails ), 0 );

    {$IF CompilerVersion < 23}{$RANGECHECKS OFF}{$IFEND} // RangeCheck might cause Internal-Error C1118
    if ( Execute( Code, ( InstructionCount+1 )*MAX_INSTRUCTION_LEN_, StrL_Out, Address-( Cardinal( InstructionCount*MAX_INSTRUCTION_LEN_-i ) ), nil, @tDetails ) > 0 ) then
    {$IF CompilerVersion < 23}{$RANGECHECKS ON}{$IFEND} // RangeCheck might cause Internal-Error C1118
      begin
      for j := InstructionCount to StrL_Out.Count-1 do
        begin
        if ( tDetails.Items[ j ].Offset = Address ) AND ( StrL_Out[ j ] = CurInst ) AND ( StrL_Out[ j-InstructionCount ] <> 'INVALID' ) then
          begin
          {$IF CompilerVersion < 23}{$RANGECHECKS OFF}{$IFEND} // RangeCheck might cause Internal-Error C1118
          Offset := tDetails.Items[ j-InstructionCount ].Offset;
          result := tDetails.Items[ j ].Offset;
          result := result-Offset;
          {$IF CompilerVersion < 23}{$RANGECHECKS ON}{$IFEND} // RangeCheck might cause Internal-Error C1118
          if ( InstructionBytes <> nil ) then
            begin
            B := Code;
            {$IF CompilerVersion < 23}{$RANGECHECKS OFF}{$IFEND} // RangeCheck might cause Internal-Error C1118
            Inc( B, Offset - Address );
            {$IF CompilerVersion < 23}{$RANGECHECKS ON}{$IFEND} // RangeCheck might cause Internal-Error C1118
            CopyMemory( InstructionBytes, B, tDetails.Items[ j-InstructionCount ].Size );
            end;
          if ( Instruction <> nil ) then
            Instruction^ := StrL_Out[ j-InstructionCount ];
          if ( Detail <> nil ) then
            CopyMemory( Detail, @tDetails.Items[ j-InstructionCount ], SizeOf( tx86AssemblyDetail ) );
          Break;
          end;
        end;
      end;
    Inc( Code );
    end;
  SetLength( tDetails.Items, 0 );
  SetLength( tDetails.JumpTargets, 0 );
  StrL_Out.free;
end;

function TIced.FindPreviousInstruction( Code : TMemoryStream; CodeOffset : UInt64; Address : UInt64; var Offset : UInt64; DesiredInstructionOffset : UInt64; InstructionBytes : pByteInstruction = nil; Instruction : PString = nil; Detail : pIcedDetail = nil ) : Cardinal;
begin
  result := 0;
  {$IF CompilerVersion < 23}{$RANGECHECKS OFF}{$IFEND} // RangeCheck might cause Internal-Error C1118
  Offset := 0;
  {$IF CompilerVersion < 23}{$RANGECHECKS ON}{$IFEND} // RangeCheck might cause Internal-Error C1118
  if ( InstructionBytes <> nil ) then
    FillChar( InstructionBytes^, SizeOf( InstructionBytes^ ), 0 );
  if ( Instruction <> nil ) then
    Instruction^ := '';
  if ( Detail <> nil ) then
    FillChar( Detail^, SizeOf( Detail^ ), 0 );
  if ( self = nil ) then
    Exit;
  if ( Code = nil ) then
    Exit;
  result := FindPreviousInstruction( {$IF CompilerVersion < 23}PByte( PAnsiChar{$ELSE}( PByte{$IFEND}( Code.Memory )+Code.Position ), Code.Size-Code.Position, CodeOffset, Address, Offset, DesiredInstructionOffset, InstructionBytes, Instruction, Detail );
end;

function TIced.FindPreviousInstruction( Code : PByte; Size : Cardinal; CodeOffset : UInt64; Address : UInt64; var Offset : UInt64; DesiredInstructionOffset : UInt64; InstructionBytes : pByteInstruction = nil; Instruction : PString = nil; Detail : pIcedDetail = nil ) : Cardinal;
var
  i, j, k  : Integer;
  StrL_Out : TStringList;
  CurInst  : String;
  tDetails : TIcedDetails;
  bOK      : Boolean;
begin
  result := 0;
  {$IF CompilerVersion < 23}{$RANGECHECKS OFF}{$IFEND} // RangeCheck might cause Internal-Error C1118
  Offset := 0;
  {$IF CompilerVersion < 23}{$RANGECHECKS ON}{$IFEND} // RangeCheck might cause Internal-Error C1118
  if ( InstructionBytes <> nil ) then
    FillChar( InstructionBytes^, SizeOf( InstructionBytes^ ), 0 );
  if ( Instruction <> nil ) then
    Instruction^ := '';
  if ( Detail <> nil ) then
    FillChar( Detail^, SizeOf( Detail^ ), 0 );
  if ( self = nil ) then
    Exit;
  if ( Code = nil ) then
    Exit;
//  if ( Size < ( InstructionCount+1 )*MAX_INSTRUCTION_LEN_ ) then
//    Exit;
  {$IF CompilerVersion < 23}{$RANGECHECKS OFF}{$IFEND} // RangeCheck might cause Internal-Error C1118
  if ( CodeOffset < MAX_INSTRUCTION_LEN_ ) then
    Exit;
  if ( DesiredInstructionOffset >= Address ) OR ( DesiredInstructionOffset-MAX_INSTRUCTION_LEN_ < Address-CodeOffset ) then
    Exit;
  {$IF CompilerVersion < 23}{$RANGECHECKS ON}{$IFEND} // RangeCheck might cause Internal-Error C1118

  StrL_Out := TStringList.Create;
  {$IF CompilerVersion < 23}{$RANGECHECKS OFF}{$IFEND} // RangeCheck might cause Internal-Error C1118
  if ( Execute( Pointer( {$IF CompilerVersion < 23}PAnsiChar{$ELSE}PByte{$IFEND}( Code ) + CodeOffset ), MAX_INSTRUCTION_LEN_, StrL_Out, Address, nil, nil ) > 0 ) then
  {$IF CompilerVersion < 23}{$RANGECHECKS ON}{$IFEND} // RangeCheck might cause Internal-Error C1118
    CurInst := StrL_Out[ 0 ]
  else
    begin
    StrL_Out.Free;
    Exit;
    end;

  k := DesiredInstructionOffset-( Address-CodeOffset )-MAX_INSTRUCTION_LEN_;
  for i := k to k+MAX_INSTRUCTION_LEN_-1 do
    begin
    StrL_Out.Clear;
    SetLength( tDetails.Items, 0 );
    SetLength( tDetails.JumpTargets, 0 );
    FillChar( tDetails, SizeOf( tDetails ), 0 );
    {$IF CompilerVersion < 23}{$RANGECHECKS OFF}{$IFEND} // RangeCheck might cause Internal-Error C1118
    if ( Execute( Pointer( {$IF CompilerVersion < 23}PAnsiChar{$ELSE}PByte{$IFEND}( Code ) + i ), Address-DesiredInstructionOffset+MAX_INSTRUCTION_LEN_, StrL_Out, Address-CodeOffset+Cardinal( i ), nil, @tDetails ) > 0 ) then
    {$IF CompilerVersion < 23}{$RANGECHECKS ON}{$IFEND} // RangeCheck might cause Internal-Error C1118
      begin
      bOK := False;
      k := StrL_Out.Count;
      for j := 0 to StrL_Out.Count-1 do
        begin
        if ( j > 0 ) AND ( StrL_Out[ j ] = 'INVALID' ) then
          begin
          bOK := False;
          Break;
          end
        else if ( j > 0 ) AND ( tDetails.Items[ j ].Offset = Address ) AND ( StrL_Out[ j ] = CurInst ) AND ( StrL_Out[ j-1 ] <> 'INVALID' ) then
          begin
          bOK := True;
          k := j;
          Break;
          end;
        end;
      if NOT bOK then
        Continue;

      for j := k downTo 1 do
        begin
        if ( DesiredInstructionOffset > tDetails.Items[ j ].Offset ) then
          Continue;
        {$IF CompilerVersion < 23}{$RANGECHECKS OFF}{$IFEND} // RangeCheck might cause Internal-Error C1118
        if ( Offset = 0 ) OR ( ABS( tDetails.Items[ j ].Offset - DesiredInstructionOffset ) < ABS( Offset-DesiredInstructionOffset ) ) then
          begin
          Offset := tDetails.Items[ j ].Offset;
          result := Address-Offset;
          k      := j;
          end;
        {$IF CompilerVersion < 23}{$RANGECHECKS ON}{$IFEND} // RangeCheck might cause Internal-Error C1118
        end;

      if ( InstructionBytes <> nil ) then
        CopyMemory( InstructionBytes, Pointer( {$IF CompilerVersion < 23}PAnsiChar{$ELSE}PByte{$IFEND}( Code ) + CodeOffset - result ), tDetails.Items[ k ].Size );

      if ( Instruction <> nil ) then
        Instruction^ := StrL_Out[ k ];
      if ( Detail <> nil ) then
        CopyMemory( Detail, @tDetails.Items[ k ], SizeOf( TIcedDetail ) );
      Break;
      end;
    end;

  SetLength( tDetails.Items, 0 );
  SetLength( tDetails.JumpTargets, 0 );
  StrL_Out.free;
end;
*)

// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
{$IFDEF TEST_FUNCTIONS}
{
  i9, Delphi 7
    193mb without Formatting in: 1.0901749 seconds
    193mb with Masm in: 34.4844735 seconds (5.596mb/s)
    193mb with Fast in: 15.1733084 seconds (12.719mb/s)

  i9, Delphi 10.3
    193mb without Formatting in: 1.0695132 seconds
    193mb with Masm in: 34.8067761 seconds (5.544mb/s)
    193mb with Fast in: 14.9694206 seconds (12.892mb/s)
}
function Benchmark( Data : PByte; Size : Cardinal; Bitness : TIcedBitness; AFormat : Boolean = False ) : Double;
var
  Instruction : TInstruction;
  f, t1, t2   : Int64;
  tOutput     : Array [ 0..255 ] of AnsiChar;
begin
  Iced.Decoder.Bitness := Bitness;
  Iced.Decoder.SetData( Data, Size );
  
  if AFormat then
    begin
    Iced.Formatter.FormatterType := ftFast;
    Iced.Formatter.VerifyOutputHasEnoughBytesLeft := False;    
    QueryPerformanceCounter( t1 );
    while Iced.Decoder.CanDecode do
      begin
      Iced.Decoder.Decode( Instruction );
      Iced.Formatter.Format( Instruction, tOutput, Length( tOutput ) );
      end;
    QueryPerformanceCounter( t2 );
    end
  else
    begin
    QueryPerformanceCounter( t1 );
    while Iced.Decoder.CanDecode do
      Iced.Decoder.Decode( Instruction );
    QueryPerformanceCounter( t2 );
    end;

  QueryPerformanceFrequency( f );
  result := ( t2-t1 ) / f; // * 1000;
end;

function Benchmark( FileName : String; Bitness : TIcedBitness; AFormat : Boolean = False ) : Double;
var
  ms : TMemoryStream;
begin
  if NOT FileExists( FileName ) then
    begin
    result := 0;
    Exit;
    end;

  ms := TMemoryStream.Create;
  ms.LoadFromFile( FileName );
  Result := Benchmark( ms.Memory, ms.Size, Bitness, AFormat );
  ms.Free;
end;

function Test( StrL : TStringList ) : Boolean;
begin
  if ( StrL <> nil ) then
    begin
    StrL.Clear;
    StrL.Add( 'Disassemble' );
    StrL.Add( '---' );
    end;
  result := Test_Decode(
                  @EXAMPLE_CODE, Length( EXAMPLE_CODE ), EXAMPLE_RIP,

                  StrL,
                  ftMasm{FormatterType},

                  nil{SymbolResolverCallback},
//                  SymbolResolverCallback{SymbolResolverCallback},
                  nil{FormatterOptionsProviderCallback},
//                  FormatterOptionsProviderCallback{FormatterOptionsProviderCallback},
                  nil{FormatterOutputCallback},
//                  FormatterOutputCallback{FormatterOutputCallback},

//                  False{Assembly},
                  True{Assembly},

                  False{DecodeInfo},
//                  True{DecodeInfo},

                  False{Info}
//                  True{Info}
                );

  if ( StrL <> nil ) then
    begin
    StrL.Add( '' );
    StrL.Add( 'Re-Assemble' );
    StrL.Add( '---' );
    end;
  result := result AND Test_ReEncode(
                  @EXAMPLE_CODE, Length( EXAMPLE_CODE ), EXAMPLE_RIP,

//                  True{LocalBuffer},
//                  False{LocalBuffer},

//                  True{BlockEncode},
                  False{BlockEncode},
                  StrL
                );

  if ( StrL <> nil ) then
    begin
    StrL.Add( '' );
    StrL.Add( 'Assemble' );
    StrL.Add( '---' );
    end;
  Test_Assemble( StrL );
end;

const
  HEXBYTES_COLUMN_BYTE_LENGTH = 16; // 30;

function Test_Decode( Data : PByte; Size : Cardinal; ARIP : UInt64; AOutput : TStringList; FormatterType : TIcedFormatterType = ftMasm;
                      SymbolResolver : TSymbolResolverCallback = nil;
                      FormatterOptionsProviderCallback : TFormatterOptionsProviderCallback = nil;
                      FormatterOutputCallback : TFormatterOutputCallback = nil;
                      Assembly : Boolean = True; DecodeInfos : Boolean = True; Infos : Boolean = True ) : Boolean;
var
  Instruction   : TInstruction;
  Offsets       : TConstantOffsets;
  FPU_Info      : TFpuStackIncrementInfo;
  CPUIDFeatures : TCPUIDFeaturesArray;
  OPKinds       : TOPKindsArray;
  Encoding      : TEncodingKind;
  Mnemonic      : TMnemonic;
  FlowControl   : TFlowControl;
  OPKind        : TOPCodeOperandKind;
//  OPKinds_      : TOPCodeOperandKindArray;
  Info          : TInstructionInfo;
  CC            : TConditionCode;
  RFlags        : TRFlags;
  tOutput       : Array [ 0..255 ] of AnsiChar;

  S             : String;
  C             : UInt64;
  i             : Integer;
begin
  result := False;
  if ( AOutput = nil ) then
    Exit;
  AOutput.Clear;

  {$IF CompilerVersion < 23}{$RANGECHECKS OFF}{$IFEND} // RangeCheck might cause Internal-Error C1118
  Iced.Decoder.SetData( Data, Size, ARIP, doNONE );
  {$IF CompilerVersion < 23}{$RANGECHECKS ON}{$IFEND} // RangeCheck might cause Internal-Error C1118
  Iced.Formatter.FormatterType   := FormatterType;
  Iced.Formatter.SymbolResolver  := SymbolResolver;
  Iced.Formatter.OptionsProvider := FormatterOptionsProviderCallback;
  Iced.Formatter.Callback        := FormatterOutputCallback;

  while Iced.Decoder.CanDecode do
    begin
    Iced.Decoder.Decode( Instruction );

    if Assembly then
      begin
      {$IF CompilerVersion < 23}{$RANGECHECKS OFF}{$IFEND} // RangeCheck might cause Internal-Error C1118
      C := Instruction.next_rip-Instruction.len;
      {$IF CompilerVersion < 23}{$RANGECHECKS ON}{$IFEND} // RangeCheck might cause Internal-Error C1118
      S := Format( '%.16x ', [ C ] );

      for i := 0 to Instruction.len-1 do
        begin
        S := S + Format( '%.2x', [ Data^ ] );
        Inc( Data );
        end;

      for i := 0 to HEXBYTES_COLUMN_BYTE_LENGTH-Instruction.len*2+1 do
        S := S + ' ';

      if ( @FormatterOutputCallback <> nil ) then
        Iced.Formatter.Format( Instruction );
      Iced.Formatter.Format( Instruction, tOutput, Length( tOutput ) );
      AOutput.Add( S + String( tOutput ) );
      end;

    if DecodeInfos then
      begin
      // Gets offsets in the instruction of the displacement and immediates and their sizes.
      // This can be useful if there are relocations in the binary. The encoder has a similar
      // method. This method must be called after decode() and you must pass in the last
      // instruction decode() returned.
      Iced.Decoder.GetConstantOffsets( Instruction, Offsets );

//      FillChar( OPKinds_, SizeOf( OPKinds_ ), 0 );
//      Instruction.OpCodeInfo_OPKinds( OPKinds_ );

      if Infos then
        begin
        AOutput.Add( '    OpCode: ' + Instruction.OpCodeInfo_OpCodeString );
        AOutput.Add( '    Instruction: ' + Instruction.OpCodeInfo_InstructionString );

        Encoding := Instruction.Encoding;
        AOutput.Add( '    Encoding: ' + Encoding.AsString );

        Mnemonic := Instruction.Mnemonic;
        AOutput.Add( '    Mnemonic: ' + Mnemonic.AsString );
        AOutput.Add( '    Code: ' + Instruction.code.AsString );
        end;

      CPUIDFeatures := Instruction.CPUIDFeatures;
      if Infos then
        begin
        S := '';
        for i := 0 to CPUIDFeatures.Count-1 do
          begin
          if ( i > 0 ) then
            S := S + 'AND ' + CPUIDFeatures.Entries[ i ].AsString
          else
            S := CPUIDFeatures.Entries[ i ].AsString;
          end;
        AOutput.Add( '    CpuidFeature: ' + S );

        FlowControl := Instruction.FlowControl;
        AOutput.Add( '    FlowControl: ' + FlowControl.AsString );
        end;

      FPU_Info := Instruction.FPU_StackIncrementInfo;
      if Infos then
        begin
        if FPU_Info.writes_top then
          begin
          if ( FPU_Info.increment = 0 ) then
            AOutput.Add( '    FPU TOP: the instruction overwrites TOP' )
          else
            AOutput.Add( Format( '    FPU TOP inc: %d', [ FPU_Info.increment ] ) );

          if FPU_Info.conditional then
            AOutput.Add( '    FPU TOP cond write: true' )
          else
            AOutput.Add( '    FPU TOP cond write: false' );
          end;

        if ( Offsets.displacement_size <> 0 ) then
          AOutput.Add( Format( '    Displacement offset = %d, size = %d', [ Offsets.displacement_offset, Offsets.displacement_size ] ) );
        if ( Offsets.immediate_size <> 0 ) then
          AOutput.Add( Format( '    Immediate offset = %d, size = %d', [ Offsets.immediate_offset, Offsets.immediate_size ] ) );
        if ( Offsets.immediate_size2 <> 0 ) then
          AOutput.Add( Format( '    Immediate #2 offset = %d, size = %d', [ Offsets.immediate_offset2, Offsets.immediate_size2 ] ) );

        if Instruction.IsStackInstruction then
          AOutput.Add( Format( '    SP Increment: %d', [ Instruction.StackPointerIncrement ] ) );
        end;

      CC := Instruction.ConditionCode;
      RFlags := Instruction.RFlags;
      if Infos then
        begin
        if ( CC.ConditionCode <> cc_None ) then
          AOutput.Add( Format( '    Condition code: %s', [ CC.AsString ] ) );

        if ( NOT RFlags.Read.IsNone ) OR ( NOT RFlags.Written.IsNone ) OR ( NOT RFlags.Cleared.IsNone ) OR ( NOT RFlags.Set_.IsNone ) OR ( NOT RFlags.Undefined.IsNone ) OR ( NOT RFlags.Modified.IsNone ) then
          begin
          if ( NOT RFlags.Read.IsNone ) then
            AOutput.Add( '    RFLAGS Read: ' + RFlags.Read.AsString );
          if ( NOT RFlags.Written.IsNone ) then
            AOutput.Add( '    RFLAGS Written: ' + RFlags.Written.AsString );
          if ( NOT RFlags.Cleared.IsNone ) then
            AOutput.Add( '    RFLAGS Cleared: ' + RFlags.Cleared.AsString );
          if ( NOT RFlags.Set_.IsNone ) then
            AOutput.Add( '    RFLAGS Set: ' + RFlags.Set_.AsString );
          if ( NOT RFlags.Undefined.IsNone ) then
            AOutput.Add( '    RFLAGS Undefined: ' + RFlags.Undefined.AsString );
          if ( NOT RFlags.Modified.IsNone ) then
            AOutput.Add( '    RFLAGS Modified: ' + RFlags.Modified.AsString );
          end;
        end;

      FillChar( OPKinds, SizeOf( OPKinds ), 0 );
      Instruction.OPKinds( OPKinds );
      if Infos then
        begin
        for i := 0 to OPKinds.Count-1 do
          begin
          if ( OPKinds.Entries[ i ].OpKind = okMemory ) then
            begin
            {$IF CompilerVersion < 23}{$RANGECHECKS OFF}{$IFEND} // RangeCheck might cause Internal-Error C1118
            C := Instruction.MemorySize;
            if ( C <> 0 ) then
            {$IF CompilerVersion < 23}{$RANGECHECKS ON}{$IFEND} // RangeCheck might cause Internal-Error C1118
              AOutput.Add( '    Memory Size: ' + IntToStr( C ) );
            break;
            end;
          end;
        end;

      Iced.InfoFactory.Info( Instruction, Info );
      if Infos then
        begin
        for i := 0 to Instruction.OPCount-1 do
          AOutput.Add( Format( '    Op%dAccess: %s', [ i, Info.op_accesses[ i ].AsString ] ) );

        for i := 0 to Instruction.OpCodeInfo_OPCount-1 do
          begin
          OPKind := Instruction.OpCodeInfo_OPKind( i );
          AOutput.Add( Format( '    Op%d: %s', [ i, OPKind.AsString ] ) );
          end;

        for i := 0 to Info.used_registers.Count-1 do
          AOutput.Add( Format( '    Used reg: %s:%s', [ Info.used_registers.Entries[ i ].register_.AsString, Info.used_registers.Entries[ i ].access.AsString ] ) );

        for i := 0 to Info.used_memory_locations.Count-1 do
          AOutput.Add( Format( '    Used mem: %s:%s+0x%.2x:%s:%d:%s:%s:%s:%d', [
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
      end;
    end;
//  Iced.Decoder.SetIP( Decoder, EXAMPLE_RIP );
//  Iced.Decoder.SetPosition( Decoder, 0 );

  result := True;
end;

function Test_ReEncode( Data : PByte; Size : Cardinal; ARIP : UInt64; {LocalBuffer : Boolean = False;} BlockEncode : Boolean = False; AOutput : TStringList = nil ) : Boolean;
var
  Instruction : TInstruction;
  tOutput     : Array [ 0..255 ] of AnsiChar;
  Buffer      : Array of Byte;
  Block       : TInstructionArray;
  Results     : TBlockEncoderResult;
  S           : String;
  C           : UInt64;
  i           : Integer;
begin
//  if ( Output <> nil ) then
//    AOutput.Clear;

  {$IF CompilerVersion < 23}{$RANGECHECKS OFF}{$IFEND} // RangeCheck might cause Internal-Error C1118
  Iced.Decoder.SetData( Data, Size, ARIP, doNONE );
  {$IF CompilerVersion < 23}{$RANGECHECKS ON}{$IFEND} // RangeCheck might cause Internal-Error C1118
//  if LocalBuffer AND NOT BlockEncode then
//    begin
//    SetLength( Buffer, Size );
//    Iced.Encoder.SetBuffer( @Buffer[ 0 ], Size );
//    end;

  SetLength( Block, 0 );
  while Iced.Decoder.CanDecode do
    begin
    if BlockEncode then
      begin
      SetLength( Block, Length( Block )+1 );
      Iced.Decoder.Decode( Block[ High( Block ) ] );
      end
    else
      begin
      Iced.Decoder.Decode( Instruction );
      Iced.Encoder.Encode( Instruction );
      end;
    end;

  if BlockEncode then
    begin
    Iced.BlockEncoder.Encode( ARIP, Block, Results );
    SetLength( Block, 0 );
    result := CompareMem( Data, Results.code_buffer, Size );
    end
  else
    begin
//    if NOT LocalBuffer then
//      begin
      SetLength( Buffer, Size );
      Iced.Encoder.GetBuffer( @Buffer[ 0 ], Size );
//      end;
    result := CompareMem( Data, @Buffer[ 0 ], Size );

//    if LocalBuffer then
//      Iced.Encoder.SetBuffer( nil, 0 );
    end;

  if ( AOutput <> nil ) then
    begin
    if BlockEncode then
      Iced.Decoder.SetData( PByte( Results.code_buffer ), Results.code_buffer_len, Results.RIP, doNONE )
    else
      Iced.Decoder.SetData( @Buffer[ 0 ], Size, ARIP, doNONE );

    while Iced.Decoder.CanDecode do
      begin
      Iced.Decoder.Decode( Instruction );

      {$IF CompilerVersion < 23}{$RANGECHECKS OFF}{$IFEND} // RangeCheck might cause Internal-Error C1118
      C := Instruction.next_rip-Instruction.len;
      {$IF CompilerVersion < 23}{$RANGECHECKS ON}{$IFEND} // RangeCheck might cause Internal-Error C1118
      S := Format( '%.16x ', [ C ] );
      {$IF CompilerVersion < 23}{$RANGECHECKS OFF}{$IFEND} // RangeCheck might cause Internal-Error C1118
      C := C-ARIP;
      {$IF CompilerVersion < 23}{$RANGECHECKS ON}{$IFEND} // RangeCheck might cause Internal-Error C1118

      for i := 0 to Instruction.len-1 do
        begin
        S := S + Format( '%.2x', [ Data^ ] );
        Inc( Data );
        end;

      for i := 0 to HEXBYTES_COLUMN_BYTE_LENGTH-Instruction.len*2+1 do
        S := S + ' ';

      Iced.Formatter.Format( Instruction, tOutput, Length( tOutput ) );
      AOutput.Add( S + string( tOutput ) );
      end;
    end;

  SetLength( Buffer, 0 );
end;

procedure Test_Assemble( AOutput : TStringList; ARIP : UInt64 = UInt64( $00001248FC840000 ) );
var
  LabelID : UInt64;

  function CreateLabel : UInt64;
  begin
    {$IF CompilerVersion < 23}{$RANGECHECKS OFF}{$IFEND} // RangeCheck might cause Internal-Error C1118
    result := LabelID;
    Inc( LabelID );
    {$IF CompilerVersion < 23}{$RANGECHECKS ON}{$IFEND} // RangeCheck might cause Internal-Error C1118
  end;

  function AddLabel( ID : UInt64; Instruction : TInstruction ) : TInstruction;
  begin
    Instruction.SetRIP( ID );
    result := Instruction;
  end;

const
  Bitness  : TIcedBitness = bt64;
  raw_data : Array [ 0..3 ] of Byte = ( $12, $34, $56, $78 );
var
  Instructions : TInstructionArray;
  MemoryOperand: TMemoryOperand;
  Results      : TBlockEncoderResult;
  label1       : UInt64;
  data1        : UInt64;
  i            : Integer;
  Instruction  : TInstruction;
  tOutput      : Array [ 0..255 ] of AnsiChar;
  S            : String;
  Data         : PByte;
  C            : Cardinal;
begin
//  if ( AOutput <> nil ) then
//    AOutput.Clear;

  // All created instructions get an IP of 0. The label id is just an IP.
  // The branch instruction's *target* IP should be equal to the IP of the
  // target instruction.
  {$IF CompilerVersion < 23}{$RANGECHECKS OFF}{$IFEND} // RangeCheck might cause Internal-Error C1118
  LabelID := 1;

  label1 := CreateLabel;
  {$IF CompilerVersion < 23}{$RANGECHECKS ON}{$IFEND} // RangeCheck might cause Internal-Error C1118

  SetLength( Instructions, 18 );
  Instructions[ 0 ] := {T}Instruction.with1( Push_r64, RBP );
  Instructions[ 1 ] := {T}Instruction.with1( Push_r64, RDI );
  Instructions[ 2 ] := {T}Instruction.with1( Push_r64, RSI );
  Instructions[ 3 ] := {T}Instruction.with2( Sub_rm64_imm32, RSP, Cardinal( $50 ) );
  Instructions[ 4 ] := {T}Instruction.with_( VEX_Vzeroupper );
  Instructions[ 5 ] := {T}Instruction.with2(
      Lea_r64_m,
      RBP,
      {T}MemoryOperand.with_base_displ( RSP, $60 )
  );
  Instructions[ 6 ] := {T}Instruction.with2( Mov_r64_rm64, RSI, RCX );
  Instructions[ 7 ] := {T}Instruction.with2(
      Lea_r64_m,
      RDI,
      {T}MemoryOperand.with_base_displ( RBP, -$38 )
  );
  Instructions[ 8 ] := {T}Instruction.with2( Mov_r32_imm32, ECX, Cardinal( $0A ) );
  Instructions[ 9 ] := {T}Instruction.with2( Xor_r32_rm32, EAX, EAX );
  Instructions[ 10 ] := {T}Instruction.with_rep_stosd( Cardinal( Bitness ) );
  Instructions[ 11 ] := {T}Instruction.with2( Cmp_rm64_imm32, RSI, Cardinal( $12345678 ) );
  // Create a branch instruction that references label1
  Instructions[ 12 ] := {T}Instruction.with_branch( Jne_rel32_64, label1 );
  Instructions[ 13 ] := {T}Instruction.with_( Nopd );

  // Add the instruction that is the target of the branch
  Instructions[ 14 ] := AddLabel( label1, {T}Instruction.with2( Xor_r32_rm32, R15D, R15D ) );

  // Create an instruction that accesses some data using an RIP relative memory operand
  {$IF CompilerVersion < 23}{$RANGECHECKS OFF}{$IFEND} // RangeCheck might cause Internal-Error C1118
  data1 := CreateLabel;
  Instructions[ 15 ] := {T}Instruction.with2(
        Lea_r64_m,
        R14,
        {T}MemoryOperand.with_base_displ( RIP, Int64( data1 ) ) );
  {$IF CompilerVersion < 23}{$RANGECHECKS ON}{$IFEND} // RangeCheck might cause Internal-Error C1118
  Instructions[ 16 ] := {T}Instruction.with_( Nopd );
  Instructions[ 17 ] := AddLabel( data1, {T}Instruction.with_declare_byte( raw_data ) );
  Instructions[ 18 ] := {T}Instruction.with_declare_byte( raw_data );

  // Use BlockEncoder to encode a block of instructions. This block can contain any
  // number of branches and any number of instructions. It does support encoding more
  // than one block but it's rarely needed.
  // It uses Encoder to encode all instructions.
  // If the target of a branch is too far away, it can fix it to use a longer branch.
  // This can be disabled by enabling some BlockEncoderOptions flags.
  Iced.BlockEncoder.Encode( ARIP, Instructions, Results );

  // Now disassemble the encoded instructions. Note that the 'jmp near'
  // instruction was turned into a 'jmp short' instruction because we
  // didn't disable branch optimizations.
  Iced.Formatter.FormatterType := ftGas;
  Iced.Formatter.FirstOperandCharIndex := 8;

  if ( AOutput <> nil ) then
    begin
    Iced.Decoder.SetData( PByte( Results.code_buffer ), Results.code_buffer_len-NativeUInt( Length( raw_data ) ), Results.RIP, doNONE );
    Data := PByte( Results.code_buffer );

    while Iced.Decoder.CanDecode do
      begin
      Iced.Decoder.Decode( Instruction );

      C := Instruction.next_rip-Instruction.len;
      S := Format( '%.16x ', [ C ] );
      C := C-ARIP;

      for i := C to C+Instruction.len-1 do
        begin
        S := S + Format( '%.2x', [ Data^ ] );
        Inc( Data );
        end;

      for i := 0 to HEXBYTES_COLUMN_BYTE_LENGTH-Instruction.len*2+1 do
        S := S + ' ';

      Iced.Formatter.Format( Instruction, tOutput, Length( tOutput ) );
      AOutput.Add( S + String( tOutput ) );
      end;

    Instruction := {T}Instruction.with_declare_byte( raw_data );
    Iced.Formatter.Format( Instruction, tOutput, Length( tOutput ) );
    AOutput.Add( S + String( tOutput ) );
    end;

  Iced.BlockEncoder.Clear;
end;
{$ENDIF TEST_FUNCTIONS}

// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

initialization
  Iced := TIced.Create;

finalization
  Iced.Free;

end.
