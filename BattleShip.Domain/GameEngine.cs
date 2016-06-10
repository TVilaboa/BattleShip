using System.Collections.Generic;
using System.Linq;

namespace BattleShip.Domain
{
    public class GameEngine
    {
        protected static GameEngine Instance;
       

        public static GameEngine GetInstance => Instance ?? (Instance = new GameEngine());

        public List<User> WaitingList { get; set; } = new List<User>();

        public List<PlayRoom> PlayRooms { get; set; } = new List<PlayRoom>();

        public bool WasHit(User user, Position position)
        {
            return  user.Map.Ships.Any(s => s.Position == position);//TODO revisar por el resto de las coordinadas del barco
        }

        public bool HasGameEnded(User user)
        {
            return !user.Map.Ships.Any(s => s.Lifes > 0);
        }
    }
}