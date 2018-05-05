using Ships.GameLoop;

namespace ShipUI.Models
{
    public class HomeModel
    {
        public GameLoop GameLoop;

        public HomeModel()
        {
            GameLoop = new GameLoop();
        } 
    }
}