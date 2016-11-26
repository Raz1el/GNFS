using GNFS;
using GNFS.GNFS.Polynomial_generator;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Numerics;

namespace GNFSTests
{
    [TestClass]
    public class SnfsPolynomialGeneratorTest
    {
        [TestMethod]
        public void GeneratorReturnCorrectPolynomial()
        {
            var specialNumber=new SpecialNumber(3,20,2);
            var generator=new SnfsPolynomialGenerator(specialNumber,3);
            var expectedPoly = "x^3-6";
            var actual = generator.GeneratePolynomial().Polynomial;
            Assert.AreEqual(expectedPoly,actual.ToString());
        }

        [TestMethod]
        public void GeneratorReturnCorrectRoot()
        {
            var specialNumber = new SpecialNumber(3, 20, 2);
            var generator = new SnfsPolynomialGenerator(specialNumber, 3);
            BigInteger expectedRoot = 2187;
            var actual = generator.GeneratePolynomial().Root;
            Assert.AreEqual(expectedRoot, actual);
        }
    }
}
