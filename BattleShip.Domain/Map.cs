using System;
using System.Collections.Generic;

namespace BattleShip.Domain
{
    public class Map : IEntity
    {
        
        public virtual List<Position> Positions { get; set; } = new List<Position>();

        public virtual List<Ship> Ships { get; set; } = new List<Ship>();

        public virtual List<Hit> Hits { get; set; } = new List<Hit>();

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

        public DateTime Created { get; set; } = DateTime.Now;



        public int Id { get; set; }
        public string Code { get; set; }
        public bool IsDeleted { get; set; }
    }
}