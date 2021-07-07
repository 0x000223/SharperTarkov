using System;
using System.Numerics;
using System.Collections.Generic;

using SharperTarkov.UnityEngineTypes;

using SharpMemory.Ioctl;

namespace SharperTarkov.ScriptingTypes
{
    public class PlayerBody : MonoBehaviour
    {
        public PlayerBody(ulong address) : base(address)
        {
            var skeleton = Memory.Read<ulong>(Address + Offsets.PlayerBody.SkeletonRootJoint);

            BoneTransforms = MemoryHelper.ReadList<Transform>(skeleton + Offsets.Skeleton.Values);
        }

        public Transform this[EBone bone] => BoneTransforms[(int) bone] ?? new Transform();

        public List<Transform> BoneTransforms { get; }

        public List<Vector3> BonePositions { get; set; }

        public static readonly int[] BoneIndicies = 
        {
            13, 14, 15, 16, 17, 18, 20, 21, 22, 23, 29, 36, 37, 90, 91,
            92, 93, 111, 112, 113, 114, 132, 133
        };

        public static readonly int[] BoneLinkIndicies =
        {
            133, 132, 132, 29, 29, 14, 111, 132, 112, 111,
            113, 112, 114, 113, 90, 132, 91, 90, 92, 91, 93, 92,
            20, 14, 21, 20, 22, 21, 23, 22, 14, 15, 15, 16, 16, 17, 17, 18
        };

        //PlayerBones = new()
        //{
        //    Root = values[13],
        //    Pelvis = values[14],
        //    LeftThigh1 = values[15],
        //    LeftThigh2 = values[16],
        //    LeftCalf = values[17],
        //    LeftFoot = values[18],
        //    RightThigh1 = values[20],
        //    RightThigh2 = values[21],
        //    RightCalf = values[22],
        //    RightFoot = values[23],
        //    Spine1 = values[29],
        //    Spine2 = values[36],
        //    Spine3 = values[37],
        //    LeftUpperArm = values[90],
        //    LeftForearm1 = values[91],
        //    LeftForearm2 = values[92],
        //    LeftForearm3 = values[93],
        //    RightUpperArm = values[111],
        //    RightForearm1 = values[112],
        //    RightForearm2 = values[113],
        //    RightForearm3 = values[114],
        //    Neck = values[132],
        //    Head = values[133]
        //};
    }

    public enum EBone
    {
        Root = 13,
        Pelvis = 14,
        LeftFoot = 18,
        RightFoot = 23,
        Neck = 132,
        Head = 133
    }
}