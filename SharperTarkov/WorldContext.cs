﻿using System;
using System.Linq;
using System.Numerics;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

using SharpMemory.Ioctl;
using SharperTarkov.ScriptingTypes;
using SharperTarkov.UnityEngineTypes;

using Settings = SharperTarkov.Properties.Settings;

namespace SharperTarkov
{
    public class WorldContext
    {
        private static Player _local;

        private static List<Player> _players;

        public static void Initialize()
        {
            StateContext.Instance.EnterRaid += (sender, e) =>
            {
                do
                {
                    ScriptingClass = GetGameWorld();

                } while (ScriptingClass == 0);

                do
                {
                    MainCamera = Camera.GetMainCamera();

                } while (MainCamera.Address == 0);

                _players = GetPlayers();

                _local = _players.First();

                Corpses = new List<Corpse>();

                Grenades = new List<Grenade>();

                IsActive = true;

                var updatePlayersTask = Task.Run(async () =>
                {
                    while (IsActive)
                    {
                        try
                        {
                            var players = GetPlayers();

                            if (players.Count != _players.Count && players.Any())
                            {
                                Interlocked.Exchange(ref _players, players);

                                Interlocked.Exchange(ref _local, players.First(player => player.IsLocalPlayer));

                                Trace.WriteLine($"[{DateTime.Now}] updatePlayerTask : Updated players, count = {players.Count}");
                            }
                        }
                        catch (Exception e)
                        {
                            Trace.WriteLine($"[{DateTime.Now}] WorldContext.Initialize : {e.Message}");
                        }
                        finally
                        {
                            foreach (var player in _players)
                            {
                                player.Update();
                            }
                        }

                        await Task.Delay(TimeSpan.FromSeconds(1));
                    }
                });

                var updateBonesTask = Task.Run(async () =>
                {
                    while (IsActive)
                    {
                        foreach (var player in _players)
                        {
                            for (var index = 0; index < player.Body.Transforms.Count; index++)
                            {
                                player.Body.Positions[index] = player.Body.Transforms[index].GetPosition();
                            }
                        }
                    }

                    await Task.Delay(TimeSpan.FromMilliseconds(200));
                });

                var updateCorpsesTask = Task.Run(async () =>
                {
                    while (IsActive)
                    {
                        var corpses = GetCorpses();

                        if (corpses.Count != Corpses.Count)
                        {
                            lock (Corpses)
                            {
                                Corpses = corpses;
                            }
                        }

                        await Task.Delay(TimeSpan.FromSeconds(1));
                    }
                });

            };

            StateContext.Instance.ExitRaid += (sender, e) => IsActive = false;
        }

        public static ulong ScriptingClass { get; private set; }

        public static bool IsActive { get; private set; }

        public static Camera MainCamera { get; private set; }

        public static Player LocalPlayer => _local;

        public static List<Player> Players => _players;

        public static List<Corpse> Corpses { get; private set; }

        public static List<Grenade> Grenades { get; private set; }

        public static Vector2 WorldToScreen(Vector3 origin) => MainCamera.WorldToScreen(origin);

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