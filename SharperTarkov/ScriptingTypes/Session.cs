using SharpMemory.Ioctl;

namespace SharperTarkov.ScriptingTypes
{
    public class Session
    {
        public Session(ulong address)
        {
            Address = address;

            BackendConfig = new BackendConfig(Memory.Read<ulong>(address + Offsets.Session.BackendConfig));
        }

        public ulong Address { get; }

        public string LocationTime => MemoryHelper.ReadWideString(Address + Offsets.Session.LocationTime);

        public BackendConfig BackendConfig { get; }
    }
}