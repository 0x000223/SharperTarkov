
namespace SharperTarkov.ScriptingTypes
{
    public enum EMemberCategory
    {
        Default = 0,

        Developer = 1,

        UniqueId = 2,

        Trader = 4,

        Group = 8,

        System = 16,

        ChatModerator = 32,

        ChatModeratorWithPermanentBan = 64,

        UnitTest = 128,

        Sherpa = 256,

        Emissary = 512
    }

    public enum EPlayerSide
    {
        Usec = 1,
        Bear,
        Savage = 4
    }

    public enum EBodyPart
    {
        Head,

        Chest,

        Stomach,

        LeftArm,

        RightArm,

        LeftLeg,

        RightLeg,

        Common
    }

    public enum EThrowWeaponType
    {
        Frag,

        Stun,

        Smoke,

        Gas,

        Incendiary,

        Sonar,

        Flash
    }
}