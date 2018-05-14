using NUnit.Framework;
using ShipsGame.Core;

namespace ShipGameTest
{
    [TestFixture]
    public class SettingsTest
    {
        [TestCase(ShipTypes.Carrier, 5)]
        [TestCase(ShipTypes.Battleship, 4)]
        [TestCase(ShipTypes.Crusier, 3)]
        [TestCase(ShipTypes.Submarine, 3)]
        [TestCase(ShipTypes.Destroyer, 2)]
        public void Ships_Have_ProperSize(ShipTypes type, int expectedSize)
        {
            int actualSize = Settings.GetShipSize(type);

            Assert.That(actualSize, Is.EqualTo(expectedSize));
        }


        [TestCase(ShipTypes.Carrier, 1)]
        [TestCase(ShipTypes.Battleship, 1)]
        [TestCase(ShipTypes.Crusier, 1)]
        [TestCase(ShipTypes.Submarine, 1)]
        [TestCase(ShipTypes.Destroyer, 1)]
        public void Ships_Have_ProperCount(ShipTypes type, int expectedSize)
        {
            int actualSize = Settings.GetShipTypeCountLimit(type);

            Assert.That(actualSize, Is.EqualTo(expectedSize));
        }

        [Test]
        public void Board_Has_ProperSize()
        {
            var expectedHeight = 9;
            var expectedWidth = 11;

            var actualHeight = Settings.BoardHeight;
            var actualWidth = Settings.BoardWidth;

            Assert.That(actualWidth, Is.EqualTo(expectedWidth));
            Assert.That(actualHeight, Is.EqualTo(expectedHeight));
        }
    }
}