@echo off

if not exist TerraAngelSetup\TerraAngelSetup\bin\Release\net7.0\TerraAngelSetup.exe (
    echo Building TerraAngelSetup
    dotnet build TerraAngelSetup\TerraAngelSetup\TerraAngelSetup.csproj -c=Release > NUL
)

echo Running TerraAngelSetup -patch
TerraAngelSetup\TerraAngelSetup\bin\Release\net7.0\TerraAngelSetup.exe -patch -patchinput TerraAngelPatches\
