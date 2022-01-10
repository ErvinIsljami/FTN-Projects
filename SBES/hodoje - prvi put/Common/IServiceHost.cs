namespace BankServiceApp.ServiceHosts
{
    public interface IServiceHost
    {
        void OpenService();

        void CloseService();
    }
}