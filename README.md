#The Dark Forest Game Rules

Table of Contents

Introduction
Game Overview
Game Setup
3.1 Galaxy Initialization
3.2 Civilizations
3.3 Players and AI
Game Mechanics
4.1 Turn-Based System
4.2 Resources
4.3 Planets
4.4 Ships
4.5 Technologies and Research
4.6 Diplomacy and Visibility
Gameplay Actions
5.1 Research Technology
5.2 Build Ship
5.3 Colonize Planet
5.4 Launch Attack
User Interface
6.1 Map Display
6.2 Player Stats Panel
6.3 Action Menus
Win/Loss Conditions
Advanced Features
8.1 Visibility Mechanics
8.2 Task Management
Game Etiquette
Conclusion
1. Introduction

"The Dark Forest" is a turn-based strategy game set in a procedurally generated galaxy. Players control civilizations aiming to expand their influence, colonize planets, research technologies, build fleets, and interact with other civilizations through diplomacy or warfare. The game emphasizes strategic planning, resource management, and exploration.

2. Game Overview

Objective: Expand your civilization's influence across the galaxy by colonizing planets, advancing technologically, and engaging with other civilizations. The ultimate goal is to achieve dominance or fulfill specific victory conditions.
Players: The game supports multiple human players and AI-controlled civilizations.
Galaxy: A grid-based map representing space, populated with planets, ships, and other civilizations.
3. Game Setup

3.1 Galaxy Initialization
Galaxy Size: The galaxy is a two-dimensional grid with configurable width and height (e.g., 20x20 cells).
Planets: Randomly placed throughout the galaxy. Each planet has coordinates (X, Y), resources, and can be colonized.
Grid Cells: Each cell (GalaxyCell) can contain a planet, ships, or be empty.
3.2 Civilizations
Civilizations: Represent the players in the game, each with unique traits and technologies.
Traits: Include attributes like stealth bonus, resource gathering rate, and military strength modifier.
Resources: Civilizations manage resources such as minerals, energy, and intelligence.
3.3 Players and AI
Human Players: Control their civilizations through a Command-Line Interface (CLI).
AI Civilizations: Controlled by the game engine, acting according to predefined behaviors.
4. Game Mechanics

4.1 Turn-Based System
The game progresses in turns.
During each turn, players can perform actions like researching technologies, building ships, colonizing planets, or launching attacks.
After all players have completed their actions, the game simulates the turn, processes tasks, and updates the game state.
4.2 Resources
Minerals: Used to build ships and structures.
Energy: Can be used for various actions and maintenance.
Intelligence: Required to research technologies.
Resources are gathered from colonized planets and can be influenced by civilization traits.

4.3 Planets
Colonization: Uncolonized planets can be colonized to expand a civilization's influence.
Resources: Each planet provides resources to its owner every turn.
Ownership: Planets can be owned by civilizations, providing strategic advantages.
4.4 Ships
Types of Ships:
Exploration Ships: Used to explore the galaxy.
Combat Ships: Engage in battles with enemy ships or attack planets.
Espionage Ships: Gather intelligence on other civilizations.
Colonization Ships: Used to colonize uninhabited planets.
Building Ships: Requires minerals and time. Ships are built at the civilization's planets.
Movement: Ships can move across the galaxy grid to perform various actions.
4.5 Technologies and Research
Tech Tree: Each civilization has a tech tree with technologies that provide bonuses or unlock new capabilities.
Researching Technologies: Requires intelligence resources and time.
Unique Technologies: Some technologies may be unique to certain civilizations.
4.6 Diplomacy and Visibility
Diplomacy: Civilizations can interact through alliances, wars, or neutrality.
Visibility: Determines what a civilization can see on the map, influenced by technologies and traits.
Fog of War: Unexplored or out-of-range areas of the galaxy are hidden from the player.
5. Gameplay Actions

5.1 Research Technology
Available Technologies: Players can view and select from available technologies to research.
Cost: Researching a technology consumes intelligence resources.
Time: Research takes several turns to complete.
Benefits: Unlocks new abilities, improves traits, or provides bonuses.
5.2 Build Ship
Selecting Ship Type: Players choose the type of ship to build from the available options.
Cost: Building ships consumes minerals.
Time: Construction takes several turns.
Deployment: Ships are deployed at one of the player's planets upon completion.
5.3 Colonize Planet
Requirements:
A colonization ship.
An uncolonized planet within reach.
Process:
Select an uncolonized planet.
Choose a colonization ship to send.
The ship travels to the planet over several turns.
Outcome: Upon arrival, the planet becomes part of the player's civilization.
5.4 Launch Attack
Requirements:
A combat ship.
A target location (coordinates).
Process:
Select a combat ship.
Enter the target coordinates.
The ship moves towards the target over several turns.
Outcome: Upon arrival, combat is resolved with any enemy ships or planetary defenses.
6. User Interface

6.1 Map Display
Visualization: The galaxy is displayed as a grid of cells.
Symbols:
[green]P[/]: Player's own planet.
[yellow]O[/]: Uncolonized planet.
[red]X[/]: Planet owned by another civilization.
[cyan]S[/]: Player's own ship.
[darkred]E[/]: Enemy ship.
[grey].[/]: Empty space or unseen area.
Visibility: Players see only the parts of the map within their visibility range.
6.2 Player Stats Panel
Resources: Displays current amounts of minerals, energy, and intelligence.
Ongoing Tasks: Lists tasks in progress with descriptions and remaining turns.
Technologies: Shows available technologies for research.
6.3 Action Menus
Possible Actions: Players can choose from actions like researching technology, building ships, colonizing planets, launching attacks, or ending their turn.
Interactive Prompts: Menus are navigated using selection prompts.
7. Win/Loss Conditions

Victory Conditions: Can be configured but may include objectives like:
Dominating a certain percentage of the galaxy.
Achieving technological supremacy.
Eliminating rival civilizations.
Defeat Conditions: A player may lose if all their planets are conquered or they have no remaining ships and resources.
8. Advanced Features

8.1 Visibility Mechanics
Line of Sight: Players can see cells adjacent to their planets and ships.
Fog of War: Areas outside visibility are hidden or displayed as unknown.
Technologies and Traits: Certain technologies or traits can enhance visibility range.
8.2 Task Management
Game Tasks: Actions that take multiple turns to complete are managed as tasks.
Task Types: Include ship construction, technology research, colonization missions, and attacks.
Progression: Each turn, tasks' remaining turns decrease until completion.
9. Game Etiquette

Fair Play: Players are encouraged to engage in fair strategies and respect other players.
Communication: Diplomacy and communication should be conducted respectfully.
Turns: Players should take their turns promptly to maintain game flow.
10. Conclusion

"The Dark Forest" offers a deep and engaging strategy experience, challenging players to manage resources, make strategic decisions, and interact with other civilizations. By understanding the game rules and mechanics, players can develop effective strategies to lead their civilization to victory.

Note: This guide provides an overview of the game rules and mechanics. Specific implementations or updates to the game may introduce new features or alter existing ones. Players are encouraged to explore the game and adapt their strategies accordingly.
