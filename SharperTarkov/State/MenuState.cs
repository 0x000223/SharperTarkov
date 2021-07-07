using System;
using System.Diagnostics;

namespace SharperTarkov.State
{
    public class MenuState : IGameState
    {
        public MenuState() { }

        public void Off(StateContext context)
        {
            context.CurrentState = new OffState();

            context.OnTurnedOff(EventArgs.Empty);

            Trace.WriteLine($"> MenuState.Off()");
        }

        public void Menu(StateContext context)
        {
            // No change
        }

        public void Raid(StateContext context)
        {
            context.CurrentState = new RaidState();

            context.OnEnterLiveState(EventArgs.Empty);

            Trace.WriteLine($"> MenuState.Raid()");
        }
    }
}