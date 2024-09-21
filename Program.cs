// File: Program.cs
using System;
using DarkForestGame.Entities;
using DarkForestGame.Events;
using DarkForestGame.Systems;
using DarkForestGame.UI;

namespace DarkForestGame
{
    class Program
    {
        static void Main(string[] args)
        {
            // Initialize the galaxy.
            int galaxyWidth = 20;
            int galaxyHeight = 20;
            Galaxy galaxy = new Galaxy(galaxyWidth, galaxyHeight);

            // Initialize planets and other game elements.
            InitializePlanets(galaxy);
            InitializeCivilizations(galaxy);

            // Create the GUI and start the game loop
            GameUI gameUI = new GameUI(galaxy);
            gameUI.StartGameLoop();
        }

        static void InitializeCivilizations(Galaxy galaxy)
        {
            int numberOfPlayers = 2; // Adjust as needed
            Random rand = new Random();

            for (int i = 1; i <= numberOfPlayers; i++)
            {
                // Get player civilization details.
                string playerName = $"Player {i} Civilization";

                // Define traits and tech tree for the player's civilization.
                CivilizationTraits playerTraits = new CivilizationTraits
                {
                    StealthBonus = 0.1,
                    ResourceGatheringRate = 1.0,
                    MilitaryStrengthModifier = 1.0
                };

                TechTree playerTechTree = new TechTree();
                // Initialize player's tech tree.
                InitializePlayerTechTree(playerTechTree);

                // Create the player.
                Player player = new Player(playerName, playerTechTree, playerTraits);

                // Assign a starting planet to the player's civilization
                Planet startingPlanet = CreateStartingPlanet(galaxy, player.Civilization, rand);
                player.Civilization.Planets.Add(startingPlanet);

                // Add the player to the galaxy.
                galaxy.AddPlayer(player);
            }

            // Set up AI civilizations.
            int numberOfAICivilizations = 2; // Adjust as needed.

            for (int i = 1; i <= numberOfAICivilizations; i++)
            {
                string aiName = $"AI Civilization {i}";

                CivilizationTraits aiTraits = new CivilizationTraits
                {
                    StealthBonus = 0.1,
                    ResourceGatheringRate = 1.0,
                    MilitaryStrengthModifier = 1.0
                };

                TechTree aiTechTree = new TechTree();
                // Initialize AI's tech tree.
                InitializeAITechTree(aiTechTree);

                // Create the AI civilization.
                Civilization aiCivilization = new Civilization(aiName, aiTechTree, aiTraits, ControlType.AI);

                // Assign a starting planet to the AI civilization
                Planet startingPlanet = CreateStartingPlanet(galaxy, aiCivilization, rand);
                aiCivilization.Planets.Add(startingPlanet);

                // Add the AI civilization to the galaxy.
                galaxy.AddAICivilization(aiCivilization);
            }
        }

        static Planet CreateStartingPlanet(Galaxy galaxy, Civilization civ, Random rand)
        {
            // Find an unoccupied position
            int x, y;
            do
            {
                x = rand.Next(0, galaxy.Width);
                y = rand.Next(0, galaxy.Height);
            } while (galaxy.Grid[x, y].Planet != null);

            string planetName = $"{civ.Name} Homeworld";
            Resource planetResources = new Resource(
                minerals: rand.Next(100, 200),
                energy: rand.Next(80, 150)
            );

            Planet startingPlanet = new Planet(planetName, planetResources, x, y);
            startingPlanet.IsColonized = true;
            startingPlanet.OwnerCivilizationId = civ.Id;

            // Add the planet to the galaxy
            galaxy.Planets.Add(startingPlanet);
            galaxy.Grid[x, y].Planet = startingPlanet;

            return startingPlanet;
        }

        static void InitializePlayerTechTree(TechTree techTree)
        {
            // Define and add technologies to the player's tech tree.
            Technology basicSpaceflight = new Technology(
                "Basic Spaceflight",
                "Allows for basic exploration and colonization.",
                researchCost: 100,
                applyEffects: (civ) => { civ.TechLevel += 1; }
            );

            techTree.AddTechnology(basicSpaceflight);

            // Add more technologies as needed.
        }

        static void InitializeAITechTree(TechTree techTree)
        {
            // Define and add technologies to the AI's tech tree.
            Technology basicSpaceflight = new Technology(
                "Basic Spaceflight",
                "Allows for basic exploration and colonization.",
                researchCost: 100,
                applyEffects: (civ) => { civ.TechLevel += 1; }
            );

            techTree.AddTechnology(basicSpaceflight);

            // Add more technologies as needed.
        }

        static void InitializePlanets(Galaxy galaxy)
        {
            int numPlanets = 20; // Adjust as needed
            Random rand = new Random();

            for (int i = 1; i <= numPlanets; i++)
            {
                // Find an unoccupied position
                int x, y;
                do
                {
                    x = rand.Next(0, galaxy.Width);
                    y = rand.Next(0, galaxy.Height);
                } while (galaxy.Grid[x, y].Planet != null);

                string planetName = $"Planet {i}";
                Resource planetResources = new Resource(
                    minerals: rand.Next(50, 150),
                    energy: rand.Next(30, 100)
                );

                Planet planet = new Planet(planetName, planetResources, x, y);
                galaxy.Planets.Add(planet);
                galaxy.Grid[x, y].Planet = planet;
            }
        }
    }
}