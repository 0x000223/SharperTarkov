using System;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Runtime.ExceptionServices;

using SharpMemory.Ioctl;
using Mono.Simd;

#pragma warning disable CS0618

namespace SharperTarkov.UnityEngineTypes
{
    public class Transform
    {
        private readonly Vector4f _xmm0;
        private readonly Vector4f _xmm1;
        private readonly Vector4f _xmm2;

        public Transform() { }

        public Transform(ulong address)
        {
            Address = address;

            _xmm0 = new Vector4f(-2.0f,  2.0f, -2.0f, 0.0f);
            _xmm1 = new Vector4f( 2.0f, -2.0f, -2.0f, 0.0f);
            _xmm2 = new Vector4f(-2.0f, -2.0f,  2.0f, 0.0f);
        }

        public ulong Address { get; }

        [HandleProcessCorruptedStateExceptions]
        public unsafe Vector3 GetPosition()
        {
            try
            {
                var transformInternal = Memory.Read<ulong>(Address + 0x10);

                if (transformInternal == 0)
                {
                    return Vector3.Zero;
                }

                var transformAccess = Memory.Read<TransformAccess>(transformInternal + 0x38);
                var transformData = Memory.Read<TransfomData>(transformAccess.Data + 0x18);

                if (transformAccess.Data == 0 || transformData.Array == 0)
                {
                    return Vector3.Zero;
                }

                var matricesSize = (sizeof(Matrix34) * transformAccess.Index + sizeof(Matrix34));
                var indicesSize = (sizeof(int) * transformAccess.Index + sizeof(int));

                var matriciesBuffer = Memory.ReadBytes(transformData.Array, matricesSize);
                var indiciesBuffer = Memory.ReadBytes(transformData.Indicies, indicesSize);

                fixed (void* matriciesPtr = &matriciesBuffer[0], indiciesPtr = &indiciesBuffer[0])
                {
                    var result = *(Vector4f*)((ulong)matriciesPtr + 0x30 * (ulong)transformAccess.Index);
                    var indexRelation = *(int*)((ulong)indiciesPtr + 0x4 * (ulong)transformAccess.Index);

                    if (result == Vector4f.Zero)
                    {
                        return Vector3.Zero;
                    }

                    while (indexRelation >= 0)
                    {
                        var matrix34 = *(Matrix34*)((ulong)matriciesPtr + 0x30 * (ulong)indexRelation);

                        var v10 = matrix34.Vec2 * result;

                        var v11 = matrix34.Vec1.Shuffle(0);
                        var v12 = matrix34.Vec1.Shuffle((ShuffleSel)(85));
                        var v13 = matrix34.Vec1.Shuffle((ShuffleSel)(-114));
                        var v14 = matrix34.Vec1.Shuffle((ShuffleSel)(-37));
                        var v15 = matrix34.Vec1.Shuffle((ShuffleSel)(-86));
                        var v16 = matrix34.Vec1.Shuffle((ShuffleSel)(113));

                        result = (((((((v11 * _xmm1) * v13) - ((v12 * _xmm2) * v14)) *
                                     v10.Shuffle((ShuffleSel)(-86))) +
                                    ((((v15 * _xmm2) * v14) - ((v11 * _xmm0) * v16)) *
                                     v10.Shuffle((ShuffleSel)(85)))) +
                                   (((((v12 * _xmm0) * v16) - ((v15 * _xmm1) * v13)) *
                                     v10.Shuffle(0)) + v10)) + matrix34.Vec0);

                        indexRelation = *(int*)((ulong)indiciesPtr + 0x4 * (ulong)indexRelation);
                    }

                    return new Vector3(result.X, result.Y, result.Z);
                }
            }
            catch (Exception)
            {
                return Vector3.Zero;
            }
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct TransformAccess
    {
        public ulong Data;
        public int Index;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct TransfomData
    {
        public ulong Array;
        public ulong Indicies;
    }

    public struct Matrix34
    {
        public Vector4f Vec0, Vec1, Vec2;
    }
}
