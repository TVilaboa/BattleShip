using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BattleShip.Domain
{
    public class User : IdentityUser
    {
        public IPrincipal Principal { get; set; }
        public string ConnectionId { get; set; }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
        [NotMapped]
        public Map Map { get; set; }
       
        public virtual List<GameHistory> GameHistories { get; set; } = new List<GameHistory>();
        public virtual GameStage Stage { get; set; }

        public enum GameStage
        {
            NotPlaying,
            WaitingForOponent,
            SettingShips,
            ShipsReady,
            Firing,
            WaitingForOponentPlay
        }
    }
}