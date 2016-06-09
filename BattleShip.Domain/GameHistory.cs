using System;

namespace BattleShip.Domain
{
    public class GameHistory : IEntity
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime Ended { get; set; } = DateTime.Now;
        public virtual User Enemy { get; set; }
        public virtual GameStatus Status { get; set; }

        public enum GameStatus
        {
            Win,
            Loss,
            Draw
        }

        public int Hitted { get; set; }
        public int Missed { get; set; }
        public double Duration { get; set; }
    }
}