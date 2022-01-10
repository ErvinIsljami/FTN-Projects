using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace BankService.DatabaseManagement.Repositories
{
	/// <summary>
	/// Used to expose methods for unit which is responsible for saving data to permanent storage.
	/// </summary>
	public interface IRepository<TEntity> where TEntity : class
	{
		/// <summary>
		/// Gets entity from database by its ID.
		/// </summary>
		/// <param name="id">Entities ID.</param>
		/// <returns>Entity if it is found in database, otherwise <b>null</b>.</returns> 
		TEntity Get(long id);

		/// <summary>
		/// Get all entities from database which satisfy expression.
		/// </summary>
		/// <param name="predicate">Comparison expression.</param>
		/// <returns>Entities which satisfy comparison predicate.</returns>
		IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);

		/// <summary>
		/// Get all entities from database which satisfy expression.
		/// </summary>
		/// <param name="predicate">Comparison expression.</param>
		/// <returns>Entities which satisfy comparison predicate.</returns>
		TEntity FindEntity(Expression<Func<TEntity, bool>> predicate);

		/// <summary>
		/// Save entity to permanent storage.
		/// </summary>
		/// <param name="entity">Command to save.</param>
		void AddEntity(TEntity entity);

		/// <summary>
		/// Removes entity from permanent storage by entities id.
		/// </summary>
		/// <param name="entityId">ID of the entity to be removed.</param>
		void RemoveEntity(long entityId);

		/// <summary>
		/// Updates entity from permanent storage.
		/// </summary>
		/// <param name="entity">Entity to update.</param>
		void Update(TEntity entity);

		/// <summary>
		/// Read all entities from permanent storage.
		/// </summary>
		/// <returns>List of entities in permanent storage.</returns>
		List<TEntity> ReadAllEntities();
	}
}
