using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Musem.Modules
{
    public class Requests
    {
        public async Task<byte[]> GetRequestResponceContentAsync(string url)
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(url);
                if (response.StatusCode == HttpStatusCode.OK)
                    return await response.Content.ReadAsByteArrayAsync();
                return new byte[0];
            }
        }
        public async Task<int> GetCodetAsync(string url)
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(url);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var stingJson = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<JsonObj>(stingJson).Code;
                }
                return -1;
            }
        }
        public async Task<string> GetTextAsync(string url)
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(url);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var stingJson = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<JsonText>(stingJson).Text;
                }
                return null;
            }
        }


    }
}
internal class JsonObj
{
    public int Code { get; set; }
}

internal class JsonText
{
    public string Text { get; set; }
}