using System;

namespace SharperTarkov.ScriptingTypes
{
    public class ClientApplication
    {
        public ClientApplication(ulong address)
        {
            Address = address;

            Backend = new Backend(address + Offsets.ClientApplication.Backend);
        }

        public ulong Address { get; }

        public Backend Backend { get; }
    }
}
