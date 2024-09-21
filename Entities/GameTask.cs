// File: Entities/GameTask.cs
using System;
using DarkForestGame.Events;

namespace DarkForestGame.Entities
{
    /// <summary>
    /// Represents an ongoing action that takes multiple turns to complete.
    /// </summary>
    public class GameTask
    {
        public string Description { get; set; }
        public int TurnsRemaining { get; set; }
        public Action ExecuteOnCompletion { get; set; }

        // Event for task completion
        public event TaskCompletedEventHandler TaskCompleted;

        public GameTask(string description, int turns, Action onCompletion)
        {
            Description = description;
            TurnsRemaining = turns;
            ExecuteOnCompletion = onCompletion;
        }

        public void DecrementTurn()
        {
            TurnsRemaining--;
            if (TurnsRemaining <= 0)
            {
                ExecuteOnCompletion?.Invoke();
                OnTaskCompleted(new TaskCompletedEventArgs { CompletedTask = this });
            }
        }

        protected virtual void OnTaskCompleted(TaskCompletedEventArgs e)
        {
            TaskCompleted?.Invoke(this, e);
        }
    }
}