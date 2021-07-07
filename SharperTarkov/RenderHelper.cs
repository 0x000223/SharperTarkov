using System;
using SharpDX;
using SharpRender;
using SharpRender.Direct2D;

namespace SharperTarkov
{
    public class RenderHelper
    {
        public RenderHelper()
        {
            Helper.GetPrimaryScreenBounds(out var height, out var width);

            var form = OverlayForm.Create(height, width);

            var renderTarget = RenderTargetFactory.Create(form);

            Renderer = new Renderer(form, renderTarget);

            Brushes = new BrushHelper(renderTarget);
        }

        public Renderer Renderer { get; }

        public BrushHelper Brushes { get; }

        //public void RenderBones(Player player)
        //{
        //    var context = StateContext.Instance;

        //    try
        //    {
        //        for (var index = 0; index < PlayerBody.BoneLinkIndicies.Count; index++)
        //        {
        //            var w1 = player.PlayerBody.BoneTransforms[PlayerBody.BoneLinkIndicies[index]].GetPosition();
        //            var w2 = player.PlayerBody.BoneTransforms[PlayerBody.BoneLinkIndicies[index + 1]].GetPosition();

        //            var s1 = context.GameWorld.MainCamera.WorldToScreen(w1);
        //            var s2 = context.GameWorld.MainCamera.WorldToScreen(w2);

        //            if (s1 == Vector2.Zero || s2 == Vector2.Zero)
        //            {
        //                continue;
        //            }

        //            renderer.Line(s1.X, s1.Y, s2.X, s2.Y, renderer.Brushes[Color.LimeGreen]);
        //        }
        //    }
        //    catch (Exception) { }
        //}
    }
}
