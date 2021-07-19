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

            for (var index = 0; index < CumulativeExperience.Length; index++)
            {
                if (experience < CumulativeExperience[index])
                {
                    return index;
                }
            }

            return 0;
        }

        private static readonly int[] CumulativeExperience = 
        {
            1000, 4017, 8432, 14256, 21477, 30023, 39936, 51204, 63723, 77563, 92713, 111881, 134674,
            161139, 191417, 225194, 262366, 302484, 345751, 391649, 440444, 492366, 547896, 609066, 675913,
            748474, 826786, 910885, 1000809, 1096593, 1198275, 1309251, 1429580, 1559321, 1698532, 1847272, 
            2005600, 2173575, 2351255, 2538699, 2735966, 2946585, 3170637, 3408202, 3659361, 3924195,
            4202784, 4495210, 4801553, 5121894, 5456314, 5809667, 6182063, 6573613, 6984426, 7414613, 
            7864284, 8333549, 8831052, 9360623, 9928578, 10541848, 11206300, 11946977, 12789143, 13820522,
            15229487, 17206065, 19706065, 22706065, 26206065, 30206065, 34706065, 39706065, 45206065, 
            51206065, 58206065, 68206065
        };
    }
}
