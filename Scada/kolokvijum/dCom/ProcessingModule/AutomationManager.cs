using Common;
using System;
using System.Collections.Generic;
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


		private void AutomationWorker_DoWork()
		{
            PointIdentifier rezervoar = new PointIdentifier(PointType.ANALOG_OUTPUT, 1000);
            PointIdentifier  temperatura= new PointIdentifier(PointType.ANALOG_OUTPUT, 1001);
            PointIdentifier ventil = new PointIdentifier(PointType.DIGITAL_OUTPUT, 2000);
            PointIdentifier grejac = new PointIdentifier(PointType.DIGITAL_OUTPUT, 2002);

            List<IPoint> listaPointa = storage.GetPoints(new List<PointIdentifier> { rezervoar, temperatura, ventil, grejac });

            int heating = 0;

			while (!disposedValue)
			{

                if (listaPointa[3].RawValue == 1)
                {
                    if (listaPointa[1].RawValue < 30)
                        heating = 2;
                    else if (listaPointa[1].RawValue >= 30 && listaPointa[1].RawValue <= 50)
                        heating = 5;
                    else if (listaPointa[1].RawValue > 30)
                        heating = 20;

                    processingManager.ExecuteWriteCommand(listaPointa[1].ConfigItem, configuration.GetTransactionId(), configuration.UnitAddress, 1001, (int)((listaPointa[1] as IAnalogPoint).EguValue + heating));
                    if((int)(listaPointa[1] as IAnalogPoint).EguValue > 57)
                    {
                        processingManager.ExecuteWriteCommand(listaPointa[2].ConfigItem, configuration.GetTransactionId(), configuration.UnitAddress, 2000, 1);
                        processingManager.ExecuteWriteCommand(listaPointa[3].ConfigItem, configuration.GetTransactionId(), configuration.UnitAddress, 2002, 0);
                    }

                }
                else if (listaPointa[2].RawValue == 1)
                {
                    if((int)(listaPointa[0] as IAnalogPoint).EguValue >= 10)
                    {
                        processingManager.ExecuteWriteCommand(listaPointa[0].ConfigItem, configuration.GetTransactionId(), configuration.UnitAddress, 1000, (int)((listaPointa[0] as IAnalogPoint).EguValue - 10));
                        processingManager.ExecuteWriteCommand(listaPointa[1].ConfigItem, configuration.GetTransactionId(), configuration.UnitAddress, 1001, (int)((listaPointa[1] as IAnalogPoint).EguValue - 4));
                    }
                    else
                    {
                        int proporcija = ((int)(listaPointa[0] as IAnalogPoint).EguValue * 4) / 10;
                        processingManager.ExecuteWriteCommand(listaPointa[1].ConfigItem, configuration.GetTransactionId(), configuration.UnitAddress, 1001, (int)((listaPointa[1] as IAnalogPoint).EguValue - proporcija));
                        processingManager.ExecuteWriteCommand(listaPointa[0].ConfigItem, configuration.GetTransactionId(), configuration.UnitAddress, 1000, 0);
                    }
                  


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
