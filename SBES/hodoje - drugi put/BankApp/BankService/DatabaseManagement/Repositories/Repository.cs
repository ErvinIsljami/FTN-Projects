using Common.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;

namespace BankService.DatabaseManagement.Repositories
{
	public class Repository<TEntity> : IRepository<TEntity>
		where TEntity : IdentifiedObject
	{
		private readonly SemaphoreSlim synchronization;
		protected readonly DbContext dbContext;

		private bool TryOpenConnectionIfClosed()
		{
			if(dbContext.Database.Connection.State != System.Data.ConnectionState.Open)
			{
				try
				{
					dbContext.Database.Connection.Open();
					return true;
				}
				catch(Exception e)
				{
					return false;
					//throw new ObjectDisposedException("Database connection was not opened!", e);
				}
			}
			else
			{
				return true;
			}
		}

		public Repository(DbContext dbContext, SemaphoreSlim synchronization)
		{
			this.dbContext = dbContext;
			//this.dbContext.ChangeTracker.DetectChanges();
			this.synchronization = synchronization;
		}

		public void AddEntity(TEntity entity)
		{
			if (!TryOpenConnectionIfClosed())
			{
				return;
			}

			synchronization.Wait();

			dbContext.Set<TEntity>().Add(entity);
			dbContext.SaveChanges();
			synchronization.Release();
		}

		public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
		{
			if (!TryOpenConnectionIfClosed())
			{
				return new List<TEntity>(0);
			}

			return dbContext.Set<TEntity>().Where(predicate).ToList();
		}

		public TEntity Get(long id)
		{
			if (!TryOpenConnectionIfClosed())
			{
				return null;
			}

			return dbContext.Set<TEntity>().FirstOrDefault(x => x.ID == id);
		}

		public List<TEntity> ReadAllEntities()
		{
			if (!TryOpenConnectionIfClosed())
			{
				return new List<TEntity>(0);
			}

			return dbContext.Set<TEntity>().ToList();
		}

		public void RemoveEntity(long entityId)
		{
			if (!TryOpenConnectionIfClosed())
			{
				return;
			}

			synchronization.Wait();

			dbContext.Set<TEntity>().Remove(Get(entityId));
			dbContext.SaveChanges();

			synchronization.Release();
		}

		public void Update(TEntity entity)
		{
			if (!TryOpenConnectionIfClosed())
			{
				return;
			}

			synchronization.Wait();

			dbContext.Set<TEntity>().Attach(entity);
			dbContext.Entry(entity).State = EntityState.Modified;
			dbContext.SaveChanges();

			synchronization.Release();
		}

		public TEntity FindEntity(Expression<Func<TEntity, bool>> predicate)
		{
			if (!TryOpenConnectionIfClosed())
			{
				return default(TEntity);
			}

			return dbContext.Set<TEntity>().Where(predicate).FirstOrDefault();
		}
	}
}