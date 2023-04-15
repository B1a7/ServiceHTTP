using HttpService.Modules;
using System.Net;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HttpService
{
    public class HttpServiceSingleton
    {
        JsonSerializerOptions options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        private HttpListener _service;
        private static HttpServiceSingleton _instance = null;
        private static readonly object syncLock = new object();

        public TangibleModule TangibleModule { get; set; } = new();
        public DlKinectModule DlKinectModule { get; set; } = new();
        public VoiceModule VoiceModule { get; set; } = new();
        public SensorsModule SensorsModule { get; set; } = new();

        private HttpServiceSingleton()
        {
        }

        public static HttpServiceSingleton GetInstance()
        {
            lock (syncLock)
            {
                if (_instance == null)
                {
                    _instance = new HttpServiceSingleton();
                }
                return _instance;
            }
        }

        /// <summary>method <c>InitializeServer</c> Gets url where server will be hosted and initialize HttpListener.</summary>
        public void InitializeServer(string url)
        {
            _service = new HttpListener();
            _service.Prefixes.Add(url);
            _service.Start();
        }
        /// <summary>method <c>ServerListenerAsync</c> Start listening http requests.</summary>
        public async Task ServerListenerAsync()
        {
            while (true)
            {
                var context = _service.GetContext();
                ProcessRequest(context);
            }
        }

        /// <summary>method <c>ProcessRequest</c>Logic of proccessing http request.</summary>
        private void ProcessRequest(HttpListenerContext context)
        {
            var request = context.Request;
            var response = context.Response;

            if (request.HttpMethod == "POST" || request.HttpMethod == "GET")
            {
                ProcessMessage(context);
                context.Response.StatusCode = 200;
            }
            else
            {
                response.StatusCode = (int)HttpStatusCode.MethodNotAllowed;
            }
            response.Close();
        }

        /// <summary>method <c>ProcessMessage</c> Takes HttpListenerContext, then deserialize json body to appropriate property of type ModuleBase.</summary>
        private void ProcessMessage(HttpListenerContext context)
        {
            using (var streamReader = new StreamReader(context.Request.InputStream))
            {
                var json = streamReader.ReadToEnd();
                var url = context.Request.Url;

                using JsonDocument document = JsonDocument.Parse(json);
                JsonElement root = document.RootElement;
                JsonElement idElement = root.GetProperty("id");

                int id = idElement.GetInt32();

                if (Enum.TryParse(id.ToString(), out ModuleEnum moduleEnum))
                {
                    PropertyInfo propertyInfo = typeof(HttpServiceSingleton).GetProperty(moduleEnum.ToString());
                    Type moduleType = propertyInfo.PropertyType;
                    ModuleBase jsonModule = (ModuleBase)JsonSerializer.Deserialize(json, moduleType, options);
                    propertyInfo.SetValue(this, jsonModule);
                }
            }
        }
    }
}