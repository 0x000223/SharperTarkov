using System;
using System.Linq;
using SharpMemory.Ioctl;
using SharperTarkov.UnityEngineTypes;

namespace SharperTarkov.ScriptingTypes
{
    public class Player : MonoBehaviour
    {
        public Player(ulong address) : base(address)
        {
            Profile = new PlayerProfile(Memory.Read<ulong>(Address + Offsets.Player.PlayerProfile));

            PlayerBody = new PlayerBody(Memory.Read<ulong>(Address + Offsets.Player.PlayerBody));

            Physical = new PlayerPhysical(Memory.Read<ulong>(Address + Offsets.Player.PlayerPhysical));

            HealthController = new HealthController(Memory.Read<ulong>(Address + Offsets.Player.HealthController));
        }

        public PlayerProfile Profile { get; }

        public PlayerBody PlayerBody { get; }

        public PlayerPhysical Physical { get; }

        public HealthController HealthController { get; }

        public float Health => HealthController.BodyParts.Sum(bodypart => bodypart.Current);

        public string PlayerName => Profile.Nickname;

        public bool IsSavage => Profile.Side == EPlayerSide.Savage;
    }
}
