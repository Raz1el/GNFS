using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using GNFS.Integer_arithmetic;
using GNFS.Polynomial_arithmetic;

namespace GNFS.GNFS.Polynomial_generator
{
    public class GnfsPolynomialGenerator : IPolynomialGenerator
    {
        BigInteger _number;
        int _polynomialDeg; 
        public GnfsPolynomialGenerator(BigInteger number, int polynomialDeg)
        {
            _number = number;
            _polynomialDeg = polynomialDeg;
        }
        public PolynomialInfo GeneratePolynomial()
        {
           var root=new IntegerNthRoot();
           var m = root.Root(_number, _polynomialDeg)-1;
           var coeff=new List<BigInteger>();
            var n = _number;
            while (n>0)
            {
                coeff.Add(n%m);
                n = n/m;
            }
            return new PolynomialInfo(new Polynomial(coeff.ToArray()),m );
        }
    }
}
