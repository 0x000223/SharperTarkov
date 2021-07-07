namespace SharperTarkov.State
{
    public interface IGameState
    {
        void Off(StateContext context);

        void Menu(StateContext context);

        void Raid(StateContext context);
    }
}