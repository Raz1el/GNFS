using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GNFS.GNFS.Factor_bases
{
    public class Pair
    {
        public BigInteger Item1 { get; }
        public BigInteger Item2 { get; }


        public Pair(BigInteger item1, BigInteger item2)
        {
            Item1 = item1;
            Item2 = item2;
        }


        public override bool Equals(object obj)
        {
            var element = obj as Pair;
            if (element == null)
                return false;
            return element.Item1 == Item1 && element.Item2 == Item2;
        }

        public override int GetHashCode()
        {
            return (Item1.ToString() + Item2).GetHashCode();
        }
    }
}
