using System;
using SharpMemory.Ioctl;

namespace SharperTarkov
{
    public static class MemoryContext
    {
        static MemoryContext()
        {
            StateContext.Instance.TurnedOn += State_TurnedOn;

            StateContext.Instance.TurnedOff += State_TurnedOff;
        }
        
        public static ulong ModuleAddress { get; set; }

        private static void State_TurnedOn(object sender, EventArgs e)
        {
            Memory.AttachToProcess("EscapeFromTarkov");

            ModuleAddress = Memory.GetModuleAddress("UnityPlayer.dll");
        }

        private static void State_TurnedOff(object sender, EventArgs e)
        {
            ModuleAddress = 0;
        }

        public static void Initialize()
        {
            Memory.Initialize(new CommunicationToken(@"\\.\GenericIOCTL", 0x00222233));

            if(Memory.AttachToProcess("EscapeFromTarkov"))
            {
                ModuleAddress = Memory.GetModuleAddress("UnityPlayer.dll");
            }
        }
    }
}
