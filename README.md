![Main workflow](https://github.com/ton-actions/ton-client-dotnet/workflows/Main%20workflow/badge.svg)
[<img src="https://avatars3.githubusercontent.com/u/67861283?s=150&u=4536b61595a1b422604fab8a7012092d891278f6&v=4" align="right" width="150">](https://freeton.org/)

# Free TON .NET Client 


Free TON is modern and fast crypto network. Lets do this network convenient both for users and developers!

- This client was automatically generated from [api.json](https://github.com/tonlabs/TON-SDK/blob/master/tools/api.json) (see [ClientGenerator](https://github.com/ton-actions/ton-client-dotnet/tree/master/tools/ClientGenerator)) 
- Fully supported methods provided in SDK documentation https://github.com/tonlabs/TON-SDK/tree/master/docs
- No Newtonsoft.Json required, it is kinda legacy now, last release was over a year ago. New System.Text.Json is ten times faster
- The most complete support of CancellationToken
- Net Standard 2.1 compatible


# Quick start 

## Add Nuget Package to your project

```
dotnet add package ch1seL.TonNet.Client
```

## Register in DI  
**Be careful!** ServerAddress is "main.ton.dev" as default

```
public void ConfigureServices(IServiceCollection services)
{
    services.AddTonClient();
}
```

## Ready to use everywhere 

```
public class YourTonService {
    private readonly ITonClient _tonClient;

    public YourTonService(ITonClient tonClient) {
        _tonClient = tonClient;
    }
    
    public string GetTonSecretPhase() {
        var mnemonic = await _tonClient.Crypto.MnemonicFromRandom(new ParamsOfMnemonicFromRandom());
        return mnemonic.Phrase;
    }
}
```

## IPackageManager interface

There is easy option to load contracts abi and tvm info from files in this client.

Now available following async methods:

```
Task<Package> LoadPackage(string name); // Package entity just contains Abi and Tvc
Task<Abi> LoadAbi(string name);
Task<string> LoadTvc(string name);
```

Default contracts path is `_contracts`

## IDebotBrowser interface

**Attention!** DeBot module is UNSTABLE yet, see TON SDK description

More information:

- https://ru.freeton.wiki/Технические_характеристики_DeBot

- https://github.com/tonlabs/ton-labs-contracts/tree/master/solidity/debots

As default IDebotBrowser will resolve instance of DefaultDebotBrowser.

Can be inherited or implemented self-owned: `services.AddTransient<IDebotBrowser, MyDebotBrowser>()`

## Advanced client configuration

See configuration parameters here https://github.com/tonlabs/TON-SDK/blob/master/docs/mod_client.md#NetworkConfig

```
public void ConfigureServices(IServiceCollection services)
{
    services.AddTonClient(networkConfig =>
    {
        networkConfig.ServerAddress = "net.ton.dev"; // or http://localhost, main.ton.dev is default
        networkConfig.NetworkRetriesCount = 5;
    }, packageManagerConfig =>
    {
        packageManagerConfig.PackagesPath = "packages"; // path to abi.json and tvc files, _contracts is default
    });  
}
```

### or configure options by appsettings.json or another configuration provider

https://docs.microsoft.com/en-us/dotnet/core/extensions/configuration-providers

#### Example for appsettings.json 
```
{
  "NetworkConfig": {
    "server_address": "net.ton.dev",
    "network_retries_count": "10",
    "message_retries_count": "5",
    "message_processing_timeout": 100,
    "wait_for_timeout": 100,
    "out_of_sync_threshold": 5000,
    "access_key": "YourAccessKey"
  },
  "PackageManager":{
    "PackagesPath": "path_to_contracts"
  }
}
```

```
public void ConfigureServices(IServiceCollection services)
{
    services
        .Configure<NetworkConfig>(Configuration.GetSection("NetworkConfig"))
        .Configure<PackageManagerOptions>(Configuration.GetSection("PackageManager"));

    services.AddTonClient();
}
```

## Logging

Fully compatible with https://docs.microsoft.com/en-us/dotnet/core/extensions/logging 

## Anonymous type extensions

There are a few properties with type JsonElement in data models. 
And this client provide methods to easy convert this properties to/from Anonymous prototype.

### Convert to anonymous type example:

```
ResultOfParse parseResult = await tonClient.Boc.ParseMessage(new ParamsOfParse
{
    Boc = "te6ccgEBAQEAWAAAq2n+AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAE/zMzMzMzMzMzMzMzMzMzMzMzMzMzMzMzMzMzMzMzMzMzSsG8DgAAAAAjuOu9NAL7BxYpA"
});
var parsedPrototype = new {type = default(int), id = default(string)};
var parsedMessage = parseResult.Parsed!.Value.ToAnonymous(parsedPrototype);

_logger.LogInformation("Parsed message id: {id} type: {type}", parsedMessage.id, parsedMessage.type);
```

### Convert from anonymous type example:

```
await tonClient.Net.WaitForCollection(new ParamsOfWaitForCollection
{
    Collection = "transactions",
    Filter = new {in_msg = new {eq = parsedMessage.id}}.ToJsonElement(),
    Result = "id"
});
```

## Samples

https://github.com/ton-actions/ton-client-dotnet/tree/master/samples/

***Enjoy!***

---
>My Coffee Surf address:
>>ton://surf/0:9b487d68e4f029ab6d92640892d99d1c549ae69b198df414e905350559a165bf

>https://ton.surf
