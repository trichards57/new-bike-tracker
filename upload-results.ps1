# upload results to AppVeyor
$wc = New-Object 'System.Net.WebClient'
$wc.UploadFile("https://ci.appveyor.com/api/testresults/xunit/$($env:APPVEYOR_JOB_ID)", (Resolve-Path .\test-results.xml))

Get-ChildItem ".\reports\karma-results\junit" -Filter *.xml | 
Foreach-Object {
	$wc.UploadFile("https://ci.appveyor.com/api/testresults/junit/$($env:APPVEYOR_JOB_ID)", (Resolve-Path $_.FullName))
}


