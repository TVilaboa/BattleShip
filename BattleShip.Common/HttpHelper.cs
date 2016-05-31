using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using Newtonsoft.Json;

namespace BattleShip.Common
{
    public class HttpHelper
    {
        public static string ApplicationRoot => HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority;

        public static bool ImageFileExist(string sku)
        {
            string relativePath = ConfigurationManager.AppSettings["ImagesRelativePath"];
            var path = HttpContext.Current.Request.PhysicalApplicationPath + relativePath;
            var fullPath = Path.Combine(path, sku + ".jpg");

            return File.Exists(fullPath);
        }
        public static bool ImageFileExistByFileName(string fileName)
        {
            string relativePath = ConfigurationManager.AppSettings["ImagesRelativePath"];
            var path = HttpContext.Current.Request.PhysicalApplicationPath + relativePath;
            var fullPath = Path.Combine(path, fileName);

            return File.Exists(fullPath);
        }

        public static string GetBinPhysicalPath()
        {
            return HttpContext.Current.Request.PhysicalApplicationPath + @"\Bin";
        }

        public static TObj GET<TObj>(string baseAddress, string url)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseAddress);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage response = client.GetAsync(url).Result;
                    if (response.IsSuccessStatusCode)
                        return response.Content.ReadAsAsync<TObj>().Result;
                    else
                        throw new Exception("Error en GET: Status code: " + response.StatusCode + " Reason Phrase : " + response.ReasonPhrase);
                }
            }
            catch { throw; }
        }
        public static TObj POST<TObj>(string baseAddress, string url, IDictionary<string, string> parameters)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    HttpContent content = new FormUrlEncodedContent(parameters);

                    client.BaseAddress = new Uri(baseAddress);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage response = client.PostAsync(url, content).Result;
                    if (response.IsSuccessStatusCode)
                        return response.Content.ReadAsAsync<TObj>().Result;
                    else
                        throw new Exception("Error en POST:");
                }
            }
            catch { throw; }
        }

        public static TObj POST<TObj, TObj2>(string baseAddress, string url, TObj2 obj)
        {
            try
            {
                using (HttpClient client = new HttpClient
                    {
                        BaseAddress = new Uri(baseAddress)
                    })
                {
                    
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
   
                    HttpResponseMessage response = client.PostAsync(url,new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8,"application/json")).Result;
                    if (response.IsSuccessStatusCode)
                        return response.Content.ReadAsAsync<TObj>().Result;
                    else
                        throw new Exception("Error en POST:");
                }
            }
            catch { throw; }
        }
    }
}
