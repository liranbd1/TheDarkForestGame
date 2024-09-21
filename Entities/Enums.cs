// File: Entities/Enums.cs
namespace DarkForestGame.Entities
{
    /// <summary>
    /// Indicates whether a civilization is controlled by a human player or AI.
    /// </summary>
    public enum ControlType
    {
        Human,
        AI
    }

    /// <summary>
    /// Represents different statuses in diplomacy.
    /// </summary>
    public enum DiplomacyStatus
    {
        Neutral,
        Allied,
        AtWar
    }

    /// <summary>
    /// Enumeration for ship types.
    /// </summary>
    public enum ShipType
    {
        Exploration,
        Combat,
        Espionage,
        Colonization
    }
}