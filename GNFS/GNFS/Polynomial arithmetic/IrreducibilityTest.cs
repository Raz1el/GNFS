using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GNFS.Polynomial_arithmetic
{
    public class IrreducibilityTest
    {
        public bool IsIrreducible(Polynomial poly,BigInteger mod)
        {
            var factors=new List<int>();
            var deg = poly.Deg;
            if(deg%2==0)
                factors.Add(2);
            for (int i = 3; i<=deg; i+=2)
            {
                if (deg % i == 0)
                    factors.Add(i);
            }
            var polyMath=new PolynomialMath(mod);
            var x=new Polynomial(new BigInteger[] {0,1});
            var xPow = polyMath.ModPow(x, BigInteger.Pow(mod, deg), poly);
            var bigPoly = polyMath.Sub(xPow, x);
            if (bigPoly.Deg != -1)
                return false;
            var polynomialGcd=new PolynomialGcd();
            foreach (var factor in factors)
            {
                xPow = polyMath.ModPow(x, BigInteger.Pow(mod, deg/factor), poly);
                bigPoly = polyMath.Sub(xPow, x);
                var gcd=polynomialGcd.Calculate(bigPoly, poly,mod);
                if (gcd.Deg != 0 || (gcd[0] + mod)%mod != 1)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
