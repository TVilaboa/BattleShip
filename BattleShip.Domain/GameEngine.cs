using System.Collections.Generic;

namespace BattleShip.Domain
{
    public class GameEngine
    {
        protected static GameEngine Instance;
       

        public static GameEngine GetInstance => Instance ?? (Instance = new GameEngine());

        public List<User> WaitingList = new List<User>();
    }
}