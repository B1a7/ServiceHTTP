# HttpService
Service that supports communication between Unity pong game and control modules

Each module has its own Id which is used to recognize the sender of the http message:
VoiceModule = 1,
TangibleModule = 2,
SensorsModule = 3,
DlKinectmodule = 4,

An example that sends an http request (app will be working in local network. Ulr is defined in connection.json which me be in the same directory as .exe file):

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

Expected json body for each module:

TangibleModule:
    {
        "object": string,
        "coordinateX": int,
        "coordinateY": int,
        "rotationAngle": int
    }

SensorsModule:
    {
        "racketDirection": int,  // value <0,1>
    }
   
VoiceModule:
    {
        "scoreboard": int,
        "ballVelocity": int
    }
    
DlKinectModule:
    {
        "racketDirection": int,  // value <0,1>
    }
    
To run program and test your comunication you can build it in visual studio by your own (.sln file is located in http_service/HttpService/) or open .exe file in bin/Release/net6.0
