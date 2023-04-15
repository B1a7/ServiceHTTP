using HttpService;
using HttpService.Modules;
using System.Reflection;
using Microsoft.Extensions.Configuration;

namespace ConsoleDisplayer
{
    class Displayer
    {
        static async Task Main(string[] args)
        {

            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("connection.json")
                .Build();
            var url = configuration["url"];

            var service = HttpServiceSingleton.GetInstance();
            service.InitializeServer(url);
            PropertyInfo[] properties = service.GetType().GetProperties();

            Task.Run(async () =>
            {
                await service.ServerListenerAsync();
            });

            // Display all values in Console
            while (true)
            {
                foreach (var property in properties)
                {
                    var module = (ModuleBase)property.GetValue(service);
                    PropertyInfo[] moduleProperties = module.GetType().GetProperties();
                    Console.WriteLine($"{property.Name}:");
                    foreach (PropertyInfo moduleProperty in moduleProperties)
                    {
                        Console.WriteLine($"  {moduleProperty.Name}: {moduleProperty.GetValue(module)}");
                    }
                }

                await Task.Delay(500);
                Console.Clear();
            }
        }
    }
}