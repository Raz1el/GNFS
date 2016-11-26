using System.Collections.Generic;

namespace GNFS.GNFS.Factor_bases
{
    public class AlgebraicFactorbase
    {
        public List<FactorbaseElement> Elements { get; }

        public AlgebraicFactorbase(List<FactorbaseElement> elements)
        {
            Elements = elements;
        }
    }
}
