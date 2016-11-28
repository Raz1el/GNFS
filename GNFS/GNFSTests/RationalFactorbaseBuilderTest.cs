using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GNFS.GNFS.Factor_bases;
using GNFS.GNFS.Factor_bases.Builders;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GNFSTests
{
    [TestClass]
    public class RationalFactorbaseBuilderTest
    {
        [TestMethod]
        public void RationalFactorbaseBuildMethodTest()
        {
            //factorbase for n=45113, m=31 with bound B=29
            RationalFactorbaseBuilder builder=new RationalFactorbaseBuilder(31,29);
            var actual=builder.Build();
            var expected=new List<Pair>()
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
                 };
            CollectionAssert.AreEquivalent(expected,actual.Elements);
        }
    }
}
