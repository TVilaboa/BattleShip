using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BattleShip.Domain
{
    public class Domain:IEntity,IEquatable<Domain>
    {
        public Domain()
        {
            Paths = new List<Path>();
        }
       [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Key]
        public string Code { get; set; }

        public bool IsDeleted { get; set; }

        public string Name { get; set; }
        public virtual List<Path> Paths { get; set; }
        public bool Equals(Domain other)
        {
            return Code == other.Code;
        }
    }
}