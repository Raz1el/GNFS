using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using GNFS.Polynomial_arithmetic;

namespace GNFS.GNFS.Polynomial_generator
{
    public class SnfsPolynomialGenerator:IPolynomialGenerator
    {
        SpecialNumber _number;
        int _polynomialDeg;

        public SnfsPolynomialGenerator(SpecialNumber number,int polynomialDeg)
        {
            _number = number;
            _polynomialDeg = polynomialDeg;
        }
        public PolynomialInfo GeneratePolynomial()
        {
            var k =(int) Math.Ceiling(_number.Exp/(_polynomialDeg*1.0));
            var poly=new BigInteger[_polynomialDeg+1];
            poly[_polynomialDeg] = 1;
            poly[0] = -_number.Const*BigInteger.Pow(_number.Base, k*_polynomialDeg - _number.Exp);
            return new PolynomialInfo(new Polynomial(poly),BigInteger.Pow(_number.Base,k));
        }
    }
}
