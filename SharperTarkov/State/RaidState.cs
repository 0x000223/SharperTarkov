using System;
using System.Diagnostics;

namespace SharperTarkov.State
{
    public class RaidState : IGameState
    {
        public RaidState() { }

        public void Off(StateContext context)
        {
            context.CurrentState = new OffState();

            context.OnExitLiveState(EventArgs.Empty);

            context.OnTurnedOff(EventArgs.Empty);

            Trace.WriteLine($"> RaidState.Off()");
        }

        public void Menu(StateContext context)
        {
            context.CurrentState = new MenuState();

            context.OnExitLiveState(EventArgs.Empty);

            Trace.WriteLine($"> RaidState.Menu()");
        }

        public void Raid(StateContext context)
        {
            // No change
        }
    }
}
