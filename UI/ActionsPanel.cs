// File: UI/ActionsPanel.cs
using DarkForestGame.Entities;
using DarkForestGame.Events;
using DarkForestGame.Systems;
using Spectre.Console;
using System;
using System.Collections.Generic;

namespace DarkForestGame.UI
{
    /// <summary>
    /// Handles displaying possible actions to the player and processing their input.
    /// </summary>
    public class ActionsPanel
    {
        private Galaxy galaxy;

        public ActionsPanel(Galaxy galaxy)
        {
            this.galaxy = galaxy;
        }

        /// <summary>
        /// Displays the action menu and handles the player's choice.
        /// </summary>
        public bool DisplayPossibleActions(Player player)
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
    }
}