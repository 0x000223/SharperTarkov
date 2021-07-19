using System;

using SharpMemory.Ioctl;

namespace SharperTarkov.ScriptingTypes
{
    public class Item
    {
        public Item() { }

        public Item(ulong address)
        {
            if (address == 0)
            {
                return;
            }

            Address = address;

            SpawnedInSession = Memory.Read<bool>(address + Offsets.Item.SpawnedInSession);

            Template = new ItemTemplate(Memory.Read<ulong>(address + Offsets.Item.ItemTemplate));
        }

        public ulong Address { get; }

        public bool SpawnedInSession { get; }

        public ItemTemplate Template { get; }
    }
}