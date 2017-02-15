using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GNFS.Integer_arithmetic
{
    public class ModularInverse
    {
        public BigInteger Inverse(BigInteger number,BigInteger mod,out BigInteger gcd)
        {
            var exEuclideanAlgorithm=new ExtendedEuclideanAlgorithm();
            var bezoutCoeff = exEuclideanAlgorithm.FindBezoutCoefficients(number, mod);
            gcd = bezoutCoeff.Item3;
            return bezoutCoeff.Item1;
        }
        public BigInteger Inverse(BigInteger number, BigInteger mod)
        {
            var exEuclideanAlgorithm = new ExtendedEuclideanAlgorithm();
            var bezoutCoeff = exEuclideanAlgorithm.FindBezoutCoefficients(number, mod);
            return bezoutCoeff.Item1;
        }
    }
}
