using System.Web.Mvc;
using BattleShip.Services;
using Microsoft.AspNet.Identity;

namespace BattleShip.MVC.Controllers
{
    public class HomeController : Controller
    {
        [Authorize]
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            return View(new UserService().Find(u => u.Id == userId).GameHistories);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        
        
        public ActionResult Board()
        {
            return View();
        }
    }
}