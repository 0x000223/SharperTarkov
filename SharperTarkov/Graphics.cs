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

        private void RenderDebug()
        {
            RenderTarget.DrawTextOutlined($"frame rate: {FrameRate}", Fonts["debug"], Form.Left + 50f, Form.Top + 50f, Brushes[Color.Gray], Brushes[Color.Black]);
            RenderTarget.DrawTextOutlined($"frame time: {FrameTime:F3}", Fonts["debug"], Form.Left + 50f, Form.Top + 65f, Brushes[Color.Gray], Brushes[Color.Black]);
        }

        private void RenderPlayers()
        {
            if (!WorldContext.IsActive)
            {
                return;
            }

            try
            {
                var posLocal = WorldContext.GameWorld.LocalPlayer.PlayerBody[EBone.Pelvis].GetPosition();

                foreach (var player in WorldContext.GameWorld.Players)
                {
                    if (player == WorldContext.GameWorld.LocalPlayer)
                    {
                        continue;
                    }

                    var head3 = player.PlayerBody[EBone.Head].GetPosition();
                    var head2 = WorldContext.WorldToScreen(head3);

                    var pelvis3 = player.PlayerBody[EBone.Pelvis].GetPosition();
                    var pelvis2 = WorldContext.WorldToScreen(pelvis3);

                    if (head2 == Vector2.Zero)
                    {
                        continue;
                    }

                    var distance = Vector3.Distance(pelvis3, posLocal);

                    var nameBrush = player.IsSavage ? Brushes[Color.WhiteSmoke] : Brushes[Color.Lime];

                    RenderTarget.DrawTextOutlined(player.PlayerName, Fonts["debug"], pelvis2.X, pelvis2.Y, nameBrush, Brushes[Color.Black]);

                    RenderTarget.DrawTextOutlined($"{player.Health:F0}HP[{distance:F0}m]", Fonts["debug"], pelvis2.X, pelvis2.Y + 15f, Brushes[Color.Cornsilk], Brushes[Color.Black]);
                }
            }
            catch (Exception)
            {
                Thread.Sleep(TimeSpan.FromSeconds(2));
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

                Parallel.Invoke(RenderDebug, RenderPlayers);

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
