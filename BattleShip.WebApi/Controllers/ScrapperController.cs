using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using System.Web.Http;
using BattleShip.Domain;
using BattleShip.Services;
using Newtonsoft.Json.Linq;

namespace BattleShip.WebApi.Controllers
{
    public class ScrapperController : ApiController
    {
        [System.Web.Http.HttpGet]
        public async Task<string> Scrap(string baseUrl, int min, int max)
        {
            try
            {
                var service = new SchemaService();
                var schema = service.GetByBaseUrl(baseUrl);
                if (schema == null)
                {
                    return "There is no Schema for that Url";
                }


                var docService = new ScrappedDocumentService();
                var scrapperService = new ScrapperService();
                var domainService = new DomainService();
                var pathService = new PathService();
                var path = schema.Path;
                var range = Enumerable.Range(min, max - min);
                var watch = new Stopwatch();
                //watch.Start();
                var tasks = range.Select(i => scrapperService.ScrapFromSchemaAsync(schema.ToJSchema(), baseUrl + i).ContinueWith(
                    t =>
                    {
                        if (t.Result != null)
                        {
                            var doc = new ScrappedDocument()
                            {
                                UsedSchema = schema,
                                Name = docService.GetNameForDocument(t.Result, schema),
                                ScrapIdentifier = i.ToString(),
                                ScrappedJson = t.Result.ToString(),
                                Uri = baseUrl + i
                            };
                            var existing = docService.GetByCode(doc.Code);
                            if (existing != null)
                            {
                                existing.UpdateDate = DateTime.Now;
                                existing.ScrappedJson = doc.ScrappedJson;

                                path.ScrappedDocuments[path.ScrappedDocuments.IndexOf(existing)] = existing;


                                // docService.Update(existing);
                            }
                            else
                            {
                                path.ScrappedDocuments.Add(doc);
                            }


                        }
                    }));
                await Task.WhenAll(tasks).ContinueWith(t =>
                {
                    pathService.Update(path);

                });
                return "Done!!!";



            }
            catch (Exception e)
            {

                return e.ToString();
            }
        }

        [System.Web.Http.HttpPost]
        public async Task<JObject> Scrap(FormDataCollection form)
        {
            string url = form["url"];
            string jSchema = form["jSchema"];
            var service = new SchemaService();
            Schema schema = service.GetByBaseUrl(url);
            if (schema == null)
            {
                  schema = new Schema();

            var domain = new DomainService().All().FirstOrDefault(d => url.Contains(d.Code));
            if (domain != null)
            {
                var path = domain.Paths.FirstOrDefault(p => url.Contains(p.Code));
                if (path != null)
                {
                    schema.Path = path;
                }
               
                    schema.Domain = domain;
               
            }
            else
            {
                var path = new Path(){Code = url,Name = url};
                schema.Domain = new Domain.Domain(){Code = url,Name = url,Paths = new List<Path>(){path}};
                schema.Path = path;
            }
            schema.JSchemaStr = jSchema;
                schema.BaseUrl = url;
                schema.Code = url;
                schema = service.AddOrUpdate(schema);
                
            }


            var docService = new ScrappedDocumentService();
            var scrapperService = new ScrapperService();
            var pathService = new PathService();
            var realPath = schema.Path;
            ScrappedDocument doc =null;
            var task = scrapperService.ScrapFromSchemaAsync(schema.ToJSchema(), url).ContinueWith(  t =>
                {
                    if (t.Result != null)
                    {
                
                        doc = new ScrappedDocument()
                              {
                                  UsedSchema = schema,
                                  Name = docService.GetNameForDocument(t.Result, schema),
                                  ScrapIdentifier = url.ToString(),
                                  ScrappedJson = t.Result.ToString(),
                                  Uri = url
                              };
                        var existing = docService.GetByCode(doc.Code);
                        if (existing != null)
                        {
                            existing.UpdateDate = DateTime.Now;
                            existing.ScrappedJson = doc.ScrappedJson;

                            realPath.ScrappedDocuments[realPath.ScrappedDocuments.IndexOf(existing)] = existing;


                            // docService.Update(existing);
                        }
                        else
                        {
                            realPath.ScrappedDocuments.Add(doc);
                        }


                    }
                });
         
            await Task.WhenAll(task).ContinueWith(t => pathService.Update(realPath));

            return doc != null ? JObject.Parse(doc.ScrappedJson) : new JObject(){"Result","Error"};


        }
    }
}
