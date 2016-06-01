using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BattleShip.MVC.Models
{
    public class Map
    {
        public List<List<Position>> Positions { get; set; }

        public List<Ship> Ships { get; set; }

        public List<Hit> Hits { get; set; }
    }
}