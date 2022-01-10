using PZ3_NetworkService.Commands;
using PZ3_NetworkService.Containers;
using PZ3_NetworkService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PZ3_NetworkService.ViewModels
{
	public class NetworkDataViewModel : BindableBase
	{
		public NetworkDataViewModel()
		{

		}

		// Returns list of errors
		private List<string> CanDeleteReactor(string idString, out int outId)
		{
			List<string> errors = new List<string>();
			outId = -1;

			// Check ID
			if (String.IsNullOrWhiteSpace(idString))
			{
				errors.Add("Item ID must be specified.");
			}
			else
			{
				if (Int32.TryParse(idString, out int id))
				{
					bool reactorExists = false;

					foreach (var r in Container.Reactors)
					{
						if (r.Id == id)
						{
							reactorExists = true;
						}
					}

					if (!reactorExists)
					{
						errors.Add($"Item with ID = {id} does not exist.");
					}
					else
					{
						outId = id;
					}
				}
				else
				{
					errors.Add("You entered an invalid ID.");
				}
			}

			return errors;
		}

		// Returns list of errors from CanDelete
		public List<string> DeleteReactor(string idString)
		{
			List<string> possibleErrors = CanDeleteReactor(idString, out int id);

			if(possibleErrors.Count == 0)
			{
				Reactor reactorToBeDeleted = Container.Reactors.FirstOrDefault(r => r.Id == id);
				DeleteReactorCommand deleteCommand = new DeleteReactorCommand(reactorToBeDeleted);
				Container.CommandInvoker.AddAndExecute(deleteCommand);
			}

			return possibleErrors;
		}

		// Returns list of errors
		private List<string> CanAddReactor(string idString, string name, string reactorTypeString, out Reactor reactor)
		{
			List<string> errors = new List<string>();
			reactor = new Reactor();

			// Check Id
			if (String.IsNullOrWhiteSpace(idString))
			{
				errors.Add("Item ID must be specified.");
			}
			else
			{
				if(Int32.TryParse(idString, out int id))
				{
					bool reactorExists = false;

					foreach(var r in Container.Reactors)
					{
						if(r.Id == id)
						{
							errors.Add($"An item with ID = {id} already exists.");
							reactorExists = true;
							break;
						}
					}

					if (!reactorExists)
					{
						reactor.Id = id;
					}
				}
				else
				{
					errors.Add("You entered an invalid ID.");
				}
			}


			// Check Name
			if (String.IsNullOrWhiteSpace(name))
			{
				errors.Add("Item Name must by specified.");
			}
			else
			{
				reactor.Name = name;
			}

			// Check ReactorType
			if(Container.ReactorTypeToStringMap.TryGetValue(reactorTypeString, out ReactorType reactorType))
			{
				reactor.ReactorType = reactorType;
				reactor.Image = Container.ReactorTypesAndImagesMapper[reactorType];
			}
			else
			{
				errors.Add("You entered an invalid ReactorType.");
			}

			return errors;
		}

		// Returns list of errors from CanAdd
		public List<string> AddReactor(string idString, string name, string reactorTypeString)
		{
			List<string> possibleErrors = CanAddReactor(idString, name, reactorTypeString, out Reactor reactorToBeAdded);

			if(possibleErrors.Count == 0)
			{
				AddReactorCommand addCommand = new AddReactorCommand(reactorToBeAdded);
				Container.CommandInvoker.AddAndExecute(addCommand);
			}

			return possibleErrors;
		}

		public void ClearFilter()
		{
			Container.FilterCollection.Source = Container.Reactors;
		}

		private List<string> CanFilter(string filterType, string lessThanOrGreaterThanOrEqual, string idValue)
		{
			List<string> errors = new List<string>();

			// Check filter type
			if(!Container.ReactorTypeToStringMap.TryGetValue(filterType, out ReactorType reactorType))
			{
				errors.Add("Invalid filter type.");
			}

			if(lessThanOrGreaterThanOrEqual != "LessThan" && lessThanOrGreaterThanOrEqual != "GreaterOrEqual")
			{
				errors.Add("Invalid 'lessThanOrGreaterThanOrEqual' type.");
			}

			if(!Int32.TryParse(idValue, out int id))
			{
				errors.Add("Invalid ID parameter.");
			}

			return errors;
		}

		/// <summary>
		/// Filters datagrid by Reactor type and defined Id value to which Reactor Id will be compared.
		/// </summary>
		/// <param name="filterType">Reactor type by which we will filter</param>
		/// <param name="lessThanOrGreaterAndEqual">'LessThan' to filter values less than 'value', 'GreaterOrEqual' to filter values greater than or equal to 'value'</param>
		/// <param name="idValue">Value of Reactor Id to which we compare all of reactors.</param>
		public List<string> Filter(string filterType, string lessThanOrGreaterThanOrEqual, string idValue)
		{
			List<string> possibleErrors = CanFilter(filterType, lessThanOrGreaterThanOrEqual, idValue);

			if(possibleErrors.Count == 0)
			{
				int id = Int32.Parse(idValue);
				FilterCommand filterCommand = new FilterCommand(filterType, lessThanOrGreaterThanOrEqual, id);
				Container.CommandInvoker.AddAndExecute(filterCommand);
			}

			return possibleErrors;
		}
	}
}
