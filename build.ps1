
$invocation = (Get-Variable MyInvocation).Value

if($Invocation.MyCommand.Path)
{
    $baseDir = Split-Path $Invocation.MyCommand.Path
}
else
{
    $baseDir = $Invocation.InvocationName.Substring(0,$Invocation.InvocationName.LastIndexOf("\"));
}

$outputFolder = $baseDir + "\output"
$releaseFolder = $baseDir + "\src\SeaMist\bin\Release"
$msbuild = $env:systemroot + "\Microsoft.NET\Framework64\v4.0.30319\MSBuild.exe";
$binDir = $baseDir + "\src\SeaMist\bin"
$objDir = $baseDir + "\src\SeaMist\obj"

If (Test-Path $outputFolder){
	Remove-Item $outputFolder -Force -Recurse
}

# Build options
$options = "/noconsolelogger /p:Configuration=Release /fl /flp:logfile=Build.log"

# make sure our working directory is correct
$baseDir = $baseDir + "\src"
cd $baseDir 

$clean = $msbuild + " ""SeaMist.sln"" " + $options + " /t:Clean"
$build = $msbuild + " ""SeaMist.sln"" " + $options + " /t:Build"
Invoke-Expression $clean
Invoke-Expression $build

[System.IO.Directory]::Move($releaseFolder, $outputFolder)

If (Test-Path $binDir){
	Remove-Item $binDir -Force -Recurse
}
If (Test-Path $objDir){
	Remove-Item $objDir -Force -Recurse
}

