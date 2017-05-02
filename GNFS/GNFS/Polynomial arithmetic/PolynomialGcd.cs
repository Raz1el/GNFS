using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GNFS.Polynomial_arithmetic
{
    public class PolynomialGcd
    {
        public Polynomial Calculate(Polynomial firstArg,Polynomial secondArg,BigInteger mod)
        {
            if(firstArg.Deg==secondArg.Deg&&firstArg.Deg==-1)
                throw new InvalidOperationException();
            if(firstArg.Deg==0||secondArg.Deg==0)
                return new Polynomial(new BigInteger[] {1});
            if (firstArg.Deg == -1)
                return secondArg;
            if (secondArg.Deg == -1)
                return firstArg;
            if (firstArg.Deg < secondArg.Deg || secondArg.Deg == -1)
            {
                var tmp = secondArg;
                secondArg = firstArg;
                firstArg = tmp;
            }
            var polyMath=new PolynomialMath(mod);
            firstArg=firstArg.Reduce(mod);
            secondArg=secondArg.Reduce(mod);
            while (secondArg.Deg!=-1)
            {
                var tmp = firstArg;
                firstArg = secondArg;
                secondArg = polyMath.Rem(tmp, secondArg);
            }
            return polyMath.Normalize(firstArg);
        }
    }
}
