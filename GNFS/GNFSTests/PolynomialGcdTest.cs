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
    public class PolynomialGcdTest
    {
        [TestMethod]
        public void GcdTest()
        {
            var mod = 17;
            var f=new Polynomial(new BigInteger[] {1,0,7,0,0,0,0,0,0,1,0,7});
            var g = new Polynomial(new BigInteger[] { 1,0,7,0,0,-1,0,-7 });
            var gcd=new PolynomialGcd();
            var actual = gcd.Calculate(g, f,mod);
            var expected=new Polynomial(new BigInteger[] {5,0,1});
            for (int i = 0; i <= actual.Deg; i++)
            {
                var x = (expected[i] + mod)%mod;
                var y = (actual[i] + mod)%mod;
                if ((actual[i] + mod)%mod != (expected[i] + mod)%mod)
                {
                    throw new Exception();
                }
            }
        }
    }
}
