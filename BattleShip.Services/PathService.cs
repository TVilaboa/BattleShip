using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using BattleShip.Data;
using BattleShip.Domain;

namespace BattleShip.Services
{
    public class PathService
    {
        public PathService()
        {
            UnitOfWork = new UnitOfWork();

        }

        UnitOfWork UnitOfWork { get; set; }

        public IList<Path> All()
        {
            return UnitOfWork.Paths.All().ToList();
        }

        public IList<Path> FindAll(Expression<Func<Path, bool>> match)
        {
            return UnitOfWork.Paths.FindAll(match).ToList();
        }

        public Path Get(int id)
        {
            return UnitOfWork.Paths.GetById(id);
        }

        public Path Add(Path path)
        {
            return UnitOfWork.Paths.Add(path, true);
        }

        public Path GetByCode(string code)
        {
            return UnitOfWork.Paths.Find(s => s.Code == code);
        }

        public Path Update(Path path)
        {
            return UnitOfWork.Paths.Update(path, false, true);
        }
    }
}