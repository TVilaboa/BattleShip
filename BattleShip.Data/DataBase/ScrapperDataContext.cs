using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Web;
using BattleShip.Domain;
using Elmah.Contrib.EntityFramework;

namespace BattleShip.Data.DataBase
{
    public class ScrapperDataContext : DbContext
    {
        private static ScrapperDataContext instance;
        public static ScrapperDataContext GetInstance
        {
            get
            {
                return ((ScrapperDataContext)HttpContext.Current.Items["ScrapperDataContext"]);
            }            
        }
        public ScrapperDataContext()
            : base("DataBase")
        {
            this.Configuration.AutoDetectChangesEnabled = true;
            this.Configuration.LazyLoadingEnabled = true;
            this.Configuration.ProxyCreationEnabled = true;
        }

        public static void InitializeDatabase()
        {
            Database.SetInitializer(new ScrapperDataBaseInitializer());

            HttpContext.Current.Items["ScrapperDataContext"] = new ScrapperDataContext();
            //if (!ScrapperDataContext.GetInstance.Database.Exists())
            ScrapperDataContext.GetInstance.Database.Initialize(true);
            //HttpContext.Current.Items["ScrapperDataContext"] = null;
        }
        
        public DbSet<Schema> Schemas { get; set; }
        public DbSet<ScrappedDocument> ScrappedDocuments { get; set; }
        public DbSet<Path> Paths { get; set; }
        public DbSet<Domain.Domain> Domains { get; set; }

        public DbSet<ElmahError> ElmahErrors { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Properties<DateTime>().Configure(c => c.HasColumnType("datetime2"));
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            
            //modelBuilder.Configurations.Add(new UserMap());
            
            
            
        }
    }
}
