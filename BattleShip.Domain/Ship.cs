using System;

namespace BattleShip.Domain
{
    public class Ship
    {
        public string Name { get; set; }
        public int Lifes { get; set; }

        public Position InitialPosition { get; set; }

        public Boolean IsOnXAxis { get; set; }

        public void Hit() {
            if (this.Lifes > 0)
                this.Lifes--;
        }

        public void ChangeAxis() {
            this.IsOnXAxis = !this.IsOnXAxis;
        }


       
    }
}