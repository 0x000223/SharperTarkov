using System;
using System.Linq;
using System.Numerics;
using SharpMemory.Ioctl;

using SharperTarkov.UnityEngineTypes;

namespace SharperTarkov.ScriptingTypes
{
    public class Player : MonoBehaviour
    {
        public Player() { }

        public Player(ulong address) : base(address)
        {
            Initialize(address);
        }

        public PlayerProfile Profile { get; private set; }

        public PlayerBody Body { get; private set; }

        public PlayerPhysical Physical { get; private set; }

        public HealthController HealthController { get; private set; }

        public ProceduralWeaponAnimation ProcWepAnim { get; private set; }

        public MovementContext MovementContext { get; private set; }

        public float Health { get; private set; }

        public bool IsLocalPlayer { get; private set; }

        public bool IsSavage { get; private set; }

        public bool IsSavagePlayer { get; private set; }

        public string PlayerName => Profile.Nickname;

        public int PlayerLevel => Profile.Level;

        public ulong HandsController => Memory.Read<ulong>(Address + Offsets.Player.HandsController);

        private void Initialize(ulong address)
        {
            if (address == 0)
            {
                return;
            }

            Profile = new PlayerProfile(Memory.Read<ulong>(address + Offsets.Player.PlayerProfile));

            Body = new PlayerBody(Memory.Read<ulong>(address + Offsets.Player.PlayerBody));

            Physical = new PlayerPhysical(Memory.Read<ulong>(address + Offsets.Player.PlayerPhysical));

            HealthController = new HealthController(Memory.Read<ulong>(address + Offsets.Player.HealthController));

            ProcWepAnim = new ProceduralWeaponAnimation(Memory.Read<ulong>(address + Offsets.Player.ProceduralWeaponAnimation));

            MovementContext = new MovementContext(Memory.Read<ulong>(Address + Offsets.Player.MovementContext));

            IsLocalPlayer = Memory.Read<bool>(address + Offsets.Player.IsLocalPlayer);

            IsSavage = Profile.Side == EPlayerSide.Savage;

            IsSavagePlayer = IsSavage && PlayerLevel > 0;
        }

        public void Update()
        {
            Health = HealthController.BodyParts.Sum(bodypart => bodypart.Current);
        }

        public float Distance(Player other) => Vector3.Distance(Body[EBone.Pelvis], other.Body[EBone.Pelvis]);

        public Transform GetFireport()
        {
            var playerBones = Memory.Read<ulong>(Address + Offsets.Player.PlayerBones);

            var bifacialTransform = Memory.Read<ulong>(playerBones + Offsets.PlayerBones.Fireport);

            var scriptingTransform = Memory.Read<ulong>(bifacialTransform + Offsets.BifacialTransform.Original);

            var transformAddress = Memory.Read<ulong>(scriptingTransform + 0x10);

            if (transformAddress == 0)
            {
                return new Transform();
            }

            return new Transform(transformAddress);
        }
    }
}
