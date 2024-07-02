iced-x86-delphi
[![Latest version](https://img.shields.io/crates/v/iced-x86.svg)](https://crates.io/crates/iced-x86)
[![Documentation](https://docs.rs/iced-x86/badge.svg)](https://docs.rs/iced-x86)
![License](https://img.shields.io/crates/l/iced-x86.svg)

iced-x86 is a blazing fast and correct x86 (16/32/64-bit) instruction decoder, disassembler and assembler written in Rust.

- 👍 Supports all Intel and AMD instructions
- 👍 Correct: All instructions are tested and iced has been tested against other disassemblers/assemblers (xed, gas, objdump, masm, dumpbin, nasm, ndisasm) and fuzzed
- 👍 The formatter supports masm, nasm, gas (AT&T), Intel (XED) and there are many options to customize the output
- 👍 Blazing fast: Decodes >200 MB/s
- 👍 Small decoded instructions, only 40 bytes and the decoder doesn't allocate any memory
- 👍 The encoder can be used to re-encode decoded instructions at any address
- 👍 API to get instruction info, eg. read/written registers, memory and rflags bits; CPUID feature flag, control flow info, etc
- 👍 License: MIT

## Minimum supported version
iced-x86 supports `Delphi 7` or later.

## Usage

Add this to your Project:

```pascal
  uIced.Types,
  uIced.Imports,
  uIced,
```

## How-tos (See Samples-Folder)
## Samples are designed to work with older compilers and can be simplyfied on recent versions.
- [Disassemble (decode and format instructions)](#disassemble-decode-and-format-instructions)
- [Disassemble with a symbol resolver](#disassemble-with-a-symbol-resolver)
- [Disassemble with colorized text](#disassemble-with-colorized-text)
- [Move code in memory (eg. hook a function)](#move-code-in-memory-eg-hook-a-function)
- [Get instruction info, eg. read/written regs/mem, control flow info, etc](#get-instruction-info-eg-readwritten-regsmem-control-flow-info-etc)
- [Get the virtual address of a memory operand](#get-the-virtual-address-of-a-memory-operand)
- [Disassemble old/deprecated CPU instructions](#disassemble-olddeprecated-cpu-instructions)
- [Disassemble as fast as possible](#disassemble-as-fast-as-possible)
- [Create and encode instructions](#create-and-encode-instructions)


## Disassemble (decode and format instructions)

This example uses a [`Decoder`] and one of the [`Formatter`]s to decode and format the code,
eg. [`GasFormatter`], [`IntelFormatter`], [`MasmFormatter`], [`NasmFormatter`], [`SpecializedFormatter`] (or [`FastFormatter`]).

[`Decoder`]: https://docs.rs/iced-x86/1.21.0/iced_x86/struct.Decoder.html
[`Formatter`]: https://docs.rs/iced-x86/1.21.0/iced_x86/trait.Formatter.html
[`GasFormatter`]: https://docs.rs/iced-x86/1.21.0/iced_x86/struct.GasFormatter.html
[`IntelFormatter`]: https://docs.rs/iced-x86/1.21.0/iced_x86/struct.IntelFormatter.html
[`MasmFormatter`]: https://docs.rs/iced-x86/1.21.0/iced_x86/struct.MasmFormatter.html
[`NasmFormatter`]: https://docs.rs/iced-x86/1.21.0/iced_x86/struct.NasmFormatter.html
[`SpecializedFormatter<TraitOptions>`]: https://docs.rs/iced-x86/1.21.0/iced_x86/struct.SpecializedFormatter.html
[`FastFormatter`]: https://docs.rs/iced-x86/1.21.0/iced_x86/type.FastFormatter.html

```pascal
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

  Iced.Decoder.SetData( @EXAMPLE_CODE[ 0 ], Length( EXAMPLE_CODE ), EXAMPLE_CODE_RIP, doNONE );

  // Formatters: Masm*, Nasm*, Gas* (AT&T) and Intel* (XED).
  // For fastest code, see `SpecializedFormatter` which is ~3.3x faster. Use it if formatting
  // speed is more important than being able to re-assemble formatted instructions.
  Iced.Formatter.FormatterType := ftNasm;

  // Change some options, there are many more
  Iced.Formatter.DigitSeparator        := '`';
  Iced.Formatter.FirstOperandCharIndex := 10;

  while Iced.Decoder.CanDecode do
    begin
    Iced.Decoder.Decode( Instruction );

    // Eg. "00007FFAC46ACDB2 488DAC2400FFFFFF     lea       rbp,[rsp-100h]"

    // Hex
    start_index := instruction.RIP-EXAMPLE_CODE_RIP;
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
```

## Disassemble with a symbol resolver

Creates a custom [`SymbolResolver`] that is called by a [`Formatter`].

[`SymbolResolver`]: https://docs.rs/iced-x86/1.21.0/iced_x86/trait.SymbolResolver.html
[`Formatter`]: https://docs.rs/iced-x86/1.21.0/iced_x86/trait.Formatter.html

```pascal
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
    if ( Symbols[ i ].Offset = Address ) then
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
  Iced.Decoder.SetData( @EXAMPLE_CODE[ 0 ], Length( EXAMPLE_CODE ), EXAMPLE_CODE_RIP, doNone );

  Iced.Decoder.Decode( Instruction );

  Iced.Formatter.FormatterType  := ftNasm;
  Iced.Formatter.SymbolResolver := SymbolResolverCallback;
  Iced.Formatter.ShowSymbols    := True;

  WriteLn( Iced.Formatter.FormatToString( Instruction ) );

  WriteLn( 'Press enter to exit.' );
  ReadLn;
```

## Disassemble with colorized text

Creates a custom [`FormatterOutput`] that is called by a [`Formatter`].

This example will fail to compile unless you install the `colored` crate, see below.

[`FormatterOutput`]: https://docs.rs/iced-x86/1.21.0/iced_x86/trait.FormatterOutput.html
[`Formatter`]: https://docs.rs/iced-x86/1.21.0/iced_x86/trait.Formatter.html

```pascal
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

  Iced.Decoder.SetData( @EXAMPLE_CODE[ 0 ], Length( EXAMPLE_CODE ), EXAMPLE_CODE_RIP, doNONE );

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
    start_index := instruction.RIP-EXAMPLE_CODE_RIP;
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
```

## Move code in memory (eg. hook a function)

Uses instruction info API and the encoder to patch a function to jump to the programmer's function.

```pascal
{
This example produces the following output:
Original code:
00007FFAC46ACDA4 48895C2410              mov       [rsp+10h],rbx
00007FFAC46ACDA9 4889742418              mov       [rsp+18h],rsi
00007FFAC46ACDAE 55                      push      rbp
00007FFAC46ACDAF 57                      push      rdi
00007FFAC46ACDB0 4156                    push      r14
00007FFAC46ACDB2 488DAC2400FFFFFF        lea       rbp,[rsp-100h]
00007FFAC46ACDBA 4881EC00020000          sub       rsp,200h
00007FFAC46ACDC1 488B0518570A00          mov       rax,[rel 7FFAC47524E0h]
00007FFAC46ACDC8 4833C4                  xor       rax,rsp
00007FFAC46ACDCB 488985F0000000          mov       [rbp+0F0h],rax
00007FFAC46ACDD2 4C8B052F240A00          mov       r8,[rel 7FFAC474F208h]
00007FFAC46ACDD9 488D05787C0400          lea       rax,[rel 7FFAC46F4A58h]
00007FFAC46ACDE0 33FF                    xor       edi,edi

Original + patched code:
00007FFAC46ACDA4 48B8F0DEBC9A78563412    mov       rax,123456789ABCDEF0h
00007FFAC46ACDAE FFE0                    jmp       rax
00007FFAC46ACDB0 4156                    push      r14
00007FFAC46ACDB2 488DAC2400FFFFFF        lea       rbp,[rsp-100h]
00007FFAC46ACDBA 4881EC00020000          sub       rsp,200h
00007FFAC46ACDC1 488B0518570A00          mov       rax,[rel 7FFAC47524E0h]
00007FFAC46ACDC8 4833C4                  xor       rax,rsp
00007FFAC46ACDCB 488985F0000000          mov       [rbp+0F0h],rax
00007FFAC46ACDD2 4C8B052F240A00          mov       r8,[rel 7FFAC474F208h]
00007FFAC46ACDD9 488D05787C0400          lea       rax,[rel 7FFAC46F4A58h]
00007FFAC46ACDE0 33FF                    xor       edi,edi

Moved code:
00007FFAC48ACDA4 48895C2410              mov       [rsp+10h],rbx
00007FFAC48ACDA9 4889742418              mov       [rsp+18h],rsi
00007FFAC48ACDAE 55                      push      rbp
00007FFAC48ACDAF 57                      push      rdi
00007FFAC48ACDB0 E9FBFFDFFF              jmp       00007FFAC46ACDB0h
}

const
  EXAMPLE_CODE_BITNESS        = bt64;
  HEXBYTES_COLUMN_BYTE_LENGTH = 12;

procedure Disassemble( Data : PByte; Size : Cardinal; RIP : UInt64 );
var
  Instruction : TInstruction;
  sOutput     : String;
  start_index : Integer;
  i           : Integer;
begin
  Iced.Decoder.Bitness := EXAMPLE_CODE_BITNESS;

  Iced.Decoder.SetData( Data, Size, RIP, doNONE );

  // Formatters: Masm*, Nasm*, Gas* (AT&T) and Intel* (XED).
  // For fastest code, see `SpecializedFormatter` which is ~3.3x faster. Use it if formatting
  // speed is more important than being able to re-assemble formatted instructions.
  Iced.Formatter.FormatterType := ftNasm;

  // Change some options, there are many more
//  Iced.Formatter.DigitSeparator        := '`';
  Iced.Formatter.FirstOperandCharIndex := 10;

  while Iced.Decoder.CanDecode do
    begin
    Iced.Decoder.Decode( Instruction );

    // Eg. "00007FFAC46ACDB2 488DAC2400FFFFFF     lea       rbp,[rsp-100h]"

    // Hex
    start_index := instruction.RIP-RIP;
    sOutput := '';
    for i := start_index to start_index+Instruction.len-1 do
      begin
      sOutput := sOutput + IntToHex( Data^, 2 );
      Inc( Data );
      end;

    if ( Instruction.len < HEXBYTES_COLUMN_BYTE_LENGTH ) then
      begin
      for i := 0 to HEXBYTES_COLUMN_BYTE_LENGTH-Instruction.len-1 do
        sOutput := sOutput + '  '
      end;

    sOutput := Format( '%.16x ', [ instruction.RIP ] ) + sOutput + Iced.Formatter.FormatToString( Instruction );

    WriteLn( sOutput );
    end;

  WriteLn( '' );    
end;

const
  EXAMPLE_CODE_RIP                       = $00007FFAC46ACDA4;
var
  EXAMPLE_CODE : Array [ 0..61 ] of Byte = ( $48, $89, $5C, $24, $10, $48, $89, $74, $24, $18, $55, $57, $41, $56, $48, $8D,
                                             $AC, $24, $00, $FF, $FF, $FF, $48, $81, $EC, $00, $02, $00, $00, $48, $8B, $05,
                                             $18, $57, $0A, $00, $48, $33, $C4, $48, $89, $85, $F0, $00, $00, $00, $4C, $8B,
                                             $05, $2F, $24, $0A, $00, $48, $8D, $05, $78, $7C, $04, $00, $33, $FF
                                           );
const
  // In 64-bit mode, we need 12 bytes to jump to any address:
  //      mov rax,imm64   // 10
  //      jmp rax         // 2
  // We overwrite rax because it's probably not used by the called function.
  // In 32-bit mode, a normal JMP is just 5 bytes
  required_bytes = 10+2;
var
  YOUR_FUNC : UInt64 = UInt64( $123456789ABCDEF0 ); // Address of your code
var
  Instruction            : TInstruction;
  Instructions           : TInstructionList;
  sOutput                : String;
  start_index            : Integer;
  i                      : Integer;
  total_bytes            : Cardinal;
  relocated_base_address : UInt64;
  Results                : TBlockEncoderResult;
  _target                : UInt64;
begin
  if NOT IsInitDLL then
    begin
    WriteLn( 'Library not loaded.' );
    WriteLn( 'Press enter to exit.' );
    ReadLn;
    Exit;
    end;

  WriteLn( 'Original code:' );
  Disassemble( @EXAMPLE_CODE[ 0 ], Length( EXAMPLE_CODE ), EXAMPLE_CODE_RIP );

  Instructions := TInstructionList.Create;

  Iced.Decoder.SetData( @EXAMPLE_CODE[ 0 ], Length( EXAMPLE_CODE ), EXAMPLE_CODE_RIP, doNONE );

  total_bytes := 0;
  while Iced.Decoder.CanDecode do
    begin
    Iced.Decoder.Decode( Instruction );
    if NOT Instruction.IsValid then
      begin
      WriteLn( 'Library not loaded.' );
      WriteLn( 'Press enter to exit.' );
      ReadLn;
      Instructions.Free;
      Exit;
      end;

    Instructions.Add( Instruction );
    Inc( total_bytes, Instruction.len );

    if ( total_bytes >= required_bytes ) then
      break;

    case Instruction.FlowControl.FlowControl of
      fcNext : ;

      fcUnconditionalBranch : begin
                              // You could check if it's just jumping forward a few bytes and follow it
                              // but this is a simple example so we'll fail.
                              if ( Instruction.op_kinds[ 0 ].OpKind = okNearBranch64 ) then
                                  _target := Instruction.NearBranchTarget;

                              WriteLn( 'Not supported by this simple example.' );
                              WriteLn( 'Press enter to exit.' );
                              ReadLn;
                              Instructions.Free;
                              Exit;
                              end;

      fcIndirectBranch,
      fcConditionalBranch,
      fcReturn,
      fcCall,
      fcIndirectCall,
      fcInterrupt,
      fcXbeginXabortXend,
      fcException           : begin
                              WriteLn( 'Not supported by this simple example.' );
                              WriteLn( 'Press enter to exit.' );
                              ReadLn;
                              Instructions.Free;
                              Exit;
                              end;
    end;
    end;

  if ( total_bytes < required_bytes ) OR ( Instructions.Count < 1 ) then
    begin
    WriteLn( 'Not enough bytes!' );
    WriteLn( 'Press enter to exit.' );
    ReadLn;
    Exit;
    end;

  // Create a JMP instruction that branches to the original code, except those instructions
  // that we'll re-encode. We don't need to do it if it already ends in 'ret'
  Instruction := Instructions[ Instructions.Count-1 ];
  if ( Instruction.FlowControl.FlowControl <> fcReturn ) then
    Instructions.Add( Instruction.with_branch( Jmp_rel32_64, Instruction.next_rip ) );

  // Relocate the code to some new location. It can fix short/near branches and
  // convert them to short/near/long forms if needed. This also works even if it's a
  // jrcxz/loop/loopcc instruction which only have short forms.
  //
  // It can currently only fix RIP relative operands if the new location is within 2GB
  // of the target data location.
  //
  // Note that a block is not the same thing as a basic block. A block can contain any
  // number of instructions, including any number of branch instructions. One block
  // should be enough unless you must relocate different blocks to different locations.
  relocated_base_address := EXAMPLE_CODE_RIP + $200000;

  Iced.BlockEncoder.Encode( relocated_base_address, Instructions, Results );
  Instructions.free;

  // Patch the original code. Pretend that we use some OS API to write to memory...
  // We could use the BlockEncoder/Encoder for this but it's easy to do yourself too.
  // This is 'mov rax,imm64; jmp rax'
  EXAMPLE_CODE[ 0 ] := $48; // \ 'MOV RAX,imm64'
  EXAMPLE_CODE[ 1 ] := $B8;
  Move( YOUR_FUNC, EXAMPLE_CODE[ 2 ], SizeOf( YOUR_FUNC ) );
  EXAMPLE_CODE[ 10 ] := $FF; // \ JMP RAX
  EXAMPLE_CODE[ 11 ] := $E0;

  // Disassemble it
  WriteLn( 'Original + patched code:' );
  Disassemble( @EXAMPLE_CODE[ 0 ], Length( EXAMPLE_CODE ), EXAMPLE_CODE_RIP );

  // Disassemble the moved code
  WriteLn( 'Moved code:' );
  Disassemble( PByte( Results.code_buffer ), Results.code_buffer_len, relocated_base_address );

  WriteLn( 'Press enter to exit.' );
  ReadLn;
```

## Get instruction info, eg. read/written regs/mem, control flow info, etc

Shows how to get used registers/memory and other info. It uses [`Instruction`] methods
and an [`InstructionInfoFactory`] to get this info.

[`Instruction`]: https://docs.rs/iced-x86/1.21.0/iced_x86/struct.Instruction.html
[`InstructionInfoFactory`]: https://docs.rs/iced-x86/1.21.0/iced_x86/struct.InstructionInfoFactory.html

```pascal
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
  tOutput       : Array [ 0..255 ] of AnsiChar;

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
  Iced.Decoder.SetData( @EXAMPLE_CODE[ 0 ], Length( EXAMPLE_CODE ), EXAMPLE_CODE_RIP, doNONE );

  // Formatters: Masm*, Nasm*, Gas* (AT&T) and Intel* (XED).
  // For fastest code, see `SpecializedFormatter` which is ~3.3x faster. Use it if formatting
  // speed is more important than being able to re-assemble formatted instructions.
  Iced.Formatter.FormatterType := ftMasm;

  while Iced.Decoder.CanDecode do
    begin
    Iced.Decoder.Decode( Instruction );

    // Assembly
    C := Instruction.next_rip-Instruction.len;
    S := Format( '%.16x ', [ C ] );

    for i := 0 to Instruction.len-1 do
      begin
      S := S + Format( '%.2x', [ Data^ ] );
      Inc( Data );
      end;

      S := S + ' ';

    // For quick hacks, it's fine to use the Display trait to format an instruction,
    // but for real code, use a formatter, eg. MasmFormatter. See other examples.

    Iced.Formatter.Format( Instruction, tOutput, Length( tOutput ) );
    WriteLn( S + String( tOutput ) );

    // Gets offsets in the instruction of the displacement and immediates and their sizes.
    // This can be useful if there are relocations in the binary. The encoder has a similar
    // method. This method must be called after decode() and you must pass in the last
    // instruction decode() returned.
    Iced.Decoder.GetConstantOffsets( Instruction, Offsets );

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

  WriteLn( 'Press enter to exit.' );
  ReadLn;
```

## Get the virtual address of a memory operand

```pascal
function VirtualAddressResolverCallback( Register: TRegister; Index : NativeUInt; Size : NativeUInt; var Address : UInt64; UserData : Pointer ) : boolean; cdecl;
begin
  result := True;
  case Register.Register of
    // The base address of ES, CS, SS and DS is always 0 in 64-bit mode
    ES, CS, SS, DS: Address := 0;

    RDI : Address := $10000000;
    R12 : Address := $000400000000;
  else
    result := False;
  end;
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

  Iced.Decoder.SetData( @EXAMPLE_CODE[ 0 ], Length( EXAMPLE_CODE ), EXAMPLE_CODE_RIP, doNone );

  Iced.Decoder.Decode( Instruction );
  Offset := Instruction.VirtualAddress( VirtualAddressResolverCallback );
  if ( Offset = EXAMPLE_OFFSET ) then
    WriteLn( 'OK: ' + Format( '%.16x', [ Offset ] ) )
  else
    WriteLn( 'Failed.' );

  WriteLn( 'Press enter to exit.' );
  ReadLn;
```

## Disassemble old/deprecated CPU instructions

```pascal
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

  Iced.Decoder.SetData( @EXAMPLE_CODE[ 0 ], Length( EXAMPLE_CODE ), EXAMPLE_CODE_RIP, doMPX OR doMOV_TR OR doCYRIX OR doCYRIX_DMI OR doALTINST );

  // Formatters: Masm*, Nasm*, Gas* (AT&T) and Intel* (XED).
  // For fastest code, see `SpecializedFormatter` which is ~3.3x faster. Use it if formatting
  // speed is more important than being able to re-assemble formatted instructions.
  Iced.Formatter.FormatterType := ftNasm;

  while Iced.Decoder.CanDecode do
    begin
    Iced.Decoder.Decode( Instruction );

    // Eg. "00007FFAC46ACDB2 488DAC2400FFFFFF     lea       rbp,[rsp-100h]"

    // Hex
    start_index := instruction.RIP-EXAMPLE_CODE_RIP;
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
```

## Disassemble as fast as possible

For fastest possible disassembly you should set [`ENABLE_DB_DW_DD_DQ`] to `false`
and you should also override the unsafe [`verify_output_has_enough_bytes_left()`] and return `false`.

[`ENABLE_DB_DW_DD_DQ`]: https://docs.rs/iced-x86/trait.SpecializedFormatterTraitOptions.html#associatedconstant.ENABLE_DB_DW_DD_DQ
[`verify_output_has_enough_bytes_left()`]: https://docs.rs/iced-x86/trait.SpecializedFormatterTraitOptions.html#method.verify_output_has_enough_bytes_left

```pascal
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

  Iced.Decoder.SetData( @EXAMPLE_CODE[ 0 ], Length( EXAMPLE_CODE ), EXAMPLE_CODE_RIP, doNONE );

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

  while Iced.Decoder.CanDecode do
    begin
    Iced.Decoder.Decode( Instruction );

    // Eg. "00007FFAC46ACDB2 488DAC2400FFFFFF     lea       rbp,[rsp-100h]"

    // Hex
    start_index := instruction.RIP-EXAMPLE_CODE_RIP;
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
```

## Create and encode instructions

NOTE: It's much easier to just use [`CodeAssembler`], see the example above.
This example shows how to create instructions without using it.

This example uses a [`BlockEncoder`] to encode created [`Instruction`]s.

[`BlockEncoder`]: https://docs.rs/iced-x86/1.21.0/iced_x86/struct.BlockEncoder.html
[`CodeAssembler`]: https://docs.rs/iced-x86/1.21.0/iced_x86/code_asm/struct.CodeAssembler.html
[`Instruction`]: https://docs.rs/iced-x86/1.21.0/iced_x86/struct.Instruction.html

```pascal
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
  label1 := Instructions.CreateLabel;

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
  data1 := Instructions.CreateLabel;
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
  Iced.BlockEncoder.Encode( EXAMPLE_CODE_RIP, Instructions, Results );
  Instructions.Free;

  // Now disassemble the encoded instructions. Note that the 'jmp near'
  // instruction was turned into a 'jmp short' instruction because we
  // didn't disable branch optimizations.
  Iced.Decoder.Bitness := EXAMPLE_CODE_BITNESS;

  Data := PByte( Results.code_buffer );
  Iced.Decoder.SetData( PByte( Results.code_buffer ), Results.code_buffer_len-Length( raw_data ), EXAMPLE_CODE_RIP{Results.RIP}, doNONE );

  Iced.Formatter.FormatterType         := ftGas;
  Iced.Formatter.FirstOperandCharIndex := 8;

  while Iced.Decoder.CanDecode do
    begin
    Iced.Decoder.Decode( Instruction );

    // Assembly
    S := Format( '%.16x ', [ Instruction.RIP ] );

    for i := 0 to Instruction.len-1 do
      begin
      S := S + Format( '%.2x', [ Data^ ] );
      Inc( Data );
      end;

    for i := 0 to HEXBYTES_COLUMN_BYTE_LENGTH-Instruction.len*2+1 do
      S := S + ' ';

    WriteLn( S + Iced.Formatter.FormatToString( Instruction ) );
    end;

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

  WriteLn( 'Press enter to exit.' );
  ReadLn;
```
