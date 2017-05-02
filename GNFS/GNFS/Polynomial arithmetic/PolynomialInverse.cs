using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GNFS.Polynomial_arithmetic
{
    public class PolynomialInverse
    {
        public Polynomial Inverse(Polynomial polynomial,Polynomial mod)
        {
            var exEuclideanAlgorithm = new PolynomialExtendedEuclideanAlgorithm();
            var bezoutCoeff = exEuclideanAlgorithm.FindBezoutCoefficients(polynomial, mod);
            return bezoutCoeff.Item1;
        }
    }
}
