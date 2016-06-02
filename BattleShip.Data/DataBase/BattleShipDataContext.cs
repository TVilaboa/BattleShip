using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Web;
using BattleShip.Domain;
using Elmah.Contrib.EntityFramework;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BattleShip.Data.DataBase
{
    public class BattleShipDataContext : IdentityDbContext<User>
    {
        private static BattleShipDataContext instance;
        public static BattleShipDataContext GetInstance
        {
            get
            {
                return ((BattleShipDataContext)HttpContext.Current.Items["ScrapperDataContext"]);
            }            
        }
        public static BattleShipDataContext Create()
        {
            return new BattleShipDataContext();
        }
        public BattleShipDataContext()
            : base("DefaultConnection")
        {
            this.Configuration.AutoDetectChangesEnabled = true;
            this.Configuration.LazyLoadingEnabled = true;
            this.Configuration.ProxyCreationEnabled = true;
        }

        public static void InitializeDatabase()
        {
            Database.SetInitializer(new BattleShipDataBaseInitializer());

            HttpContext.Current.Items["ScrapperDataContext"] = new BattleShipDataContext();
            //if (!ScrapperDataContext.GetInstance.Database.Exists())
            BattleShipDataContext.GetInstance.Database.Initialize(true);
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
