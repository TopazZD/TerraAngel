@echo off

echo Building TerraAngelSetup
git submodule update > NUL
dotnet build TerraAngelSetup\TerraAngelSetup\TerraAngelSetup.csproj -c=Release > NUL

echo Running TerraAngelSetup
TerraAngelSetup\TerraAngelSetup\bin\Release\net6.0\TerraAngelSetup.exe -auto -patchinput TerraAngelSetup\TerraAngelSetup\Patches\TerraAngelPatches

echo Buidling TerraAngel as x64
dotnet build src\TerraAngel\Terraria\Terraria.csproj -p:Configuration=Release;Platform=x64 > build_log_x64.txt

echo Running TerraAngelSetup
TerraAngelSetup\TerraAngelSetup\bin\Release\net6.0\TerraAngelSetup.exe -prebuild -patchinput TerraAngelSetup\TerraAngelSetup\Patches\TerraAngelPatches -compileroutput src\TerraAngel\Terraria\bin\x64\Release\net6.0

echo Finished building! Your build should be in src\TerraAngel\Terraria\bin\x64\Release\net6.0