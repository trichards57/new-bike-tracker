nuget update -self
rmdir /s /q tools\ModelGenerator\
nuget install ModelGenerator -source https://www.myget.org/F/trichards57/api/v3/index.json -o tools -ExcludeVersion
tools\ModelGenerator\tools\ModelGenerator BikeTracker.Core\model.xml BikeTracker.Core