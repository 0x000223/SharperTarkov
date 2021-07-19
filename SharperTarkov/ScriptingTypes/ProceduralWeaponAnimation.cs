using System.ComponentModel;
using System.Numerics;
using SharpMemory.Ioctl;

namespace SharperTarkov.ScriptingTypes
{
    public class ProceduralWeaponAnimation
    {
        private readonly ulong _shotEffector;
        private readonly ulong _breathEffector;
        private readonly ulong _motionEffector;
        private readonly ulong _walkEffector;
        private readonly ulong _forceEffector;

        public ProceduralWeaponAnimation(ulong address)
        {
            Address = address;

            _shotEffector = Memory.Read<ulong>(address + Offsets.ProceduralWeaponAnimation.ShotEffector);
            _breathEffector = Memory.Read<ulong>(address + Offsets.ProceduralWeaponAnimation.BreathEffector);
            _motionEffector = Memory.Read<ulong>(address + Offsets.ProceduralWeaponAnimation.MotionEffector);
            _walkEffector = Memory.Read<ulong>(address + Offsets.ProceduralWeaponAnimation.WalkEffector);
            _forceEffector = Memory.Read<ulong>(address + Offsets.ProceduralWeaponAnimation.ForceEffector);
        }

        public ulong Address { get; }

        public Vector3 LocalShotDirection => Memory.Read<Vector3>(Address + Offsets.ProceduralWeaponAnimation.ShotDirection);

        public void SetShotEffector(Vector2 recoilStrength, float intensity)
        {
            Memory.Write(_shotEffector + Offsets.ShotEffector.RecoilStrengthXy, recoilStrength);
            Memory.Write(_shotEffector + Offsets.ShotEffector.RecoilStrengthZ, recoilStrength);
            Memory.Write(_shotEffector + Offsets.ShotEffector.Intensity, intensity);
        }

        public void SetBreathEffector(float value)
        {
            Memory.Write(_breathEffector + 0xA4, value);
        }

        public void SetMotionEffector(float value)
        {
            Memory.Write(_motionEffector + 0xD0, value);
        }

        public void SetWalkEffector(float value)
        {
            Memory.Write(_walkEffector + 0x44, value);
        }

        public void SetForceEffector(float value)
        {
            Memory.Write(_forceEffector + 0x30, value);
        }

        public void SetAnimationMask(int value)
        {
            Memory.Write(Address + Offsets.ProceduralWeaponAnimation.Mask, value);
        }
    }
}