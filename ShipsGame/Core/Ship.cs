namespace ShipsGame.Core
{
    public class Ship
    {
        public ShipTypes shipType { get; }
        

        public Ship(ShipTypes shipType)
        {
            this.shipType = shipType;
            this.SetupSize();
        }



        //TODO Refactor it to use configs
        private void SetupSize()
        {
            switch (shipType)
            {
                case ShipTypes.Battleship:

            }
        }
    }
}