using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BattleShip.MVC.Models
{
    public class Ship
    {
        public string Name { get; set; }
        public int Lifes { get; set; }

        public Position initialPosition { get; set; }

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