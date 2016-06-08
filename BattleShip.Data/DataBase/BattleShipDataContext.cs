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
        private static BattleShipDataContext _instance;
        public static BattleShipDataContext GetInstance => ((BattleShipDataContext)HttpContext.Current.Items["BattleShipDataContext"]);

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

            HttpContext.Current.Items["BattleShipDataContext"] = new BattleShipDataContext();
            //if (!BattleShipDataContext.GetInstance.Database.Exists())
            BattleShipDataContext.GetInstance.Database.Initialize(true);
            //HttpContext.Current.Items["BattleShipDataContext"] = null;
        }
        
        //public DbSet<User> Users { get; set; }
     

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
