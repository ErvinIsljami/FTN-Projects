using System;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using Common.Model;
using BankService.DatabaseManagement.Repositories;
using System.Linq.Expressions;

namespace BankService.DatabaseManagement
{
	/// <summary>
	/// Unit responsible for database handling.
	/// </summary>
	internal class DatabaseManager<T> : IDatabaseManager<T>, IDisposable
		where T : IdentifiedObject
	{
		private List<T> commandsInDatabase;
		private IRepository<T> dataPersistence;
		private ReaderWriterLockSlim locker;
		/// <summary>
		/// Initializes new instance of <see cref="DatabaseManager"/> class. 
		/// </summary>
		/// <param name="dataPersistence">Unit used for data persistence.</param>
		public DatabaseManager(IRepository<T> dataPersistence)
		{
			this.dataPersistence = dataPersistence;

			locker = new ReaderWriterLockSlim();

			commandsInDatabase = new List<T>();
		}

		public void AddEntity(T entity)
		{
			locker.EnterWriteLock();

			dataPersistence?.AddEntity(entity);

			if (commandsInDatabase.Exists(x => x.ID == entity.ID))
			{
				locker.ExitWriteLock();

				// log ERROR
				Console.WriteLine($"[DatabaseManager] Command with {entity.ID} already exists in the database.");
				return;
			}

			commandsInDatabase.Add(entity);
			

			locker.ExitWriteLock();
			// log successful add to DB
		}

		public void Dispose()
		{
			locker.Dispose();
			((IDisposable)dataPersistence).Dispose();
			commandsInDatabase.Clear();
		}

		public IEnumerable<T> Find(Expression<Func<T, bool>> predicate)
		{
			IEnumerable<T> foundEntities;

			locker.EnterReadLock();

			foundEntities = dataPersistence.Find(predicate);

			locker.ExitReadLock();

			return foundEntities;
		}

		public T FindEntity(Expression<Func<T, bool>> predicate)
		{
			T foundEntity;

			locker.EnterReadLock();

			foundEntity = dataPersistence.FindEntity(predicate);

			locker.ExitReadLock();

			return foundEntity;
		}

		public T Get(long id)
		{
			T entity;
			locker.EnterReadLock();

			entity = dataPersistence.Get(id);

			locker.ExitReadLock();

			return entity;
		}

		/// <inheritdoc/>
		public List<T> ReadAllEntities()
		{
			List<T> commandsInDatabase;
			locker.EnterReadLock();
			
			commandsInDatabase = new List<T>(this.commandsInDatabase);

			locker.ExitReadLock();

			return commandsInDatabase;
		}

		/// <inheritdoc/>
		public void RemoveEntity(long commandId)
		{
			locker.EnterReadLock();

			T commandInDB = commandsInDatabase.FirstOrDefault(x => x.ID == commandId);

			locker.ExitReadLock();

			if (commandInDB == null)
			{
				Console.WriteLine($"[ERROR][DatabaseManager] Command with {commandId} does not exist in database.");

				//log ERROR
			}

			locker.EnterWriteLock();

			commandsInDatabase.Remove(commandInDB);
			dataPersistence?.RemoveEntity(commandId);

			locker.ExitWriteLock();

			// log successful remove
		}
		public void Update(T entity)
		{
			locker.EnterWriteLock();

			dataPersistence.Update(entity);

			locker.ExitWriteLock();
		}
	}
}
