using Common.Model;
using System.Data.Entity;

namespace BankService.DatabaseManagement
{
	public class BankDomainContext : DbContext
	{
		public BankDomainContext() : base("DefaultDomainString")
		{
			Database.SetInitializer<BankCommandingContext>(null);
		}
		public BankDomainContext(string stringConnection) : base(stringConnection)
		{
			Database.SetInitializer<BankCommandingContext>(null);
		}

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Entity<User>()
				.HasOptional<Loan>(x => x.Loan)
				.WithRequired(x => x.User);
			modelBuilder.Entity<BankAccount>()
				.HasOptional(ba => ba.User);
		}

		public DbSet<User> Users { get; set; }
		public DbSet<BankAccount> BankAccounts { get; set; }
		public DbSet<Loan> Loans { get; set; }
	}
}
