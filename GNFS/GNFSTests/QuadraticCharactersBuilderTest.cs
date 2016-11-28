using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using GNFS.GNFS.Factor_bases;
using GNFS.GNFS.Factor_bases.Builders;
using GNFS.Polynomial_arithmetic;
using GNFS.Polynomial_arithmetic.GaloisFieldLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GNFSTests
{
    [TestClass]
    public class QuadraticCharactersBuilderTest
    {
        [TestMethod]
        public void QuadraticCharactersBuilderBuildMethodTest()
        {
            //quadratic characters for n=45113, m=31 with  lower bound B1=104, upper bound B2=109 and polynomial f(x)=x^3+15x^2+29x+8
            Polynomial poly = new Polynomial(new BigInteger[] { 8, 29, 15, 1 });
            QuadraticCharactersBuilder builder = new QuadraticCharactersBuilder(poly, new BruteforceRootFinder(), 104,109);
            var actual = builder.Build();
            var expected = new List<Pair>()
                {
                    new Pair(4,107),
                    new Pair(8,107),
                    new Pair(80,107),
                    new Pair(99,109),
   
                 };
            CollectionAssert.AreEquivalent(expected, actual.Elements);
        }
    }
}
