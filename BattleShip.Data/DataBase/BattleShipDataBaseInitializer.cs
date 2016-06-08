using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using BattleShip.Domain;
using Path = BattleShip.Domain.Path;

namespace BattleShip.Data.DataBase
{
    public class BattleShipDataBaseInitializer : DropCreateDatabaseIfModelChanges<BattleShipDataContext>
    {

        protected override void Seed(BattleShipDataContext context)
        {
            //SeedDomainAndPaths(context);

            //SeedSchemas(context);

            SeedELMAHSps(context);

            base.Seed(context);
        }

        //private List<Domain.Domain> SeedDomainAndPaths(BattleShipDataContext context)
        //{
        //    List<Domain.Domain> domains = new List<Domain.Domain>();
        //    List<Path> laNacionPaths = new List<Path>() { new Path() { Code = "article", Name = "La Nacion Article" } };
        //    domains.Add(new Domain.Domain() {Code = "lanacion", Name = "La Nacion", Paths = laNacionPaths});
        //    List<Path> vademecumPaths = new List<Path>() { new Path() { Code = "product", Name = "Vademecum Product" } };
        //    domains.Add(new Domain.Domain() { Code = "prvademecum", Name = "Vademecum", Paths = vademecumPaths });

        //    context.Domains.AddRange(domains);
        //    context.SaveChanges();
        //    return domains;
        //}

        //private void SeedSchemas(BattleShipDataContext context)
        //{
        //    var lanacionDomain = context.Domains.First(d => d.Code == "lanacion");
        //    var articlePath = lanacionDomain.Paths.First(p => p.Code == "article");
        //    var vademecumDomain = context.Domains.First(d => d.Code == "prvademecum");
        //    var productPath = vademecumDomain.Paths.First(p => p.Code == "product");
        //    context.Schemas.Add(new Schema() { JSchemaStr = System.IO.File.ReadAllText(HttpContext.Current.Server.MapPath("Files/LaNacionSchema.txt")), Code = "LaNacion", BaseUrl = "http://www.lanacion.com.ar/", Domain = lanacionDomain, Path = articlePath });
        //    context.Schemas.Add(new Schema() { JSchemaStr = System.IO.File.ReadAllText(HttpContext.Current.Server.MapPath("Files/VademecumSchema.txt")), Code = "Vademecum", BaseUrl = "http://ar.prvademecum.com/producto.php?producto=", Domain = vademecumDomain, Path = productPath, SecondPropertyForTitle = "laboratory" });
        //    context.SaveChanges();
        //}

        private void SeedELMAHSps(BattleShipDataContext context)
        {
            // Add Stored Procedures
            foreach (var file in Directory.GetFiles(HttpContext.Current.Server.MapPath("Files/ElmahSps"), "*.sql"))
            {
                context.Database.ExecuteSqlCommand(File.ReadAllText(file), new object[0]);
            }
        }
    }
}
