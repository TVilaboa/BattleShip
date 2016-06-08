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
        private readonly Dictionary<Type, object> _repositories = new Dictionary<Type, object>();

        public UnitOfWork() : this( BattleShipDataContext.GetInstance) { }
        public UnitOfWork(BattleShipDataContext context)
        {
            this.Context = context;
        }

        public IRepository<User> Users => this.GetRepository<User>();


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
            BattleShipDataContext.InitializeDatabase();
        }
        private IRepository<T> GetRepository<T>() where T : class
        {
            if (!this._repositories.ContainsKey(typeof(T)))
            {
                var type = typeof(GenericRepository<T>);

                this._repositories.Add(typeof(T), Activator.CreateInstance(type, this.Context));
            }

            return (IRepository<T>)this._repositories[typeof(T)];
        }
    }
}
