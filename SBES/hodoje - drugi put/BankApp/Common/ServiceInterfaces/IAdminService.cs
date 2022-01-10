using Common.Commanding;
using System.ServiceModel;

namespace Common.ServiceInterfaces
{
	[ServiceContract(CallbackContract = typeof(IClientServiceCallback))]
	public interface IAdminService
	{
		/// <summary>
		/// Initializes new database.
		/// </summary>
		[OperationContract]
		CommandNotification CreateNewDatabase();

		/// <summary>
		/// Deletes stale commands.
		/// </summary>
		[OperationContract]
		CommandNotification DeleteStaleCommands();
	}
}
