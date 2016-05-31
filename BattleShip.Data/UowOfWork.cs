using System;
using System.Collections.Generic;
using System.Data.Entity;
using BattleShip.Data.DataBase;
using BattleShip.Domain;

namespace BattleShip.Data
{
 


    public class UnitOfWork : IUnitOfWork
    {
        DbContext Context { get; set; }
        private readonly Dictionary<Type, object> repositories = new Dictionary<Type, object>();

        public UnitOfWork() : this( ScrapperDataContext.GetInstance) { }
        public UnitOfWork(ScrapperDataContext context)
        {
            this.Context = context;
        }

        public IRepository<Schema> Schemas
        {
            get
            {
                return this.GetRepository<Schema>();
            }
        }

        public IRepository<ScrappedDocument> ScrappedDocuments
        {
            get
            {
                return this.GetRepository<ScrappedDocument>();
            }
        }

        public IRepository<Domain.Domain> Domains
        {
            get
            {
                return this.GetRepository<Domain.Domain>();
            }
        }

        public IRepository<Path> Paths
        {
            get
            {
                return this.GetRepository<Path>();
            }
        }
       

        public int SaveChanges()
        {
            return this.Context.SaveChanges();
        }
        public void Dispose()
        {
            if (this.Context != null)
            {
                this.Context.Dispose();
            }
        }
        public void InitializeDatabase()
        {
            ScrapperDataContext.InitializeDatabase();
        }
        private IRepository<T> GetRepository<T>() where T : class
        {
            if (!this.repositories.ContainsKey(typeof(T)))
            {
                var type = typeof(GenericRepository<T>);

                this.repositories.Add(typeof(T), Activator.CreateInstance(type, this.Context));
            }

            return (IRepository<T>)this.repositories[typeof(T)];
        }
    }
}
