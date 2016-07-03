rem msbuild BikeTracker.sln /v:m /maxcpucount

if not exist "TestResults" mkdir TestResults
if not exist "TestResults\Report" mkdir TestResults\Report

cd BikeTracker.XTests\bin\Debug

..\..\..\packages\OpenCover.4.6.519\tools\OpenCover.Console.exe -target:"..\..\..\packages\xunit.runner.console.2.1.0\tools\xunit.console.x86.exe" -targetargs:"BikeTracker.XTests.dll -noshadow" -output:..\..\..\TestResults\TestResults.xml -register:user -excludebyattribute:"*.ExcludeFromCodeCoverage*" -coverbytest:*.XTest.dll -filter:"+[Bike*]* -[Bike.Tests*]* -[Bike.XTests*]*

cd ..\..\..

.\packages\ReportGenerator.2.4.5.0\tools\ReportGenerator.exe -reports:.\TestResults\TestResults.xml -targetdir:.\TestResults\Report -historydir:.\TestResults\Report\History

if defined APPVEYOR .\packages\coveralls.io.1.3.4\tools\coveralls.net.exe --opencover ".\TestResults\TestResults.xml" --full-sources