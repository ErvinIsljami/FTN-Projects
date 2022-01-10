using BankService.DatabaseManagement.Repositories;

namespace BankService.DatabaseManagement
{
	/// <summary>
	/// Interface which exposes methods of a database management unit.
	/// </summary>
	public interface IDatabaseManager<T> : IRepository<T>
		where T : class
	{

	}
}
