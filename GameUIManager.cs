// File: UI/GameUIManager.cs
using DarkForestGame.Entities;
using DarkForestGame.Events;
using DarkForestGame.Systems;
using Spectre.Console;
using System;

namespace DarkForestGame.UI
{
    /// <summary>
    /// Manages the user interface, coordinating between different UI panels.
    /// </summary>
    public class GameUIManager
    {
        private Galaxy galaxy;
        private MapPanel mapPanel;
        private PlayerStatsPanel statsPanel;
        private ActionsPanel actionsPanel;

        public GameUIManager(Galaxy galaxy)
        {
            this.galaxy = galaxy;
            this.mapPanel = new MapPanel(galaxy);
            this.statsPanel = new PlayerStatsPanel();
            this.actionsPanel = new ActionsPanel(galaxy);

            // Subscribe to game events
            galaxy.TurnEnded += OnTurnEnded;

            foreach (var player in galaxy.Players)
            {
                SubscribeToPlayerEvents(player);
            }
        }

        /// <summary>
        /// Updates the UI for the given player.
        /// </summary>
        public void UpdateForPlayer(Player player)
        {
            // Clear the console for the player's turn
            AnsiConsole.Clear();

            // Display the map
            mapPanel.DisplayMap(player.Civilization);

            // Display player stats
            statsPanel.DisplayPlayerStats(player.Civilization);
        }

        /// <summary>
        /// Displays possible actions and handles user input.
        /// </summary>
        public bool DisplayPossibleActions(Player player)
        {
            return actionsPanel.DisplayPossibleActions(player);
        }

        /// <summary>
        /// Waits for user input before proceeding to the next turn.
        /// </summary>
        public void WaitForNextTurn()
        {
            // Wait for user input before proceeding to the next turn
            AnsiConsole.Markup("[bold yellow]Press [green]Enter[/] to continue to the next turn...[/]");
            Console.ReadLine();
        }

        private void SubscribeToPlayerEvents(Player player)
        {
            player.ShipBuilt += OnShipBuilt;
            player.PlanetColonized += OnPlanetColonized;
            player.NewOngoingTaskCreated += OnNewOngoingTaskCreated;
        }

        private void OnNewOngoingTaskCreated(object sender, TaskCreatedEventArgs e)
        {
            if (sender is Player player)
            {
                AnsiConsole.Clear();
                mapPanel.DisplayMap(player.Civilization);
                // Update the stats panel to reflect new tasks
                statsPanel.DisplayPlayerStats(player.Civilization);
            }
        }

        private void OnTurnEnded(object sender, TurnEndedEventArgs e)
        {
            // Handle any end-of-turn UI updates if necessary
        }

        private void OnShipBuilt(object sender, ShipBuiltEventArgs e)
        {
            // Handle UI updates for ship built events
            AnsiConsole.MarkupLine($"[green]Ship '{e.NewShip.Name}' has been built.[/]");
        }

        private void OnPlanetColonized(object sender, PlanetColonizedEventArgs e)
        {
            // Handle UI updates for planet colonized events
            AnsiConsole.MarkupLine($"[green]Planet '{e.ColonizedPlanet.Name}' has been colonized.[/]");
        }
    }
}