using System.Collections.Generic;

namespace BattleShip.Domain
{
    public class Map
    {
        
        public List<List<Position>> Positions { get; set; }

        public List<Ship> Ships { get; set; }

        public List<Hit> Hits { get; set; }

        public Map()
        {
            //TODO generate map
        }

        
    }
}