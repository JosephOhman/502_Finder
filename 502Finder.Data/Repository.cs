using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace _502Finder.Data
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected DbSet<T> _dbSet;

        public Repository(DataContext dataContext)
        {
            _dbSet = dataContext.Set<T>();
        }

        public void Insert(T entity)
        {
            _dbSet.Add(entity);
        }

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        public IQueryable<T> SearchFor(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.Where(predicate);
        }

        public IQueryable<T> GetTop(int count)
        {
            return _dbSet.Take(count);
        }

        public IQueryable<T> GetAll()
        {
            return _dbSet.AsQueryable();
        }

        public T GetById(int id)
        {
            return _dbSet.Find(id);
        }
    }
}
