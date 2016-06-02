using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BattleShip.MVC.Models
{
    public class Hit
    {
        public Position HitPosition { get; set; }

        public Boolean HasHit { get; set; } 
    }
}