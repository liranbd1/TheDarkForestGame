// File: UI/MapPanel.cs
using DarkForestGame.Entities;
using DarkForestGame.Systems;
using Spectre.Console;
using Spectre.Console.Rendering;
using System;
using System.Collections.Generic;

namespace DarkForestGame.UI
{
    /// <summary>
    /// Responsible for displaying the galaxy map to the player.
    /// </summary>
    public class MapPanel
    {
        private Galaxy galaxy;

        public MapPanel(Galaxy galaxy)
        {
            this.galaxy = galaxy;
        }

        /// <summary>
        /// Displays the map for the given civilization.
        /// </summary>
        public void DisplayMap(Civilization civ)
        {
            var table = new Table();
            table.Border = TableBorder.None;
            table.HideHeaders();

            for (int x = 0; x < galaxy.Width; x++)
            {
                table.AddColumn(new TableColumn("").Centered());
            }

            for (int y = 0; y < galaxy.Height; y++)
            {
                var row = new List<IRenderable>();
                for (int x = 0; x < galaxy.Width; x++)
                {
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
            // For simplicity, visibility is determined by a radius around planets and ships.
            int visibilityRange = 2; // Adjust as needed

            foreach (var planet in civ.Planets)
            {
                if (GetDistance(planet.X, planet.Y, x, y) <= visibilityRange)
                {
                    return true;
                }
            }
            foreach (var ship in civ.Ships)
            {
                if (GetDistance(ship.X, ship.Y, x, y) <= visibilityRange)
                {
                    return true;
                }
            }
            return false;
        }

        private double GetDistance(int x1, int y1, int x2, int y2)
        {
            // Calculates the Euclidean distance between two points
            return Math.Sqrt(Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2));
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
                // Display the first ship's type and ownership
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
    }
}