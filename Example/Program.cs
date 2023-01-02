// See https://aka.ms/new-console-template for more information
using pocketbase_csharp_sdk;
using System.Diagnostics;

Console.WriteLine("Hello, World!");

PocketBase pocketBase = new PocketBase("http://127.0.0.1:8090");
var adminUser = await pocketBase.Admin.AuthenticateWithPassword("test@test.de", "0123456789");

pocketBase.Records.Subscribe("restaurants", "*", cb =>
{
    Debugger.Break();
});

Console.ReadKey();