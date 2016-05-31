using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using BattleShip.Common;
using BattleShip.Domain;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using Serialize.Linq.Extensions;

namespace BattleShip.MVC.Services
{
    public class ScrappedDocumentService
    {
        public ScrappedDocumentService()
        {
            _formatters = new List<MediaTypeFormatter> { new JsonMediaTypeFormatter
            {
                SerializerSettings = new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Objects
                }
            }};
            _mediaTypeJson = new MediaTypeWithQualityHeaderValue("application/json");
        }

        public static ScrappedDocumentService GetInstance => new ScrappedDocumentService();


        private   List<MediaTypeFormatter> _formatters;
        private   MediaTypeWithQualityHeaderValue _mediaTypeJson;

      


       

        private HttpClient PrepareHttpClient()
        {
            var client = new HttpClient { BaseAddress = new Uri(ConfigurationManager.AppSettings["WebAPIUrl"]) };
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(_mediaTypeJson);

            return client;
        }

        public async Task<IList<ScrappedDocument>> FindAll(Expression<Func<ScrappedDocument, bool>> query)
        {
            
                var queryNode = query.ToExpressionNode();
                using (var client = PrepareHttpClient())
                {
                    var response = await client.PostAsync("api/scrappedData/FindAll", queryNode, _formatters[0], _mediaTypeJson,CancellationToken.None);
                    response.EnsureSuccessStatusCode();
                    return await response.Content.ReadAsAsync<IList<ScrappedDocument>>();
                }
                //return HttpHelper.GET<IList<ScrappedDocument>>(new Uri(ConfigurationManager.AppSettings["WebAPIUrl"]).ToString(),
                //    "api/ScrappedData/getall?sortorder=" + sortOrder);

            
           
        }

        public async Task<ScrappedDocument> Find(Expression<Func<ScrappedDocument, bool>> query)
        {

            var queryNode = query.ToExpressionNode();
            using (var client = PrepareHttpClient())
            {
                var response = await client.PostAsync("api/scrappedData/Find", queryNode, _formatters[0], _mediaTypeJson, CancellationToken.None);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsAsync<ScrappedDocument>();
            }
            //return HttpHelper.GET<IList<ScrappedDocument>>(new Uri(ConfigurationManager.AppSettings["WebAPIUrl"]).ToString(),
            //    "api/ScrappedData/getall?sortorder=" + sortOrder);



        }



        public ScrappedDocument Get(int id)
        {
            try
            {
                return HttpHelper.GET<ScrappedDocument>(new Uri(ConfigurationManager.AppSettings["WebAPIUrl"]).ToString(),
                   "api/ScrappedData/get?id=" + id);

            }
            catch { throw; }
        }

       



      


        public void AddOrUpdate(ScrappedDocument scrappedDocument)
        {
            try
            {
                HttpHelper.POST<ScrappedDocument, ScrappedDocument>(new Uri(ConfigurationManager.AppSettings["WebAPIUrl"]).ToString(), "api/ScrappedDocument/addorupdate", scrappedDocument);

            }
            catch { throw; }
        }

        public void Delete(ScrappedDocument scrappedDocument)
        {
            try
            {

                HttpHelper.POST<ScrappedDocument, ScrappedDocument>(new Uri(ConfigurationManager.AppSettings["WebAPIUrl"]).ToString(), "api/ScrappedDocument/delete", scrappedDocument);

            }
            catch { throw; }
        }


       
    }
}