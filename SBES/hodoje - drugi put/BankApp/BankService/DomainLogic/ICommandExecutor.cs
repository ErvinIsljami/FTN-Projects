using Common.Model;
using System.Collections.Generic;

namespace BankService.CommandExecutor
{
	/// <summary>
	/// Interface exposes methods for unit which is responsible for executing domain logic.
	/// </summary>
	public interface ICommandExecutor
    {
        void Start();
        void Stop();
		void CreateDatabase();
		List<BankAccount> GetUsersAccount(string username);
    }
}
