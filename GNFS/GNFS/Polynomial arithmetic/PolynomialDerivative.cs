using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;


namespace GNFS.Polynomial_arithmetic
{
    public class PolynomialDerivative
    {
        public Polynomial Derivative(Polynomial polynomial)
        {
            var derivative = new BigInteger[polynomial.Deg];
            for (int i = 0; i < polynomial.Deg; i++)
            {
                derivative[i] =(i+1)* polynomial[i + 1];
            }
            return new Polynomial(derivative);
        }
    }
}
