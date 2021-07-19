using System;
using SharpMemory.Ioctl;

namespace SharperTarkov.UnityEngineTypes
{
    public class Component : Object
    {
        public Component() { }

        public Component(ulong address) : base(address)
        {
            ScriptingClass = Memory.Read<ulong>(Address + Offsets.Component.ScriptingClass);

            var monoClass = Memory.ReadChain(ScriptingClass, Offsets.Component.MonoClass);

            Name = MemoryHelper.ReadNarrowString(monoClass + Offsets.Mono.ClassName);

            Namespace = MemoryHelper.ReadNarrowString(monoClass + Offsets.Mono.ClassNamespace);
        }

        public string Name { get; }

        public string Namespace { get; }

        public ulong ScriptingClass { get; }
    }
}
