using System;

namespace SharperTarkov.ScriptingTypes
{
    public class PlayerPhysical
    {
        public PlayerPhysical(ulong address)
        {
            Address = address;
        }

        public ulong Address { get; }
    }
}