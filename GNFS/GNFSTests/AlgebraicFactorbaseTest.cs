using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using GNFS.GNFS.Factor_bases;
using GNFS.GNFS.Factor_bases.Builders;
using GNFS.Polynomial_arithmetic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GNFSTests
{
    [TestClass]
    public class AlgebraicFactorbaseTest
    {
        [TestMethod]
        public void AlgebraicFactorbaseBuildMethodTest()
        {
            //algebraic factorbase for n=45113, m=31 with bound B=103 and polynomial f(x)=x^3+15x^2+29x+8
            Polynomial poly=new Polynomial(new BigInteger[] {8,29,15,1});
            AlgebraicFactorbaseBuilder builder=new AlgebraicFactorbaseBuilder(poly,new BruteforceRootFinder(), 103);
            var actual = builder.Build();
            var expected = new List<Pair>()
                {
                    new Pair(0,2),
                    new Pair(6,7),
                    new Pair(13,17),
                    new Pair(11,23),
                    new Pair(26,29),
                    new Pair(18,31),
                    new Pair(19,41),
                    new Pair(13,43),
                    new Pair(1,53),
                    new Pair(46,61),
                    new Pair(2,67),
                    new Pair(6,67),
                    new Pair(44,67),
                    new Pair(50,73),
                    new Pair(23,79),
                    new Pair(47,79),
                    new Pair(73,79),
                    new Pair(28,89),
                    new Pair(62,89),
                    new Pair(73,89),
                    new Pair(28,97),
                    new Pair(87,101),
                     new Pair(47,103)
                 };
            CollectionAssert.AreEquivalent(expected, actual.Elements);
        }
    }
}
