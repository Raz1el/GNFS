using System.Collections.Generic;

namespace GNFS.GNFS.Factor_bases
{
    public class AlgebraicFactorbase
    {
        public List<Pair> Elements { get; }

        public AlgebraicFactorbase(List<Pair> elements)
        {
            Elements = elements;
        }
    }
}
