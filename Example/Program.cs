// See https://aka.ms/new-console-template for more information
using Example;
using pocketbase_csharp_sdk;
using pocketbase_csharp_sdk.Models;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;


Console.WriteLine("Hello, World!");

//PocketBase pocketBase1 = new PocketBase("https://orm-csharp-test.pockethost.io");//"http://127.0.0.1:8090");
//var user1 = await pocketBase1.User.AuthenticateWithPassword("test@mail.com", "123456789");
//var user2 = await pocketBase1.AuthCollection<Users2>("users2").AuthenticateWithPassword("iluvadev@gmail.com", "123456789");


PocketBase pocketBase = new PocketBase("http://127.0.0.1:8090");
var adminUser = await pocketBase.Admin.AuthenticateWithPassword("test@test.de", "0123456789");

pocketBase.Records.Subscribe("restaurants", "*", cb =>
{
    Debugger.Break();
});

Console.ReadKey();