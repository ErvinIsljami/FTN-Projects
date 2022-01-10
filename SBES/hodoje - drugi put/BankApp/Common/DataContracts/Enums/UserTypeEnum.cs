using System.Runtime.Serialization;
[DataContract(Name = "UserType")]
public enum UserType
{
	[EnumMember]
	Admin,
	[EnumMember]
	Customer
}