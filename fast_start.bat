@echo off

if not exist TerraAngelSetup\TerraAngelSetup\bin\Release\net7.0\TerraAngelSetup.exe (
    echo Building TerraAngelSetup
    REM git submodule update --remote --recursive > NUL
    dotnet build TerraAngelSetup\TerraAngelSetup\TerraAngelSetup.csproj -c=Release > NUL
)

echo Running TerraAngelSetup
TerraAngelSetup\TerraAngelSetup\bin\Release\net7.0\TerraAngelSetup.exe -auto -nocopy -patchinput TerraAngelPatches\
