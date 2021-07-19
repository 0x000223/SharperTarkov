using System;
using SharpMemory.Ioctl;

namespace SharperTarkov.ScriptingTypes
{
    public class PlayerPhysical
    {
        private readonly ulong _staminaAddress;
        private readonly ulong _handsStaminaAddress;
        private readonly ulong _oxygenAddress;

        public PlayerPhysical(ulong address)
        {
            Address = address;

            _staminaAddress = Memory.Read<ulong>(address + Offsets.Physical.Stamina);

            _handsStaminaAddress = Memory.Read<ulong>(address + Offsets.Physical.HandsStamina);

            _oxygenAddress = Memory.Read<ulong>(address + Offsets.Physical.Oxygen);
        }

        public ulong Address { get; }

        public void SetStamina(float value)
        {
            Memory.Write(_staminaAddress + 0x48, value);
        }

        public void SetHandsStamina(float value)
        {
            Memory.Write(_handsStaminaAddress + 0x48, value);
        }

        public void SetOxygen(float value)
        {
            Memory.Write(_oxygenAddress + 0x48, value);
        }
    }
}