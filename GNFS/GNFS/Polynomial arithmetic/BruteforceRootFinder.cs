using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GNFS.Polynomial_arithmetic
{
    public class BruteforceRootFinder:IRootFinder
    {
        public List<long> FindRoots(Polynomial polynomial, long mod)
        {
            polynomial = polynomial.Reduce(mod);

            var roots=new List<long>();
            for (long i = 0; i < mod; i++)
            {
                if (polynomial.Value(i)%mod == 0)
                {
                    roots.Add(i);
                }
            }
            return roots;
        } 
    }
}
