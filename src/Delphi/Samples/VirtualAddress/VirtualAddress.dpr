program VirtualAddress;

{$APPTYPE CONSOLE}

uses
  SysUtils,

  uIced.Types in '..\..\uIced.Types.pas',
  uIced.Imports in '..\..\uIced.Imports.pas',
  uIced in '..\..\uIced.pas';

function VirtualAddressResolverCallback( Register: TRegister; Index : NativeUInt; Size : NativeUInt; var Address : UInt64; UserData : Pointer ) : boolean; cdecl;
begin
  result := True;
  {$IF CompilerVersion < 23}{$RANGECHECKS OFF}{$IFEND} // RangeCheck might cause Internal-Error C1118
  case Register of
    // The base address of ES, CS, SS and DS is always 0 in 64-bit mode
    ES, CS, SS, DS: Address := 0;

    RDI : Address := $10000000;
    R12 : Address := $000400000000;

//    AL    : Address := ;
//    CL    : Address := ;
//    DL    : Address := ;
//    BL    : Address := ;
//    AH    : Address := ;
//    CH    : Address := ;
//    DH    : Address := ;
//    BH    : Address := ;
//    SPL   : Address := ;
//    BPL   : Address := ;
//    SIL   : Address := ;
//    DIL   : Address := ;
//    R8L   : Address := ;
//    R9L   : Address := ;
//    R10L  : Address := ;
//    R11L  : Address := ;
//    R12L  : Address := ;
//    R13L  : Address := ;
//    R14L  : Address := ;
//    R15L  : Address := ;
//    AX    : Address := ;
//    CX    : Address := ;
//    DX    : Address := ;
//    BX    : Address := ;
//    SP    : Address := ;
//    BP    : Address := ;
//    SI    : Address := ;
//    DI    : Address := ;
//    R8W   : Address := ;
//    R9W   : Address := ;
//    R10W  : Address := ;
//    R11W  : Address := ;
//    R12W  : Address := ;
//    R13W  : Address := ;
//    R14W  : Address := ;
//    R15W  : Address := ;
//    EAX   : Address := ;
//    ECX   : Address := ;
//    EDX   : Address := ;
//    EBX   : Address := ;
//    ESP   : Address := ;
//    EBP   : Address := ;
//    ESI   : Address := ;
//    EDI   : Address := ;
//    R8D   : Address := ;
//    R9D   : Address := ;
//    R10D  : Address := ;
//    R11D  : Address := ;
//    R12D  : Address := ;
//    R13D  : Address := ;
//    R14D  : Address := ;
//    R15D  : Address := ;
//    RAX   : Address := ;
//    RCX   : Address := ;
//    RDX   : Address := ;
//    RBX   : Address := ;
//    RSP   : Address := ;
//    RBP   : Address := ;
//    RSI   : Address := ;
//    RDI   : Address := ;
//    R8    : Address := ;
//    R9    : Address := ;
//    R10   : Address := ;
//    R11   : Address := ;
//    R12   : Address := ;
//    R13   : Address := ;
//    R14   : Address := ;
//    R15   : Address := ;
//    EIP   : Address := ;
//    RIP   : Address := ;
//    FS    : Address := ;
//    GS    : Address := ;
//    XMM0  : Address := ;
//    XMM1  : Address := ;
//    XMM2  : Address := ;
//    XMM3  : Address := ;
//    XMM4  : Address := ;
//    XMM5  : Address := ;
//    XMM6  : Address := ;
//    XMM7  : Address := ;
//    XMM8  : Address := ;
//    XMM9  : Address := ;
//    XMM10 : Address := ;
//    XMM11 : Address := ;
//    XMM12 : Address := ;
//    XMM13 : Address := ;
//    XMM14 : Address := ;
//    XMM15 : Address := ;
//    XMM16 : Address := ;
//    XMM17 : Address := ;
//    XMM18 : Address := ;
//    XMM19 : Address := ;
//    XMM20 : Address := ;
//    XMM21 : Address := ;
//    XMM22 : Address := ;
//    XMM23 : Address := ;
//    XMM24 : Address := ;
//    XMM25 : Address := ;
//    XMM26 : Address := ;
//    XMM27 : Address := ;
//    XMM28 : Address := ;
//    XMM29 : Address := ;
//    XMM30 : Address := ;
//    XMM31 : Address := ;
//    YMM0  : Address := ;
//    YMM1  : Address := ;
//    YMM2  : Address := ;
//    YMM3  : Address := ;
//    YMM4  : Address := ;
//    YMM5  : Address := ;
//    YMM6  : Address := ;
//    YMM7  : Address := ;
//    YMM8  : Address := ;
//    YMM9  : Address := ;
//    YMM10 : Address := ;
//    YMM11 : Address := ;
//    YMM12 : Address := ;
//    YMM13 : Address := ;
//    YMM14 : Address := ;
//    YMM15 : Address := ;
//    YMM16 : Address := ;
//    YMM17 : Address := ;
//    YMM18 : Address := ;
//    YMM19 : Address := ;
//    YMM20 : Address := ;
//    YMM21 : Address := ;
//    YMM22 : Address := ;
//    YMM23 : Address := ;
//    YMM24 : Address := ;
//    YMM25 : Address := ;
//    YMM26 : Address := ;
//    YMM27 : Address := ;
//    YMM28 : Address := ;
//    YMM29 : Address := ;
//    YMM30 : Address := ;
//    YMM31 : Address := ;
//    ZMM0  : Address := ;
//    ZMM1  : Address := ;
//    ZMM2  : Address := ;
//    ZMM3  : Address := ;
//    ZMM4  : Address := ;
//    ZMM5  : Address := ;
//    ZMM6  : Address := ;
//    ZMM7  : Address := ;
//    ZMM8  : Address := ;
//    ZMM9  : Address := ;
//    ZMM10 : Address := ;
//    ZMM11 : Address := ;
//    ZMM12 : Address := ;
//    ZMM13 : Address := ;
//    ZMM14 : Address := ;
//    ZMM15 : Address := ;
//    ZMM16 : Address := ;
//    ZMM17 : Address := ;
//    ZMM18 : Address := ;
//    ZMM19 : Address := ;
//    ZMM20 : Address := ;
//    ZMM21 : Address := ;
//    ZMM22 : Address := ;
//    ZMM23 : Address := ;
//    ZMM24 : Address := ;
//    ZMM25 : Address := ;
//    ZMM26 : Address := ;
//    ZMM27 : Address := ;
//    ZMM28 : Address := ;
//    ZMM29 : Address := ;
//    ZMM30 : Address := ;
//    ZMM31 : Address := ;
//    K0    : Address := ;
//    K1    : Address := ;
//    K2    : Address := ;
//    K3    : Address := ;
//    K4    : Address := ;
//    K5    : Address := ;
//    K6    : Address := ;
//    K7    : Address := ;
//    BND0  : Address := ;
//    BND1  : Address := ;
//    BND2  : Address := ;
//    BND3  : Address := ;
//    CR0   : Address := ;
//    CR1   : Address := ;
//    CR2   : Address := ;
//    CR3   : Address := ;
//    CR4   : Address := ;
//    CR5   : Address := ;
//    CR6   : Address := ;
//    CR7   : Address := ;
//    CR8   : Address := ;
//    CR9   : Address := ;
//    CR10  : Address := ;
//    CR11  : Address := ;
//    CR12  : Address := ;
//    CR13  : Address := ;
//    CR14  : Address := ;
//    CR15  : Address := ;
//    DR0   : Address := ;
//    DR1   : Address := ;
//    DR2   : Address := ;
//    DR3   : Address := ;
//    DR4   : Address := ;
//    DR5   : Address := ;
//    DR6   : Address := ;
//    DR7   : Address := ;
//    DR8   : Address := ;
//    DR9   : Address := ;
//    DR10  : Address := ;
//    DR11  : Address := ;
//    DR12  : Address := ;
//    DR13  : Address := ;
//    DR14  : Address := ;
//    DR15  : Address := ;
//    ST0   : Address := ;
//    ST1   : Address := ;
//    ST2   : Address := ;
//    ST3   : Address := ;
//    ST4   : Address := ;
//    ST5   : Address := ;
//    ST6   : Address := ;
//    ST7   : Address := ;
//    MM0   : Address := ;
//    MM1   : Address := ;
//    MM2   : Address := ;
//    MM3   : Address := ;
//    MM4   : Address := ;
//    MM5   : Address := ;
//    MM6   : Address := ;
//    MM7   : Address := ;
//    TR0   : Address := ;
//    TR1   : Address := ;
//    TR2   : Address := ;
//    TR3   : Address := ;
//    TR4   : Address := ;
//    TR5   : Address := ;
//    TR6   : Address := ;
//    TR7   : Address := ;
//    TMM0  : Address := ;
//    TMM1  : Address := ;
//    TMM2  : Address := ;
//    TMM3  : Address := ;
//    TMM4  : Address := ;
//    TMM5  : Address := ;
//    TMM6  : Address := ;
//    TMM7  : Address := ;
  else
    result := False;
  end;
  {$IF CompilerVersion < 23}{$RANGECHECKS ON}{$IFEND} // RangeCheck might cause Internal-Error C1118
end;

const
  HEXBYTES_COLUMN_BYTE_LENGTH            = 5;
  EXAMPLE_CODE_BITNESS                   = bt64;
  EXAMPLE_CODE_RIP                       = 0;
  EXAMPLE_CODE : Array [ 0..7 ] of Byte  = ( $42, $01, $B4, $E7, $34, $12, $5A, $A5 );
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
  {$IF CompilerVersion < 23}{$RANGECHECKS OFF}{$IFEND} // RangeCheck might cause Internal-Error C1118
  Offset := Instruction.VirtualAddress( VirtualAddressResolverCallback );
  if ( Offset = EXAMPLE_OFFSET ) then
  {$IF CompilerVersion < 23}{$RANGECHECKS ON}{$IFEND} // RangeCheck might cause Internal-Error C1118
    WriteLn( 'OK: ' + Format( '%.16x', [ Offset ] ) )
  else
    WriteLn( 'Failed.' );

  WriteLn( 'Press enter to exit.' );
  ReadLn;

end.
