﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Providers.Entities;
using BattleShip.Data.DataBase;
using BattleShip.Domain;
using BattleShip.Services;
using Microsoft.AspNet.SignalR;
using User = BattleShip.Domain.User;

namespace BattleShip.MVC.Hubs
{
    public class GameHub : Hub
    {
        public GameEngine Engine = GameEngine.GetInstance;
      


        public void Send(string name, string message)
        {
            var playRoom =
               Engine.PlayRooms.FirstOrDefault(
                   p =>
                       p.Player1.ConnectionId == Context.ConnectionId || p.Player2.ConnectionId == Context.ConnectionId);
            // Call the addNewMessageToPage method to update clients.
            if (playRoom?.Player1 != null && playRoom.Player2 != null)
            {
                Clients.Client(playRoom.Player1.ConnectionId).addNewMessageToPage(name, message);
                Clients.Client(playRoom.Player2.ConnectionId).addNewMessageToPage(name, message);
            } 
          
            //Clients.All.addNewMessageToPage(name, message);
        }

        public void SetMap(List<Ship> ships)
        {
            var playRoom =
              Engine.PlayRooms.FirstOrDefault(
                  p =>
                      p.Player1.ConnectionId == Context.ConnectionId || p.Player2.ConnectionId == Context.ConnectionId);
           var enemy = playRoom.Player1.ConnectionId != Context.ConnectionId ? playRoom.Player1 : playRoom.Player2;
            var user = playRoom.Player1.ConnectionId == Context.ConnectionId ? playRoom.Player1 : playRoom.Player2;

            if (user.Stage == User.GameStage.SettingShips)
            {
                user.Map = new Map() { Ships = ships };
                user.Stage = User.GameStage.ShipsReady;
                if (enemy.Stage == User.GameStage.SettingShips)
                {
                    Clients.Caller.wait("Waiting for opponent to set ships...");
                }
                else if (enemy.Stage == User.GameStage.ShipsReady)
                {
                    Clients.Client(playRoom.Player1.ConnectionId).startGame();
                    Clients.Client(playRoom.Player2.ConnectionId).startGame();
                    if (new Random().Next(1, 10) > 5)
                    {
                        user.Stage = User.GameStage.Firing;
                        enemy.Stage = User.GameStage.WaitingForOponentPlay;
                        Clients.Caller.beginTurn();
                        Clients.Client(enemy.ConnectionId).wait("Waiting for opponent to fire...");
                    }
                    else
                    {
                        enemy.Stage = User.GameStage.Firing;
                        user.Stage = User.GameStage.WaitingForOponentPlay;
                        Clients.Caller.wait("Waiting for opponent to fire...");
                        Clients.Client(enemy.ConnectionId).beginTurn();
                    }

                }
            }
           
        }

        public void Hit(Position position)
        {
            var playRoom =
                Engine.PlayRooms.FirstOrDefault(
                    p =>
                        p.Player1.ConnectionId == Context.ConnectionId || p.Player2.ConnectionId == Context.ConnectionId);
            var hittedPlayer = playRoom.Player1.ConnectionId != Context.ConnectionId ? playRoom.Player1 : playRoom.Player2;
            var hitterPlayer = playRoom.Player1.ConnectionId == Context.ConnectionId ? playRoom.Player1 : playRoom.Player2;
            if (hitterPlayer.Stage == User.GameStage.Firing && Engine.CanHitPosition(hittedPlayer,position))
            {
                bool isShipSunken;
                var wasHit = Engine.WasHit(hittedPlayer, position,out isShipSunken);
                hittedPlayer.Map.Hits.Add(new Hit { HasHit = wasHit, HitPosition = position });

                var hasGameEnded = Engine.HasGameEnded(hittedPlayer);
                if (hasGameEnded)
                {
                    var dbHittedPlayer = new UserService().Get(hittedPlayer.Id);
                    var dbHitterPlayer = new UserService().Get(hitterPlayer.Id);
                    dbHitterPlayer.GameHistories.Add(new GameHistory
                    {
                        
                        EnemyUserName = hittedPlayer.UserName,
                        Code = playRoom.Guid,
                        Hitted = hittedPlayer.Map.Hits.Count(h => h.HasHit),
                        Missed = hittedPlayer.Map.Hits.Count(h => !h.HasHit),
                        Status = GameHistory.GameStatus.Win,
                        Duration = (DateTime.Now - playRoom.CreatedDate).TotalMinutes
                    });
                    dbHittedPlayer.GameHistories.Add(new GameHistory
                    {

                        EnemyUserName = hitterPlayer.UserName,
                        Code = playRoom.Guid,
                        Hitted = hitterPlayer.Map.Hits.Count(h => h.HasHit),
                        Missed = hitterPlayer.Map.Hits.Count(h => !h.HasHit),
                        Status = GameHistory.GameStatus.Loss,
                        Duration = (DateTime.Now - playRoom.CreatedDate).TotalMinutes
                    });
                    dbHitterPlayer.Stage = User.GameStage.NotPlaying;
                    dbHittedPlayer.Stage = User.GameStage.NotPlaying;
                    new UserService().Update(dbHitterPlayer);
                    new UserService().Update(dbHittedPlayer);
                    Engine.PlayRooms.RemoveAll(
                        p =>
                            (p.Player1.UserName == dbHitterPlayer.UserName ||
                             p.Player2.UserName == dbHitterPlayer.UserName) &&
                            (p.Player1.UserName == dbHittedPlayer.UserName ||
                             p.Player2.UserName == dbHittedPlayer.UserName));
                }

                Clients.Caller.receiveHitResponse(position,true, wasHit, hasGameEnded,isShipSunken);
                Clients.Client(hittedPlayer.ConnectionId).receiveHitResponse(position, false, wasHit, hasGameEnded, isShipSunken);
            }
        }

        public void EndTurn()
        {
            Clients.Caller.wait("Waiting for opponent to fire...");
            var playRoom =
               Engine.PlayRooms.FirstOrDefault(
                   p =>
                       p.Player1.ConnectionId == Context.ConnectionId || p.Player2.ConnectionId == Context.ConnectionId);
            var enemy = playRoom.Player1.ConnectionId != Context.ConnectionId ? playRoom.Player1 : playRoom.Player2;
            var user = playRoom.Player1.ConnectionId == Context.ConnectionId ? playRoom.Player1 : playRoom.Player2;
            enemy.Stage = User.GameStage.Firing;
            user.Stage = User.GameStage.WaitingForOponentPlay;
            Clients.Client(enemy.ConnectionId).beginTurn();

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
            var existentPlayRoom = Engine.PlayRooms.Find(p => p.Player1.UserName == name || p.Player2.UserName == name);
            if (existentPlayRoom != null)
            {
                Clients.Caller.wait("Reconnecting...");
                var player = existentPlayRoom.Player1.UserName == name
                    ? existentPlayRoom.Player1 : existentPlayRoom.Player2;
                var enemy = existentPlayRoom.Player1.UserName != name
                    ? existentPlayRoom.Player1 : existentPlayRoom.Player2;
                player.ConnectionId = Context.ConnectionId;
                RebuildGame(player,enemy);
            }
            else
            {
                user.ConnectionId = Context.ConnectionId;
                user.Stage = User.GameStage.WaitingForOponent;
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
                    user.Stage = User.GameStage.SettingShips;
                    opponent.Stage = User.GameStage.SettingShips;
                    var playRoom = new PlayRoom
                    {
                        Guid = Guid.NewGuid().ToString(),
                        Player1 = opponent,
                        Player2 = user
                    };
                    Engine.PlayRooms.Add(playRoom);
                    Clients.Client(playRoom.Player1.ConnectionId).createGame();
                    Clients.Client(playRoom.Player2.ConnectionId).createGame();
                    Clients.Client(playRoom.Player1.ConnectionId).addNewMessageToPage("Server", "Ready");
                    Clients.Client(playRoom.Player2.ConnectionId).addNewMessageToPage("Server", "Ready");
                }
            }
          
        }

        private void RebuildGame(User player,User enemy)
        {
            if (player.Stage == User.GameStage.SettingShips)
            {
                Clients.Caller.createGame();
            }
            else
            {
                Clients.Caller.createGame();
                Clients.Caller.setShips(player.Map.Ships);
                Clients.Caller.startGame();
               
                foreach (var hit in player.Map.Hits)
                {
                    var imageName = hit.HasHit ? "1466050855_Explosion.png" : "1466050417_ksplash.png";
                    var image = UrlHelper.GenerateContentUrl($"~/Content/Images/{imageName}",Context.Request.GetHttpContext());
                    Clients.Caller.renderHit(hit.HitPosition, false, image);
                }
                foreach (var hit in enemy.Map.Hits)
                {
                    var imageName = hit.HasHit ? "1466050855_Explosion.png" : "1466050417_ksplash.png";
                    var image = UrlHelper.GenerateContentUrl($"~/Content/Images/{imageName}", Context.Request.GetHttpContext());
                    Clients.Caller.renderHit(hit.HitPosition, true, image);
                }
                if (player.Stage == User.GameStage.WaitingForOponentPlay)
                {
                    Clients.Caller.wait("Waiting for opponent to fire...");
                }else if (player.Stage == User.GameStage.Firing)
                {
                    Clients.Caller.beginTurn();
                }
            }
            

        }

        public override Task OnDisconnected(bool stopCalled)
        {
            if (BattleShipDataContext.GetInstance != null)
            {
                ((BattleShipDataContext)System.Web.HttpContext.Current.Items["BattleShipDataContext"]).Dispose();
                System.Web.HttpContext.Current.Items["BattleShipDataContext"] = null;
            }
            return base.OnDisconnected(stopCalled);
        }

        public override Task OnConnected()
        {
            if (BattleShipDataContext.GetInstance == null)
                System.Web.HttpContext.Current.Items["BattleShipDataContext"] = new BattleShipDataContext();
            return base.OnConnected();
        }
    }
}