This assembly has all the disassembler unit tests (xUnit).

It's recommended to run all unit tests from the command line and not use Visual Studio's
Test Explorer because it's too slow. xUnit's `-noappdomain` option should also be used
to disable running the unit tests in a new AppDomain. Running the unit tests in a new
AppDomain will serialize and deserialize lots of data and slows things down a lot.
