using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AngleSharp;
using AngleSharp.Dom;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;

namespace BattleShip.Services
{
    public class ScrapperService
    {
        public JObject  ScrapFromSchema(JSchema schema, string url)
        {
          JObject scrapResult = new JObject();
         

          // Setup the configuration to support document loading
          var config = Configuration.Default.WithDefaultLoader();
          // Load the names of all The Big Bang Theory episodes from Wikipedia
          //var address = "https://en.wikipedia.org/wiki/List_of_The_Big_Bang_Theory_episodes";
          // Asynchronously get the document in a new context using the configuration
          //var page = await BrowsingContext.New(config).OpenAsync(address);
            var page = BrowsingContext.New(config).OpenAsync(url).Result;
            
                
               
                foreach (var prop in schema.Properties)
                {
                    JToken obj;
                    var extract = prop.Value.ExtensionData["extract"].Value<string>();
                    var select = page.QuerySelectorAll(prop.Value.ExtensionData["selector"].Value<string>());
                    obj = prop.Value.Type == JSchemaType.Array ? new JArray(@select.Select(node => ProcessJToken(node, extract, prop)).ToArray()) : ProcessJToken(@select.FirstOrDefault(), extract, prop);
               
                    scrapResult.Add(prop.Key,obj );
                }
            
           
            return scrapResult;
        }

        private JToken ProcessJToken(IElement select, string extract, KeyValuePair<string, JSchema> prop)
        {
            JToken obj;
            var uniqueSelect = select;
            obj = uniqueSelect != null
                ? extract != "text"
                    ? uniqueSelect.GetAttribute(extract.Replace("[", "").Replace("]", ""))
                    : uniqueSelect.TextContent == "" ? uniqueSelect.InnerHtml : uniqueSelect.TextContent
                : prop.Value.Default;
            if (prop.Value.ExtensionData.ContainsKey("schema"))
            {
                var date = DateTime.Parse(obj.ToString());
                obj = date.ToString(prop.Value.ExtensionData["schema"].Value<string>());
            }
            return obj;
        }

        public async Task<JObject> ScrapFromSchemaAsync(JSchema schema, string url)
        {
            try
            {
                JObject scrapResult = new JObject();


                // Setup the configuration to support document loading
                var config = Configuration.Default.WithDefaultLoader();
                // Load the names of all The Big Bang Theory episodes from Wikipedia
                //var address = "https://en.wikipedia.org/wiki/List_of_The_Big_Bang_Theory_episodes";
                // Asynchronously get the document in a new context using the configuration
                var page = await BrowsingContext.New(config).OpenAsync(url);
                //var page = BrowsingContext.New(config).OpenAsync(url).Result;



                foreach (var prop in schema.Properties)
                {
                    JToken obj;
                    var extract = prop.Value.ExtensionData["extract"].Value<string>();
                    var select = page.QuerySelectorAll(prop.Value.ExtensionData["selector"].Value<string>());
                    obj = prop.Value.Type == JSchemaType.Array ? new JArray(@select.Select(node => ProcessJToken(node, extract, prop)).ToArray()) : ProcessJToken(@select.FirstOrDefault(), extract, prop);

                    scrapResult.Add(prop.Key, obj);
                }


                return scrapResult;
            }
            catch (Exception e)
            {
                return new JObject();
            }
        }


    }
}