using Common;
using Common.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemController.Services
{
	public class GeneratorIdService : IGeneratorIdService
	{
		public string GenerateGeneratorId()
		{
			string generatorGuid = "";
			bool isNewIdValid = true;
			do
			{
				generatorGuid = Guid.NewGuid().ToString();
				foreach (var lc in DataAccessHelper.Instance.LCs)
				{
					if (lc.Generators.FirstOrDefault(g => g.Id == generatorGuid) != null)
					{
						isNewIdValid = false;
						break;
					}
				}
			} while (!isNewIdValid);

			return generatorGuid;
		}
	}
}
