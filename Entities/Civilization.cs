// File: Entities/Civilization.cs
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DarkForestGame.Entities
{
    /// <summary>
    /// Represents a civilization in the game.
    /// </summary>
    public class Civilization
    {
        public Guid Id { get; private set; }
        public string Name { get; set; }
        public TechTree TechTree { get; set; }
        public int TechLevel { get; set; }
        public double Visibility { get; set; }
        public Resource Resources { get; set; }
        public List<Planet> Planets { get; set; }
        public List<Ship> Ships { get; set; }
        public Dictionary<Guid, DiplomacyStatus> Diplomacy { get; set; }
        public Dictionary<string, Technology> ResearchedTechnologies { get; set; }
        public CivilizationTraits Traits { get; set; }
        public ControlType ControlType { get; set; }
        public ObservableCollection<GameTask> OngoingTasks { get; set; }

        public Civilization(string name, TechTree techTree, CivilizationTraits traits, ControlType controlType)
        {
            Id = Guid.NewGuid();
            Name = name;
            TechTree = techTree;
            TechLevel = 1;
            Visibility = 10.0; // Initial visibility
            Resources = new Resource();
            Planets = new List<Planet>();
            Ships = new List<Ship>();
            Diplomacy = new Dictionary<Guid, DiplomacyStatus>();
            ResearchedTechnologies = new Dictionary<string, Technology>();
            Traits = traits;
            ControlType = controlType;
            OngoingTasks = new ObservableCollection<GameTask>();
        }
    }
}