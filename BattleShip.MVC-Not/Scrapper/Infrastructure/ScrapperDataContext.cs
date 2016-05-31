using System.Data.Entity;
using BattleShip.Domain;

namespace BattleShip.MVC.Scrapper.Infrastructure
{
    public class ScrapperDataContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
    
        public ScrapperDataContext() : base("name=ScrapperDataContext")
        {
        }

        public System.Data.Entity.DbSet<ScrappedDocument> ScrappedDocuments { get; set; }
    }
}
