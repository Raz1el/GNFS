using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GNFS.GNFS.Factor_bases;

namespace GNFS.GNFS.Sieve
{
    public interface ISievingAlgorithm
    {
        List<Pair> Sieve(ulong pairsCount,SieveOptions options);
    }
}
