using System;
using System.Collections.Generic;

using SharpMemory.Ioctl;

namespace SharperTarkov.ScriptingTypes
{
    public class HealthController
    {
        public HealthController(ulong address)
        {
            Address = address;

            BodyParts = new List<BodyPartState>();

            var dictionary = MemoryHelper.ReadDictionary<int, ulong>(address + Offsets.HealthController.BodyParts);

            foreach (var entry in dictionary)
            {
                BodyParts.Add(new BodyPartState(entry.Value));
            }
        }

        public ulong Address { get; }

        public List<BodyPartState> BodyParts { get; }

        public class BodyPartState
        {
            private readonly Health _health;

            public BodyPartState(ulong address)
            {
                Address = address;

                _health = new Health(Memory.Read<ulong>(address + Offsets.BodyPartState.Health));
            }

            public ulong Address { get; }

            public float Current => _health.Current;

            public bool IsDestroyed => Memory.Read<bool>(Address + Offsets.BodyPartState.IsDestroyed);
        }
    }

    public class Health
    {
        public Health(ulong address)
        {
            Address = address;
        }

        public ulong Address { get; }

        public float Current => Memory.Read<float>(Address + Offsets.Health.Current);

        public float Maximum => Memory.Read<float>(Address + Offsets.Health.Maximum);

        public float Minimum => Memory.Read<float>(Address + Offsets.Health.Minimum);

        public float Normalized => Current / Maximum;
    }
}
