using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using GNFS.Polynomial_arithmetic.GaloisFieldLib;

namespace GNFS.Polynomial_arithmetic
{
    public class BruteforceRootFinder:IRootFinder
    {
        public List<ulong> FindRoots(Polynomial polynomial, ulong mod)
        {
            polynomial = polynomial.Reduce(mod);

            var roots=new List<ulong>();
            for (ulong i = 0; i < mod; i++)
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
