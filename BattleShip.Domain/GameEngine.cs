using System.Collections.Generic;
using System.Linq;

namespace BattleShip.Domain
{
    public class GameEngine
    {
        protected static GameEngine Instance;
       

        public static GameEngine GetInstance => Instance ?? (Instance = new GameEngine());

        public List<User> WaitingList = new List<User>();

        public List<PlayRoom> PlayRooms { get; set; }

        public bool WasHit(User user, Position position)
        {
            return  user.Map.Ships.Any(s => s.InitialPosition == position);//TODO revisar por el resto de las coordinadas del barco
        }

        public bool HasGameEnded(User user)
        {
            return !user.Map.Ships.Any(s => s.Lifes > 0);
        }
    }
}