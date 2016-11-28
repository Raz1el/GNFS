using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using GNFS.GNFS;
using GNFS.Polynomial_arithmetic.GaloisFieldLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GNFSTests
{
    [TestClass]
    public class PolynomialConverterTest
    {
        [TestMethod]
        public void ToHomogenusPolynomialMethodTest()
        {
            //convert f(x)=x^3+15x^2+29x+8
            //to homogenus F(x,y)=(-y)^3*f(-x/y) where y=3
            var polynomial=new Polynomial(new BigInteger[] {8,29,15,1});
            var converter=new PolynomialConverter();
            var expected=new Polynomial(new BigInteger[] {-8*3*3*3,29*3*3,-15*3,1});
            var actual = converter.ToHomogenusPolynomial(3, polynomial);
            Assert.AreEqual(expected,actual);
        }
    }
}
