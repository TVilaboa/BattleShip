using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using BattleShip.Domain;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.Identity;

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
            var user = playRoom.Player1.ConnectionId != Context.ConnectionId ? playRoom.Player1 : playRoom.Player2;
            var wasHit = Engine.WasHit(user,position);
            var hasGameEnded = Engine.HasGameEnded(user);
            Clients.Caller.receiveHitResponse(position, wasHit, hasGameEnded);
        }
        /// <summary>
        /// Invoked when a new client joins the system
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
            // 8: Add the game to the list of games that the Engine must simulate

            var user = new User()
            {
                ConnectionId = Context.ConnectionId,
                Principal = System.Web.HttpContext.Current.User,
                Map = new Map()
            };
            //_userRepository.AddUser(user);
            if (!Engine.WaitingList.Any())
            {
                Engine.WaitingList.Add(user);
                Clients.Caller.wait();
            }
            else
            {
                var opponent = Engine.WaitingList.First();
                Engine.WaitingList.Clear();
                var playRoom = new PlayRoom()
                {
                    Guid = Guid.NewGuid().ToString(),
                    Player1 = opponent,
                    Player2 = user
                };
                //_roomRepository.Add(playRoom);
                Task t1 = Groups.Add(opponent.ConnectionId, playRoom.Guid);
                Task t2 = Groups.Add(user.ConnectionId, playRoom.Guid);

                t1.Wait();
                t2.Wait();

                // Rough solution. We have to be sure the clients have received the group add messages over the wire
                // TODO: ask maybe on Jabbr or on StackOverflow and think about a better solution
                Thread.Sleep(3000);

                //Player player1 = Engine.CreatePlayer(playRoom.Player1, 1, true);
                //Player player2 = Engine.CreatePlayer(playRoom.Player2, 2, false);

                //Game game = Engine.CreateGame(playRoom.Id, player1, player2);

                //dynamic matchOptions = new ExpandoObject();
                //matchOptions.PlayRoomId = playRoom.Id;
                //matchOptions.Player1 = playRoom.Player1;
                //matchOptions.Player2 = playRoom.Player2;
                //matchOptions.BallDirection = game.Ball.Direction;

                //Clients[playRoom.Id].setupMatch(matchOptions);

                Thread.Sleep(3000);
                //Engine.AddGame(game);
                dynamic @group = Clients.Group(playRoom.Guid);
                @group.createGame();
                @group.addNewMessageToPage("Server", "Ready");
            }
        }
    }
}