using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using GNFS.Integer_arithmetic;
using GNFS.Polynomial_arithmetic;

namespace GNFS.GNFS.Square_root
{
    public class AlgebraicSqrt
    {
        Random _random=new Random();
        public BigInteger Sqrt(Polynomial sqr,Polynomial polynomial,BigInteger n,BigInteger root)
        {
            var nextPrime=new NextPrime();
            var mod = nextPrime.GetNext(10*n);
            var polyMath=new PolynomialMath(mod);


            var rx = GeneratePoly(polynomial.Deg-1,mod);
            var power = Pow(
                new Tuple<Polynomial, Polynomial>(rx, new Polynomial(new BigInteger[] {-1})),
                (BigInteger.Pow(mod,polynomial.Deg)-1)/2, polynomial,mod,sqr
                );
            var check = polyMath.Rem(polyMath.Mul(polyMath.Mul(sqr, power.Item2),power.Item2),polynomial);
            while (!(check.Deg == 0 && check[0] == 1))
            {
                rx = GeneratePoly(polynomial.Deg - 1, mod);
                power = Pow(
                    new Tuple<Polynomial, Polynomial>(rx, new Polynomial(new BigInteger[] { -1 })),
                    (BigInteger.Pow(mod, polynomial.Deg) - 1) / 2, polynomial, mod, sqr
                    );
                check = polyMath.Rem(polyMath.Mul(polyMath.Mul(sqr, power.Item2), power.Item2), polynomial);
            }
            var k = 0;
            var r = power.Item2;
            while (true)
            {
                k++;
                var pk = BigInteger.Pow(mod, k);
                polyMath=new PolynomialMath(pk);
                var inverse=new ModularInverse();
                r =polyMath.Mul(new Polynomial(new BigInteger[] {inverse.Inverse(2,pk)}), polyMath.Mul(r,
                    polyMath.Sub(new Polynomial(new BigInteger[] {3}), polyMath.Mul(polyMath.Mul(sqr, r), r))));
                check = polyMath.Rem(polyMath.Mul(polyMath.Mul(sqr, r), r), polynomial);
                if (check==sqr)
                    break;
            }
            polyMath=new PolynomialMath(mod);
            var dx=new PolynomialDerivative();
            var df = dx.Derivative(polynomial);
            return polyMath.Mul(polyMath.Mul(sqr, r), df).Value(root);
        }

        Polynomial GeneratePoly(long deg,BigInteger mod)
        {
            var poly=new BigInteger[deg+1];
            for (int i = 0; i < deg; i++)
            {
                poly[i] = _random.Next();
            }
            poly[deg] = _random.Next(1,int.MaxValue);
            return new Polynomial(poly,mod);
        }


        public Tuple<Polynomial, Polynomial> Pow(Tuple<Polynomial, Polynomial> x, BigInteger pow, Polynomial modPoly,BigInteger mod, Polynomial s)
        {
            if (pow == 0)
                return new Tuple<Polynomial, Polynomial>(new Polynomial(new BigInteger[] { 1}), new Polynomial(new BigInteger[] { 0 }));

            var polyMath=new PolynomialMath(mod);
            var res = new Tuple<Polynomial, Polynomial>(new Polynomial(new BigInteger[] { 1 }), new Polynomial(new BigInteger[] { 0 }));
            Tuple<Polynomial, Polynomial> a = x;
            while (pow != 0)
            {
                Polynomial first, second;

                if (pow % 2 != 0)
                {
                    first = polyMath.Add(polyMath.Mul(a.Item1, res.Item1),polyMath.Mul(polyMath.Mul(a.Item2,res.Item2),s));
                    second = polyMath.Add(polyMath.Mul(a.Item2, res.Item1), polyMath.Mul(a.Item1, res.Item2));
                    res = new Tuple<Polynomial, Polynomial>(polyMath.Rem(first,modPoly),polyMath.Rem(second,modPoly));
                }


                first = polyMath.Add(polyMath.Mul(a.Item1, a.Item1), polyMath.Mul(polyMath.Mul(a.Item2, a.Item2), s));
                second = polyMath.Mul(polyMath.Mul(a.Item1, a.Item2),new Polynomial(new BigInteger[] {2}));
               
                
                a = new Tuple<Polynomial, Polynomial> (polyMath.Rem(first, modPoly), polyMath.Rem(second, modPoly));
                pow >>= 1;
            }
            return res;
        }
    }
}
