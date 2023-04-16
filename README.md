#HttpService

Service that supports communication between Unity pong game and control modules

Each module has its own Id which is used to recognize the sender of the http message: VoiceModule = 1, TangibleModule = 2, SensorsModule = 3, DlKinectmodule = 4,

An example how to send an http request (I recommend to use Postman):

using System.Text.Json;

Console.WriteLine("Sending messages...");
var client = new HttpClient();
client.BaseAddress = new Uri("http://localhost:5001/");

while (true)
{
    Console.WriteLine("Enter message: ");
    var message = Console.ReadLine();

    // Create a JSON payload
    var payload = new
    {
        id = 1,
        racketDirection = message,
    };
    var json = JsonSerializer.Serialize(payload);

    var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
    var response = await client.PostAsync("", content);

    Console.WriteLine($"Response: {response.StatusCode}");

}


Expected json body for each module:

VoiceModule: 
{ 
    "id": 1,
    "scoreboard": int, 
    "ballVelocity": int 
}

TangibleModule: 
{ 
    "id": 2,
    "object": string, 
    "coordinateX": int, 
    "coordinateY": int, 
    "rotationAngle": int
}

SensorsModule: 
{ 
    "id": 3,
    "racketDirection": int // value <0,1> 
}

DlKinectModule: 
{ 
    "id": 4,
    "racketDirection": int // value <0,1> 
}

To run program and test your comunication you can build it in visual studio by your own (.sln file is located in http_service/HttpService/) or open .exe file in bin/Release/net6.0
