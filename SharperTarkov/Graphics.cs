using System;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows.Forms;
using System.Collections.Generic;

using SharpDX;
using SharpDX.DXGI;
using SharpDX.Windows;
using SharpDX.Direct2D1;
using SharpDX.DirectWrite;

using SharpRender;
using SharpRender.Direct2D;

using Vector2 = System.Numerics.Vector2;
using Vector3 = System.Numerics.Vector3;
using Factory = SharpDX.Direct2D1.Factory;
using AlphaMode = SharpDX.Direct2D1.AlphaMode;
using FactoryType = SharpDX.Direct2D1.FactoryType;

using SharperTarkov.ScriptingTypes;

namespace SharperTarkov
{
    public class Graphics
    {
        private readonly Stopwatch _stopwatch;

        public Graphics(Form form)
        {
            Form = form;

            RenderTarget = CreateRenderTarget(form);

            Brushes = RenderTarget.CreateBrushCollection();

            Fonts = new Dictionary<string, TextFormat>
            {
                { "debug", CreateTextFormat("Calibri", 13f) }
            };

            _stopwatch = new Stopwatch();
        }

        public Form Form { get; }

        public int FrameRate { get; private set; }

        public float FrameTime { get; private set; }

        public RenderTarget RenderTarget { get; }

        public Dictionary<Color, SolidColorBrush> Brushes { get; }

        public Dictionary<string, TextFormat> Fonts { get; }

        public static RenderTarget CreateRenderTarget(Form form)
        {
            var winodwRenderProperties = new HwndRenderTargetProperties
            {
                Hwnd = form.Handle,
                PixelSize = new Size2(form.Width, form.Height),
                PresentOptions = PresentOptions.Immediately
            };

            var renderTargetProperties = new RenderTargetProperties
            {
                PixelFormat = new PixelFormat(Format.B8G8R8A8_UNorm, AlphaMode.Premultiplied),
                DpiX = 96f,
                DpiY = 96f,
                MinLevel = FeatureLevel.Level_DEFAULT,
                Usage = RenderTargetUsage.None
            };

            return new WindowRenderTarget(new Factory(FactoryType.MultiThreaded), renderTargetProperties, winodwRenderProperties);
        }

        public TextFormat CreateTextFormat(string fontFamilyName, float fontSize)
        {
            return new(new SharpDX.DirectWrite.Factory(SharpDX.DirectWrite.FactoryType.Shared), fontFamilyName, fontSize);
        }

        private void DrawTextOutlined(string text, TextFormat textFormat, float x, float y, Brush brush)
            => RenderTarget.DrawTextOutlined(text, textFormat, x, y, brush, Brushes[Color.Black]);

        private void KeyEventHandler()
        {
            if ((GetAsyncKeyState(Keys.F1) & 0x01) != 0)
            {
                Settings.Default.IsRecoil = !Settings.Default.IsRecoil;
                Settings.Default.Save();
                return;
            }

            if ((GetAsyncKeyState(Keys.Left) & 0x01) != 0)
            {
                Scripts.ToggleNightVision();
                return;
            }

            if ((GetAsyncKeyState(Keys.Down) & 0x01) != 0)
            {
                Settings.Default.RenderCorpses = !Settings.Default.RenderCorpses;
                Settings.Default.Save();
                return;
            }

            if ((GetAsyncKeyState(Keys.V) & 0x01) != 0)
            {
                Scripts.ToggleVisorEffect();
                return;
            }

            if ((GetAsyncKeyState(Keys.F) & 0x8000) != 0)
            {
                RenderTarget.DrawCircle(Form.Width / 2f, Form.Height / 2f, Settings.Default.AimFov, Brushes[Color.Gainsboro], 0.5f);

                try
                {
                    var target = Scripts.GetBestTarget(EBone.Head);

                    var rotation = Scripts.CalculateRotation(target, EBone.Head);

                    WorldContext.LocalPlayer.MovementContext.Rotation = rotation;
                }
                catch
                {
                    return;
                }

                Scripts.FullNoRecoil();
            }
        }

        private void RenderDebug()
        {
            if (!WorldContext.IsActive)
            {
                return;
            }

            var playerCount = WorldContext.Players.Count;
            var rotation = WorldContext.LocalPlayer.MovementContext.Rotation;

            DrawTextOutlined($"frame rate: {FrameRate}", Fonts["debug"], Form.Left + 50f, Form.Top + 50f, Brushes[Color.Gray]);
            DrawTextOutlined($"frame time: {FrameTime:F3}", Fonts["debug"], Form.Left + 50f, Form.Top + 65f, Brushes[Color.Gray]);
            DrawTextOutlined($"rotation: {rotation.X:F0}, {rotation.Y:F0}", Fonts["debug"], Form.Left + 50f, Form.Top + 80f, Brushes[Color.Gray]);
            DrawTextOutlined($"entities: {playerCount}", Fonts["debug"], Form.Left + 50f, Form.Top + 95f, Brushes[Color.Gray]);
        }

        private void RenderPlayers()
        {
            if (!WorldContext.IsActive)
            {
                return;
            }

            Parallel.ForEach(WorldContext.Players, _parallelOptions, player =>
            {
                if (player.IsLocalPlayer)
                {
                    return;
                }

                var head3 = player.Body[EBone.Head];
                var head2 = WorldContext.WorldToScreen(head3);

                var pelvis3 = player.Body[EBone.Pelvis];
                var pelvis2 = WorldContext.WorldToScreen(pelvis3);

                if (head2 == Vector2.Zero)
                {
                    return;
                }

                var distance = Vector3.Distance(pelvis3, WorldContext.LocalPlayer.Body[EBone.Pelvis]);

                var nameBrush = player.IsSavage ? Brushes[Color.DimGray] : Brushes[Color.Lime];

                nameBrush = player.IsSavagePlayer ? Brushes[Color.Coral] : nameBrush;

                RenderTarget.DrawTextOutlined(player.PlayerName, Fonts["debug"], pelvis2.X, pelvis2.Y, nameBrush, Brushes[Color.Black]);

                RenderTarget.DrawTextOutlined($"{player.Health:F0}hp [{distance:F0}m] [Lvl {player.PlayerLevel}]", Fonts["debug"], pelvis2.X, pelvis2.Y + 15f, Brushes[Color.SlateGray], Brushes[Color.Black]);

                #region Bones
                for (var index = 0; index < PlayerBody.BoneLinkIndices.Length; index += 2)
                {
                    var index1 = PlayerBody.BoneLinkIndices[index];
                    var index2 = PlayerBody.BoneLinkIndices[index + 1];

                    var f3 = player.Body.Positions[index1];
                    var f2 = WorldContext.WorldToScreen(f3);

                    var t3 = player.Body.Positions[index2];
                    var t2 = WorldContext.WorldToScreen(t3);

                    if (f2 == Vector2.Zero || t2 == Vector2.Zero)
                    {
                        continue;
                    }

                    RenderTarget.DrawLine(f2.X, f2.Y, t2.X, t2.Y, Brushes[Color.Fuchsia]);
                }
                #endregion
            });
        }

        private void RenderCorpses()
        {
            if (!WorldContext.IsActive || !Properties.Settings.Default.RenderCorpses)
            {
                return;
            }

            foreach (var corpse in WorldContext.Corpses)
            {
                var pos3 = corpse.Position;
                var pos2 = WorldContext.WorldToScreen(pos3);

                if (pos2 == Vector2.Zero || pos3 == Vector3.Zero)
                {
                    continue;
                }

                var distance = Vector3.Distance(pos3, WorldContext.LocalPlayer.Body[EBone.Pelvis]);

                DrawTextOutlined($"[{corpse.PlayerSide}][{distance:F0}m]", Fonts["debug"], pos2.X, pos2.Y, Brushes[Color.DarkGray]);
            }
        }

        private void RenderGrenades()
        {
            if (!WorldContext.IsActive)
            {
                return;
            }

            foreach (var grenade in WorldContext.Grenades)
            {
                var pos3 = grenade.Position;
                var pos2 = WorldContext.WorldToScreen(pos3);

                if (pos2 == Vector2.Zero || pos3 == Vector3.Zero)
                {
                    continue;
                }

                var distance = Vector3.Distance(pos3, WorldContext.LocalPlayer.Body[EBone.Pelvis]);

                var brush = grenade.Template.GrenadeType == EThrowWeaponType.Frag ? Brushes[Color.Red] : Brushes[Color.YellowGreen];

                DrawTextOutlined($"[{grenade.Template.GrenadeType}][{distance:F0}m]", Fonts["debug"], pos2.X, pos2.Y, brush);
            }
        }

        public void Loop()
        {
            var frameCounter = 0;

            RenderLoop.Run(Form, () => 
            {
                if (!_stopwatch.IsRunning)
                {
                    _stopwatch.Restart();
                }

                RenderTarget.BeginDraw();
                RenderTarget.Clear(null);

                KeyEventHandler();
                
                Parallel.Invoke(RenderDebug, RenderPlayers, RenderCorpses, RenderGrenades);
                
                RenderTarget.Flush();
                RenderTarget.EndDraw();

                if (_stopwatch.ElapsedMilliseconds >= 1000)
                {
                    _stopwatch.Stop();

                    FrameRate = frameCounter;

                    FrameTime = 1000 / (float)frameCounter;

                    frameCounter = 0;
                }

                frameCounter++;
            });
        }
    }
}
