// File: Player.cs
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using DarkForestGame.Entities;
using DarkForestGame.Events;
using DarkForestGame.Systems;

namespace DarkForestGame
{
    /// <summary>
    /// Represents a human player in the game.
    /// </summary>
    public class Player
    {
        /// <summary>
        /// The player's civilization.
        /// </summary>
        public Civilization Civilization { get; private set; }

        // Events
        public event ShipBuiltEventHandler ShipBuilt;
        public event PlanetColonizedEventHandler PlanetColonized;
        public event TaskCreatedEventHandler NewOngoingTaskCreated;

        /// <summary>
        /// Initializes a new instance of the Player class.
        /// </summary>
        public Player(string civilizationName, TechTree techTree, CivilizationTraits traits)
        {
            // Create the player's civilization.
            Civilization = new Civilization(civilizationName, techTree, traits, ControlType.Human);

            // Initialize the player's starting resources.
            Civilization.Resources = new Resource(minerals: 1000, energy: 500, intelligence: 100);
            Civilization.OngoingTasks.CollectionChanged += OngoingTasksCollectionChanged;
        }

        // Methods for player actions.

        /// <summary>
        /// Gets the list of available technologies for research.
        /// </summary>
        public List<Technology> GetAvailableTechnologies()
        {
            return Civilization.TechTree.GetAvailableTechnologies(Civilization);
        }

        /// <summary>
        /// Initiates research on a selected technology.
        /// </summary>
        public bool ResearchTechnology(Technology selectedTech)
        {
            if (Civilization.Resources.Intelligence >= selectedTech.ResearchCost)
            {
                Civilization.Resources.Intelligence -= selectedTech.ResearchCost;

                // Calculate research time
                int researchTime = selectedTech.ResearchCost / 50;
                if (researchTime < 1) researchTime = 1;

                // Create research task
                var researchTask = new GameTask(
                    description: $"Researching {selectedTech.Name}",
                    turns: researchTime,
                    onCompletion: () =>
                    {
                        Civilization.ResearchedTechnologies[selectedTech.Name] = selectedTech;
                        selectedTech.ApplyEffects(Civilization);
                        // Optionally, fire an event or handle notifications elsewhere
                    }
                );
                Civilization.OngoingTasks.Add(researchTask);
                return true;
            }
            else
            {
                return false; // Not enough intelligence resources
            }
        }

        /// <summary>
        /// Gets a dictionary of buildable ship types with their costs.
        /// </summary>
        public Dictionary<string, (ShipType Type, int Cost)> GetBuildableShips()
        {
            return new Dictionary<string, (ShipType, int)>
            {
                { "Exploration (Cost: 100 Minerals)", (ShipType.Exploration, 100) },
                { "Combat (Cost: 200 Minerals)", (ShipType.Combat, 200) },
                { "Espionage (Cost: 150 Minerals)", (ShipType.Espionage, 150) },
                { "Colonization (Cost: 250 Minerals)", (ShipType.Colonization, 250) }
            };
        }

        /// <summary>
        /// Initiates the building of a ship of the specified type.
        /// </summary>
        public bool BuildShip(ShipType type, int cost)
        {
            if (Civilization.Planets.Count == 0)
            {
                // Cannot build ships without a planet
                return false;
            }

            if (Civilization.Resources.Minerals >= cost)
            {
                Civilization.Resources.Minerals -= cost;
                int constructionTime = cost / 50;
                if (constructionTime < 1) constructionTime = 1;

                var buildTask = new GameTask(
                    description: $"Building {type} Ship",
                    turns: constructionTime,
                    onCompletion: () =>
                    {
                        var newShip = new Ship($"{type} Ship", type, Civilization.Id, Civilization.Planets[0].X, Civilization.Planets[0].Y);
                        Civilization.Ships.Add(newShip);
                        OnShipBuilt(new ShipBuiltEventArgs { NewShip = newShip });
                        // Handle any post-build actions
                    }
                );
                Civilization.OngoingTasks.Add(buildTask);
                return true;
            }
            else
            {
                // Not enough minerals
                return false;
            }
        }

        private void OngoingTasksCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            var eventArgs = new TaskCreatedEventArgs();
            NewOngoingTaskCreated?.Invoke(this, eventArgs);
        }

        /// <summary>
        /// Gets a list of uncolonized planets in the galaxy.
        /// </summary>
        public List<Planet> GetUncolonizedPlanets(Galaxy galaxy)
        {
            return galaxy.Planets.FindAll(p => !p.IsColonized);
        }

        /// <summary>
        /// Gets a list of the player's colonization ships.
        /// </summary>
        public List<Ship> GetColonizationShips()
        {
            return Civilization.Ships.FindAll(s => s.Type == ShipType.Colonization);
        }

        /// <summary>
        /// Initiates the colonization of a selected planet using a selected ship.
        /// </summary>
        public bool ColonizePlanet(Galaxy galaxy, Planet selectedPlanet, Ship selectedShip)
        {
            int distance = Math.Abs(selectedPlanet.X - selectedShip.X) + Math.Abs(selectedPlanet.Y - selectedShip.Y);
            int travelTime = distance / 2;
            if (travelTime < 1) travelTime = 1;

            var colonizeTask = new GameTask(
                description: $"Colonizing {selectedPlanet.Name}",
                turns: travelTime,
                onCompletion: () =>
                {
                    // Move ship to planet location
                    selectedShip.X = selectedPlanet.X;
                    selectedShip.Y = selectedPlanet.Y;

                    // Colonize the planet
                    selectedPlanet.IsColonized = true;
                    selectedPlanet.OwnerCivilizationId = Civilization.Id;
                    Civilization.Planets.Add(selectedPlanet);

                    // Remove the colonization ship
                    Civilization.Ships.Remove(selectedShip);

                    // Increase visibility due to colonization
                    Civilization.Visibility += 5.0;

                    OnPlanetColonized(new PlanetColonizedEventArgs { ColonizedPlanet = selectedPlanet });
                }
            );
            Civilization.OngoingTasks.Add(colonizeTask);
            return true;
        }

        /// <summary>
        /// Gets a list of the player's combat ships.
        /// </summary>
        public List<Ship> GetCombatShips()
        {
            return Civilization.Ships.FindAll(s => s.Type == ShipType.Combat);
        }

        /// <summary>
        /// Initiates an attack on a target location using a selected ship.
        /// </summary>
        public bool LaunchAttack(Galaxy galaxy, Ship ship, int targetX, int targetY)
        {
            int weaponSpeed = GetWeaponSpeed("StandardWeapon");
            int distance = Math.Abs(targetX - ship.X) + Math.Abs(targetY - ship.Y);
            int attackTime = distance / weaponSpeed;
            if (attackTime < 1) attackTime = 1;

            var attackTask = new GameTask(
                description: $"Attacking target at ({targetX}, {targetY})",
                turns: attackTime,
                onCompletion: () =>
                {
                    // Resolve attack logic
                    // For now, this is a placeholder
                }
            );
            Civilization.OngoingTasks.Add(attackTask);

            return true;
        }

        private int GetWeaponSpeed(string weaponType)
        {
            // Implement logic to return weapon speed based on weapon type
            return 5; // Default value
        }

        protected virtual void OnShipBuilt(ShipBuiltEventArgs e)
        {
            ShipBuilt?.Invoke(this, e);
        }

        protected virtual void OnPlanetColonized(PlanetColonizedEventArgs e)
        {
            PlanetColonized?.Invoke(this, e);
        }
    }
}
