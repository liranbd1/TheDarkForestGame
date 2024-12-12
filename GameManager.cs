// File: GameManager.cs
using DarkForestGame.Entities;
using DarkForestGame.Events;
using DarkForestGame.Systems;
using DarkForestGame.UI;
using System;

namespace DarkForestGame
{
    /// <summary>
    /// Manages the game flow and interactions between game logic and UI.
    /// </summary>
    public class GameManager
    {
        private Galaxy galaxy;
        private GameUIManager uiManager;

        public GameManager()
        {
            // Initialize the galaxy.
            int galaxyWidth = 20;
            int galaxyHeight = 20;
            galaxy = new Galaxy(galaxyWidth, galaxyHeight);

            // Initialize planets and other game elements.
            InitializePlanets();
            InitializeCivilizations();

            // Initialize the UI manager.
            uiManager = new GameUIManager(galaxy);
        }

        /// <summary>
        /// Starts the game loop.
        /// </summary>
        public void StartGame()
        {
            bool gameRunning = true;
            while (gameRunning)
            {
                // Handle player turns
                foreach (var player in galaxy.Players)
                {
                    // Update the player's civilization status
                    galaxy.UpdatePlayerStatus(player);

                    // Update UI for the player
                    uiManager.UpdateForPlayer(player);

                    // Handle player actions
                    bool endTurn = false;
                    while (!endTurn)
                    {
                        endTurn = uiManager.DisplayPossibleActions(player);
                    }
                }

                // Check game status
                gameRunning = CheckGameStatus();

                // Wait for user input before proceeding to the next turn
                uiManager.WaitForNextTurn();
            }
        }

        private void InitializeCivilizations()
        {
            int numberOfPlayers = 2; // Adjust as needed
            Random rand = new Random();

            for (int i = 1; i <= numberOfPlayers; i++)
            {
                string playerName = $"Player {i} Civilization";

                CivilizationTraits playerTraits = new CivilizationTraits
                {
                    StealthBonus = 0.1,
                    ResourceGatheringRate = 1.0,
                    MilitaryStrengthModifier = 1.0
                };

                TechTree playerTechTree = new TechTree();
                InitializePlayerTechTree(playerTechTree);

                Player player = new Player(playerName, playerTechTree, playerTraits);

                Planet startingPlanet = CreateStartingPlanet(player.Civilization, rand);
                player.Civilization.Planets.Add(startingPlanet);

                galaxy.AddPlayer(player);
            }

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
                InitializeAITechTree(aiTechTree);

                Civilization aiCivilization = new Civilization(aiName, aiTechTree, aiTraits, ControlType.AI);

                Planet startingPlanet = CreateStartingPlanet(aiCivilization, rand);
                aiCivilization.Planets.Add(startingPlanet);

                galaxy.AddAICivilization(aiCivilization);
            }
        }

        private Planet CreateStartingPlanet(Civilization civ, Random rand)
        {
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

            Planet startingPlanet = new Planet(planetName, planetResources, x, y)
            {
                IsColonized = true,
                OwnerCivilizationId = civ.Id
            };

            galaxy.Planets.Add(startingPlanet);
            galaxy.Grid[x, y].Planet = startingPlanet;

            return startingPlanet;
        }

        private void InitializePlanets()
        {
            int numPlanets = 20; // Adjust as needed
            Random rand = new Random();

            for (int i = 1; i <= numPlanets; i++)
            {
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

        private void InitializePlayerTechTree(TechTree techTree)
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

        private void InitializeAITechTree(TechTree techTree)
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

        private bool CheckGameStatus()
        {
            // Implement win/loss condition checks
            return true;
        }
    }
}