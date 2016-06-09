using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BattleShip.MVC.Controllers
{
    public class GameController : Controller
    {
      
        [Authorize]
        public ActionResult Game()
        {
            return View();
        }
    }
}