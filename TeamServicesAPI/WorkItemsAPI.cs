using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using System.Configuration;
using Newtonsoft.Json.Linq;

namespace Bot_Application1.TeamServicesAPI
{
    public class WorkItemsAPI
    {


        public WorkItemsAPI()
        {

        }

        public string GetWorkItem(string id)
        {
            string _personalAccessToken = ConfigurationManager.AppSettings["TSAccessToken"];
            string _credentials = Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(string.Format("{0}:{1}", "", _personalAccessToken)));
            /*string _id = "309"*/;

            //use the httpclient
            using (var client = new HttpClient())
            {
                //set our headers
                client.BaseAddress = new Uri("https://ncpts.visualstudio.com/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", _credentials);

                //send the request and content
                HttpResponseMessage response = client.GetAsync("_apis/wit/workitems/" + id + "?api-version=2.2").Result;

                if (response.IsSuccessStatusCode)
                {
                    string result = response.Content.ReadAsStringAsync().Result;
                    return result;
                }
                return "0";
            }
        }
    }
}