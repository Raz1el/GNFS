using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using GNFS.Polynomial_arithmetic;

namespace GNFS.GNFS.Polynomial_generator
{
    public class PolynomialInfo
    {
        public Polynomial Polynomial { get; }
        public BigInteger Root { get; }

        public PolynomialInfo(Polynomial polynomial,BigInteger root)
        {
            Polynomial = polynomial;
            Root = root;
        }
    }
}
