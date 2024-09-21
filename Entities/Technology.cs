// File: Entities/Technology.cs
using System;
using System.Collections.Generic;

namespace DarkForestGame.Entities
{
    /// <summary>
    /// Represents a technology that can be researched.
    /// </summary>
    public class Technology
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int ResearchCost { get; set; }
        public List<string> Prerequisites { get; set; }
        public Action<Civilization> ApplyEffects { get; set; }
        public bool IsUnique { get; set; }

        public Technology(string name, string description, int researchCost, Action<Civilization> applyEffects, bool isUnique = false)
        {
            Name = name;
            Description = description;
            ResearchCost = researchCost;
            ApplyEffects = applyEffects;
            Prerequisites = new List<string>();
            IsUnique = isUnique;
        }
    }

    /// <summary>
    /// Represents the tech tree for a civilization.
    /// </summary>
    public class TechTree
    {
        public Dictionary<string, Technology> Technologies { get; set; }

        public TechTree()
        {
            Technologies = new Dictionary<string, Technology>();
        }

        public void AddTechnology(Technology tech)
        {
            Technologies[tech.Name] = tech;
        }

        // Method to get available technologies for research
        public List<Technology> GetAvailableTechnologies(Civilization civ)
        {
            var availableTechs = new List<Technology>();
            foreach (var tech in Technologies.Values)
            {
                if (!civ.ResearchedTechnologies.ContainsKey(tech.Name) && ArePrerequisitesMet(civ, tech))
                {
                    availableTechs.Add(tech);
                }
            }
            return availableTechs;
        }

        private bool ArePrerequisitesMet(Civilization civ, Technology tech)
        {
            foreach (var prereq in tech.Prerequisites)
            {
                if (!civ.ResearchedTechnologies.ContainsKey(prereq))
                {
                    return false;
                }
            }
            return true;
        }
    }
}