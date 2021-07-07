using System;
using System.Linq;
using System.Collections.Generic;
using System.Numerics;
using SharperTarkov.UnityEngineTypes;

namespace SharperTarkov.ScriptingTypes
{
    public class GameWorld
    {
        public GameWorld(ulong address)
        {

            Address = address;

            do
            {
                MainCamera = Camera.GetMainCameraComponent();

            } while (MainCamera is null);

            Players = GetPlayers();

            LocalPlayer = Players.First();
        }

        public ulong Address { get; }

        public Player LocalPlayer { get; set; }

        public List<Player> Players { get; set; }

        public Camera MainCamera { get; set; }

        public Vector2 WorldToScreen(Vector3 origin) => MainCamera.WorldToScreen(origin);

        public List<Player> GetPlayers()
        {
            return MemoryHelper.ReadList<Player>(Address + Offsets.GameWorld.RegisteredPlayers);
        }
    }
}