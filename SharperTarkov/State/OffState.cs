using System;
using System.Diagnostics;

namespace SharperTarkov.State
{
    public class OffState : IGameState
    {
        public OffState() { }

        public void Off(StateContext context)
        {
            // No change
        }

        public void Menu(StateContext context)
        {
            context.CurrentState = new MenuState();

            context.OnTurnedOn(EventArgs.Empty);

            Trace.WriteLine($"> OffState.Menu()");
        }

        public void Raid(StateContext context)
        {
            context.CurrentState = new RaidState();

            context.OnTurnedOn(EventArgs.Empty);

            context.OnEnterLiveState(EventArgs.Empty);

            Trace.WriteLine($"> OffState.Raid()");
        }
    }
}