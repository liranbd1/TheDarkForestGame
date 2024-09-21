// File: Systems/DiplomacySystem.cs
using DarkForestGame.Entities;
using System;

namespace DarkForestGame.Systems
{
    /// <summary>
    /// Manages diplomacy interactions between civilizations.
    /// </summary>
    public static class DiplomacySystem
    {
        public static void SendMessage(Civilization sender, Civilization receiver, string message)
        {
            // Increase visibility due to communication
            sender.Visibility += 2.0;

            // Risk of interception by other civilizations
            // (Implement interception mechanics as needed)

            // Deliver message to receiver
            // (Implement message handling)
        }

        public static void FormAlliance(Civilization civA, Civilization civB)
        {
            civA.Diplomacy[civB.Id] = DiplomacyStatus.Allied;
            civB.Diplomacy[civA.Id] = DiplomacyStatus.Allied;
        }

        public static void BreakAlliance(Civilization civA, Civilization civB)
        {
            civA.Diplomacy[civB.Id] = DiplomacyStatus.Neutral;
            civB.Diplomacy[civA.Id] = DiplomacyStatus.Neutral;
        }
    }
}