using System;
using System.Collections.Generic;
using System.Numerics;
using GNFS.GNFS;
using GNFS.GNFS.Factor_bases;
using GNFS.GNFS.Sieve;
using GNFS.Polynomial_arithmetic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GNFSTests
{
    [TestClass]
    public class SimpleSieveAlgorithmTest
    {
        [TestMethod]
        public void SieveMethodTest()
        {
            var algebraicFactorbase =new AlgebraicFactorbase( new List<Pair>()
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
                 });
            var rationalFactorbase =new RationalFactorbase( new List<Pair>()
                {
                    new Pair(1,2),
                    new Pair(1,3),
                    new Pair(1,5),
                    new Pair(3,7),
                    new Pair(9,11),
                    new Pair(5,13),
                    new Pair(14,17),
                    new Pair(12,19),
                    new Pair(8,23),
                    new Pair(2,29),
                 });
            var polynomial=new Polynomial(new BigInteger[] {8,29,15,1});
            var root = 31;
            var options=new SieveOptions(1000,-1000,algebraicFactorbase,rationalFactorbase,polynomial,root);
            var sieveAlgorithm=new SimpleSieveAlgorithm();
            var result = sieveAlgorithm.Sieve(40, options);
            var smoothTester=new SmoothTester();

            foreach (var pair in result)
            {
                Assert.IsTrue(smoothTester.IsSmoothOverRationalFactorbase(pair.Item1,pair.Item2,root,rationalFactorbase));
                Assert.IsTrue(smoothTester.IsSmoothOverAlgebraicFactorbase(pair.Item1,pair.Item2,polynomial,algebraicFactorbase));
            }

        
        }
    }
}
