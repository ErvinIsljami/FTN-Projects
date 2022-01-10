using Common.Commanding;
using System.ServiceModel;

namespace Common.ServiceInterfaces
{
	public interface IClientServiceCallback
	{
		[OperationContract]
		void SendNotification(CommandNotification commandNotificaiton);
	}
}
