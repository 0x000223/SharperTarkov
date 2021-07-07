using System;
using SharpMemory.Ioctl;

namespace SharperTarkov.ScriptingTypes
{
    public class PlayerProfile
    {
        public PlayerProfile(ulong address)
        {
            Address = address;

            Id = MemoryHelper.ReadWideString(address + Offsets.PlayerProfile.Id);

            AccountId = MemoryHelper.ReadWideString(address + Offsets.PlayerProfile.AccountId);

            var info = Memory.Read<ulong>(Address + Offsets.PlayerProfile.ProfileInfo);

            Nickname = MemoryHelper.ReadWideString(info + Offsets.ProfileInfo.Nickname);

            Side = (EPlayerSide) Memory.Read<int>(info + Offsets.ProfileInfo.Side);

            MemberCategory = (EMemberCategory) Memory.Read<int>(info + Offsets.ProfileInfo.MemberCategory);

            Level = CalculateLevel();
        }

        public ulong Address { get; }

        public string Id { get; }

        public string AccountId { get; }

        public string Nickname { get; }

        public int Level { get; }

        public EPlayerSide Side { get; }

        public EMemberCategory MemberCategory { get; }

        private int CalculateLevel()
        {
            var profileInfo = Memory.Read<ulong>(Address + Offsets.PlayerProfile.ProfileInfo);

            var experience = Memory.Read<int>(profileInfo + Offsets.ProfileInfo.Experience);

            return default;
        }

        private static readonly int[] CumulativeExperience = 
        {
            1000, 4017, 8432, 14256, 21477, 30023, 39936, 51204, 63723, 77563, 92713, 111881, 134674,
            161139, 191417, 225194, 262366, 302484, 345751, 391649, 440444, 492366, 547896, 609066, 675913,
            748474, 826786, 910885, 1000809, 1096593, 1198275, 1309251, 1429580, 1559321, 1698532, 1847272, 
            2005600, 2173575, 2351255, 2538699, 2735966, 2946585, 3170637, 3408202, 3659361, 3924195
        };
    }
}
