
using System.CodeDom;

namespace SharperTarkov.Offsets
{
    public struct Global
    {
        public const uint GameObjectManager = 0x156C698;
        public const uint RenderManager = 0x156CCB8;
        public const uint TypeManager = 0x156C690;
        public const uint NetworkManager = 0;

        public const uint Context = 0;
    }

    public struct String
    {
        public const uint Length = 0x10;
        public const uint Start = 0x14;
    }

    public struct Array
    {
        public const uint Count = 0x18;
        public const uint Base = 0x20;
    }

    public struct List
    {
        public const uint Array = 0x10;
        public const uint Size = 0x18;
    }

    public struct Dictionary
    {
        public const uint Entries = 0x18;
        public const uint Count = 0x40;
    }

    public struct DictionaryEntry
    {
        public const uint Hashcode = 0x0;
        public const uint Next = 0x04;
        public const uint Key = 0x8;
        public const uint Value = 0x10;
    }

    public struct LinkedList
    {
        public const uint Head = 0x10;
        public const uint Count = 0x28;
    }

    public struct LinkedListNode
    {
        public const uint List = 0x10;
        public const uint Next = 0x18;
        public const uint Prev = 0x20;
        public const uint Item = 0x28;
    }

    public struct Mono
    {
        public const uint ClassParent = 0x30;
        public const uint ClassName = 0x48;
        public const uint ClassNamespace = 0x50;
        public const uint ClassType = 0xB8;
    }

    public struct GameObjectManager
    {
        public const uint NextObject = 0x8;
        public const uint TaggedObjects = 0x8;
        public const uint LastActiveObject = 0x10;
        public const uint ActiveObjects = 0x18;
    }

    public struct GameObject
    {
        public const uint ComponentArray = 0x30;
        public const uint ComponentCount = 0x40;
        public const uint Layer = 0x50;
        public const uint Tag = 0x54;
        public const uint ActiveSelf = 0x56;
        public const uint IsAcive = 0x57;
        public const uint IsBeingDeactivated = 0x58;
        public const uint Name = 0x60;
    }

    public struct Component
    {
        public static readonly uint[] MonoClass = { 0, 0 };

        public const uint Transform = 0x10;
        public const uint GameObject = 0x30;
        public const uint ScriptingClass = 0x28;
    }

    public struct Camera
    {
        public const uint FocalLength = 0x50;
        public const uint Fov = 0x5C;
        public const uint GateFit = 0x54;
        public const uint WorldToCameraMatrix = 0x58;
        public const uint ProjectionMatrix = 0x98;
        public const uint ViewMatrix = 0xD8;
        public const uint CullingMask = 0x414;
        public const uint EventMask = 0x418;
        public const uint Depth = 0x41C;
        public const uint Velocity = 0x428;
        public const uint Aspect = 0x4C8;
        public const uint ProjectionMatrixMode = 0x4EC;
        public const uint Type = 0x510;
    }

    public struct Player
    {
        public const uint PlayerBody = 0xA8;
        public const uint ProceduralWeaponAnimation = 0x190;
        public const uint PlayerProfile = 0x458;
        public const uint PlayerPhysical = 0x468;
        public const uint HealthController = 0x488;
        public const uint InventoryController = 0x498;
        public const uint HandsController = 0x4A0;
        public const uint QuestController = 0x4A8;
    }

    public struct PlayerProfile
    {
        public const uint Id = 0x10;
        public const uint AccountId = 0x18;
        public const uint ProfileInfo = 0x28;
        public const uint Health = 0x40;
        public const uint Inventory = 0x48;

    }

    public struct ProfileInfo
    {
        public const uint Nickname = 0x10;
        public const uint GroupId = 0x18;
        public const uint Side = 0x50;
        public const uint RegistrationDate = 0x54;
        public const uint MemberCategory = 0x68;
        public const uint Experience = 0x6C;
    }

    public struct HealthController
    {
        public const uint BodyParts = 0x50;

    }

    public struct BodyPartState
    {
        public const uint Health = 0x10;
        public const uint IsDestroyed = 0x18;
    }

    public struct Health
    {
        public const uint Current = 0x10;
        public const uint Maximum = 0x14;
        public const uint Minimum = 0x18;
        public const uint LastDiff = 0x1C;
        public const uint DownMult = 0x20;
    }

    public struct PlayerBody
    {
        public const uint SkeletonRootJoint = 0x28;
        public const uint PlayerSide = 0x7C;
    }

    public struct Skeleton
    {
        public const uint Bones = 0x18;
        public const uint Keys = 0x20;
        public const uint Values = 0x28;
    }

    public struct PlayerPhysical
    {
        public const uint Stamina = 0x28;
        public const uint HandsStamina = 0x30;
        public const uint Oxygen = 0x38;
    }

    public struct GameWorld
    {
        public const uint ExfilManager = 0x18;
        public const uint RegisteredPlayers = 0x80;
    }

    public struct ClientApplication
    {
        public const uint Backend = 0x28;
        public const uint Insurance = 0x40;
    }

    public struct Backend
    {
        public const uint Cache = 0x20;
        public const uint Version = 0x48;
        public const uint Session = 0x60;
        public const uint Status = 0x70;
    }

    public struct Session
    {
        public const uint LocationTime = 0xE0;
        public const uint BackendConfig = 0x100;
    }

    public struct BackendConfig
    {
        public const uint Config = 0x10;
        public const uint ItemPresets = 0x18;
        public const uint BotPresets = 0x20;
        public const uint BotWeaponScatterings = 0x28;
        public const uint BackendTime = 0x30;
    }

    public struct Config
    {
        public const uint Experience = 0x20;
        public const uint MaxBotsAliveOnMap = 0xC0;
        public const uint SavagePlayCooldown = 0xC8;
        public const uint SkillFatigueReset = 0x110;
        public const uint SkillAtrophy = 0x11C;
    }

    public struct ConfigExperience
    {
        public const uint Kill = 0x18;
        public const uint Level = 0x20;
        public const uint Heal = 0x28;
        public const uint MatchEnd = 0x30;
        public const uint ExpForLockedDoorBreach = 0x3C;
    }
}
