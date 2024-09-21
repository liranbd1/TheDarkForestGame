// File: UI/GameUI.cs
using DarkForestGame.Entities;
using DarkForestGame.Events;
using DarkForestGame.Systems;
using Spectre.Console;
using Spectre.Console.Rendering;
using System;
using System.Collections.Generic;

namespace DarkForestGame.UI
{
    /// <summary>
    /// Handles user interactions and game display.
    /// </summary>
    public class GameUI
    {
        private Galaxy galaxy;
        private Grid playerStatsGrid;
        public GameUI(Galaxy galaxy)
        {
            this.galaxy = galaxy;

            // Subscribe to galaxy events
            galaxy.TurnEnded += OnTurnEnded;

            // Subscribe to player events
            foreach (var player in galaxy.Players)
            {
                SubscribeToPlayerEvents(player);
            }
        }

        private void SubscribeToPlayerEvents(Player player)
        {
            player.ShipBuilt += OnShipBuilt;
            player.PlanetColonized += OnPlanetColonized;
            // Subscribe to other events as needed
        }

        public void StartGameLoop()
        {

            bool gameRunning = true;
            while (gameRunning)
            {
                // Clear the console at the start of each turn
                AnsiConsole.Clear();

                // Handle player input
                foreach (var player in galaxy.Players)
                {
                    HandlePlayerTurn(player);
                }

                // Simulate a turn
                galaxy.SimulateTurn();

                // Check game status
                gameRunning = CheckGameStatus();

                // Wait for user input before proceeding to the next turn
                AnsiConsole.Markup("[bold yellow]Press [green]Enter[/] to continue to the next turn...[/]");
                Console.ReadLine();
            }
        }

        private void HandlePlayerTurn(Player player)
        {
            player.NewOngoingTaskCreated += OnNewOngoingTaskCreated;
            // Clear the console for the player's turn
            AnsiConsole.Clear();

            // Display the map
            DisplayMap(player.Civilization);

            // Display player stats
            DisplayPlayerStats(player.Civilization);

            // Display possible actions
            bool endTurn = false;
            while (!endTurn)
            {
                endTurn = DisplayPossibleActions(player);
            }

            player.NewOngoingTaskCreated -= OnNewOngoingTaskCreated;
        }

        private void OnNewOngoingTaskCreated(object sender, TaskCreatedEventArgs e)
        {
            AnsiConsole.Clear();
            if (sender is Player player)
            {
                DisplayMap(player.Civilization);
                DisplayPlayerStats(player.Civilization);
            }
        }

        private void DisplayMap(Civilization civ)
        {
            // Create a table for the map
            var table = new Table();
            table.Border = TableBorder.None;
            table.HideHeaders();

            // Add columns for each column in the galaxy
            for (int x = 0; x < galaxy.Width; x++)
            {
                table.AddColumn(new TableColumn("").Centered());
            }

            // Build the map rows
            for (int y = 0; y < galaxy.Height; y++)
            {
                var row = new List<IRenderable>();
                for (int x = 0; x < galaxy.Width; x++)
                {
                    // Determine if the cell is visible to the player
                    bool isVisible = IsCellVisibleToPlayer(civ, x, y);

                    if (isVisible)
                    {
                        var cellContent = GetCellContent(civ, x, y);
                        row.Add(new Markup(cellContent).Centered());
                    }
                    else
                    {
                        row.Add(new Markup("[grey]·[/]").Centered());
                    }
                }
                table.AddRow(row);
            }

            var panel = new Panel(table)
            {
                Header = new PanelHeader($"[bold blue]{civ.Name}'s View of the Galaxy[/]"),
                Padding = new Padding(1, 1)
            };

            AnsiConsole.Write(panel);
        }

        private bool IsCellVisibleToPlayer(Civilization civ, int x, int y)
        {
            // Simple visibility logic: player can see cells adjacent to their planets and ships
            foreach (var planet in civ.Planets)
            {
                if (Math.Abs(planet.X - x) <= 1 && Math.Abs(planet.Y - y) <= 1)
                {
                    return true;
                }
            }
            foreach (var ship in civ.Ships)
            {
                if (Math.Abs(ship.X - x) <= 1 && Math.Abs(ship.Y - y) <= 1)
                {
                    return true;
                }
            }
            return false;
        }

        private string GetCellContent(Civilization civ, int x, int y)
        {
            GalaxyCell cell = galaxy.Grid[x, y];

            if (cell.Planet != null)
            {
                if (cell.Planet.OwnerCivilizationId == civ.Id)
                {
                    // Player's own planet
                    return "[green]P[/]";
                }
                else if (cell.Planet.IsColonized)
                {
                    // Colonized planet owned by another civilization
                    return "[red]X[/]";
                }
                else
                {
                    // Uncolonized planet
                    return "[yellow]O[/]";
                }
            }
            else if (cell.Ships.Count > 0)
            {
                // For simplicity, display the first ship's type and ownership
                Ship ship = cell.Ships[0];
                if (ship.OwnerCivilizationId == civ.Id)
                {
                    // Player's own ship
                    return "[cyan]S[/]";
                }
                else
                {
                    // Ship belonging to another civilization
                    return "[darkred]E[/]"; // Enemy ship
                }
            }
            else
            {
                // Empty space
                return "[grey].[/]";
            }
        }

        private void DisplayPlayerStats(Civilization civ)
        {
            // Create a grid for the stats
            var grid = new Grid();
            grid.AddColumn();
            grid.AddColumn();

            // Resources
            grid.AddRow("[bold]Resources[/]", "");
            grid.AddRow("Minerals:", civ.Resources.Minerals.ToString());
            grid.AddRow("Energy:", civ.Resources.Energy.ToString());
            grid.AddRow("Intelligence:", civ.Resources.Intelligence.ToString());

            // Ongoing Tasks
            grid.AddEmptyRow();
            grid.AddRow("[bold]Ongoing Tasks[/]", "");
            if (civ.OngoingTasks.Count == 0)
            {
                grid.AddRow("No ongoing tasks.", "");
            }
            else
            {
                foreach (var task in civ.OngoingTasks)
                {
                    grid.AddRow(task.Description, $"Turns remaining: {task.TurnsRemaining}");
                }
            }

            // Create a panel with the grid as content
            var panel = new Panel(grid)
            {
                Header = new PanelHeader("[bold yellow]Player Stats[/]"),
                Padding = new Padding(1, 1)
            };

            AnsiConsole.Write(panel);
        }

        private bool DisplayPossibleActions(Player player)
        {
            var prompt = new SelectionPrompt<string>()
                .Title("[bold green]Choose an action:[/]")
                .PageSize(10)
                .AddChoices(new[] {
                    "Research Technology",
                    "Build Ship",
                    "Colonize Planet",
                    "Launch Attack",
                    "End Turn"
                });

            string choice = AnsiConsole.Prompt(prompt);

            switch (choice)
            {
                case "Research Technology":
                    HandleResearchTechnology(player);
                    return false;
                case "Build Ship":
                    HandleBuildShip(player);
                    return false;
                case "Colonize Planet":
                    HandleColonizePlanet(player);
                    return false;
                case "Launch Attack":
                    HandleLaunchAttack(player);
                    return false;
                case "End Turn":
                    return true;
                default:
                    return false;
            }
        }

        private void HandleResearchTechnology(Player player)
        {
            var availableTechs = player.GetAvailableTechnologies();
            if (availableTechs.Count == 0)
            {
                AnsiConsole.MarkupLine("[red]No technologies available for research.[/]");
                return;
            }

            var techChoices = new List<string>();
            foreach (var tech in availableTechs)
            {
                techChoices.Add($"{tech.Name} (Cost: {tech.ResearchCost} Intelligence)");
            }
            techChoices.Add("Cancel");

            var prompt = new SelectionPrompt<string>()
                .Title("Select a technology to research:")
                .AddChoices(techChoices);

            string choice = AnsiConsole.Prompt(prompt);

            if (choice == "Cancel")
                return;

            int selectedIndex = techChoices.IndexOf(choice);
            var selectedTech = availableTechs[selectedIndex];

            bool success = player.ResearchTechnology(selectedTech);
            if (success)
            {
                AnsiConsole.MarkupLine($"[green]Research on '{selectedTech.Name}' started.[/]");
            }
            else
            {
                AnsiConsole.MarkupLine("[red]Not enough Intelligence resources.[/]");
            }
        }

        private void HandleBuildShip(Player player)
        {
            if (player.Civilization.Planets.Count == 0)
            {
                AnsiConsole.MarkupLine("[red]You need at least one planet to build ships.[/]");
                return;
            }

            var shipOptions = player.GetBuildableShips();
            var choices = new List<string>(shipOptions.Keys);
            choices.Add("Cancel");

            var prompt = new SelectionPrompt<string>()
                .Title("Choose a ship type to build:")
                .AddChoices(choices);

            string choice = AnsiConsole.Prompt(prompt);

            if (choice == "Cancel")
                return;

            var (type, cost) = shipOptions[choice];

            bool success = player.BuildShip(type, cost);
            if (success)
            {
                AnsiConsole.MarkupLine($"[green]{type} ship construction started.[/]");
            }
            else
            {
                AnsiConsole.MarkupLine("[red]Not enough Minerals to build the ship.[/]");
            }
        }

        private void HandleColonizePlanet(Player player)
        {
            var availablePlanets = player.GetUncolonizedPlanets(galaxy);
            if (availablePlanets.Count == 0)
            {
                AnsiConsole.MarkupLine("[red]No uncolonized planets available.[/]");
                return;
            }

            var planetChoices = new List<string>();
            foreach (var planet in availablePlanets)
            {
                planetChoices.Add($"{planet.Name} at ({planet.X}, {planet.Y})");
            }
            planetChoices.Add("Cancel");

            var planetPrompt = new SelectionPrompt<string>()
                .Title("Select a planet to colonize:")
                .AddChoices(planetChoices);

            string planetChoice = AnsiConsole.Prompt(planetPrompt);

            if (planetChoice == "Cancel")
                return;

            int planetIndex = planetChoices.IndexOf(planetChoice);
            var selectedPlanet = availablePlanets[planetIndex];

            var colonizationShips = player.GetColonizationShips();
            if (colonizationShips.Count == 0)
            {
                AnsiConsole.MarkupLine("[red]You need a Colonization Ship to colonize a planet.[/]");
                return;
            }

            var shipChoices = new List<string>();
            foreach (var ship in colonizationShips)
            {
                shipChoices.Add($"{ship.Name} at ({ship.X}, {ship.Y})");
            }
            shipChoices.Add("Cancel");

            var shipPrompt = new SelectionPrompt<string>()
                .Title("Select a Colonization Ship to use:")
                .AddChoices(shipChoices);

            string shipChoice = AnsiConsole.Prompt(shipPrompt);

            if (shipChoice == "Cancel")
                return;

            int shipIndex = shipChoices.IndexOf(shipChoice);
            var selectedShip = colonizationShips[shipIndex];

            bool success = player.ColonizePlanet(galaxy, selectedPlanet, selectedShip);
            if (success)
            {
                AnsiConsole.MarkupLine($"[green]Colonization of '{selectedPlanet.Name}' initiated.[/]");
            }
            else
            {
                AnsiConsole.MarkupLine("[red]Failed to initiate colonization.[/]");
            }
        }

        private void HandleLaunchAttack(Player player)
        {
            var combatShips = player.GetCombatShips();
            if (combatShips.Count == 0)
            {
                AnsiConsole.MarkupLine("[red]No combat ships available.[/]");
                return;
            }

            var shipChoices = new List<string>();
            foreach (var ship in combatShips)
            {
                shipChoices.Add($"{ship.Name} at ({ship.X}, {ship.Y})");
            }
            shipChoices.Add("Cancel");

            var shipPrompt = new SelectionPrompt<string>()
                .Title("Select a combat ship to use:")
                .AddChoices(shipChoices);

            string shipChoice = AnsiConsole.Prompt(shipPrompt);

            if (shipChoice == "Cancel")
                return;

            int shipIndex = shipChoices.IndexOf(shipChoice);
            var selectedShip = combatShips[shipIndex];

            // Prompt for target coordinates
            int targetX = AnsiConsole.Ask<int>("Enter target X coordinate:");
            int targetY = AnsiConsole.Ask<int>("Enter target Y coordinate:");

            bool success = player.LaunchAttack(galaxy, selectedShip, targetX, targetY);
            if (success)
            {
                AnsiConsole.MarkupLine($"[green]Attack on ({targetX}, {targetY}) initiated.[/]");
            }
            else
            {
                AnsiConsole.MarkupLine("[red]Failed to launch attack.[/]");
            }
        }

        private void OnTurnEnded(object sender, TurnEndedEventArgs e)
        {
            // Optional: Add end-of-turn notifications
        }

        private void OnShipBuilt(object sender, ShipBuiltEventArgs e)
        {
            // Optional: Add ship built notifications
        }

        private void OnPlanetColonized(object sender, PlanetColonizedEventArgs e)
        {
            // Optional: Add planet colonized notifications
        }

        private bool CheckGameStatus()
        {
            // Implement win/loss condition checks
            return true;
        }
    }
}
