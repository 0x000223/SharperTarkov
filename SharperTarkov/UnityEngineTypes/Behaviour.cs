using System;

namespace SharperTarkov.UnityEngineTypes
{
    public class Behaviour : Component
    {
        public Behaviour(ulong address) : base(address)
        {
            // TODO
        }

        public bool Enabled { get; }

        public bool IsActiveAndEnabled { get; }
    }
}
