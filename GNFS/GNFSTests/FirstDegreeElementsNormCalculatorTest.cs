using System;
using System.Numerics;
using GNFS.GNFS;
using GNFS.Polynomial_arithmetic.GaloisFieldLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GNFSTests
{
    [TestClass]
    public class FirstDegreeElementsNormCalculatorTest
    {
        [TestMethod]
        public void CalculateNormMethodTest()
        {
            // N(-8+3x) in Z[x]/(x^3+15x^2+29x+8)
            var polynomial = new Polynomial(new BigInteger[] { 8, 29, 15, 1 });
            var normCalculator=new FirstDegreeElementsNormCalculator(polynomial,3);
            var actual = normCalculator.CalculateNorm(-8);
            var expected = -5696;
            Assert.AreEqual(expected, actual);
        }
    }
}
