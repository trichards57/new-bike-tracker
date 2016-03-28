del duplicates-report.txt

cd .\BikeTracker

..\Simian\simian-2.4.0 -excludes=**/*.g.* **/*.cs  >> ..\duplicates-report.txt

cd ..\BikeTracker.Tests

..\Simian\simian-2.4.0 -excludes=**/*.g.* **/*.cs  >> ..\duplicates-report.txt

cd ..