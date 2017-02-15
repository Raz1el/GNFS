using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GNFS.Integer_arithmetic
{
    public class ExtendedEuclideanAlgorithm
    {
        public Tuple<BigInteger, BigInteger,BigInteger> FindBezoutCoefficients(BigInteger firstArg, BigInteger secondArg)
        {

            BigInteger u0 = 1;

            BigInteger u1 = 0;

            BigInteger v0 = 0;

            BigInteger v1 = 1;

            BigInteger u2, v2, q0, r2;

            BigInteger r0 = firstArg;

            BigInteger r1 = secondArg;

            while (r1 != 0)
            {
                q0 = r0 / r1;
                u2 = u0 - q0 * u1;
                v2 = v0 - q0 * v1;
                r2 = u2 * firstArg + v2 * secondArg;
                r0 = r1;
                r1 = r2;
                u0 = u1;
                u1 = u2;
                v0 = v1;
                v1 = v2;
            }
            if (r0 < 0)
            {
                return new Tuple<BigInteger, BigInteger, BigInteger>(-u0, -v0, -r0);
            }
            else
            {
                return new Tuple<BigInteger, BigInteger, BigInteger>(u0, v0, r0);
            }
           
        }
    }
}
