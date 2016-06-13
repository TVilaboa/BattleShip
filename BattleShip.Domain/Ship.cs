using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BattleShip.Domain
{
    public class Ship : IEntity
    {
        public string Name { get; set; }
        public int Lifes { get; set; }
        public int Size { get; set; }

        public Position Position { get; set; }

        public Boolean IsOnXAxis { get; set; }

        public void Hit() {
            if (this.Lifes > 0)
                this.Lifes--;
        }

        public void ChangeAxis() {
            this.IsOnXAxis = !this.IsOnXAxis;
        }


        public int Id { get; set; }
        public string Code { get; set; }
        public bool IsDeleted { get; set; }
        [NotMapped]
        private List<Position> _allPositions;
        [NotMapped]
        public List<Position> AllPositions
        {
            get
            {
                if (_allPositions != null)
                {
                    return _allPositions;
                }
                else
                {
                    var allPositions = new List<Position>() { Position };
                    for (int i = 1; i < Size; i++)
                    {
                        if (IsOnXAxis)
                        {
                            allPositions.Add(new Position()
                            {
                                XPosition = Position.XPosition + i,
                                YPosition = Position.YPosition
                            });
                        }
                        else
                        {
                            allPositions.Add(new Position() { XPosition = Position.XPosition, YPosition = Position.YPosition + i });
                        }
                    }
                    _allPositions = allPositions;
                    return allPositions;
                }
            }
            
        }
    }
}