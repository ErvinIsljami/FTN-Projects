using PZ3_NetworkService.Commands;
using PZ3_NetworkService.Containers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PZ3_NetworkService.ViewModels
{
	public class ContentViewModel : BindableBase
	{
		private string consoleInput;
		ObservableCollection<string> consoleOutput;
		private BindableBase currentViewModel;

		public string ConsoleInput
		{
			get { return consoleInput; }
			set
			{
				SetField(ref consoleInput, value);
			}
		}
		public ObservableCollection<string> ConsoleOutput
		{
			get { return consoleOutput; }
			set
			{
				SetField(ref consoleOutput, value);
			}
		}
		public BindableBase CurrentViewModel
		{
			get { return currentViewModel; }
			set { SetField(ref currentViewModel, value); }
		}
		public NetworkDataViewModel NetworkDataViewModel { get; set; }
		public NetworkViewViewModel NetworkViewViewModel { get; set; }
		public ChartViewModel ChartViewModel { get; set; }

		public MyICommand HandleConsoleInputCommand { get; set; }
		public Dictionary<string, string> CommandsHelp { get; set; }
		public Dictionary<string, Func<string, List<string>>> CommandActionsMap { get; set; }

		public ContentViewModel()
		{
			NetworkDataViewModel = new NetworkDataViewModel();
			NetworkViewViewModel = new NetworkViewViewModel();
			ChartViewModel = new ChartViewModel();
			CurrentViewModel = NetworkDataViewModel;

			ConsoleOutput = new ObservableCollection<string>() { $"Reactors Console [Version 1.0.0]{Environment.NewLine}(c) 2019 Milica's Corporation. All rights reserved.{Environment.NewLine}" };
			CommandsHelp = new Dictionary<string, string>();
			CommandsHelp.Add("add", $"Format: add=id,name,reactorType. Possible reactor types: [{ReactorType.Thermal}, {ReactorType.Fusion}]");
			CommandsHelp.Add("delete", "Format: delete=id");
			CommandsHelp.Add("filter", $"Format: filter=filterType,lessThanOrGreaterThanOrEqual,idValue. {Environment.NewLine}- Possible 'filterType' values: [{ReactorType.Thermal}, {ReactorType.Fusion}]. {Environment.NewLine}- Possible lessThanOrGreaterThanOrEqual values ['LessThan', 'GreaterOrEqual'] {Environment.NewLine}- To clear filter: filter=clear");
			CommandsHelp.Add("navigate", "Format: navigate=view. Possible view values [NetworkData, NetworkView, Chart]");
			CommandsHelp.Add("refreshChart", "Format: refreshChart");
			CommandsHelp.Add("undo", "Format: undo");
			CommandsHelp.Add("help", "Format: help");

			HandleConsoleInputCommand = new MyICommand(OnHandleConsoleInput);

			CommandActionsMap = new Dictionary<string, Func<string, List<string>>>();
			CommandActionsMap.Add("add", HandleAdd);
			CommandActionsMap.Add("delete", HandleDelete);
			CommandActionsMap.Add("filter", HandleFilter);
			CommandActionsMap.Add("navigate", HandleNavigate);
			CommandActionsMap.Add("refreshChart", HandleRefreshChart);
			CommandActionsMap.Add("undo", HandleUndo);
			CommandActionsMap.Add("help", HandleHelp);
		}

		#region Command Handlers
		private List<string> HandleAdd(string unparsedParams)
		{
			List<string> possibleErrors = new List<string>();
			// Command format for adding new reactor
			// add=id,name,reactorType
			string[] addArgs = unparsedParams.Split(',');
			if(addArgs.Length != 3)
			{
				possibleErrors.Add("Wrong number of parameters.");
			}
			else
			{
				foreach(var error in NetworkDataViewModel.AddReactor(addArgs[0], addArgs[1], addArgs[2]))
				{
					possibleErrors.Add(error);
				}
			}

			return possibleErrors;
		}
		private List<string> HandleDelete(string unparsedParams)
		{
			List<string> possibleErrors = new List<string>();
			// Command format for deleting a reactor
			// delete=id
			string[] deleteArgs = unparsedParams.Split(',');
			if(deleteArgs.Length != 1)
			{
				possibleErrors.Add("Wrong number of parameters.");
			}
			else
			{
				foreach(var error in NetworkDataViewModel.DeleteReactor(deleteArgs[0]))
				{
					possibleErrors.Add(error);
				}
			}

			return possibleErrors;
		}
		private List<string> HandleFilter(string unparsedParams)
		{
			List<string> possibleErrors = new List<string>();
			// Command format for filtering
			// filter=filterType,lessThanOrGreaterThanOrEqual,idValue
			// To clear filter
			// filter=clear
			string[] filterArgs = unparsedParams.Split(',');
			if(filterArgs.Length == 1)
			{
				if(filterArgs[0] != "clear")
				{
					possibleErrors.Add("Wrong number of params. Only single parameter action is for 'filter=clear'.");
				}
				else
				{
					NetworkDataViewModel.ClearFilter();
				}
			}
			else if(filterArgs.Length == 3)
			{
				foreach(var error in NetworkDataViewModel.Filter(filterArgs[0], filterArgs[1], filterArgs[2]))
				{
					possibleErrors.Add(error);
				}
			}
			else
			{
				possibleErrors.Add("Wrong number of params.");
			}

			return possibleErrors;
		}
		private List<string> HandleNavigate(string unparsedParams)
		{
			List<string> possibleErrors = new List<string>();
			// Command format for navigating
			// navigate=view
			// possible view values [NetworkData, NetworkView, Chart]
			if(unparsedParams != "NetworkData" && unparsedParams != "NetworkView" && unparsedParams != "Chart")
			{
				possibleErrors.Add("Invalid parameter for command 'navigate'.");
			}
			else
			{
				ExecuteNavCommand(unparsedParams);
			}

			return possibleErrors;
		}
		// This method has this signature just so it can be stored in the command dictionary
		private List<string> HandleUndo(string unparsedParams)
		{
			Container.CommandInvoker.Undo();
			return new List<string>();
		}
		// This method has this signature just so it can be stored in the command dictionary
		private List<string> HandleHelp(string unparsedParams)
		{
			List<string> helpMessages = new List<string>();
			foreach(var commandHelpPair in CommandsHelp)
			{
				helpMessages.Add($"{commandHelpPair.Key} - {commandHelpPair.Value}");
			}
			return helpMessages;
		}
		// This method has this signature just so it can be stored in the command dictionary
		private List<string> HandleRefreshChart(string unparsedParams)
		{
			ChartViewModel.ShowHistoryChart();
			return new List<string>();
		}
		#endregion

		private void AddConsoleOutputEntry(string entry)
		{
			ConsoleOutput.Add(entry);
		}

		private void OnHandleConsoleInput()
		{
			AddConsoleOutputEntry(ConsoleInput);

			string[] splittedInput = ConsoleInput.Split('=');
			string action = "";
			string unparsedParams = "";

			if(splittedInput.Length == 1)
			{
				action = splittedInput[0];
			}

			if(splittedInput.Length == 2)
			{
				action = splittedInput[0];
				unparsedParams = splittedInput[1];
			}

			if(splittedInput.Length > 2)
			{
				AddConsoleOutputEntry("Unknown action, please type 'help' for available actions and their descriptions.");
			}
			else
			{
				if (CommandActionsMap.TryGetValue(action, out Func<string, List<string>> handler))
				{
					List<string> possibleErrors = handler.Invoke(unparsedParams);

					foreach (var error in possibleErrors)
					{
						AddConsoleOutputEntry($"{error}");
					}
				}
				else
				{
					AddConsoleOutputEntry("Unknown action, please type 'help' for available actions and their descriptions.");
				}
			}

			AddConsoleOutputEntry(Environment.NewLine);
			ConsoleInput = String.Empty;
		}

		private void ExecuteNavCommand(string parameter)
		{
			string previousTab = CurrentViewModel.GetType().Name.Replace("ViewModel", "");
			NavCommand navCommand = new NavCommand(this, previousTab, parameter);
			Container.CommandInvoker.AddAndExecute(navCommand);
		}

		public void Navigate(string navigateTo)
		{
			switch (navigateTo)
			{
				case "NetworkData":
					CurrentViewModel = NetworkDataViewModel;
					break;
				case "NetworkView":
					CurrentViewModel = NetworkViewViewModel;
					break;
				case "Chart":
					CurrentViewModel = ChartViewModel;
					ChartViewModel.ShowHistoryChart();
					break;
			}
		}
	}
}
