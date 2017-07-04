using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonCode;
using System.Net.Http;
using System.Net.Http.Headers;
using ErrorLoggerModel;
using System.Net.NetworkInformation;

namespace DLLLibrary
{
    public class DLL
    {
        private static int _appId;
        private static int _minloglevel;
        //private static int SERVICE_PORT = 16642;
        //private static string SERVICE_URL = "http://localhost:{0}";
        private static string SERVICE_URL = "http://localhost/IPLoggerService/";
        private static string ADD_LOG = "Api/Rest/AddLog?log=";
        Logger logger;
        public DLL(int id, int minlevel)
        {
            _appId = id;
            _minloglevel = minlevel;
            logger = new Logger();
        }

        public void Log(string errorMessage, int logLevel, Exception ex = null)
        {
            if(logLevel >= _minloglevel)
            {
                try
                {
                    LogModelView lmv = new LogModelView();
                    lmv.errorDescription = errorMessage == null ? "No Exception" : errorMessage;
                    lmv.exception = ex == null ?  null : "Exception";
                    lmv.errorTime = DateTime.Now;
                    lmv.logLevel = logLevel;
                    lmv.ApplicationRefId = _appId;

                    string json = Newtonsoft.Json.JsonConvert.SerializeObject(lmv);
                    HttpContent content = new StringContent(json);
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                    string result = string.Empty;
                    HttpClient client = new HttpClient();
                    //client.BaseAddress = new Uri(String.Format(SERVICE_URL, SERVICE_PORT));
                    client.BaseAddress = new Uri(SERVICE_URL);
                
                    var response = client.PostAsync(ADD_LOG, content).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        Task<string> task = response.Content.ReadAsStringAsync();  // returns immediately
                        string temp = task.Result;  // blocks until task completes

                        result = temp.ToString();
                    }
                    else
                    {

                        result = string.Format("ERROR: {0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                    }
                }
                catch(Exception exception)
                {                  
                    logger.LogSystemError(exception.ToString());
                }
            }
        }
    }
}
