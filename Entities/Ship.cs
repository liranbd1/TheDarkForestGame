// File: Entities/Ship.cs
using System;

namespace DarkForestGame.Entities
{
    /// <summary>
    /// Represents a ship used by a civilization.
    /// </summary>
    public class Ship
    {
        public Guid Id { get; private set; }
        public string Name { get; set; }
        public ShipType Type { get; set; }
        public double StealthLevel { get; set; }
        public double Speed { get; set; }
        public Guid OwnerCivilizationId { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        public Ship(string name, ShipType type, Guid ownerCivilizationId, int x, int y)
        {
            Id = Guid.NewGuid();
            Name = name;
            Type = type;
            StealthLevel = 1.0; // Base stealth level
            Speed = 1.0; // Base speed
            OwnerCivilizationId = ownerCivilizationId;
            X = x;
            Y = y;
        }
    }
}