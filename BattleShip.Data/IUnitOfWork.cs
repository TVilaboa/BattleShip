using System;
using BattleShip.Domain;

namespace BattleShip.Data
{
    public interface IUnitOfWork : IDisposable
    {
        //DbContext Context { get;  set; }

       
        IRepository<User> Users { get; }
              
       
        
        int SaveChanges();

        void InitializeDatabase();
    }
}
