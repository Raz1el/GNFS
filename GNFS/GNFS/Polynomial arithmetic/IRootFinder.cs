using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;


namespace GNFS.Polynomial_arithmetic
{
    public interface IRootFinder
    {
        List<long> FindRoots(Polynomial polynomial, long mod);
    }
}
