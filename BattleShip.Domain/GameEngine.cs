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
            var comprarer = Position.YPositionXPositionComparer;
            var hittenShip = user.Map.Ships.Find(s => s.AllPositions.Any(p => comprarer.Equals(p, position)));
            if (hittenShip != null)
            {
                hittenShip.Hit();
                return true;
                
            }
            else
            {
                return false;
            }
           /* return  hittenShip;*///TODO revisar por el resto de las coordinadas del barco
        }

        public bool HasGameEnded(User user)
        {
            return !user.Map.Ships.Any(s => s.Lifes > 0);
        }

        public bool CanHitPosition(User hittedPlayer, Position position)
        {
            var comprarer = Position.YPositionXPositionComparer;
            var positionHasBeenHit = hittedPlayer.Map.Hits.Any(h => comprarer.Equals(h.HitPosition, position));
            return !positionHasBeenHit;
        }
    }
}