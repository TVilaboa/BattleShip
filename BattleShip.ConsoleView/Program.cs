using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using Scrapper.Domain;
using Scrapper.Services;

namespace Scrapper.ConsoleView
{
    class Program
    {
        static void Main(string[] args)
        {
            
            Console.WriteLine("Enter base URL");
            string baseUrl = Console.ReadLine();
            Console.WriteLine("Enter min range");
            int min = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter max range");
            int max = int.Parse(Console.ReadLine());
           
            var service = new SchemaService();
            var schema = service.GetByBaseUrl(baseUrl);
            
           
           
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
                    path.ScrappedDocuments.Add(new ScrappedDocument(){UsedSchema = schema,Name = docService.GetNameForDocument(t.Result,schema),ScrapIdentifier = i.ToString(),ScrappedJson = t.Result.ToString(),Uri = baseUrl + i});
                    pathService.Update(path);
                }));
            Task.WhenAll(tasks);
        
          
            Console.ReadLine();
        }
    }
}
