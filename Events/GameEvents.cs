// File: Events/GameEvents.cs
using System;
using DarkForestGame.Entities;

namespace DarkForestGame.Events
{
    public class TurnEndedEventArgs : EventArgs
    {
        public int TurnNumber { get; set; }
    }

    public class TaskCompletedEventArgs : EventArgs
    {
        public GameTask CompletedTask { get; set; }
    }

    public class TaskCreatedEventArgs : EventArgs
    {
        public GameTask CompletedTask { get; set; }
    }

    public class ShipBuiltEventArgs : EventArgs
    {
        public Ship NewShip { get; set; }
    }

    public class PlanetColonizedEventArgs : EventArgs
    {
        public Planet ColonizedPlanet { get; set; }
    }

    // Event handler delegates
    public delegate void TurnEndedEventHandler(object sender, TurnEndedEventArgs e);
    public delegate void TaskCompletedEventHandler(object sender, TaskCompletedEventArgs e);
    public delegate void ShipBuiltEventHandler(object sender, ShipBuiltEventArgs e);
    public delegate void PlanetColonizedEventHandler(object sender, PlanetColonizedEventArgs e);
    public delegate void TaskCreatedEventHandler(object sender, TaskCreatedEventArgs e);
}