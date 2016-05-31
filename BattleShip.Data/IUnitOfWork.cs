using System;
using BattleShip.Domain;

namespace BattleShip.Data
{
    public interface IUnitOfWork : IDisposable
    {
        //DbContext Context { get;  set; }

       
        IRepository<Schema> Schemas { get; }
              
       
        
        int SaveChanges();

        void InitializeDatabase();
    }
}
