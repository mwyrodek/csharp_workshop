/*using System;
using System.Linq;
using NUnit.Framework;
using ShipsGame.Core;

namespace ShipGameTest
{
    [TestFixture]
    public class CellIDTests
    {
        [Test]
        public void GenerateFirstId()
        {
            const string ExpectedID = "A1";
            var actualCellId = CellID.CreateFirstCell();

            Assert.That(actualCellId.Id,Is.EqualTo(ExpectedID));
        }

        [TestCase("A1")]
        [TestCase("B2")]
        [TestCase("F9")]
        [TestCase("H8")]
        [TestCase("K9")]
        public void CreateId_validInput_ReturnCellID(string expectedID)
        {
            var actualCellId = new CellID(expectedID);

            Assert.That(actualCellId.Id, Is.EqualTo(expectedID));
        }

        [Test]
        public void CreateId_validInputLowerCase_ReturnCellID()
        {
            string id = "a2";
            var actualCellId = new CellID(id);

            Assert.That(actualCellId.Id, Is.EqualTo("A2"));
        }

        [TestCase("A0")]
        [TestCase("a0")]
        [TestCase("Z5")]
        [TestCase("H10")]
        [TestCase("F12")]
        [TestCase("11")]
        [TestCase("1")]
        [TestCase("")]
        public void CreateId_InvalidInput_ThrowErros(string expectedID)
        {

            var exception = Assert.Throws<ArgumentOutOfRangeException>(
                delegate { new CellID(expectedID); }

            );
            

            Assert.That(exception.Message, Contains.Substring(expectedID.ToUpper()));
        }


        [TestCase(null)]
        public void CreateId_NullInput_ThrowErros(string expectedID)
        {

            var exception = Assert.Throws<NullReferenceException>(
                delegate { new CellID(expectedID); }

            );

            Assert.That(exception.Message, Is.EqualTo("Input was null."));
        }

        [TestCase("A1", "A2,B1")]
        [TestCase("A2", "A3,B2")]
        [TestCase("F5", "F6,G5")]
        [TestCase("K1", "K2")]
        [TestCase("B9", "C9")]
        public void GenereteNextID_retruns_ValidOutput(string strattingID, string expectedResults)
        {
            var expectedResultsList= expectedResults.Split(',').ToList();
            
            var actualResult = new CellID(strattingID).GetNeighbourIDs();

            Assert.That(actualResult, Is.EqualTo(expectedResultsList));
        }

        [TestCase("K9")]
        public void GenereteNextID_edge_returnsNull(string edgeID)
        {
            

            var actualResult = new CellID(edgeID).GetNeighbourIDs();

            Assert.That(actualResult.Count, Is.EqualTo(0));
        }
    }
}*/