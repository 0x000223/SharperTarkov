using SharpMemory.Ioctl;

namespace SharperTarkov.ScriptingTypes
{
    public class BackendConfig
    {
        public BackendConfig(ulong address)
        {
            Addresss = address;

            Config = new Config(Memory.Read<ulong>(address + Offsets.BackendConfig.Config));
        }

        public ulong Addresss { get; }

        public Config Config { get; }
    }
}