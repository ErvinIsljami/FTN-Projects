﻿using Common;
using System;
using System.Collections.Generic;
using System.Threading;

namespace ProcessingModule
{
	/// <summary>
	/// Class containing logic for periodic polling.
	/// </summary>
	public class Acquisitor : IDisposable
	{
		private AutoResetEvent acquisitionTrigger;
		private IProcessingManager processingManager;
		private Thread acquisitionWorker;
		private IStateUpdater stateUpdater;
		private IConfiguration configuration;

		/// <summary>
		/// Initializes a new instance of the <see cref="Acquisitor"/> class.
		/// </summary>
		/// <param name="acquisitionTrigger">The acquisition trigger.</param>
		/// <param name="processingManager">The processing manager.</param>
		/// <param name="stateUpdater">The state updater.</param>
		/// <param name="configuration">The configuration.</param>
		public Acquisitor(AutoResetEvent acquisitionTrigger, IProcessingManager processingManager, IStateUpdater stateUpdater, IConfiguration configuration)
		{
			this.stateUpdater = stateUpdater;
			this.acquisitionTrigger = acquisitionTrigger;
			this.processingManager = processingManager;
			this.configuration = configuration;
			this.InitializeAcquisitionThread();
			this.StartAcquisitionThread();
		}

		#region Private Methods

		/// <summary>
		/// Initializes the acquisition thread.
		/// </summary>
		private void InitializeAcquisitionThread()
		{
			this.acquisitionWorker = new Thread(Acquisition_DoWork);
			this.acquisitionWorker.Name = "Acquisition thread";
		}

		/// <summary>
		/// Starts the acquisition thread.
		/// </summary>
		private void StartAcquisitionThread()
		{
			acquisitionWorker.Start();
		}

		/// <summary>
		/// Acquisitor thread logic.
		/// </summary>
		private void Acquisition_DoWork()
		{
			int i = 0;
			int largest = 0;

			List<IConfigItem> items = configuration.GetConfigurationItems();

			while(true)
			{
				i++;
				acquisitionTrigger.WaitOne();

				foreach(IConfigItem item in items)
				{
					if (largest <= item.AcquisitionInterval)
						largest = item.AcquisitionInterval;

					if (item.AcquisitionInterval == i)
						processingManager.ExecuteReadCommand(item, configuration.GetTransactionId(), configuration.UnitAddress, item.StartAddress, item.NumberOfRegisters);
				}

				if (largest == i)
					i = 0;
			}
		}

		#endregion Private Methods

		/// <inheritdoc />
		public void Dispose()
		{
			acquisitionWorker.Abort();
		}
	}
}