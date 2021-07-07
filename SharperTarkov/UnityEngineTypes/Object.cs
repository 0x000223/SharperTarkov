using System;

namespace SharperTarkov.UnityEngineTypes
{
    public class Object
    {
        public Object() { }

        public Object(ulong address)
        {
            Address = address;
        }

        public ulong Address { get; }

        public override int GetHashCode() => HashCode.Combine(Address);

        public override bool Equals(object obj) => Equals(obj as Object);

        public bool Equals(Object other) => Address == other.Address;

        public static bool operator == (Object a, Object b)
        {
            if (a is null && b is null)
            {
                return true;
            }

            if (a is null || b is null)
            {
                return false;
            }

            return a.Address == b.Address;
        }

        public static bool operator != (Object a, Object b)
        {
            if (a is null && b is null)
            {
                return false;
            }

            if (a is null || b is null)
            {
                return true;
            }

            return a.Address != b.Address;
        }
    }
}
