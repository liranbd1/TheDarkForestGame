// File: Systems/Galaxy.cs
using System;
using System.Collections.Generic;
using DarkForestGame.Entities;
using DarkForestGame.Events;

namespace DarkForestGame.Systems
{
    /// <summary>
    /// Represents the game world containing civilizations and planets.
    /// </summary>
    public class Galaxy
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public GalaxyCell[,] Grid { get; set; }
        public List<Civilization> Civilizations { get; set; }
        public List<Player> Players { get; set; }
        public List<Planet> Planets { get; set; }
        private Random random;
        public int CurrentTurn { get; private set; }

        // Event for when a turn ends
        public event TurnEndedEventHandler TurnEnded;

        public Galaxy(int width, int height)
        {
            Width = width;
            Height = height;
            Grid = new GalaxyCell[width, height];
            Civilizations = new List<Civilization>();
            Players = new List<Player>();
            Planets = new List<Planet>();
            random = new Random();
            CurrentTurn = 0;

            // Initialize the grid cells
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    Grid[x, y] = new GalaxyCell(x, y);
                }
            }
        }

        /// <summary>
        /// Adds a human player to the galaxy.
        /// </summary>
        public void AddPlayer(Player player)
        {
            Players.Add(player);
            Civilizations.Add(player.Civilization);
        }

        /// <summary>
        /// Adds an AI-controlled civilization to the galaxy.
        /// </summary>
        public void AddAICivilization(Civilization aiCivilization)
        {
            Civilizations.Add(aiCivilization);
        }

        /// <summary>
        /// Simulates a turn in the game.
        /// </summary>
        public void UpdatePlayerStatus(Player player)
        {
            CurrentTurn++;
            ProcessCivilizationTurn(player.Civilization);
            // Handle interactions between civilizations.
            HandleInteractions();
        }

        protected virtual void OnTurnEnded(TurnEndedEventArgs e)
        {
            TurnEnded?.Invoke(this, e);
        }

        private void ProcessCivilizationTurn(Civilization civ)
        {
            // Implement resource gathering.
            GatherResources(civ);

            // Process ongoing tasks
            for (int i = civ.OngoingTasks.Count - 1; i >= 0; i--)
            {
                GameTask task = civ.OngoingTasks[i];
                task.DecrementTurn();
                if (task.TurnsRemaining <= 0)
                {
                    // Task completed
                    civ.OngoingTasks.RemoveAt(i);
                }
            }

            // AI actions are handled elsewhere
        }

        private void GatherResources(Civilization civ)
        {
            foreach (var planet in civ.Planets)
            {
                civ.Resources.Add(planet.Resources);
            }

            // Apply civilization traits
            civ.Resources.Minerals = (int)(civ.Resources.Minerals * civ.Traits.ResourceGatheringRate);
            civ.Resources.Energy = (int)(civ.Resources.Energy * civ.Traits.ResourceGatheringRate);
        }

        private void HandleInteractions()
        {
            // Implement detection, diplomacy, and warfare mechanics
        }

    }
}