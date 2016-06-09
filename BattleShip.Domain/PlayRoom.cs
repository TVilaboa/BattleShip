using System;
using System.Security.Principal;

namespace BattleShip.Domain
{
    public class PlayRoom
    {
        public User Player1 { get; set; }
        public User Player2 { get; set; }
        public string Guid { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}