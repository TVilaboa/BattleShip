using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json.Schema;

namespace BattleShip.Domain
{
    public class Schema : IEntity

    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Key]
        public string Code { get; set; }

        public bool IsDeleted { get; set; }


        public string JSchemaStr { get; set; }
        public string BaseUrl { get; set; }
        public virtual Domain Domain { get; set; }
        public virtual Path Path { get; set; }

        public JSchema ToJSchema()
        {
            return JSchema.Parse(JSchemaStr.Replace(@"""type"": ""date""", @"""type"": ""string"""));
        }

        public string SecondPropertyForTitle { get; set; }

        public Schema()
        {
            this.SecondPropertyForTitle = "";
        }
    }
}