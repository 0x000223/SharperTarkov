using System.Numerics;
using System.Threading;
using SharperTarkov.UnityEngineTypes;
using SharpMemory.Ioctl;

namespace SharperTarkov.ScriptingTypes
{
    public class Grenade
    {
        public Grenade(ulong address)
        {
            Address = address;

            var weaponSource = new Item(Memory.Read<ulong>(address + Offsets.Grenade.WeaponSource));

            Template = new GrenadeTemplate(Memory.Read<ulong>(weaponSource.Address + Offsets.Item.ItemTemplate));

            Thread.Sleep(200);

            var component = new Component(Memory.Read<ulong>(address + 0x10));

            Transform = component.GetTransform();
        }

        public ulong Address { get; }

        public GrenadeTemplate Template { get; }

        public Transform Transform { get; }

        public Vector3 Position => Transform.GetPosition();

        public float TimeSpent => Memory.Read<float>(Address + Offsets.Grenade.TimeSpent);

        public float ExplosionTimer => Template.ExplostionDelay - TimeSpent;
    }
}