using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;

using SharperTarkov.ScriptingTypes;
using SharperTarkov.UnityEngineTypes;

using SharpRender;
using SharpMemory.Ioctl;

namespace SharperTarkov
{
    class Program
    {
        static void Main()
        {
            MemoryContext.Initialize();

            WorldContext.Initialize();

            _ = Task.Run(() =>
            {
                while (true)
                {
                    try
                    {
                        StateContext.Instance.ExecuteState();
                    }
                    catch (Exception)
                    {
                        continue;
                    }

                    Task.Delay(500);
                }
            });

            Helper.GetPrimaryScreenBounds(out var height, out var width);

            var form = OverlayForm.Create(height, width);

            var graphics = new Graphics(form);

            graphics.Loop();
        }

        // Temp
        public static void Test()
        {
            var mainApplication = GameObject.GetActiveObjectByName("Application");

            var component = mainApplication.GetComponentByName("MainApplication");

            var clientApplication = new ClientApplication(component.ScriptingClass);

            var backend = clientApplication.Backend;

            var session = new Session(Memory.Read<ulong>(backend.Address + 0x60));

            var backendConfig = session.BackendConfig;

            var config = backendConfig.Config;

            var experienceTable = MemoryHelper.ReadArray<Config.ConfigExperience.ExperienceLevel.ValueBox>(0x1D65EEEE8B0);

            var cumulativeExperience = GetCumulativeExperiecneTable(experienceTable);

            using var file = File.CreateText(@"C:\Users\Max\Desktop\ExperienceTable.txt");

            for (var index = 0; index < cumulativeExperience.Count; index++)
            {
                file.WriteLine($"{index+1}{experienceTable[index].Value,12}{cumulativeExperience[index],22}");
            }
        }

        // Temp
        public static List<int> GetCumulativeExperiecneTable(ICollection<Config.ConfigExperience.ExperienceLevel.ValueBox> table)
        {
            var total = 0;

            var cumulativeTable = new List<int>();

            foreach (var entry in table)
            {
                total += entry.Value;

                cumulativeTable.Add(total);
            }

            return cumulativeTable;
        }
    }
}
