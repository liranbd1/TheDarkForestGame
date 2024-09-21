// File: DisplaySettings.cs
using System;
using System.Collections.Generic;

namespace DarkForestGame
{
    public static class DisplaySettings
    {
        public static Dictionary<string, (char Symbol, ConsoleColor Color)> EntityDisplay = new Dictionary<string, (char, ConsoleColor)>
        {
            { "Empty", ('.', ConsoleColor.Gray) },
            { "Planet", ('O', ConsoleColor.Green) },
            { "ExplorationShip", ('E', ConsoleColor.Blue) },
            { "CombatShip", ('C', ConsoleColor.Red) },
            { "EspionageShip", ('I', ConsoleColor.Yellow) },
            { "ColonizationShip", ('L', ConsoleColor.Magenta) },
            // Add more as needed
        };
    }
}