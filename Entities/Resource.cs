// File: Entities/Resource.cs
namespace DarkForestGame.Entities
{
    /// <summary>
    /// Represents resources available to a civilization or on a planet.
    /// </summary>
    public class Resource
    {
        public int Minerals { get; set; }
        public int Energy { get; set; }
        public int Intelligence { get; set; }

        public Resource(int minerals = 0, int energy = 0, int intelligence = 0)
        {
            Minerals = minerals;
            Energy = energy;
            Intelligence = intelligence;
        }

        // Methods for adding and subtracting resources
        public void Add(Resource other)
        {
            Minerals += other.Minerals;
            Energy += other.Energy;
            Intelligence += other.Intelligence;
        }

        public void Subtract(Resource other)
        {
            Minerals -= other.Minerals;
            Energy -= other.Energy;
            Intelligence -= other.Intelligence;
        }
    }
}