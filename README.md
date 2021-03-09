# BLOBi
[![Build status](https://dev.azure.com/DavidVanderheyden/BLOBi/_apis/build/status/BLOBi-ASP.NET%20Core-CI)](https://dev.azure.com/DavidVanderheyden/BLOBi/_build/latest?definitionId=15)

Azure Blob Storage Integration Library

## Get it on Nuget

The main package on [nuget.org](https://www.nuget.org/packages/BLOBi/):
``` csharp
PM> Install-Package BLOBi
```

## Usage
### Registering dependencies
Registering the dependencies in an ASP.NET Core application, using Microsoft.Extensions.DependencyInjection, is pretty simple:
- Provide a section in de AppSettings as below example from a storage emulator
```` 
 "azureStorageManagement": {
    "accountName": "devstoreaccount1",
    "accountKey": "Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==",
    "connectionString": "DefaultEndpointsProtocol=http;AccountName=devstoreaccount1;AccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==;BlobEndpoint=http://127.0.0.1:10000/devstoreaccount1;"
  },
````
- Call below code inside the ````Configure```` method in Startup.cs or from inside an IServiceCollection extension 
```` 
services.AddBlobICore(_configuration.GetSection("AzureStorageManagement"));
````
