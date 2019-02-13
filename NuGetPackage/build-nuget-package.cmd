nuget pack "..\Rpi.SenseHat\Rpi.SenseHat\Rpi.SenseHat.csproj" -Symbols -IncludeReferencedProjects -Prop Configuration=Release
@echo.
@echo.
@echo After a successful NuGet package build:
@echo.
@echo First execute:
@echo nuget setApiKey {API KEY}
@echo Where the {API KEY} is gotten from https://www.nuget.org/account.
@echo.
@echo Then execute:
@echo nuget push Emmellsoft.IoT.Rpi.SenseHat.xxxxxxxx.nupkg
