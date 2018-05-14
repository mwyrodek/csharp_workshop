using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ships.GameLoop;
using Ships.StubsForTestingViaUI;

namespace ShipsWeb.Controllers
{
    public class HomeController : Controller
    {
        private GameLoop game;
        private ShipReadForTest ship;
        public ActionResult Index(string command)
        {
            //ProperGame(command);
            OnlyShips(command);
            return View();
        }

        private void ProperGame(string command)
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

            ViewBag.Message = $"{game.Act(command)}";
            TempData["Game"] = game;
        }

        private void OnlyShips(string command)
        {
            if (TempData.ContainsKey("Ship"))
            {
                //If so access it here
                ship = TempData["Ship"] as ShipReadForTest;
            }
            else
            {
                ship = new ShipReadForTest();
            }

            ViewBag.Message = $"{ship.ActOnShip(command)}";
            TempData["Ship"] = ship;
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