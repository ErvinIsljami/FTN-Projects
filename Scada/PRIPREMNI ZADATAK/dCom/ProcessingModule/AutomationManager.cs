using Common;
using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography;
using System.Threading;

namespace ProcessingModule
{
	/// <summary>
	/// Class containing logic for automated work.
	/// </summary>
	public class AutomationManager : IAutomationManager, IDisposable
	{
		private Thread automationWorker;
		private AutoResetEvent automationTrigger;
		private IStorage storage;
		private IProcessingManager processingManager;
		private int delayBetweenCommands;
		private IConfiguration configuration;

		/// <summary>
		/// Initializes a new instance of the <see cref="AutomationManager"/> class.
		/// </summary>
		/// <param name="storage">The storage.</param>
		/// <param name="processingManager">The processing manager.</param>
		/// <param name="automationTrigger">The automation trigger.</param>
		/// <param name="configuration">The configuration.</param>
		public AutomationManager(IStorage storage, IProcessingManager processingManager, AutoResetEvent automationTrigger, IConfiguration configuration)
		{
			this.storage = storage;
			this.processingManager = processingManager;
			this.configuration = configuration;
			this.automationTrigger = automationTrigger;
		}

		/// <summary>
		/// Initializes and starts the threads.
		/// </summary>
		private void InitializeAndStartThreads()
		{
			InitializeAutomationWorkerThread();
			StartAutomationWorkerThread();
		}

		/// <summary>
		/// Initializes the automation worker thread.
		/// </summary>
		private void InitializeAutomationWorkerThread()
		{
			automationWorker = new Thread(AutomationWorker_DoWork);
			automationWorker.Name = "Aumation Thread";
		}

		/// <summary>
		/// Starts the automation worker thread.
		/// </summary>
		private void StartAutomationWorkerThread()
		{
			automationWorker.Start();
		}

		private ushort GetHeatingValue(IPoint tempVazduha)
		{
			if (tempVazduha.RawValue < 30)
				return 2;
			else if (tempVazduha.RawValue > 30 && tempVazduha.RawValue < 50)
				return 5;
			else
				return 20;
		}

		private void AutomationWorker_DoWork()
		{
			PointIdentifier p1 = new PointIdentifier(PointType.ANALOG_OUTPUT, 1000);
			PointIdentifier p2 = new PointIdentifier(PointType.ANALOG_OUTPUT, 1001);
			PointIdentifier p3 = new PointIdentifier(PointType.DIGITAL_OUTPUT, 2000);
			PointIdentifier p4 = new PointIdentifier(PointType.DIGITAL_OUTPUT, 2002);

			List<IPoint> points = storage.GetPoints(new List<PointIdentifier> { p1, p2, p3, p4 });

			IPoint NIVO_VODE = points[0];
			IPoint TEMP_VAZDUHA = points[1];
			IPoint VENTIL = points[2];
			IPoint GREJAC = points[3];

			while (!disposedValue)
			{
				if (GREJAC.RawValue == 1)
				{
					ushort newTemperature = (ushort)(GetHeatingValue(TEMP_VAZDUHA) + TEMP_VAZDUHA.RawValue);

					// temperatura prebacila TRESHOLD (57*C), VENTIL SE AUTOMATSKI OTVARA
					if (newTemperature > 57)
					{
						processingManager.ExecuteWriteCommand(VENTIL.ConfigItem, configuration.GetTransactionId(), configuration.UnitAddress, 2000, 1);
						processingManager.ExecuteWriteCommand(GREJAC.ConfigItem, configuration.GetTransactionId(), configuration.UnitAddress, 2002, 0);
					}
					processingManager.ExecuteWriteCommand(TEMP_VAZDUHA.ConfigItem, configuration.GetTransactionId(), configuration.UnitAddress, 1001, (int)newTemperature);
				}
				else
				{
					processingManager.ExecuteWriteCommand(TEMP_VAZDUHA.ConfigItem, configuration.GetTransactionId(), configuration.UnitAddress, 1001, (int)TEMP_VAZDUHA.RawValue);
				}

				if (VENTIL.RawValue == 1)
				{
					if (NIVO_VODE.RawValue >= 10)
					{
						ushort newTemperature = (ushort)(TEMP_VAZDUHA.RawValue - 4);
						ushort newWaterLevel = (ushort)(NIVO_VODE.RawValue - 10);

						processingManager.ExecuteWriteCommand(NIVO_VODE.ConfigItem, configuration.GetTransactionId(), configuration.UnitAddress, 1000, (int)newWaterLevel);
						processingManager.ExecuteWriteCommand(NIVO_VODE.ConfigItem, configuration.GetTransactionId(), configuration.UnitAddress, 1001, (int)newTemperature);

						if(TEMP_VAZDUHA.RawValue <= 24)
							processingManager.ExecuteWriteCommand(GREJAC.ConfigItem, configuration.GetTransactionId(), configuration.UnitAddress, 2000, 0);
					}
				}
				else if(VENTIL.RawValue == 0 && GREJAC.RawValue == 0)
				{
					processingManager.ExecuteWriteCommand(TEMP_VAZDUHA.ConfigItem, configuration.GetTransactionId(), configuration.UnitAddress, 1000, (int)NIVO_VODE.RawValue);
					processingManager.ExecuteWriteCommand(TEMP_VAZDUHA.ConfigItem, configuration.GetTransactionId(), configuration.UnitAddress, 1001, (int)TEMP_VAZDUHA.RawValue);
				}

				automationTrigger.WaitOne();
			}
			

		}

		#region IDisposable Support
		private bool disposedValue = false; // To detect redundant calls


		/// <summary>
		/// Disposes the object.
		/// </summary>
		/// <param name="disposing">Indication if managed objects should be disposed.</param>
		protected virtual void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
				}
				disposedValue = true;
			}
		}


		// This code added to correctly implement the disposable pattern.
		public void Dispose()
		{
			// Do not change this code. Put cleanup code in Dispose(bool disposing) above.
			Dispose(true);
			// GC.SuppressFinalize(this);
		}

		/// <inheritdoc />
		public void Start(int delayBetweenCommands)
		{
			InitializeAndStartThreads();
		}

		/// <inheritdoc />
		public void Stop()
		{
			Dispose();
		}
		#endregion
	}
}
