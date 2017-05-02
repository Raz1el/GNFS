using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GNFS.Polynomial_arithmetic
{
    public class PolynomialExtendedEuclideanAlgorithm
    {
        public Tuple<Polynomial, Polynomial, Polynomial> FindBezoutCoefficients(Polynomial firstArg, Polynomial secondArg)
        {
            var polyMath=new PolynomialMath(-1);
            Polynomial u0 = new Polynomial(new BigInteger[] {1});

            Polynomial u1 = new Polynomial(new BigInteger[] { 0 });

            Polynomial v0 = new Polynomial(new BigInteger[] { 0 });

            Polynomial v1 = new Polynomial(new BigInteger[] { 1 });

            Polynomial u2, v2, q0, r2;

            Polynomial r0 = firstArg;

            Polynomial r1 = secondArg;

            while (r1.Deg != -1)
            {
                q0 = polyMath.Div(r0 , r1);
                u2 = polyMath.Sub(u0 , polyMath.Mul( q0 , u1));
                v2 = polyMath.Sub(v0 , polyMath.Mul(q0 , v1));
                r2 = polyMath.Add(polyMath.Mul(u2 , firstArg) , polyMath.Mul(v2 , secondArg));
                r0 = r1;
                r1 = r2;
                u0 = u1;
                u1 = u2;
                v0 = v1;
                v1 = v2;
            }
            return new Tuple<Polynomial, Polynomial, Polynomial>(u0, v0, r0);

        }
    }
}
