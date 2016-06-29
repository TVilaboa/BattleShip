using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BattleShip.Domain
{
    public class GameHistory : IEntity
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime Ended { get; set; } = DateTime.Now;
        [JsonConverter(typeof(StringEnumConverter))]
        public virtual GameStatus Status { get; set; }
        public string EnemyUserName { get; set; }
        public enum GameStatus
        {
            [EnumMember(Value = "Win")]
            Win,
            [EnumMember(Value = "Loss")]
            Loss,
            [EnumMember(Value = "Draw")]
            Draw
        }

        public int Hitted { get; set; }
        public int Missed { get; set; }
        public double Duration { get; set; }
    }
}