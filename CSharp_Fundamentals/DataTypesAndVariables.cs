//This is reference to liblaries we are using
using NUnit.Framework;

namespace CSharp_Fundamentals
{
    // [] - defines attributes we will take closer look at them in day 3
    [TestFixture]
    public class DataTypesAndVariables
    {
        /*
         *1. Select each test and uncomment it (Ctrl+K, Ctrl+U)
         *2. Then follow the rules described in summary.
         *3. run test.
         *4. If test passed, you could move to next exercise.
         */

        //        /// <summary>
        //        /// Integers are symbolized as int
        //        /// create 3 integers with following names
        //        /// singleDigit, number, lessThenZero
        //        /// make they values:  6   14  -100
        //        /// </summary>
        //        [Test]
        //        public void Creating_Integeres()
        //        {
        //            // int - describes type
        //            // example - is name of variable, english names are preferred
        //            // =  equal sign is used for assigning values.
        //            // 0 - the value
        //            //  ; - IMPORTANT ';' sign means and of the command usually it is on end of the line - remember about it
        //            int example = 0;
        //
        //            Assert.AreEqual(example, 0, "Example Has wrong value did you changed it?");
        //            Assert.AreEqual(singleDigit, 6, "Example Has wrong value did you changed it?");
        //            Assert.That(singleDigit, Is.TypeOf<int>(), "You sure it is int?");
        //            Assert.AreEqual(number, 14, "Example Has wrong value did you changed it?");
        //            Assert.That(singleDigit, Is.TypeOf<int>(), "You sure it is int?");
        //            Assert.AreEqual(lessThenZero, -100, "Example Has wrong value did you changed it?");
        //            Assert.That(singleDigit, Is.TypeOf<int>(), "You sure it is int?");
        //        }

        /// <summary>
        /// Basic operations at integers are
        /// add(+), subtract(-), multiply(*), divide(/)
        /// make integer variables: subtract, multiply and perform operations.
        /// discuss why divide is not working
        /// </summary>
        [Test]
        public void Operation_On_Integers()
        {
            int X = 4;
            int Y = 5;

            /*
             * There are few other ways you can do this
             * int add = 0;
             * add = x+y;
             */
            int add = X + Y;
            int subtract = X - Y;
            int multiply = X * Y;
            int divide = X / Y;

            Assert.AreEqual(add, 9, "Example Has wrong value did you changed it?");
            Assert.AreEqual(subtract, -1, "Wrong value maybe other way around?");
            Assert.That(subtract, Is.TypeOf<int>(), "You sure it is int?");
            Assert.AreEqual(multiply, 20, "Example Has wrong value did you changed it?");
            Assert.That(multiply, Is.TypeOf<int>(), "You sure it is int?");
            Assert.AreEqual(divide, 1.25, "Example Has wrong value did you changed it?");
            Assert.That(divide, Is.TypeOf<int>(), "You sure it is int?");
        }
    }
}
