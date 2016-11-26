using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using GNFS.Polynomial_arithmetic;
using GNFS.Polynomial_arithmetic.GaloisFieldLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GNFSTests
{
    [TestClass]
    public class BruteforceRootFinderTest
    {
        [TestMethod]
        public void FindRootsMethodTest()
        {
            var solver=new BruteforceRootFinder();
            var polynomial = new Polynomial(new BigInteger[] { -5, 3, 1 });
            ulong mod = 7;
            //roots x^2+3x-5=0 mod 7
            var expected =new ulong[]{5,6 };
            var actual = solver.FindRoots(polynomial,mod);
            CollectionAssert.AreEquivalent(expected,actual);
        }
    }
}
