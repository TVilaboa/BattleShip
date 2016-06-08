using System;

namespace BattleShip.Domain
{
    public class Hit : IEntity
    {
        public virtual Position HitPosition { get; set; }

        public Boolean HasHit { get; set; }

        public int Id { get; set; }
        public string Code { get; set; }
        public bool IsDeleted { get; set; }
    }
}