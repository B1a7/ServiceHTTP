
# HttpService

Service that supports communication between Unity pong game and control modules



## Messages structure 

Each module has its own Id which is used to recognize the sender of the http message:   

- VoiceModule = 1
- TangibleModule = 2
- SensorsModule = 3
- DlKinectmodule = 4

Expected json body for each module:

- VoiceModule: 
```json
{ 
    "id": int, 
    "scoreboard": int, 
    "ballVelocity": int 
}
```

- TangibleModule: 
```json
{ 
    "id": int, 
    "object": string, 
    "coordinateX": int, 
    "coordinateY": int, 
    "rotationAngle": int 
}
```

- SensorsModule: 
```json
{ 
    "id": 3, 
    "racketDirection": int // value <0,1> 
}
```

- DlKinectModule: 
```json
{ 
    "id": 4, 
    "racketDirection": int // value <0,1> 
}
```

## Run Locally

Clone the project

```bash
  git clone https://github.com/B1a7/ServiceHTTP.git
```

Go to the project directory

```bash
  cd ServiceHttp
```

Configure conection.json 

```json
{
  "url": "http://localhost:5001/"
}
```

Start the program - open .exe file in bin/Release/net6.0

Send messages

## Tech Stack

- **.NET 6**
## Nuget packages

- **System.Text.Json** v7.0.2

