using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GNFS.QS
{
    public class SievePolynomial
    {
        public Dictionary<long,long> FirstRoot { get; set; }
        public Dictionary<long, long> SecondRoot { get; set; }

        public Dictionary<long, long> InvA { get; set; }

        public BigInteger A { get; set; }
        public BigInteger B { get; set; }
        public BigInteger C { get; set; }

        public long MinFactorA { get; set; }
        public long MaxFactorA { get; set; }

    }
}
