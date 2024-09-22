// File: UI/PlayerStatsPanel.cs
using DarkForestGame.Entities;
using Spectre.Console;
using System.Collections.Generic;

namespace DarkForestGame.UI
{
    public class PlayerStatsPanel
    {
        private Panel panel;
        private Grid grid;

        public PlayerStatsPanel()
        {
        }

        public void DisplayPlayerStats(Civilization civ)
        {
            grid = new Grid();
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

            panel = new Panel(grid)
            {
                Header = new PanelHeader("[bold yellow]Player Stats[/]"),
                Padding = new Padding(1, 1)
            };
            AnsiConsole.Write(panel);
        }

        // public void UpdateOngoingTasks(List<GameTask> ongoingTasks)
        // {
        //     // Remove existing task rows
        //     // Assuming tasks start after the "Ongoing Tasks" header, find the index
        //     var rows = grid.Rows.ToList();
        //     var bob = grid.Rows[0];
        //     int index = rows.FindIndex(row => row.Columns[0].ToString().Contains("[bold]Ongoing Tasks[/]")) + 1;

        //     // Remove old task rows
        //     while (grid.Rows.Count > index)
        //     {
        //         grid.Rows.RemoveAt(index);
        //     }

        //     // Add updated tasks
        //     if (ongoingTasks.Count == 0)
        //     {
        //         grid.AddRow("No ongoing tasks.", "");
        //     }
        //     else
        //     {
        //         foreach (var task in ongoingTasks)
        //         {
        //             grid.AddRow(task.Description, $"Turns remaining: {task.TurnsRemaining}");
        //         }
        //     }

        //     // Refresh the panel
        //     AnsiConsole.Clear();
        //     AnsiConsole.Write(panel);
        // }
    }
}