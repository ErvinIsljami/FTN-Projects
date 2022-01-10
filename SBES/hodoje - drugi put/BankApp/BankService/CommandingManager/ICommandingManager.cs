using Common.Commanding;
using Common.ServiceInterfaces;

namespace BankService.CommandingManager
{
	/// <summary>
	/// Unit responsible for sending commands and response handling.
	/// </summary>
	public interface ICommandingManager
	{
		/// <summary>
		/// Enqueues command on the specific commanding queue.
		/// </summary>
		/// <param name="command"></param>
		long EnqueueCommand(BaseCommand command);

		/// <summary>
		/// Creates new database.
		/// </summary>
		void CreateDatabase();

		/// <summary>
		/// Deletes commands which are timed out.
		/// </summary>
		void ClearStaleCommands();

		/// <summary>
		/// Starts listening for connected sectors.
		/// </summary>
		void StartListeningForConnectedSectors();

		/// <summary>
		/// Starts listening for pings comming from sectors.
		/// </summary>
		void StartListeningForAlivePings();
	}
}
