using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using GNFS.Integer_arithmetic;

namespace GNFS.Polynomial_arithmetic
{
    public class GcdRootFinder : IRootFinder
    {
        Random _random = new Random();


        public List<long> FindRoots(Polynomial polynomial, long mod)
        {
            var result=new List<long>();
            var gcd=new PolynomialGcd();
            var h = new BigInteger[mod+1];
            var polyMath=new PolynomialMath(mod);
            h[1] = -1;
            h[mod] = 1;

            var g = gcd.Calculate(polynomial, new Polynomial(h), mod);
            if (g.Value(0)%mod == 0)
            {
                result.Add(0);
                g=polyMath.Div(g, new Polynomial(new BigInteger[] {0, 1}));

            }
            Roots(g,mod,result);
            return result;
        }

        void Roots(Polynomial polynomial,long mod,List<long> roots)
        {
            if (polynomial.Deg == 2)
            {
                if (mod%2 == 0)
                {
                    for (long i = 0; i < mod; i++)
                    {
                        if (polynomial.Value(i)%mod==0)
                        {
                            roots.Add(i);
                        }
                    }
                }
                else
                {
                    var a = polynomial[2];
                    var b = polynomial[1];
                    var c = polynomial[0];

                    var sqrt=new ModularSqrt();
                    var inverse=new ModularInverse();
                    var inv2A = inverse.Inverse(2*a, mod);

                    var tmp = inv2A*b;
                    var root = sqrt.Sqrt(tmp*tmp - inverse.Inverse(a, mod)*c,mod);
                    roots.Add((long)((-tmp + root) % mod));
                    roots.Add((long)((-tmp- root)%mod));
                }
                return;
            }
            if (polynomial.Deg == 1)
            {
                var lc = (polynomial[1] + mod)%mod;
                if (lc == 1)
                {
                    roots.Add((long)(-polynomial[0]));
                   
                }
                else if(lc ==mod -1)
                {
                    roots.Add((long)(polynomial[0]));
                }
                else
                {
                    var inverse = new ModularInverse();

                    roots.Add((long)(-polynomial[0] * inverse.Inverse(polynomial[1], mod)));
                }
                return;
            }
            var polyMath=new PolynomialMath(mod);
            var arr=new BigInteger[2];
            arr[0] = _random.Next()%mod;
            arr[1] = 1;
            var h=new Polynomial(arr);
            h = polyMath.ModPow(h, (mod - 1)/2,polynomial);
            h[0] = h[0]- 1;
            var gcd=new PolynomialGcd();
            var g = gcd.Calculate(polynomial, h,mod);
            while (g.Deg==0||g==h)
            {
                arr[0] = _random.Next() % mod;
                h = new Polynomial(arr);
                h = polyMath.ModPow(h, (mod - 1) / 2, polynomial);
                h[0] -= 1;
                g = gcd.Calculate(polynomial, h, mod);
            }
            Roots(g,mod,roots);
            Roots(polyMath.Div(polynomial,g), mod, roots);
        } 
    }
}
