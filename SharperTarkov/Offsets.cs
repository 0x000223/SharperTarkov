
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

    public struct UnityType
    {
        public const uint TypeIndex = 0x30;
        public const uint DerivedCount = 0x34;
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
        public static readonly uint[] MonoClass = {0, 0};

        public const uint Transform = 0x10;
        public const uint GameObject = 0x30;
        public const uint ScriptingClass = 0x28;
    }

    public struct Transform
    {
        public const uint Hierarchy = 0x38;
        public const uint Index = 0x40;
        public const uint ChildCount = 0x80;
        public const uint Parent = 0x90;
    }

    public struct TransformHierarchy
    {
        public const uint Capacity = 0x10;
        public const uint Count = 0x14;
        public const uint Vertices = 0x18;
        public const uint Indices = 0x20;
        public const uint TransformHierarchyList = 0x30;
    }

    public struct BifacialTransform
    {
        public const uint Original = 0x10;
        public const uint UseImitation = 0xA8;
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
        public const uint MovementContext = 0x40;
        public const uint PlayerBody = 0xA8;
        public const uint ProceduralWeaponAnimation = 0x190;
        public const uint PlayerProfile = 0x458;
        public const uint PlayerPhysical = 0x468;
        public const uint AiData = 0x478;
        public const uint HealthController = 0x488;
        public const uint InventoryController = 0x498;
        public const uint HandsController = 0x4A0;
        public const uint QuestController = 0x4A8;
        public const uint PlayerBones = 0x4E8;
        public const uint IsLocalPlayer = 0x79b;
    }

    public struct HandsController
    {
        public const uint IsAiming = 0x28;
        public const uint Item = 0x50;
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

    public struct AiData
    {
        public const uint AiBossPlayer = 0x68;
    }

    public struct AiBossPlayer
    {
        public const uint IsBoss = 0x28;
    }

    public struct PlayerBones
    {
        public const uint RootJoint = 0x78;
        public const uint Fireport = 0x140;
    }

    public struct Physical
    {
        public const uint Stamina = 0x28;
        public const uint HandsStamina = 0x30;
        public const uint Oxygen = 0x38;
    }

    public struct MovementContext
    {
        public const uint Rotation = 0x20C;
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

    public struct ProceduralWeaponAnimation
    {
        public const uint BreathEffector = 0x28;
        public const uint WalkEffector = 0x30;
        public const uint MotionEffector = 0x38;
        public const uint ForceEffector = 0x40;
        public const uint ShotEffector = 0x48;
        public const uint ShotDirection = 0x1DC;
        public const uint Mask = 0x100;
        public const uint AimingDisplacementStr = 0x2F4;
        public const uint AlignToZero = 0x22C;
    }

    public struct ShotEffector
    {
        public const uint RecoilStrengthXy = 0x38;
        public const uint RecoilStrengthZ = 0x40;
        public const uint Intensity = 0x68;
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

    public struct ItemHandsController
    {
        public const uint Item = 0x50;
    }

    public struct Item
    {
        public const uint Id = 0x10;
        public const uint StringCache = 0x28;
        public const uint ItemTemplate = 0x38;
        public const uint SpawnedInSession = 0x64;
    }

    public struct ItemTemplate
    {
        public const uint Name = 0x10;
        public const uint ShortName = 0x10;
        public const uint Description = 0x20;
        public const uint Prefab = 0x30;
        public const uint CreditPrice = 0xC0;
        public const uint Rarity = 0xC4;
    }

    public struct Grenade
    {
        public const uint WeaponSource = 0x70;
        public const uint GrenadeSettings = 0x78;
        public const uint Transform = 0x88;
        public const uint TimeSpent = 0x98;
    }

    public struct GrenadeTemplate
    {
        public const uint FragmentType = 0x100;
        public const uint ThrowType = 0x108;
        public const uint ExplosionDelay = 0x10C;
        public const uint MinExplosionDistance = 0x110;
        public const uint MaxExplosionDistance = 0x114;
        public const uint FragmentsCount = 0x118;
        public const uint MinFragmentDamage = 0x11C;
        public const uint MaxFragmentDamage = 0x120;
        public const uint Strength = 0x124;
        public const uint ArmorDistanceDamage = 0x148;
    }

    public struct Weapon
    {
        public const uint Chambers = 0x90;
        public const uint OpticCalibrationPoints = 0x98;
        public const uint AimIndex = 0xA8;
        public const uint ShellsInChambers = 0xB0;
        public const uint MalfuntionState = 0xB8;
        public const uint Armed = 0xC1;
    }

    public struct LootItem
    {
        public const uint Item = 0x50;
        public const uint Name = 0x60;
    }

    public struct Corpse
    {
        public const uint PelvisTransform = 0x120;
        public const uint PlayerSide = 0x140;
    }

    public struct GameWorld
    {
        public const uint ExfilManager = 0x18;
        public const uint LootList = 0x60;
        public const uint ItemOwners = 0x68;
        public const uint LootItems = 0x70;
        public const uint RegisteredPlayers = 0x80;
        public const uint Grenades = 0xD8;
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

    public struct NightVision
    {
        public const uint Intensity = 0xA8;
        public const uint Red = 0xBC;
        public const uint Green = 0xC0;
        public const uint Blue = 0xC4;
        public const uint IsOn = 0xCC;
    }

    public struct ThermalVision
    {
        public const uint IsOn = 0xD0;
    }

    public struct VisorEffect
    {
        public const uint Intensity = 0xB8;
        public const uint IsVisible = 0x100;
    }
}
