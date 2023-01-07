// See https://aka.ms/new-console-template for more information
using pocketbase_csharp_sdk;
using System.Diagnostics;

Console.WriteLine("Hello, World!");

PocketBase pocketBase = new PocketBase("https://orm-csharp-test.pockethost.io");//"http://127.0.0.1:8090");
//var user2 = await pocketBase1.AuthCollection<Users2>("users2").AuthenticateWithPassword("iluvadev@gmail.com", "123456789");

//PocketBase pocketBase = new PocketBase("http://127.0.0.1:8090");
//var adminUser = await pocketBase.Admin.AuthenticateWithPassword("test@test.de", "0123456789");

//pocketBase.Records.Subscribe("restaurants", "*", cb =>
pocketBase.Records.Subscribe("tags", "*", async cb =>
{
    await Task.Run(() =>
    {
        Console.WriteLine("------ New Sse Message: -----");
        Console.WriteLine(cb.ToString());
    });
    //Debugger.Break();

});
pocketBase.Records.Subscribe("categories", "sywd90gz2ifd7pf", async cb =>
{
    await Task.Run(() =>
    {
        Console.WriteLine("------ New Sse Message: -----");
        Console.WriteLine(cb.ToString());
    });
    //Debugger.Break();
});

Console.ReadKey();