// File: UI/PlayerStatsPanel.cs
using DarkForestGame.Entities;
using Spectre.Console;

namespace DarkForestGame.UI
{
    public class PlayerStatsPanel
    {
        private Panel? panel;
        private Grid? grid;

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
    }
}