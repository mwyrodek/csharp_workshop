using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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

            ViewBag.Message = Server.HtmlEncode(game.Act(command)).Replace("\r\n", "<br />").Replace("&lt;b&gt;", "<b>").Replace("&lt;/b&gt;", "</b>");
            TempData["Game"] = game;
            return View();
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
    }
}