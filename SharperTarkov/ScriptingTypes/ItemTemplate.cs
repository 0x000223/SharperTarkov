namespace SharperTarkov.ScriptingTypes
{
    public class ItemTemplate
    {
        public ItemTemplate(ulong address)
        {
            Address = address;

            ShortName = MemoryHelper.ReadWideString(address + Offsets.ItemTemplate.ShortName);
        }

        public ulong Address { get; }

        public string ShortName { get; }
    }
}