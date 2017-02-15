using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using GNFS.Polynomial_arithmetic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GNFSTests
{
    [TestClass]
    public class PolynomialTest
    {
        [TestMethod]
        public void ValueMethodTest()
        {
            var polynomial=new Polynomial(new BigInteger[] {-5,3,3});
            var point = 7;
            //3*7*7+3*7-5=163
            var expected = 163;
            var actual = polynomial.Value(point);
            Assert.AreEqual(expected,actual);
        }
        [TestMethod]
        public void ReduceMethodTest()
        {
            var polynomial = new Polynomial(new BigInteger[] { -5, 3, 2 });
            var mod = 2;
            var expected = "x-1";
            var actual = polynomial.Reduce(mod).ToString();
            Assert.AreEqual(expected, actual);
        }
    }
}
