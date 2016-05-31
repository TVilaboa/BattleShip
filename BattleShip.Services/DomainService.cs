using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using BattleShip.Data;

namespace BattleShip.Services
{
    public class DomainService
    {
        public DomainService()
        {
            UnitOfWork = new UnitOfWork();

        }

        UnitOfWork UnitOfWork { get; set; }

        public IList<Domain.Domain> All()
        {
            return UnitOfWork.Domains.All().ToList();
        }

        public IList<Domain.Domain> FindAll(Expression<Func<Domain.Domain, bool>> match)
        {
            return UnitOfWork.Domains.FindAll(match).ToList();
        }

        public Domain.Domain Get(int id)
        {
            return UnitOfWork.Domains.GetById(id);
        }

        public Domain.Domain Add(Domain.Domain domain)
        {
            return UnitOfWork.Domains.Add(domain, true);
        }

        public Domain.Domain GetByCode(string code)
        {
            return UnitOfWork.Domains.Find(s => s.Code == code);
        }
    }
}