New-Item -ItemType Directory -Force -Path $PSScriptRoot\TestResults
New-Item -ItemType Directory -Force -Path $PSScriptRoot\TestResults\Report
New-Item -ItemType Directory -Force -Path $PSScriptRoot\TestResults\Report\History

.\nuget install OpenCover -o .\tools -ExcludeVersion
.\nuget install xunit.runner.console -o .\tools -ExcludeVersion

& "$PSScriptRoot\tools\OpenCover\tools\OpenCover.Console.exe" -target:`"$PSScriptRoot\tools\xunit.runner.console\tools\xunit.console.x86.exe`" -targetargs:"`"`"$PSScriptRoot\BikeTracker.XTests\bin\Debug\BikeTracker.XTests.dll`"`" -noshadow -appveyor" -output:$PSScriptRoot\TestResults\TestResults.xml -register:user -excludebyattribute:`"*.ExcludeFromCodeCoverage*`" -coverbytest:*.XTest.dll -filter:`"+[Bike*]* -[Bike.Tests*]* -[Bike.XTests*]*`"

npm install -g karma-cli

Set-Location -Path $PSScriptRoot\BikeTracker

karma start --single-run

Set-Location -Path $PSScriptRoot

if (Test-Path Env:\APPVEYOR)
{
	$wc = New-Object 'System.Net.WebClient'
	$wc.UploadFile("https://ci.appveyor.com/api/testresults/junit/$($env:APPVEYOR_JOB_ID)", (Resolve-Path .\BikeTracker\Scripts\tests\results\*.xml))

	.\nuget install coveralls.io -o .\tools -ExcludeVersion
	& "$PSScriptRoot\tools\coveralls.io\tools\coveralls.net.exe" --opencover "$PSScriptRoot\TestResults\TestResults.xml" --full-sources
}
else
{
	.\nuget install ReportGenerator -o .\tools -ExcludeVersion
	& "$PSScriptRoot\tools\ReportGenerator\tools\ReportGenerator.exe" -reports:.\TestResults\TestResults.xml -targetdir:.\TestResults\Report -historydir:.\TestResults\Report\History
}