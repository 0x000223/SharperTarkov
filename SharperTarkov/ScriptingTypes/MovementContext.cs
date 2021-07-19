using System.Numerics;
using SharpMemory.Ioctl;

namespace SharperTarkov.ScriptingTypes
{
    public class MovementContext
    {
        public MovementContext(ulong address)
        {
            Address = address;
        }

        public ulong Address { get; }

        public Vector2 Rotation
        {
            get => Memory.Read<Vector2>(Address + Offsets.MovementContext.Rotation);

            set => Memory.Write(Address + Offsets.MovementContext.Rotation, value);
        }
    }
}