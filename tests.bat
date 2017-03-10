mkdir tools
mkdir reports\coverage\historydir

nuget install OpenCover -ExcludeVersion -OutputDirectory  tools
nuget install xunit.runner.console -ExcludeVersion -OutputDirectory tools
nuget install ReportGenerator -ExcludeVersion -OutputDirectory tools

copy xunit.console.x86.exe.config tools\xunit.runner.console\tools\xunit.console.x86.exe.config /Y

tools\OpenCover\tools\OpenCover.Console.exe -target:"tools\xunit.runner.console\tools\xunit.console.x86.exe" -targetargs:"BikeTracker.Tests\bin\Debug\BikeTracker.Tests.dll -noShadow -xml test-results.xml" -register:user -output:"reports\coverage\coverage.xml" -skipautoprops -filter:"+[BikeTracker*]* -[BikeTracker*]BikeTracker.Migrations* -[BikeTracker.XTests*]*"  -excludebyattribute:*.ExcludeFromCodeCoverage* -mergebyhash -returntargetcode -coverbytest:*.Tests.dll
tools\ReportGenerator\tools\ReportGenerator.exe -reports:reports\coverage\coverage.xml -targetdir:reports\coverage -historydir:reports\coverage\history

cd BikeTracker

call karma start --single-run

cd ..