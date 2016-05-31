using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;

namespace BattleShip.Data.DataBase
{
    public class GenericRepository<T> : IRepository<T> where T : class
    {
        public GenericRepository(DbContext context)
        {
            this.Context = context;
            this.DbSet = this.Context.Set<T>();
            this.Helper = new GraphHelper(context);
        }

        protected IDbSet<T> DbSet { get; set; }

        protected DbContext Context { get; set; }

        protected GraphHelper Helper { get; set; }

        public virtual IQueryable<T> All()
        {
            return this.DbSet.AsQueryable();
        }

        public virtual T GetById(int id)
        {
            return this.DbSet.Find(id);
        }

        public virtual T GetById(string id)
        {
            return this.DbSet.Find(id);
        }

        public virtual IQueryable<T> FindAll(Expression<Func<T, bool>> match)
        {
            return this.DbSet.Where(match);
        }

        //public virtual IQueryable<T> FindAll(string match, params object[] args)
        //{
        //    return this.DbSet.Where(match, args);
        //}

        public virtual IQueryable<T> FindAll(Expression<Func<T, bool>> match, Expression<Func<T, string>> orderBy, bool direction, int page, int pageSize)
        {
            if (direction)
                return this.DbSet.Where(match).OrderBy(orderBy).Skip((page - 1) * pageSize).Take(pageSize);
            else
                return this.DbSet.Where(match).OrderByDescending(orderBy).Skip((page - 1) * pageSize).Take(pageSize);
        }

        //public virtual IQueryable<T> FindAll(string match, string orderBy, bool direction, int page, int pageSize, params object[] args)
        //{
        //    return this.DbSet.Where(match, args).OrderBy(orderBy + ((direction) ? "" : " descending")).Skip((page - 1) * pageSize).Take(pageSize);
        //}

        public virtual int Count(Expression<Func<T, bool>> match)
        {
            return this.DbSet.Where(match).Count();
        }

        //public virtual int Count(string match, params object[] args)
        //{
        //    return this.DbSet.Where(match, args).Count();
        //}

        public virtual T Find(Expression<Func<T, bool>> match)
        {
            return this.DbSet.Where(match).FirstOrDefault();
        }

        public virtual T Add(T entity,bool saveChanges =false)
        {
            DbEntityEntry entry = this.Context.Entry(entity);
            Helper.UpdateGraph(entity);
            if (entry.State != EntityState.Detached)
            {
                entry.State = EntityState.Added;
            }
            else
            {
                this.DbSet.Add(entity);
            }
             if (saveChanges)
                Context.SaveChanges();
             return entity;
        }

        public virtual T  Update(T entity, bool updateGraph=true,  bool saveChanges =false)
        {
            DbEntityEntry entry = this.Context.Entry(entity);
            if (updateGraph)
            {
                Helper.UpdateGraph(entity);
            }
            if (entry.State == EntityState.Detached)
            {
                this.DbSet.Attach(entity);
            }

            entry.State = EntityState.Modified;

            if (saveChanges)
                Context.SaveChanges();
            return entity;
        }
        
        //public virtual IList<ModifiedProperty> UpdateAndGetModifiedProperties(T entity, bool checkGraph)
        //{
        //    var modifiedProps = new List<ModifiedProperty>();
        //    DbEntityEntry entry = this.Context.Entry(entity);
        //    if (checkGraph)
        //    {
        //        Helper.UpdateGraph(entity);
        //    }
        //    if (entry.State == EntityState.Detached)
        //    {
        //        this.DbSet.Attach(entity);
        //    }
        //    foreach (var property in entry.OriginalValues.PropertyNames)
        //    {
        //        var original = entry.OriginalValues.GetValue<object>(property);

        //        if (original == null || original.GetType().IsPrimitive || original.GetType() == typeof(string))
        //        {
        //            var current = entry.CurrentValues.GetValue<object>(property);
        //            if ((original != null && current == null) || (original == null && current != null) || (original != null && current != null && !original.ToString().Trim().Equals(current.ToString().Trim())))
        //                modifiedProps.Add(new ModifiedProperty
        //                {
        //                    PropertyName = property,
        //                    OldValue = original == null ? null : original.ToString(),
        //                    NewValue = current == null ? null : current.ToString()
        //                });
        //        }
        //    }
        //    entry.State = EntityState.Modified;
        //    return modifiedProps;
        //}

        public virtual T AddOrUpdate(T entity, bool checkGraph = true, bool saveChanges =false)
        {
            DbEntityEntry entry = this.Context.Entry(entity);
            bool isValid = (checkGraph) ? Helper.UpdateGraph(entity) : (entry.State != EntityState.Modified && entry.State != EntityState.Unchanged);

            if (isValid)
            {
                if (entry.State != EntityState.Detached)
                    entry.State = EntityState.Added;
                else
                    this.DbSet.Add(entity);
            }

            if (saveChanges)
                Context.SaveChanges();
            return entity;

        }


        public virtual void Delete(T entity)
        {
            DbEntityEntry entry = this.Context.Entry(entity);
            Helper.UpdateGraph(entity);
            if (entry.State != EntityState.Deleted)
            {
                entry.State = EntityState.Deleted;
            }
            else
            {
                this.DbSet.Attach(entity);
                this.DbSet.Remove(entity);
            }
        }

        public virtual void Delete(int id)
        {
            var entity = this.GetById(id);

            if (entity != null)
            {
                this.Delete(entity);
            }
        }

        public virtual void Detach(T entity)
        {
            DbEntityEntry entry = this.Context.Entry(entity);

            entry.State = EntityState.Detached;
        }
    }
}
