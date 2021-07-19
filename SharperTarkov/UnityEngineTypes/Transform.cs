using System;
using System.Numerics;
using System.Collections.Generic;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;

using SharpMemory.Ioctl;

namespace SharperTarkov.UnityEngineTypes
{
    public class Transform : Component
    {
        #region Xmm
        private static readonly Vector128<float> Xmm300 = Vector128.Create(-2f, -2f, 2f, 0f);
        private static readonly Vector128<float> Xmm320 = Vector128.Create(-2f, 2f, -2f, 0f);
        private static readonly Vector128<float> Xmm330 = Vector128.Create(2f, -2f, -2f, 0f);
        private static readonly Vector128<float> Xmm520 = Vector128.Create(-1f, -1f, -1f, 0f);
        private static readonly Vector128<float> Xmm610 = Vector128.Create(0f, 0f, 0f, 0f);
        private static readonly Vector128<float> Xmm690 = Vector128.Create(0f, 0f, 0f, 0f);
        private static readonly Vector128<float> XmmDe0 = Vector128.Create(1f, 1f, 1f, 1f);
        #endregion

        private readonly ulong _hierarchy;

        private readonly ulong _verticesAddress;

        public Transform() { }

        public Transform(ulong address) : base(address)
        {
            _hierarchy = Memory.Read<ulong>(Address + Offsets.Transform.Hierarchy);

            var indicesAddress = Memory.Read<ulong>(_hierarchy + Offsets.TransformHierarchy.Indices);

            _verticesAddress = Memory.Read<ulong>(_hierarchy + Offsets.TransformHierarchy.Vertices);

            HierarchyIndex = Memory.Read<int>(Address + Offsets.Transform.Index);

            HierarchyCapacity = Memory.Read<int>(_hierarchy + Offsets.TransformHierarchy.Capacity);

            HierarchyCount = Memory.Read<int>(_hierarchy + Offsets.TransformHierarchy.Count);

            Indices = MemoryHelper.ReadIntegers(indicesAddress, HierarchyIndex + 1);
        }

        private int HierarchyIndex { get; }

        private int HierarchyCount { get; }

        private int HierarchyCapacity { get; }

        private List<int> Indices { get; }

        public Vector3 Forward => GetRotation().Multiply(Vector3.UnitZ);

        public Vector3 GetPosition()
        {
            var vertices = MemoryHelper.ReadVertices128(_verticesAddress, 3 * HierarchyIndex + 3); // Reference v9/v10 in IDA

            var index = Indices[HierarchyIndex]; // Indices + 4 * capacity

            var result = vertices[3 * HierarchyIndex]; // Vertices + 0x30 * capacity

            while (index >= 0)
            {
                var v9 = vertices[3 * index + 1].AsInt32(); // Vertices + 0x30 * index + 0x10

                var v10 = Sse.Multiply(vertices[3 * index + 2], result); // Vertices + 0x30 * index + 0x20

                var v11 = Sse2.Shuffle(v9, 0).AsSingle();
                var v12 = Sse2.Shuffle(v9, 0x71).AsSingle();
                var v13 = Sse2.Shuffle(v9, 0x8E).AsSingle();
                var v14 = Sse2.Shuffle(v9, 0x55).AsSingle();
                var v15 = Sse2.Shuffle(v9, 0xAA).AsSingle();
                var v16 = Sse2.Shuffle(v9, 0xDB).AsSingle();

                result = Sse.Add(
                            Sse.Add(
                                Sse.Add(
                                    Sse.Multiply(
                                        Sse.Subtract(
                                            Sse.Multiply(Sse.Multiply(v11, Xmm330), v13),
                                            Sse.Multiply(Sse.Multiply(v14, Xmm300), v16)),
                                        Sse2.Shuffle(v10.AsInt32(), 0xAA).AsSingle()),
                                    Sse.Multiply(
                                        Sse.Subtract(
                                            Sse.Multiply(Sse.Multiply(v15, Xmm300), v16),
                                            Sse.Multiply(Sse.Multiply(v11, Xmm320), v12)),
                                        Sse2.Shuffle(v10.AsInt32(), 0x55).AsSingle())),
                                Sse.Add(
                                    Sse.Multiply(
                                        Sse.Subtract(
                                            Sse.Multiply(Sse.Multiply(v14, Xmm320), v12),
                                            Sse.Multiply(Sse.Multiply(v15, Xmm330), v13)),
                                        Sse2.Shuffle(v10.AsInt32(), 0).AsSingle()),
                                v10)),
                            vertices[3 * index]); // Vertices + 0x30 * index

                index = Indices[index]; // Indices + 4 * index
            }

            return result.AsVector3();
        }

        public Quaternion GetRotation()
        {
            var vertices = MemoryHelper.ReadVertices128(_verticesAddress, 3 * HierarchyIndex + 3);

            var index = Indices[HierarchyIndex];

            var v6 = vertices[3 * HierarchyIndex].AsInt32();

            if (index >= 0)
            {
                var v9 = Sse2.And(Xmm520.AsInt32(), Xmm610.AsInt32());

                var v11 = Sse.And(Sse2.Shuffle(Vector128<int>.Zero, 0).AsSingle(), Xmm690);

                do
                {
                    var v14 = vertices[3 * index + 1]; // Vertices + 0x30 * index + 0x10

                    var v15 = Sse2.Xor(Sse2.And(Xmm610.AsInt32(), vertices[3 * index + 2].AsInt32()), XmmDe0.AsInt32()); // Vertices + 0x30 * index + 0x20

                    index = Indices[index]; // Indices + 4 * index

                    var v16 = Sse2.Xor(
                        Sse2.And(Xmm610.AsInt32(),
                            Sse.Or(
                                Sse.AndNot(
                                    Xmm690,
                                    Sse.Multiply(Sse2.Shuffle(v15, 0x41).AsSingle(),
                                        Sse2.Shuffle(v15, 0x9A).AsSingle())),
                                v11).AsInt32()),
                        v6).AsSingle();

                    v6 = Sse2.Xor(
                        v9,
                        Sse2.Shuffle(
                            Sse.Subtract(
                                Sse.Subtract(
                                    Sse.Subtract(
                                        Sse.Multiply(v14, Sse2.Shuffle(v16.AsInt32(), 0xD2).AsSingle()),
                                        Sse2.Shuffle(
                                            Sse.Multiply(v14, Sse2.Shuffle(v16.AsInt32(), 0x88).AsSingle()).AsInt32(),
                                            0x1E).AsSingle()),
                                    Sse2.Shuffle(
                                        Sse.Multiply(Sse2.Shuffle(v14.AsInt32(), 0xAF).AsSingle(), v16).AsInt32(),
                                        0x8D).AsSingle()),
                                Sse2.Shuffle(
                                    Sse.Multiply(
                                        Sse.MoveLowToHigh(v14, v14),
                                        Sse2.Shuffle(v16.AsInt32(), 0xF5).AsSingle()).AsInt32(),
                                    0x63).AsSingle()).AsInt32(),
                            0xD2));

                } while (index >= 0);
            }

            var vec4 = v6.AsSingle().AsVector4();

            return new Quaternion
            {
                X = vec4.X,
                Y = vec4.Y,
                Z = vec4.Z,
                W = vec4.W
            };
        }

        public Vector3 TransformDirection(Vector3 localDirection)
        {
            var localDirection128 = localDirection.AsVector128();

            var vertices = MemoryHelper.ReadVertices128(_verticesAddress, 3 * HierarchyIndex + 3);

            var vector = vertices[3 * HierarchyIndex + 1].AsInt32();
            var index = Indices[HierarchyIndex];

            var v10 = Sse2.Shuffle(vector, 0).AsSingle();
            var v11 = Sse2.Shuffle(vector, 0x71).AsSingle();
            var v12 = Sse2.Shuffle(vector, 0x8E).AsSingle();
            var v13 = Sse2.Shuffle(vector, 0x55).AsSingle();
            var v14 = Sse2.Shuffle(vector, 0xAA).AsSingle();
            var v15 = Sse2.Shuffle(vector, 0xDB).AsSingle();

            var a1 = Sse.Add(
                        Sse.Add(
                            Sse.Multiply(
                                Sse.Subtract(
                                    Sse.Multiply(Sse.Multiply(v10, Xmm330.AsSingle()), v12),
                                    Sse.Multiply(Sse.Multiply(v13, Xmm300.AsSingle()), v15)),
                                Sse2.Shuffle(localDirection128.AsInt32(), 0xAA).AsSingle()),
                            Sse.Multiply(
                                Sse.Subtract(
                                    Sse.Multiply(Sse.Multiply(v14, Xmm300.AsSingle()), v15),
                                    Sse.Multiply(Sse.Multiply(v10, Xmm320.AsSingle()), v11)),
                                Sse2.Shuffle(localDirection128.AsInt32(), 0x55).AsSingle())),
                        Sse.Add(
                            Sse.Multiply(
                                Sse.Subtract(
                                    Sse.Multiply(Sse.Multiply(v13, Xmm320.AsSingle()), v11),
                                    Sse.Multiply(Sse.Multiply(v14, Xmm330.AsSingle()), v12)),
                                Sse2.Shuffle(localDirection128.AsInt32(), 0).AsSingle()),
                            localDirection128));

            if (index >= 0)
            {
                var v16 = Xmm610.AsInt32();

                do
                {
                    var v19 = Sse2.Subtract(
                                    Sse.CompareLessThan(vertices[index + 2].AsSingle(), Vector128<float>.Zero).AsInt32(),
                                    Sse.CompareLessThan(Vector128<float>.Zero, vertices[index + 2].AsSingle()).AsInt32());

                    var v20 = vertices[index + 1].AsInt32();

                    index = Indices[index];

                    var v21 = Sse2.Shuffle(v20, 0x8E).AsSingle();
                    var v22 = Sse2.Shuffle(v20, 0xAA).AsSingle();
                    var v23 = Sse2.Shuffle(v20, 0x71).AsSingle();
                    var v24 = Sse2.Shuffle(v20, 0xDB).AsSingle();
                    var v25 = Sse2.Shuffle(v20, 0x55).AsSingle();
                    var v26 = Sse2.Shuffle(v20, 0).AsSingle();

                    var v27 = Sse2.Xor(Sse2.And(Sse2.ConvertToVector128Single(v19).AsInt32(), v16), localDirection128.AsInt32()).AsSingle();

                    a1 = Sse.Add(
                            Sse.Add(
                                Sse.Multiply(
                                    Sse.Subtract(
                                        Sse.Multiply(Sse.Multiply(v22, Xmm300.AsSingle()), v24),
                                        Sse.Multiply(Sse.Multiply(v26, Xmm320.AsSingle()), v23)),
                                        Sse2.Shuffle(v27.AsInt32(), 0x55).AsSingle()),
                                    Sse.Multiply(
                                        Sse.Subtract(
                                            Sse.Multiply(Sse.Multiply(v26, Xmm330.AsSingle()), v21),
                                            Sse.Multiply(Sse.Multiply(v25, Xmm300.AsSingle()), v24)),
                                        Sse2.Shuffle(v27.AsInt32(), 0xAA).AsSingle())),
                                Sse.Add(
                                    Sse.Multiply(
                                        Sse.Subtract(
                                            Sse.Multiply(Sse.Multiply(v25, Xmm320.AsSingle()), v23),
                                            Sse.Multiply(Sse.Multiply(v22, Xmm330.AsSingle()), v21)),
                                        Sse2.Shuffle(v27.AsInt32(), 0).AsSingle()),
                                    v27));
                } while (index >= 0);
            }

            return a1.AsVector3();
        }

        public Quaternion InverseTransformRotation(Quaternion quaternion)
        {
            if (Memory.Read<int>(_hierarchy + 0x8) > 0)
            {

            }

            throw new NotImplementedException();
        }

        public void SetRotation(Quaternion quaternion)
        {
            var hierarchy = Memory.Read<Vector4>(Address + Offsets.Transform.Hierarchy).AsVector128().AsInt32();

            var v5 = Sse2.ConvertToInt32(Sse2.ShiftRightLogical128BitLane(hierarchy, 8));

            var v9 = Sse.UnpackLow(
                        Sse.UnpackLow(Vector128.CreateScalar(quaternion.X), Vector128.CreateScalar(quaternion.Z)),
                        Sse.UnpackLow(Vector128.CreateScalar(quaternion.Y), Vector128.CreateScalar(quaternion.W)));

            if (v5 > 0)
            {
                // Inverse transform rotation
            }

            // Set local rotation

            throw new NotImplementedException();
        }

        public void LookAt(Vector3 target)
        {
            // Get direction vector (target - this.GetPosition)

            // Look rotation to quaternion

            // This.SetRotation

            throw new NotImplementedException();
        }
    }

    public static class QuaternionExtensions
    {
        public static Vector3 Multiply(this Quaternion rotation, Vector3 point)
        {
            // Pasted from UnityEngine.Quaternion * operator overload

            var num = rotation.X * 2f;
            var num2 = rotation.Y * 2f;
            var num3 = rotation.Z * 2f;
            var num4 = rotation.X * num;
            var num5 = rotation.Y * num2;
            var num6 = rotation.Z * num3;
            var num7 = rotation.X * num2;
            var num8 = rotation.X * num3;
            var num9 = rotation.Y * num3;
            var num10 = rotation.W * num;
            var num11 = rotation.W * num2;
            var num12 = rotation.W * num3;

            return new Vector3
            {
                X = (1f - (num5 + num6)) * point.X + (num7 - num12) * point.Y + (num8 + num11) * point.Z,
                Y = (num7 + num12) * point.X + (1f - (num4 + num6)) * point.Y + (num9 - num10) * point.Z,
                Z = (num8 - num11) * point.X + (num9 + num10) * point.Y + (1f - (num4 + num5)) * point.Z
            };
        }
    }
}
