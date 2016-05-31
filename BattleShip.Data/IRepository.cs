using System;
using System.Linq;
using System.Linq.Expressions;

namespace BattleShip.Data
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> All();
        T GetById(int id);
        T GetById(string id);
        IQueryable<T> FindAll(Expression<Func<T, bool>> match);
        //IQueryable<T> FindAll(string match, params object[] args);
        IQueryable<T> FindAll(Expression<Func<T, bool>> match, Expression<Func<T, string>> orderBy, bool direction, int page, int pageSize);
        //IQueryable<T> FindAll(string match, string orderBy, bool direction, int page, int pageSize, params object[] args);

        int Count(Expression<Func<T, bool>> match);
        //int Count(string match, params object[] args);

        T Find(Expression<Func<T, bool>> match);

        T Add(T entity, bool saveChanges = false);


        T Update(T entity, bool updateGraph=true, bool saveChanges =false);

        T  AddOrUpdate(T entity, bool checkGraph = true, bool saveChanges = false);

       
        void Delete(T entity);

        void Delete(int id);

        void Detach(T entity);

        //IList<ModifiedProperty> UpdateAndGetModifiedProperties(T entity, bool checkGraph);
    }
}
