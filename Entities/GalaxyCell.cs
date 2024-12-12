// File: Entities/GalaxyCell.cs
using System.Collections.Generic;

namespace DarkForestGame.Entities
{
    /// <summary>
    /// Represents a cell in the galaxy grid.
    /// </summary>
    public class GalaxyCell
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Planet? Planet { get; set; }
        public List<Ship> Ships { get; set; }

        public GalaxyCell(int x, int y)
        {
            X = x;
            Y = y;
            Planet = null;
            Ships = new List<Ship>();
        }
    }
}