using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using GNFS.Polynomial_arithmetic.GaloisFieldLib;

namespace GNFS.Polynomial_arithmetic
{
    public interface IRootFinder
    {
        List<ulong> FindRoots(Polynomial polynomial, ulong mod);
    }
}
