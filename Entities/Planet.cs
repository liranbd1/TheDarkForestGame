// File: Entities/Planet.cs
using System;

namespace DarkForestGame.Entities
{
    /// <summary>
    /// Represents a planet in the galaxy.
    /// </summary>
    public class Planet
    {
        public Guid Id { get; private set; }
        public string Name { get; set; }
        public Resource Resources { get; set; }
        public bool IsColonized { get; set; }
        public Guid? OwnerCivilizationId { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        public Planet(string name, Resource resources, int x, int y)
        {
            Id = Guid.NewGuid();
            Name = name;
            Resources = resources;
            IsColonized = false;
            OwnerCivilizationId = null;
            X = x;
            Y = y;
        }
    }
}