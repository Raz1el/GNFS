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
    public class GcdRootFinderTest
    {
        [TestMethod]
        public void GcdRootTest()
        {
            //(x-5)(x-7)(x-23)(x-1)(x-55)(x-17)(x-22)(x-2)(x-99)(x-100)(x-31)(x-50)x
            
            var arr = new BigInteger[] { 0, 101, 17, 17, 91, 38, 15, 28, 98, 49, 74, 97, 16, 1 };
            var mod = 107;
            var poly = new Polynomial(arr, mod);
            var rootFinder = new GcdRootFinder();
            var actual = rootFinder.FindRoots(poly, mod).Select(x => (x + mod) % mod).OrderBy(x => x).ToList();
            var expected = new List<long>() { 0, 5, 7, 23, 1, 55, 17, 22, 2, 99, 100, 31, 50 }.OrderBy(x => x).ToList();
            Assert.AreEqual(actual.Count(), expected.Count());
            for (int i = 0; i < expected.Count(); i++)
            {
                if ((actual[i] + mod) % mod != (expected[i] + mod) % mod)
                {
                    throw new Exception();
                }
            }

        }
    }
}
