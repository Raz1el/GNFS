using System;
using System.Numerics;
using GNFS.Polynomial_arithmetic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GNFSTests
{
    [TestClass]
    public class PolynomialDerivativeTest
    {
        [TestMethod]
        public void DerivativeTest()
        {
            var polynomial=new Polynomial(new BigInteger[] {5,4,3,2,2});
            var derivative=new PolynomialDerivative().Derivative(polynomial).ToString();
            var expected = "8x^3+6x^2+6x+4";
            Assert.AreEqual(expected,derivative);
        }
    }
}
