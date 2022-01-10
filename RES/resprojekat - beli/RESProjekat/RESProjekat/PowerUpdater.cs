using Common;
using Common.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LocalController
{
    public class PowerUpdater
    {
        private static object padlock = new object();
        private static List<SetpointArray> setpoints = null;
        private static int setPointCounter = 0;
        private static Task updateGeneratorValuesTask;
		public Window DedicatedLc { get; set; }
        public static void UpdateSetpoints(List<SetpointArray> newArray)
        {
            lock(padlock)
            {
                setpoints = newArray;
                setPointCounter = 0;
            }
        }

        public PowerUpdater(Window dedicatedLc)
        {
			DedicatedLc = dedicatedLc;
			updateGeneratorValuesTask = new Task(() => UpdateGeneratorValues());
            updateGeneratorValuesTask.Start();
        }

        public async void UpdateGeneratorValues()
        {
            while(true)
            {
				if(DedicatedLc != null)
				{
					foreach (Generator g in (DedicatedLc as dynamic).LocalGenerators)
					{
						if (g.ControlType == ControlTypeEnum.LOCAL)
						{
							g.AzurirajAktivnuSnagu();
						}
					}
                    if (setpoints == null)
                    {
                        await Task.Delay(1000);
                        continue;
                    }

					lock (padlock)
					{
						foreach (var spArray in setpoints)
						{
							string id = spArray.Array[0].GenId;
							Generator g = ((DedicatedLc as dynamic).LocalGenerators as ObservableCollection<Generator>).FirstOrDefault(x => x.Id == id);
							if(g != null)
							{
								g.ManuallySetActivePower(spArray.Array[setPointCounter].Value);
							}							
						}

						// TODO OVA APLIKACIJA CE SE UGASITI SAMA OD SEBE ZA 100 SEKUNDI
						setPointCounter++;
						if (setPointCounter == 10)
                        {
							Environment.Exit(0);
                        }

						setPointCounter %= 10;
					}
					(DedicatedLc as dynamic).UpdateLocalData();
					DataAccessHelper.Instance.UpdateDatabase();
				}
                await Task.Delay(1000 * 10);
            }
        }
    }
}
