using NUnit.Framework;
using RisulGamigoTest.Problem1_AllDigitsUnique;

namespace RisulGamigoUnitTests
{
    [TestFixture]
    public class Problem1Tests
    {
        private UniqueDigitFinder _uniqueDigitFinder;
        [SetUp]
        public void Setup()
        {
            _uniqueDigitFinder= new UniqueDigitFinder();
        }

        [Test]
        public void NumberZeroGivesTrue()
        {

            var output = _uniqueDigitFinder.AllDigitsUnique(0);
            Assert.IsTrue(output);
        }

        [Test]
        public void Number1230IsUnique()
        {
            var output = _uniqueDigitFinder.AllDigitsUnique(1230);
            Assert.IsTrue(output);
        }
        [Test]
        public void Number1IsUnique()
        {
            var output = _uniqueDigitFinder.AllDigitsUnique(1);
            Assert.IsTrue(output);
        }

        [Test]
        public void Number911IsNoUnique()
        {
            var output = _uniqueDigitFinder.AllDigitsUnique(911);
            Assert.IsFalse(output);
        }
        [Test]
        public void Number4294967295IsNotUnique()
        {
            var output = _uniqueDigitFinder.AllDigitsUnique(4294967295);
            Assert.IsFalse(output);
        }
        [Test]
        public void Number4123567890IsUnique()
        {
            var output = _uniqueDigitFinder.AllDigitsUnique(4123567890);
            Assert.IsTrue(output);
        }

        [TearDown]
        public void TearDown()
        {
            _uniqueDigitFinder = null;
        }
    }
}