using System;
using System.Linq;
using System.Numerics;
using System.Collections.Generic;

using SharpMemory.Ioctl;
using SharperTarkov.UnityEngineTypes;

namespace SharperTarkov.ScriptingTypes
{
    public class PlayerBody : MonoBehaviour
    {
        public PlayerBody(ulong address) : base(address)
        {
            Initialize(address);
        }

        private void Initialize(ulong address)
        {
            if (address == 0)
            {
                return;
            }

            var skeleton = Memory.Read<ulong>(address + Offsets.PlayerBody.SkeletonRootJoint);

            var scriptingTransforms = MemoryHelper.ReadList<ulong>(skeleton + Offsets.Skeleton.Values, BoneIndices);

            Transforms = scriptingTransforms.Select(addr => new Transform(Memory.Read<ulong>(addr + 0x10))).ToList();

            Positions = new List<Vector3>(Transforms.Select(t => t.GetPosition()));
        }

        public Vector3 this[EBone bone] => Positions[(int) bone];

        public List<Transform> Transforms { get; private set; }

        public List<Vector3> Positions { get; private set; }

        private static readonly int[] BoneIndices =
        {
            // Indices of specific bones I need from skeleton transform list
            13, 14, 15, 16, 17, 18, 20, 21, 22, 23, 29, 36, 37, 90, 91,
            92, 93, 111, 112, 113, 114, 132, 133
        };

        public static readonly int[] BoneLinkIndices =
        {
            // Indices of Transforms list
            22, 21, 21, 2, 17, 21, 18, 17, 19, 18,
            20, 19, 13, 21, 14, 13, 15, 14, 16, 15,
            6, 2, 7, 6, 8, 7, 9, 8, 2, 3, 3, 4, 4, 5
        };
    }

    public enum EBone
    {
        Root = 0,
        Pelvis = 1,
        LeftTigh1 = 2,
        LeftFoot = 5,
        RightFoot = 9,
        Spine1 = 10,
        RightUpperArm = 17,
        RightForearm1 = 18,
        Neck = 21,
        Head = 22
    }
}