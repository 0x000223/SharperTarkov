using System;
using SharpMemory.Ioctl;

namespace SharperTarkov.ScriptingTypes
{
    public class LootItem
    {
        public LootItem() { }

        public LootItem(ulong address)
        {
            if (address == 0)
            {
                return;
            }

            Address = address;

            ItemName = MemoryHelper.ReadWideString(address + Offsets.LootItem.Name);

            Item = new Item(Memory.Read<ulong>(address + Offsets.LootItem.Item));
        }

        public ulong Address { get; }

        public string ItemName { get; }

        public Item Item { get; }
    }
}
