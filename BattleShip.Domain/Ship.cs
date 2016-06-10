using System;

namespace BattleShip.Domain
{
    public class Ship : IEntity
    {
        public string Name { get; set; }
        public int Lifes { get; set; }

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
    }
}