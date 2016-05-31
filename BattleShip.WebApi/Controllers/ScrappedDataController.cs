using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Web.Http;
using BattleShip.Common;
using BattleShip.Domain;
using BattleShip.Services;
using Newtonsoft.Json.Linq;
using Serialize.Linq.Nodes;

namespace BattleShip.WebApi.Controllers
{

[System.Web.Http.RoutePrefix("api/scrappedData")]
    public class ScrappedDataController : ApiController
    {
        [System.Web.Http.HttpGet]
        // GET api/values
        public List<ExpandoObject> Get()
        {
            List<ExpandoObject> response = new List<ExpandoObject>();
            foreach (var domain in new DomainService().All())
            {
                dynamic expando = new ExpandoObject();
                expando.url = domain.Code;
                expando.name = domain.Name;
                response.Add(expando);
            }
            return response;
        }

          
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("{domain}")]
        // GET api/values/5
        public List<ExpandoObject> Get(string domain)
        {
            List<ExpandoObject> response = new List<ExpandoObject>();
            var domainByCode = new DomainService().GetByCode(domain);
            foreach (var path in domainByCode.Paths)
            {
                dynamic expando = new ExpandoObject();
                expando.url = domainByCode.Code + "/" + path.Code;
                expando.name = path.Name;
                response.Add(expando);
            }
            return response;
        }
         [System.Web.Http.Route("{domain}/{path}")]
        [System.Web.Http.HttpGet]
        // GET api/values/5
        public List<ExpandoObject> Get(string domain, string path)
        {
            List<ExpandoObject> response = new List<ExpandoObject>();
             var domainByCode = new DomainService().GetByCode(domain);
             var pathFind = domainByCode.Paths.Find(p => p.Code ==  path);
             foreach (var doc in pathFind.ScrappedDocuments)
            {
                dynamic expando = new ExpandoObject();
                expando.url = domainByCode.Code + "/" + pathFind.Code + "/" + doc.Name.ToSlug() + "-" + doc.Code;
                expando.Name = doc.Name;
                response.Add(expando);
            }
            return response;
            
        }

         [System.Web.Http.Route("{domain}/{path}/{sha}")]
        [System.Web.Http.HttpGet]
        // GET api/values/5
        public JObject Get(string domain, string path,string sha)
         {
             var doc =
                 new DomainService().GetByCode(domain)
                     .Paths.Find(p => p.Code == path)
                     .ScrappedDocuments.Find(
                         d => d.Code == sha.Substring(sha.LastIndexOf("-", StringComparison.Ordinal) + 1));
            return JObject.Parse(doc.ScrappedJson);
        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("FindAll")]
        public IList<ScrappedDocument> FindAll([FromBody] ExpressionNode query)
    {
          
       
            try
            {
                var expression = query.ToBooleanExpression<ScrappedDocument>();
                return new ScrappedDocumentService().FindAll(expression);
            }
            catch (Exception ex)
            {
                //Console.Error.WriteLine(ex);
                return null;
            }
        
    }

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("Find")]
        public ScrappedDocument Find([FromBody] ExpressionNode query)
        {


            try
            {
                var expression = query.ToBooleanExpression<ScrappedDocument>();
                return new ScrappedDocumentService().Find(expression);
            }
            catch (Exception ex)
            {
                //Console.Error.WriteLine(ex);
                return null;
            }

        }

    }
}