msbuild /v:m /maxcpucount
.\packages\OpenCover.4.6.519\tools\OpenCover.Console.exe -target:vstest.console.exe -targetargs:"\".\BikeTracker.Tests\bin\Debug\BikeTracker.Tests.dll\"  /Logger:trx" -output:".\TestResults\TestResults.xml" -mergebyhash -register:user -coverbytest:"*.Tests.dll" -excludebyattribute:"*.ExcludeFromCodeCoverage*"
.\packages\ReportGenerator.2.4.4.0\tools\ReportGenerator.exe -reports:.\TestResults\TestResults.xml -targetdir:.\TestResults\Report -historydir:.\TestResults\Report\History
.\duplicates.bat