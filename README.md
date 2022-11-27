PocketBase C# SDK
======================================================================
This project is currently still under development. It is not recommended to use it in a productive environment. Things can and will change.

Community-developed C# SDK (Multiplatform) for interacting with the [PocketBase API](https://pocketbase.io/docs)

- [PocketBase C# SDK](#pocketbase-c-sdk)
- [Installation](#installation)
  - [Nuget](#nuget)
- [Usage](#usage)
- [Development](#development)
  - [Requirements](#requirements)
  - [Steps](#steps)

# Installation

## Nuget

Coming soon

# Usage
```c#
//create a new Client which connects to your PocketBase-API
var client = new PocketBase("http://127.0.0.1:8090");

//authenticate as a Admin
var admin = await client.Admin.AuthenticateWithPassword("test@test.de", "0123456789");

//or as a User
var user = await client.User.AuthenticateWithPassword("kekw@kekw.com", "0123456789");

//query some data (for example, some restaurants)
//note that each CRUD action requires a data type which inherits from the base class 'ItemBaseModel'.
var restaurantList = await client.Records.ListAsync<Restaurant>("restaurants");

//like this one
class Restaurant : ItemBaseModel
{
    public string? Name { get; set; }
}
```

# Development

## Requirements
- Visual Studio (Community Edition should work just fine)
- .NET 6/7 SDK

## Steps
1. Clone this repository
```cmd
git clone https://github.com/PRCV1/pocketbase-csharp-sdk
```
2. Open the [pocketbase-csharp-sdk.sln](pocketbase-csharp-sdk.sln) with Visual Studio (Community Edition should work just fine)