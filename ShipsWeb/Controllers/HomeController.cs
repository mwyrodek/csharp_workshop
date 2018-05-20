using System.Web.Mvc;
using Ships.GameLoop;

namespace ShipsWeb.Controllers
{
    public class HomeController : Controller
    {
        private GameLoop game;

        public ActionResult Index(string command)
        {
            if (TempData.ContainsKey("Game"))
            {
                //If so access it here
                game = TempData["Game"] as GameLoop;
            }
            else
            {
                game = new GameLoop();
            }

            ViewBag.Message = game.Act(command).Replace("\r\n", "<br />");
            TempData["Game"] = game;
                ModelState.Clear();

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
    }
}