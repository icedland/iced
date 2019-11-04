This assembly has all the unit tests (xUnit).

It's recommended to run all unit tests from the command line and not use Visual Studio's Test Explorer because it's too slow and doesn't show all tests only showing 1/3 of all tests. xUnit's `-noappdomain` option should also be used to disable running the unit tests in a new AppDomain. Running the unit tests in a new AppDomain will serialize and deserialize lots of data and slows things down a lot.

VS is really slow. dotnet test is faster. xunit.console.exe is fastest.
