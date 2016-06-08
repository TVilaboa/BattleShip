using System;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Providers.Entities;
using BattleShip.Domain;
using BattleShip.Services;
using Microsoft.AspNet.SignalR;

namespace BattleShip.MVC.Hubs
{
    public class GameHub : Hub
    {
        public GameEngine Engine = GameEngine.GetInstance;
      


        public void Send(string name, string message)
        {
            // Call the addNewMessageToPage method to update clients.
            Clients.All.addNewMessageToPage(name, message);
        }

        public void Hit(Position position)
        {
            var playRoom =
                Engine.PlayRooms.FirstOrDefault(
                    p =>
                        p.Player1.ConnectionId == Context.ConnectionId || p.Player2.ConnectionId == Context.ConnectionId);
            var hittedPlayerId = playRoom.Player1.ConnectionId != Context.ConnectionId ? playRoom.Player1.Id : playRoom.Player2.Id;
            var hitterPlayerId = playRoom.Player1.ConnectionId == Context.ConnectionId ? playRoom.Player1.Id : playRoom.Player2.Id;
            var hittedPlayer = new UserService().Get(hittedPlayerId);
            var hitterPlayer = new UserService().Get(hitterPlayerId);
            var wasHit = Engine.WasHit(hittedPlayer, position);
            hittedPlayer.Map.Hits.Add(new Hit {HasHit = wasHit, HitPosition = position});

            var hasGameEnded = Engine.HasGameEnded(hittedPlayer);
            if (hasGameEnded)
            {
                hitterPlayer.GameHistories.Add(new GameHistory
                                             {
                                                 Enemy = hittedPlayer,
                                                 Code = playRoom.Guid,
                                                 Hitted = hittedPlayer.Map.Hits.Count(h => h.HasHit),
                                                 Missed = hittedPlayer.Map.Hits.Count(h => !h.HasHit),
                                                 Status = GameHistory.GameStatus.Win
                                             });
                hittedPlayer.GameHistories.Add(new GameHistory
                {
                    Enemy = hitterPlayer,
                    Code = playRoom.Guid,
                    Hitted = hitterPlayer.Map.Hits.Count(h => h.HasHit),
                    Missed = hitterPlayer.Map.Hits.Count(h => !h.HasHit),
                    Status = GameHistory.GameStatus.Loss
                });
            }
            new UserService().Update(hitterPlayer);
            new UserService().Update(hittedPlayer);
            Clients.Caller.receiveHitResponse(position, wasHit, hasGameEnded);
        }

        public void EndTurn()
        {
            Clients.Caller.wait();
            var playRoom =
               Engine.PlayRooms.FirstOrDefault(
                   p =>
                       p.Player1.ConnectionId == Context.ConnectionId || p.Player2.ConnectionId == Context.ConnectionId);
            var nextPlayer = playRoom.Player1.ConnectionId != Context.ConnectionId ? playRoom.Player1 : playRoom.Player2;
            Clients.Client(nextPlayer.ConnectionId).beginTurn();

        }

        /// <summary>
        ///     Invoked when a new client joins the system
        /// </summary>
        public void Joined()
        {
            // 1: Add user to list of connected users
            // 2: If waiting list is empty add user to waiting list            
            // 3: Else find an opponent (first in the waiting list) and remove him from the waiting list
            // 4: Create room and assign both users
            // 5: Create a group for this room
            // 6: Setup match (playRoom Id, initial ball direction, player on the left and right etc...)
            // 7: Notify the group the match can start
            var name = Context.User.Identity.Name;
            var user = new UserService().Find(u => u.UserName == name);
            user.ConnectionId = Context.ConnectionId;
            user.Map = new Map();
            new UserService().AddOrUpdate(user);
            if (!Engine.WaitingList.Any())
            {
                Engine.WaitingList.Add(user);
                Clients.Caller.wait();
            }
            else
            {
                var opponent = Engine.WaitingList.First();
                Engine.WaitingList.Clear();
                var playRoom = new PlayRoom
                               {
                                   Guid = Guid.NewGuid().ToString(),
                                   Player1 = opponent,
                                   Player2 = user
                               };
                Engine.PlayRooms.Add(playRoom);
                //_roomRepository.Add(playRoom);
                //var t1 = Groups.Add(opponent.ConnectionId, playRoom.Guid);
                //var t2 = Groups.Add(user.ConnectionId, playRoom.Guid);

                //t1.Wait();
                //t2.Wait();

                //// Rough solution. We have to be sure the clients have received the group add messages over the wire
                //// TODO: ask maybe on Jabbr or on StackOverflow and think about a better solution
                //Thread.Sleep(3000);

                //Player player1 = Engine.CreatePlayer(playRoom.Player1, 1, true);
                //Player player2 = Engine.CreatePlayer(playRoom.Player2, 2, false);

                //Game game = Engine.CreateGame(playRoom.Id, player1, player2);

                //dynamic matchOptions = new ExpandoObject();
                //matchOptions.PlayRoomId = playRoom.Id;
                //matchOptions.Player1 = playRoom.Player1;
                //matchOptions.Player2 = playRoom.Player2;
                //matchOptions.BallDirection = game.Ball.Direction;

                //Clients[playRoom.Id].setupMatch(matchOptions);

                //Thread.Sleep(3000);
                ////Engine.AddGame(game);
                //dynamic @group = Clients.Group(playRoom.Guid);
                //@group.createGame();
                //@group.addNewMessageToPage("Server", "Ready");
                Clients.Client(playRoom.Player1.ConnectionId).createGame();
                Clients.Client(playRoom.Player2.ConnectionId).createGame();
                Clients.Client(playRoom.Player1.ConnectionId).addNewMessageToPage("Server", "Ready");
                Clients.Client(playRoom.Player2.ConnectionId).addNewMessageToPage("Server", "Ready");
            }
        }
    }
}