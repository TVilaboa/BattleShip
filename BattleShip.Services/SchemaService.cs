using BattleShip.Data;
using BattleShip.Domain;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;

namespace BattleShip.Services
{
    public class SchemaService
    {
        public SchemaService()
        {
            UnitOfWork = new UnitOfWork();
        }

        UnitOfWork UnitOfWork { get; set; }
        public JSchema CreateJSchema(string schema)
        {
            return JSchema.Parse(schema.Replace(@"""type"": ""date""", @"""type"": ""string"""));
        }

        public bool ValidateJSchema(JSchema schema, JObject json)
        {
            return json.IsValid(schema);
        }

        public Schema AddOrUpdate(Schema schema)
        {
            return UnitOfWork.Schemas.AddOrUpdate(schema,false,true);
        }

        public Schema GetByCode(string code)
        {
            return UnitOfWork.Schemas.Find(s => s.Code == code);
        }

        public Schema GetByBaseUrl(string baseUrl)
        {
            return UnitOfWork.Schemas.Find(s => s.BaseUrl == baseUrl);
        }

    }
}
