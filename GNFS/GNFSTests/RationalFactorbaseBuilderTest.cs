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
            var expected=new List<FactorbaseElement>()
                {
                    new FactorbaseElement(1,2),
                    new FactorbaseElement(1,3),
                    new FactorbaseElement(1,5),
                    new FactorbaseElement(3,7),
                    new FactorbaseElement(9,11),
                    new FactorbaseElement(5,13),
                    new FactorbaseElement(14,17),
                    new FactorbaseElement(12,19),
                    new FactorbaseElement(8,23),
                    new FactorbaseElement(2,29),
                 };
            CollectionAssert.AreEquivalent(expected,actual.Elements);
        }
    }
}
