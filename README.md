# Beat-Pulse-Checks
Additional liveness checks for beat pulse


## Elastic Search

### Overview
This calls the _cluster/health endpoint and checks that the returned status is either <span style="color:green">green</span> or <span style="color:yellow">yellow</span>.

### Configuration
```csharp
    services.AddBeatPulse(setup =>
    {
        setup.AddElasticsearch("http://localhost:9200");
    });
```


## AzureServiceBusManagement

### Overview
This attempts to acquire a token and then log into the `RelayManagementClient` client.

### Configuration
```csharp
    services.AddBeatPulse(setup =>
    {
        var clientID = "56233594-cc99-4a9a-b0e7-9c3d0affde3b";
        var clientSecret = "wpWb6dEUu5gh34CIAHtWqJGSm2iBWAI95ioFkeNO/CY=";
        var tenantID = "72c4dca8-0b7d-4f3b-98bd-cba5dbb4246b";
        setup.AddAzureServiceBusManagement(clientID, clientSecret, tenantID);
    });
```     