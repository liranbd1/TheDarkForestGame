// File: Entities/CivilizationTraits.cs
namespace DarkForestGame.Entities
{
    /// <summary>
    /// Represents traits unique to a civilization.
    /// </summary>
    public class CivilizationTraits
    {
        public double StealthBonus { get; set; }
        public double ResourceGatheringRate { get; set; }
        public double MilitaryStrengthModifier { get; set; }

        // Additional traits as needed
    }
}