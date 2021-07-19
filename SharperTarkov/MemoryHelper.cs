using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;

using SharpMemory.Ioctl;

namespace SharperTarkov
{
    public static class MemoryHelper
    {
        public static string ReadNarrowString(ulong address)
        {
            var stringClass = Memory.Read<ulong>(address);

            var buffer = Memory.ReadBytes(stringClass, 265);

            if (!buffer.Any())
            {
                return string.Empty;
            }

            var count = 0;

            while (buffer[count] != 0)
            {
                count++;
            }

            return Encoding.UTF8.GetString(buffer, 0, count);
        }

        public static string ReadWideString(ulong address)
        {
            var stringClass = Memory.Read<ulong>(address);

            if (stringClass == 0)
            {
                return string.Empty;
            }

            var length = Memory.Read<int>(stringClass + Offsets.String.Length);

            return length > 0
                ? Memory.ReadUnicode(stringClass + Offsets.String.Start, length * 2)
                : string.Empty;
        }

        public static List<T> ReadArray<T>(ulong address) where T : class
        {
            var arrayClass = Memory.Read<ulong>(address);

            var count = Memory.Read<int>(arrayClass + Offsets.Array.Count);

            var ret = new List<T>();

            for (uint index = 0; index < count; index++)
            {
                var instanceAddress = Memory.Read<ulong>(arrayClass + Offsets.Array.Base + index * 8);

                var instanceObject = (T)Activator.CreateInstance(typeof(T), instanceAddress);

                ret.Add(instanceObject);
            }

            return ret;
        }

        public static List<T> ReadArray<T>(ulong address, uint typeSize) where T : struct
        {
            var arrayClass = Memory.Read<ulong>(address);

            var count = Memory.Read<int>(arrayClass + Offsets.Array.Count);

            var ret = new List<T>();

            for (uint index = 0; index < count; index++)
            {
                var entry = Memory.Read<T>(arrayClass + Offsets.Array.Base + index * typeSize);

                ret.Add(entry);
            }

            return ret;
        }

        public static List<T> ReadList<T>(ulong address) where T : class
        {
            var listClass = Memory.Read<ulong>(address);

            var count = Memory.Read<int>(listClass + Offsets.List.Size);

            var array = Memory.Read<ulong>(listClass + Offsets.List.Array);

            var ret = new List<T>();

            for (uint index = 0; index < count; index++)
            {
                try
                {
                    var objectAddress = Memory.Read<ulong>(array + Offsets.Array.Base + index * 0x8);

                    var objectInstance = (T)Activator.CreateInstance(typeof(T), objectAddress);

                    ret.Add(objectInstance);
                }
                catch (Exception)
                {
                    return new List<T>();
                }
            }

            return ret;
        }

        public static List<T> ReadList<T>(ulong address, int[] indicies) where T : class
        {
            var listClass = Memory.Read<ulong>(address);

            var array = Memory.Read<ulong>(listClass + Offsets.List.Array);

            var ret = new List<T>();

            foreach (var index in indicies)
            {
                var objectAddress = Memory.Read<ulong>(array + Offsets.Array.Base + (uint)index * 0x8);

                var objectInstance = (T)Activator.CreateInstance(typeof(T), objectAddress);

                ret.Add(objectInstance);
        public static List<Vector128<float>> ReadVertices128(ulong address, int count)
        {
            var ret = new List<Vector128<float>>();

            var buffer = Memory.ReadBytes(address, count * 16);

            for (var i = 0; i < count * 16; i += 16)
            {
                var result = Vector128.Create(
                    buffer[i], buffer[i + 1], buffer[i + 2], buffer[i + 3], buffer[i + 4], buffer[i + 5],
                    buffer[i + 6], buffer[i + 7], buffer[i + 8], buffer[i + 9], buffer[i + 10], buffer[i + 11],
                    buffer[i + 12], buffer[i + 13], buffer[i + 14], buffer[i + 15])
                    .AsSingle();

                ret.Add(result);
            }

            return ret;
        }

        public static List<string> ReadStringList(ulong address)
        {
            var listClass = Memory.Read<ulong>(address);

            var count = Memory.Read<int>(listClass + Offsets.List.Size);

            var array = Memory.Read<ulong>(listClass + Offsets.List.Array);

            var ret = new List<string>();

            for (uint index = 0; index < count; index++)
            {
                var stringAddress = array + Offsets.Array.Base + index * 8;

                ret.Add(ReadWideString(stringAddress));
            }

            return ret;
        }

        public static Dictionary<TKey, TValue> ReadDictionary<TKey, TValue>(ulong address)
            where TKey : struct
            where TValue : struct
        {
            var dictionaryClass = Memory.Read<ulong>(address);

            var count = Memory.Read<int>(dictionaryClass + Offsets.Dictionary.Count);

            var entryArray = Memory.Read<ulong>(dictionaryClass + Offsets.Dictionary.Entries);

            var ret = new Dictionary<TKey, TValue>();

            for (uint index = 0; index < count; ++index)
            {
                var entryAddress = entryArray + Offsets.Array.Base + index * 0x18;

                var entry = new Entry<TKey, TValue>(entryAddress);

                ret.Add(entry.Key, entry.Value);
            }

            return ret;
        }

        public struct Entry<TKey, TValue>
            where TKey : struct
            where TValue : struct
        {
            public int Hash;
            public int Next;
            public TKey Key;
            public TValue Value;

            public Entry(ulong address)
            {
                Hash = 0;
                Next = 0;
                Key = Memory.Read<TKey>(address + Offsets.DictionaryEntry.Key);
                Value = Memory.Read<TValue>(address + Offsets.DictionaryEntry.Value);
            }
        }
    }
}
