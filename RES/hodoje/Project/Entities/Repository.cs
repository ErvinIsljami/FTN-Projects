using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Repository<T, V> : IRepository<T, V> where T : class
    {
        protected readonly DbContext _context;
        private DbSet<T> _entities;

        public Repository(DbContext context)
        {
            _context = context;
            _entities = _context.Set<T>();
        }

        public T GetById(V id)
        {
            T tEntity = null;
            if (id != null)
            {
                try
                {
                    tEntity = _entities.Find(id);
                }
                catch (InvalidOperationException)
                {
                    throw;
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return tEntity;
        }

        public IEnumerable<T> GetAll()
        {
            return _entities.ToList();
        }

        public IEnumerable<T> Find(Expression<Func<T, bool>> predicate)
        {
            IEnumerable<T> tEntityEnumerable = new List<T>();
            if (predicate != null)
            {
                tEntityEnumerable = _entities.Where(predicate).ToList();
            }
            return tEntityEnumerable;
        }

        public T Add(T entity)
        {
            if (entity != null)
            {
                if (_entities.FirstOrDefault(x => x.Equals(entity)) == null)
                {
                    _entities.Add(entity);
                    return entity;
                }
            }
            return entity;
        }

        public void AddRange(IEnumerable<T> entities)
        {
            if (entities != null)
            {
                _entities.AddRange(entities);
            }
        }

        public void Remove(T entity)
        {
            if (entity != null)
            {
                _entities.Remove(entity);
            }
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            if (entities != null)
            {
                _entities.RemoveRange(entities);
            }
        }
    }
}
