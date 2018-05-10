using System;
using System.ComponentModel;
using NUnit.Framework;
using Ships.UI;
using ShipsGame.Core;

namespace ShipGameTest
{
    [TestFixture]
    public class InputTranlatorHelperTests
    {
        [TestCase('H', Direction.Horizontal)]
        [TestCase('V', Direction.Vertical)]
        public void TranslateDirection_ValidInput_ReturnsProperDirection(char symbol, Direction expecteDirection)
        {
            var translateToDirection = InputTranslatorHelper.TranslateToDirection(symbol);
            Assert.That(translateToDirection, Is.EqualTo(expecteDirection));
        }

        [TestCase('A')]
        [TestCase('g')]
        [TestCase('h')]
        [TestCase('1')]
        [TestCase('\\')]
        public void TranslateDirection_ValidInput_ReturnsInvalidInput(char symbol)
        {
            Assert.Throws<ArgumentOutOfRangeException>(delegate
            {
                InputTranslatorHelper.TranslateToDirection(symbol);
            });
        }

        [TestCase('H', true)]
        [TestCase('V', true)]
        [TestCase('A', false)]
        [TestCase('g', false)]
        [TestCase('h', false)]
        [TestCase('1', false)]
        [TestCase('\\', false)]
        public void CheckIfInputIsProperDirection_ReturnsBool(char symbol, bool expectedResult)
        {
            var acutualResult = InputTranslatorHelper.IsDirection(symbol);
            Assert.That(acutualResult, Is.EqualTo(expectedResult),
                $"For \"{symbol}\" expected result was {expectedResult}");

        }


        [TestCase('B', true)]
        [TestCase('K', true)]
        [TestCase('C', true)]
        [TestCase('D', true)]
        [TestCase('S', true)]
        [TestCase('A', false)]
        [TestCase('g', false)]
        [TestCase('h', false)]
        [TestCase('1', false)]
        [TestCase('\\', false)]
        public void CheckIfInputIsProperShip_ReturnsBool(char symbol, bool expectedResult)
        {
            var acutualResult = InputTranslatorHelper.IsShip(symbol);
            Assert.That(acutualResult, Is.EqualTo(expectedResult),
                $"For \"{symbol}\" expected result was {expectedResult}");

        }


        [TestCase('B', ShipTypes.Battleship)]
        [TestCase('K', ShipTypes.Carrier)]
        [TestCase('C', ShipTypes.Crusier)]
        [TestCase('D', ShipTypes.Destroyer)]
        [TestCase('S', ShipTypes.Submarine)]
        public void TranslateShip_ValidInput_ReturnsProperShip(char symbol, ShipTypes expecteDirection)
        {
            var translateToDirection = InputTranslatorHelper.TranslateToShip(symbol);
            Assert.That(translateToDirection, Is.EqualTo(expecteDirection));
        }

        [TestCase('A')]
        [TestCase('g')]
        [TestCase('h')]
        [TestCase('1')]
        [TestCase('\\')]
        public void TranslateShip_InValidInput_ReturnsError(char symbol)
        {
            Assert.Throws<ArgumentOutOfRangeException>(delegate
            {
                InputTranslatorHelper.TranslateToShip(symbol);
            });
        }
        [Test]
        public void TranslateVaildComandToProperTypes_validOutput()
        {
            Command acualCommand = InputTranslatorHelper.TranlateCommand("BHB2");
            Command expected = new Command(new CellID("B2"), Direction.Horizontal, ShipTypes.Battleship);
            Assert.That(acualCommand.Direction, Is.EqualTo(expected.Direction));
            Assert.That(acualCommand.ShipType, Is.EqualTo(expected.ShipType));
            Assert.That(acualCommand.Cell.Id, Is.EqualTo(expected.Cell.Id));

        }

        [TestCase("BVZ1")]
        [TestCase("BVA25")]
        [TestCase("BBA1")]
        [TestCase("QVA1")]
        public void TranslateCommandToProperTypes_ThrowsError(string command)
        {
            Assert.Throws<ArgumentOutOfRangeException>(delegate
            {
                InputTranslatorHelper.TranlateCommand(command);
            });
        }

       
    }
}