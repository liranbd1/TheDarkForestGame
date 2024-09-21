// File: Systems/VisibilityManager.cs
using System;
using DarkForestGame.Entities;

namespace DarkForestGame.Systems
{
    /// <summary>
    /// Manages visibility and detection mechanics.
    /// </summary>
    public static class VisibilityManager
    {
        public static bool DetectCivilization(Civilization observer, Civilization target)
        {
            // Detection probability based on target's visibility and observer's detection capabilities
            double detectionChance = target.Visibility - (observer.TechLevel * 0.5) + observer.Traits.StealthBonus;
            Random random = new Random();
            return random.NextDouble() * 100 < detectionChance;
        }
    }
}