using System.Collections.Generic;

namespace GNFS.GNFS.Factor_bases
{
    public class RationalFactorbase
    {
        public List<FactorbaseElement> Elements { get; }

        public RationalFactorbase(List<FactorbaseElement> elements)
        {
            Elements = elements;
        }
    }
}
