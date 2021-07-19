using System.Numerics;
using SharperTarkov.UnityEngineTypes;
using SharpMemory.Ioctl;

namespace SharperTarkov.ScriptingTypes
{
    public class Corpse : LootItem
    {
        public Corpse(ulong address) : base(address)
        {
            PlayerSide = (EPlayerSide) Memory.Read<int>(address + Offsets.Corpse.PlayerSide);

            var scriptedTransform = Memory.Read<ulong>(address + Offsets.Corpse.PelvisTransform);

            var pelvis = new Transform(Memory.Read<ulong>(scriptedTransform + 0x10));

            Position = pelvis.GetPosition();
        }

        public EPlayerSide PlayerSide { get; }

        public Vector3 Position { get; }
    }
}