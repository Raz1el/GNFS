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
    public class AlgebraicFactorbaseTest
    {
        [TestMethod]
        public void AlgebraicFactorbaseBuildMethodTest()
        {
            //algebraic factorbase for n=45113, m=31 with bound B=103 and polynomial f(x)=x^3+15x^2+29x+8
            Polynomial poly=new Polynomial(new BigInteger[] {8,29,15,1});
            AlgebraicFactorbaseBuilder builder=new AlgebraicFactorbaseBuilder(poly,new BruteforceRootFinder(), 103);
            var actual = builder.Build();
            var expected = new List<FactorbaseElement>()
                {
                    new FactorbaseElement(0,2),
                    new FactorbaseElement(6,7),
                    new FactorbaseElement(13,17),
                    new FactorbaseElement(11,23),
                    new FactorbaseElement(26,29),
                    new FactorbaseElement(18,31),
                    new FactorbaseElement(19,41),
                    new FactorbaseElement(13,43),
                    new FactorbaseElement(1,53),
                    new FactorbaseElement(46,61),
                    new FactorbaseElement(2,67),
                    new FactorbaseElement(6,67),
                    new FactorbaseElement(44,67),
                    new FactorbaseElement(50,73),
                    new FactorbaseElement(23,79),
                    new FactorbaseElement(47,79),
                    new FactorbaseElement(73,79),
                    new FactorbaseElement(28,89),
                    new FactorbaseElement(62,89),
                    new FactorbaseElement(73,89),
                    new FactorbaseElement(28,97),
                    new FactorbaseElement(87,101),
                     new FactorbaseElement(47,103)
                 };
            CollectionAssert.AreEquivalent(expected, actual.Elements);
        }
    }
}
