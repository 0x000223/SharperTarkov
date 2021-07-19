using System;
using System.Threading.Tasks;

using SharpRender;

namespace SharperTarkov
{
    class Program
    {
        static void Main()
        {
            MemoryContext.Initialize();

            WorldContext.Initialize();

            Helper.GetPrimaryScreenBounds(out var height, out var width);

            var form = OverlayForm.Create(height, width);

            var graphics = new Graphics(form);

            _ = Task.Run(() =>
            {
                while (true)
                {
                    try
                    {
                        StateContext.Instance.ExecuteState();
                    }
                    catch (Exception)
                    {
                        continue;
                    }

                    Task.Delay(500);
                }
            });

            graphics.Loop();
        }
    }
}
