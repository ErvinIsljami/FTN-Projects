using System;
using System.Text.RegularExpressions;

namespace Common.Communication
{
	public static class StringFormatter
	{
		public static string ParseName(string winLogonName)
		{
			string[] parts = new string[] { };

			if (winLogonName.Contains("@"))
			{
				///UPN format
				parts = winLogonName.Split('@');
				return parts[0];
			}
			else if (winLogonName.Contains("\\"))
			{
				/// SPN format
				parts = winLogonName.Split('\\');
				return parts[1];
			}
			else
			{
				return winLogonName;
			}
		}

		public static string GetAttributeFromSubjetName(string subjectName, string attributeToLookFor)
		{
			string lookupString = $"{attributeToLookFor}=";
			foreach (string attribute in subjectName.Split(new string[] { ", ", ";"}, StringSplitOptions.None))
			{
				if (attribute.Contains(lookupString))
				{
					return attribute.Substring(attribute.IndexOf('=') + 1);
				}
			}

			return String.Empty;
		}
	}
}
