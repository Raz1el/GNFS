using System.Collections.Generic;

namespace GNFS.GNFS.Factor_bases
{
    public class RationalFactorbase
    {
        public List<Pair> Elements { get; }

        public RationalFactorbase(List<Pair> elements)
        {
            Elements = elements;
        }
    }
}
