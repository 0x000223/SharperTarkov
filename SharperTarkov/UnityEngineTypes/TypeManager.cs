using System;
using System.Collections.Generic;
using SharpMemory.Ioctl;

namespace SharperTarkov.UnityEngineTypes
{
    public class TypeManager
    {
        public TypeManager(ulong address)
        {
            Address = address;
        }

        public ulong Address { get; }

        public Dictionary<string, ulong> GetRegisteredTypes()
        {
            var arrayBase = Memory.Read<ulong>(Address + 0x18);

            var count = Memory.Read<uint>(Address + 0x20);

            var lastEntry = arrayBase + count + (2 * (0xC + count));

            var ret = new Dictionary<string, ulong>();

            while (arrayBase < lastEntry)
            {
                if (Memory.Read<uint>(arrayBase) < 0xFFFFFFFE)
                {
                    var typeName = MemoryHelper.ReadNarrowString(arrayBase + 0x8);

                    var typeAddress = Memory.Read<ulong>(arrayBase + 0x10);

                    ret.Add(typeName, typeAddress);
                }

                arrayBase += 0x18;
            }

            return ret;
        }

        public ulong FindTypeByName(string targetTypeName)
        {
            var arrayBase = Memory.Read<ulong>(Address + 0x18);

            var count = Memory.Read<uint>(Address + 0x20);

            var lastEntry = arrayBase + count + (2 * (0xC + count));

            while (arrayBase < lastEntry)
            {
                if (Memory.Read<uint>(arrayBase) < 0xFFFFFFFE)
                {
                    var typeAddress = arrayBase + 0x10;

                    var typeName = MemoryHelper.ReadNarrowString(arrayBase + 0x8);

                    if (typeName.Equals(targetTypeName, StringComparison.OrdinalIgnoreCase))
                    {
                        return typeAddress;
                    }
                }

                arrayBase += 0x18;
            }

            return 0;
        }

        public Dictionary<string, ulong> GetAllDerivedTypes(ulong unityTypeAddress)
        {
            var unityType = Memory.Read<ulong>(unityTypeAddress);

            var typeIndex = Memory.Read<uint>(unityType + 0x30);

            var derivedCount = Memory.Read<uint>(unityType + 0x34);

            var total = typeIndex + derivedCount;

            var ret = new Dictionary<string, ulong>();

            if (typeIndex < total)
            {
                var typeList = Memory.Read<ulong>(Address);

                var startIndex = 8 * typeIndex;

                do
                {
                    var derivedType = Memory.Read<ulong>(typeList + startIndex + 0x8);

                    var derviedTypeName = MemoryHelper.ReadNarrowString(derivedType + 0x10);

                    ret.Add(derviedTypeName, derivedType);

                    startIndex += 0x8;

                    derivedCount--;

                } while (derivedCount > 0);
            }

            return ret;
        }
    }
}