using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using BattleShip.Data;
using BattleShip.Domain;

namespace BattleShip.Services
{
    public class UserService
    {
        public UserService()
        {
            UnitOfWork = new UnitOfWork();

        }

        UnitOfWork UnitOfWork { get; set; }

        public IQueryable<User> All()
        {
            return UnitOfWork.Users.All();
        }

        public IQueryable<User> FindAll(Expression<Func<User, bool>> match)
        {
            return UnitOfWork.Users.FindAll(match);
        }

        public User Find(Expression<Func<User, bool>> match)
        {
            return UnitOfWork.Users.Find(match);
        }

        public User Get(int id)
        {
            return UnitOfWork.Users.GetById(id);
        }

        public User AddOrUpdate(User user)
        {
            return UnitOfWork.Users.AddOrUpdate(user, false,true);
        }

        
    }
}