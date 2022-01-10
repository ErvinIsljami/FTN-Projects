using System.Runtime.Serialization;

[DataContract(Name = "TransactionType")]
public enum TransactionType
{
	[EnumMember]
	Deposit,
	[EnumMember]
	Withdraw
}