using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using GNFS.Polynomial_arithmetic;

namespace GNFS.GNFS
{
    public class FirstDegreeElementsNormCalculator
    {
        //norm a+bx in Z[x]/(f)
        Polynomial _polynomial;

        public FirstDegreeElementsNormCalculator(Polynomial polynomial,BigInteger b)
        {
            var converter = new PolynomialConverter();
            _polynomial = converter.ToHomogenusPolynomial(b, polynomial);
        }
        
        public BigInteger CalculateNorm(BigInteger a)
        {
            return _polynomial.Value(a);
        }
    }
}
