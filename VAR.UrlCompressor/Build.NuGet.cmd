@echo off

:: MSBuild and tools path
if exist "%windir%\Microsoft.Net\Framework\v4.0.30319" set MsBuildPath=%windir%\Microsoft.NET\Framework\v4.0.30319
if exist "%windir%\Microsoft.Net\Framework64\v4.0.30319" set MsBuildPath=%windir%\Microsoft.NET\Framework64\v4.0.30319
if exist "C:\Program Files (x86)\MSBuild\14.0\Bin" set MsBuildPath=C:\Program Files (x86)\MSBuild\14.0\Bin
if exist "C:\Program Files (x86)\Microsoft Visual Studio\2017\Community\MSBuild\15.0\Bin" set MsBuildPath=C:\Program Files (x86)\Microsoft Visual Studio\2017\Community\MSBuild\15.0\Bin
set PATH=%MsBuildPath%;%PATH%

echo %MsBuildPath%

:: NuGet
set nuget="nuget"
if exist "%~dp0..\packages\NuGet.CommandLine.3.4.3\tools\NuGet.exe" set nuget="%~dp0\..\packages\NuGet.CommandLine.3.4.3\tools\NuGet.exe"
if exist "%~dp0..\packages\NuGet.CommandLine.4.1.0\tools\NuGet.exe" set nuget="%~dp0\..\packages\NuGet.CommandLine.4.1.0\tools\NuGet.exe"

:: Release .Net 3.5
Title Building Release .Net 3.5
msbuild VAR.UrlCompressor.csproj /t:Build /p:Configuration="Release .Net 3.5" /p:Platform="AnyCPU"

:: Release .Net 4.6.1
Title Building Release .Net 4.6.1
msbuild VAR.UrlCompressor.csproj /t:Build /p:Configuration="Release .Net 4.6.1" /p:Platform="AnyCPU"

:: Packing Nuget
Title Packing Nuget
%nuget% pack VAR.UrlCompressor.csproj -Verbosity detailed -OutputDir "NuGet" -Properties Configuration="Release .Net 4.6.1" -Prop Platform=AnyCPU

title Finished
pause
