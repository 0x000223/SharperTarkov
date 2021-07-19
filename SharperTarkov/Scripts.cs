using System;
using System.Linq;
using System.Numerics;

using SharpMemory.Ioctl;
using SharperTarkov.ScriptingTypes;
using SharperTarkov.UnityEngineTypes;

using Settings = SharperTarkov.Properties.Settings;

namespace SharperTarkov
{
    public class Scripts
    {
        public static Vector2 CalculateRotation(Player target, EBone targetBone)
        {
            var fireport3 = WorldContext.LocalPlayer.GetFireport().GetPosition();

            // Calcualte direction vector
            var delta = Vector3.Subtract(target.Body[targetBone], fireport3);

            // Calculate new rotation
            var ret = new Vector2
            {
                Y = MathF.Asin(delta.Y / delta.Length()) * (float)(180 / Math.PI),
                X = -MathF.Atan2(delta.X, -delta.Z) * (float)(180 / Math.PI)
            };

            if (ret.X < -360f)
            {
                ret.X += 360f;
            }

            if (ret.X > 360f)
            {
                ret.X -= 360f;
            }

            if (ret.Y < -360f)
            {
                ret.Y += 360f;
            }

            if (ret.Y > 360f)
            {
                ret.Y -= 360f;
            }

            ret.X -= 180f;
            ret.Y = -ret.Y;

            return ret;
        }

        public static Player GetBestTarget(EBone targetBone)
        {
            var localPlayer = WorldContext.LocalPlayer;

            var bestDistance = float.MaxValue;

            var bestTarget = new Player();

            foreach (var player in WorldContext.Players)
            {
                if (player.IsLocalPlayer)
                {
                    continue;
                }

                if (localPlayer.Distance(player) > Settings.Default.AimDistance)
                {
                    continue;
                }

                var bone = WorldContext.WorldToScreen(player.Body[targetBone]);

                var distance = Vector2.Distance(Graphics.FormCenter, bone);

                if (distance < Settings.Default.AimFov)
                {
                    if (distance < bestDistance)
                    {
                        bestDistance = distance;

                        bestTarget = player;
                    }
                }
            }

            return bestTarget;
        }

        public static void LightNoRecoil()
        {
            var localPlayer = WorldContext.LocalPlayer;
            
            localPlayer.ProcWepAnim.SetShotEffector(Vector2.Zero, 0f);
        }

        public static void FullNoRecoil()
        {
            // Nullify breath/walk/motion/force/shot intensity
            // Nullify 'alignToZero' float 0x22C
            // Nullify 'AimingDisplacementStr'
            // Mask set to 1f

            var localPlayer = WorldContext.LocalPlayer;

            localPlayer.ProcWepAnim.SetAnimationMask(1);
            localPlayer.ProcWepAnim.SetBreathEffector(0f);
            localPlayer.ProcWepAnim.SetForceEffector(0f);
            localPlayer.ProcWepAnim.SetMotionEffector(0f);
            localPlayer.ProcWepAnim.SetWalkEffector(0f);
            localPlayer.ProcWepAnim.SetShotEffector(Vector2.Zero, 0f);

            Memory.Write(localPlayer.ProcWepAnim.Address + Offsets.ProceduralWeaponAnimation.AlignToZero, 0f);
            Memory.Write(localPlayer.ProcWepAnim.Address + Offsets.ProceduralWeaponAnimation.AimingDisplacementStr, 0f);
        }

        public static void FillStamina()
        {
            try
            {
                var localPlayer = WorldContext.LocalPlayer;

                localPlayer.Physical.SetStamina(300f);
                localPlayer.Physical.SetHandsStamina(300f);
                localPlayer.Physical.SetOxygen(300f);
            }
            catch
            {
                return;
            }
        }

        public static void ToggleNightVision()
        {
            try
            {
                var gameobject = GameObject.GetTaggedObjects().First(@object => @object.Tag == 5);

                var component = gameobject.GetComponents().First(comp => comp.Name.Equals("NightVision"));

                var current = Memory.Read<bool>(component.ScriptingClass + Offsets.NightVision.IsOn);

                var value = (byte)(!current ? 1 : 0);

                Memory.Write(component.ScriptingClass + Offsets.NightVision.IsOn, value);
            }
            catch
            {
                return;
            }
        }

        public static void ToggleThermalVision()
        {
            throw new NotImplementedException();
        }

        public static void ToggleVisorEffect()
        {
            try
            {
                var gameobject = GameObject.GetTaggedObjects().First(@object => @object.Tag == 5);

                var component = gameobject.GetComponents().First(comp => comp.Name.Equals("VisorEffect"));

                var current = Memory.Read<float>(component.ScriptingClass + Offsets.VisorEffect.Intensity);

                var value = current > 0f ? 0f : 1f;

                Memory.Write(component.ScriptingClass + Offsets.VisorEffect.Intensity, value);
            }
            catch
            {
                return;
            }
        }
    }
}
