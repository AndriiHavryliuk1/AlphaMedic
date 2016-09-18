using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Rest.Tests.Tests
{
    [TestClass()]
    public class ClosureExampleTests
    {
        [TestMethod()]
        public void HelloTest()
        {
            var name = "Oleg";
            var secondName = "Andriy";
            var func = ClosureExample.Hello();
            var expected = "Hello, Andriy previous user Oleg";
            var actual = func(name);
            actual = func(secondName);
            Assert.AreEqual(expected, actual);
        }
    }
}