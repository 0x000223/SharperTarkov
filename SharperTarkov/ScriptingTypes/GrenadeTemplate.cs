using SharpMemory.Ioctl;

namespace SharperTarkov.ScriptingTypes
{
    public class GrenadeTemplate : ItemTemplate
    {
        public GrenadeTemplate(ulong address) : base(address)
        {
            GrenadeType = (EThrowWeaponType) Memory.Read<int>(address + Offsets.GrenadeTemplate.ThrowType);

            ExplostionDelay = Memory.Read<float>(address + Offsets.GrenadeTemplate.ExplosionDelay);

            Strength = Memory.Read<float>(address + Offsets.GrenadeTemplate.Strength);
        }

        public EThrowWeaponType GrenadeType { get; }

        public float ExplostionDelay { get; }

        public float Strength { get; }
    }
}