$data =  "{ 'branches': { '$Env:APPVEYOR_REPO_BRANCH': { 'commit_id': '$Env:APPVEYOR_BUILD_VERSION', 'commit_message': '$Env:APPVEYOR_REPO_COMMIT_MESSAGE', 'download_url': 'https://github.com/trichards57/new-bike-tracker/archive/$Env:APPVEYOR_BUILD_VERSION.tar.gz' } } }"

Write-Host $data

Invoke-WebRequest -UseBasicParsing $Env:Production_Build_Path -Method POST -Body $data
