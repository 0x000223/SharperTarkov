using System;
using System.Collections.Generic;

using SharpMemory.Ioctl;

namespace SharperTarkov.ScriptingTypes
{
    public class Config
    {
        public Config(ulong address)
        {
            Address = address;
        }

        public ulong Address { get; }

        public ConfigExperience Experience { get; set; }

        public int MaxBotsAliveOnMap => Memory.Read<int>(Address + Offsets.Config.MaxBotsAliveOnMap);

        public int SavagePlayCooldown => Memory.Read<int>(Address + Offsets.Config.SavagePlayCooldown);

        public class ConfigExperience
        {
            public ConfigExperience(ulong address)
            {
                Address = address;

                Level = new ExperienceLevel(Memory.Read<ulong>(address + Offsets.ConfigExperience.Level));
            }

            public ulong Address { get; }

            public ExperienceLevel Level { get; }

            public class ExperienceLevel
            {
                public ExperienceLevel(ulong address)
                {
                    Address = address;

                    Table = MemoryHelper.ReadArray<ValueBox>(address + 0x10);

                    Mastering1 = Memory.Read<int>(address + 0x18);

                    Mastering2 = Memory.Read<int>(address + 0x1C);
                }

                public ulong Address { get; }

                public int Mastering1 { get; }

                public int Mastering2 { get;  }

                public List<ValueBox> Table { get; }

                public class ValueBox
                {
                    public ValueBox(ulong address)
                    {
                        Address = address;

                        Value = Memory.Read<int>(address + 0x10);
                    }

                    public ulong Address { get; }

                    public int Value { get; }
                }
            }
        }
    }
}