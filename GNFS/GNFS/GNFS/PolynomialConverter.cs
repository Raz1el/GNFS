using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using GNFS.Polynomial_arithmetic;

namespace GNFS.GNFS
{
    public class PolynomialConverter
    {
        public Polynomial ToHomogenusPolynomial(BigInteger point,Polynomial polynomial)
        {
            var newPolynomial=new BigInteger[polynomial.Deg+1];
            for (int i = 0; i <= polynomial.Deg; i++)
            {
                newPolynomial[i]=polynomial[i]*BigInteger.Pow(-point,polynomial.Deg-i);
            }
            return new Polynomial(newPolynomial);
        }
    }
}
