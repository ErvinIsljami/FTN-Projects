using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq.Expressions;
using System.Runtime.Serialization;

namespace Common.Model
{
	[DataContract]
	[Serializable]
	public class IdentifiedObject
	{
		public IdentifiedObject()
		{

		}

		public IdentifiedObject(long id)
		{
			ID = id;
		}

		[DataMember]
		//[DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
		public long ID { get; private set; }

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			IdentifiedObject identifiedObject = obj as IdentifiedObject;
			
			if (identifiedObject == null)
			{
				return false;
			}

			return ID == identifiedObject.ID;
		}
	}
}
