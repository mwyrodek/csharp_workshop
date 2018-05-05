﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using Ships.GameLoop;
using ShipUI.Models;

namespace ShipUI.Controllers
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
            ViewBag.Message = $"{game.Act(command)}";
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