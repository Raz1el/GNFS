using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GNFS.Integer_arithmetic
{
    public class ModularSqrt
    {
        public  BigInteger Sqrt(BigInteger a, BigInteger p)
        {
            var jacobiSymbol=new JacobiSymbol();
            if (jacobiSymbol.Calculate(a, p) == -1)
            {
                throw new ArgumentException("square root doesn't exist");
            }
            if (p == 2)
                return 1;
            BigInteger t = 0;
            for (BigInteger i = 0; i < p; i++)
            {
                if (jacobiSymbol.Calculate((i * i - a), p) == -1)
                {
                    t = i;
                    break;
                }
            }
            return PowInGfP2(new Tuple<BigInteger, BigInteger>(t, 1), (p + 1) / 2, p, t * t + p - a).Item1;

        }

        Tuple<BigInteger, BigInteger> PowInGfP2(Tuple<BigInteger, BigInteger> x, BigInteger pow, BigInteger p, BigInteger sqrGenerator)
        {
            if (pow == 0)
                return new Tuple<BigInteger, BigInteger>(1, 0);
            var res = new Tuple<BigInteger, BigInteger>(1, 0);
            Tuple<BigInteger, BigInteger> a = x;
            while (pow != 0)
            {

                if (pow % 2 != 0)
                {
                    res = new Tuple<BigInteger, BigInteger>((a.Item1 * res.Item1 + a.Item2 * res.Item2 * sqrGenerator) % p, (a.Item2 * res.Item1 + a.Item1 * res.Item2) % p);
                }

                a = new Tuple<BigInteger, BigInteger>((a.Item1 * a.Item1 + a.Item2 * a.Item2 * sqrGenerator) % p, (a.Item2 * a.Item1 + a.Item1 * a.Item2) % p);
                pow >>= 1;
            }
            return res;
        }

    }
}
