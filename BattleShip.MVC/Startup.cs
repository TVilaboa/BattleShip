﻿using BattleShip.MVC;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Startup))]
namespace BattleShip.MVC
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // Any connection or hub wire up and configuration should go here
           
            new Auth().ConfigureAuth(app);
            app.MapSignalR();
        }
    }
}