using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BattleShip.Common;

namespace BattleShip.Domain
{
    public class ScrappedDocument : IEntity,IEquatable<ScrappedDocument>
    {
        public ScrappedDocument()
        {
            CreationDate = DateTime.Now;
            UpdateDate=DateTime.Now;
        }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
       
        public int Id { get; set; }
      [NotMapped]
        private string _code;
           [Key]
        public string Code {
              get
              {
                  if (string.IsNullOrWhiteSpace(_code))
                  {
                      _code = ScrapIdentifier.ToSha();
                  }
                  return _code;
              }

              set { _code = value; }
        }

        public bool IsDeleted { get; set; }
        public string ScrappedJson { get; set; }
        public virtual Schema UsedSchema { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string Uri { get; set; }
        public string ScrapIdentifier { get; set; }
        public string Name { get; set; }
        public bool Equals(ScrappedDocument other)
        {
            return Uri == other.Uri;
        }



      
    }
}