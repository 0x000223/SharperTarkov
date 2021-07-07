using SharpMemory.Ioctl;

namespace SharperTarkov.ScriptingTypes
{
    public class Backend
    {
        public Backend(ulong address)
        {
            Address = address;

            Session = new Session(Memory.Read<ulong>(Address + Offsets.Backend.Session));
        }

        public ulong Address { get; }

        public Session Session { get; }
    }
}