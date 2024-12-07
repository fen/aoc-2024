@echo off
IF not "%1" == "". GOTO No1

dotnet run -c Release --project .\src\Aoc.Cli.AppHost\Aoc.Cli.AppHost.csproj
GOTO End1

:No1
dotnet run -c Release --project .\src\Aoc.Cli.AppHost\Aoc.Cli.AppHost.csproj -- %1

:End1
