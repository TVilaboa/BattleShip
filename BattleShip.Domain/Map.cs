using System.Collections.Generic;

namespace BattleShip.Domain
{
    public class Map
    {
        
        public List<Position> Positions { get; set; } = new List<Position>();

        public List<Ship> Ships { get; set; } = new List<Ship>();

        public List<Hit> Hits { get; set; } = new List<Hit>();

        public Map()
        {
            //TODO generate map
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    Positions.Add(new Position() {XPosition = i, YPosition = j});
                }
            }
        }

        
    }
}