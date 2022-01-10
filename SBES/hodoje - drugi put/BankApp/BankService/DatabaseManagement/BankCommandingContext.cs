using Common.Commanding;
using System.Data.Entity;

namespace BankService.DatabaseManagement
{
	public class BankCommandingContext : DbContext
	{
		public BankCommandingContext() : base("DefaultCommandingConnection")
		{
			Database.SetInitializer<BankCommandingContext>(null);
		}
		public BankCommandingContext(string stringConnection) : base(stringConnection)
		{
			Database.SetInitializer<BankCommandingContext>(null);
		}

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Entity<NotificationInformation>()
			   .Ignore(b => b.UserCallback);
		}

		public DbSet<BaseCommand> Commands { get; set; }
		public DbSet<NotificationInformation> Notifications { get; set; }
		public DbSet<CommandNotification> ReadyNotifications { get; set; }
	}
}
