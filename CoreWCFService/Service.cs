using Models.BaseType;
using Models.DTO;
using Newtonsoft.Json;
using System.Collections;
using System.Net.Http.Headers;
using WebAPI;

namespace CoreWCFService
{
    public partial class Service : IService
    {
        private HttpClient client;
        public Service()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:7263/api/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
    }
}