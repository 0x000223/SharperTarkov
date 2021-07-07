using System;
using System.Numerics;
using System.Threading;
using System.Diagnostics;
using System.Threading.Tasks;

using SharperTarkov.ScriptingTypes;
using SharperTarkov.UnityEngineTypes;

namespace SharperTarkov
{
    public class WorldContext
    {
        public static void Initialize()
        {
            StateContext.Instance.EnterRaid += (sender, e) =>
            {
                do
                {
                    GameWorld = GetActiveGameWorld();

                } while (GameWorld is null);

                IsActive = true;

                UpdateTask = Task.Run(Update);
            };

            StateContext.Instance.ExitRaid += (sender, e) => IsActive = false;
        }

        public static Task UpdateTask { get; private set; }

        public static bool IsActive { get; private set; }

        public static GameWorld GameWorld { get; private set; }

        private static void Update()
        {
            while (IsActive)
            {
                UpdatePlayers();

                Thread.Sleep(TimeSpan.FromSeconds(0.5));
            }
        }

        private static void UpdatePlayers()
        {
            var players = GameWorld.GetPlayers();

            if (players.Count != GameWorld.Players.Count)
            {
                lock (GameWorld.Players)
                {
                    GameWorld.Players = players;
                }
            }
        }

        public static Vector2 WorldToScreen(Vector3 origin) => GameWorld.MainCamera.WorldToScreen(origin);

        public static GameWorld GetActiveGameWorld()
        {
            try
            {
                var gameObject = GameObject.GetActiveObjectByName("GameWorld");

                if (gameObject is null)
                {
                    return default;
                }

                var adddress = gameObject.GetComponentByName("GameWorld").ScriptingClass;

                return new GameWorld(adddress);
            }
            catch (NullReferenceException e)
            {
                Trace.WriteLine($"> GetActiveGameWorld() - {e.StackTrace}");

                return null;
            }
        }
    }
}