using System;
using System.Linq;
using SharpMemory;
using SharperTarkov.State;
using SharperTarkov.UnityEngineTypes;

namespace SharperTarkov
{
    public sealed class StateContext
    {
        public event EventHandler TurnedOn;

        public event EventHandler TurnedOff;

        public event EventHandler EnterRaid;

        public event EventHandler ExitRaid;

        private StateContext()
        {
            CurrentState = new OffState();
        }

        static StateContext()
        {
            Instance = new StateContext();
        }

        public static StateContext Instance { get; }

        public IGameState CurrentState { get; set; }

        public void OnTurnedOn(EventArgs e) => TurnedOn?.Invoke(this, e);

        public void OnTurnedOff(EventArgs e) => TurnedOff?.Invoke(this, e);

        public void OnEnterLiveState(EventArgs e) => EnterRaid?.Invoke(this, e);

        public void OnExitLiveState(EventArgs e) => ExitRaid?.Invoke(this, e);

        public void ExecuteState()
        {
            if (ProcessHelper.GetProcessId("EscapeFromTarkov") != 0)
            {
                if (GameObject.GetTaggedObjects(1).Any())
                {
                    CurrentState.Raid(this);
                    return;
                }
                    
                CurrentState.Menu(this);
                return;
            }

            CurrentState.Off(this);
        }
    }
}