using System;
using System.Linq;
using System.Numerics;
using SharpMemory.Ioctl;

namespace SharperTarkov.UnityEngineTypes
{
    public class Camera : Component
    {
        public Camera(ulong address) : base(address)
        {
        }

        public Matrix4x4 ViewMatrix => Memory.Read<Matrix4x4>(Address + Offsets.Camera.ViewMatrix);

        public Vector2 WorldToScreen(Vector3 origin)
        {
            var viewMatrix = Matrix4x4.Transpose(ViewMatrix);

            var w = viewMatrix.M41 * origin.X +
                    viewMatrix.M42 * origin.Y +
                    viewMatrix.M43 * origin.Z +
                    viewMatrix.M44;
            
            if (w < 0.098f)
                return Vector2.Zero;

            var x = viewMatrix.M11 * origin.X +
                    viewMatrix.M12 * origin.Y +
                    viewMatrix.M13 * origin.Z +
                    viewMatrix.M14;

            var y = viewMatrix.M21 * origin.X +
                    viewMatrix.M22 * origin.Y +
                    viewMatrix.M23 * origin.Z +
                    viewMatrix.M24;
            
            return new Vector2
            {
                X = (ViewportX / 2f) * (1.0095f + x / w),
                Y = (ViewportY / 2f) * (0.98f - y / w)
            };
        }

        public static Camera GetMainCamera()
        {
            try
            {
                return GameObject.GetTaggedObjects()
                    .First(@object => @object.Tag == 5)
                    .GetComponents()
                    .Where(component => component.Name.Equals("Camera") && component.Namespace.Equals("UnityEngine"))
                    .Select(component => new Camera(component.Address))
                    .First();
            }
            catch
            {
                return new Camera();
            }
        }
    }
}
