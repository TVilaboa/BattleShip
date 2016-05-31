using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BattleShip.Domain
{
    public class Path : IEntity,IEquatable<Path>
    {
        public Path()
        {
            InnerPaths = new List<Path>();
            ScrappedDocuments = new List<ScrappedDocument>();
        }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
    [Key]
        public string Code { get; set; }

        public bool IsDeleted { get; set; }

        public string Name { get; set; }
        public virtual List<ScrappedDocument> ScrappedDocuments { get; set; }
        public virtual List<Path> InnerPaths { get; set; }
        public bool Equals(Path other)
        {
            return Code == other.Code;
        }
    }
}