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

            var translationVector = new Vector3(viewMatrix.M41, viewMatrix.M42, viewMatrix.M43);

            var up = new Vector3(viewMatrix.M21, viewMatrix.M22, viewMatrix.M23);
            var right = new Vector3(viewMatrix.M11, viewMatrix.M12, viewMatrix.M13);

            var w = viewMatrix.M41 * origin.X +
                    viewMatrix.M42 * origin.Y +
                    viewMatrix.M43 * origin.Z +
                    viewMatrix.M44;
            
            if (w < 0.1f)
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
                X = (1920 / 2f) * (1f + x / w),
                Y = (1080 / 2f) * (1f - y / w)
            };
        }

        public static GameObject GetMainCamera()
        {
            return GameObject
                .GetTaggedObjects()
                .FirstOrDefault(o => o.Tag == 5);
        }

        public static Camera GetMainCameraComponent()
        {
            var mainCamera = GetMainCamera();

            if (mainCamera == null)
            {
                return null;
            }

            var gameObject = mainCamera;

            var address = gameObject
                .GetComponents()
                .First(c => c.Name.Contains("Camera"))
                .Address;

            return new Camera(address);
        }
    }
}
