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
        public bool DontExist { get; private set; }
        public Polynomial Sqrt(Polynomial sqr,Polynomial mod,Polynomial modDerivative,BigInteger sqrtNorm)
        {
            BigInteger max = 0;
            for (int i = 0; i <= sqr.Deg; i++)
            {
                if (BigInteger.Abs(sqr[i]) > max)
                {
                    max = BigInteger.Abs(sqr[i]);
                }
            }
            var nextPrime=new NextPrime();
            var bound = max*10;
            var primes=new List<BigInteger>();
            var sieve=new EratosthenesSieve();
            var primesToCheck = sieve.GetPrimes(100000000, 110000000);
            BigInteger bigMod = 1;
            foreach (var primeToCheck in primesToCheck)
            {
                var irreducibilityTest = new IrreducibilityTest();
                if (!irreducibilityTest.IsIrreducible(mod, primeToCheck))
                {
                    continue;
                }
                bigMod *= primeToCheck;
                primes.Add(primeToCheck);
                if (bigMod > bound)
                    break;
            }
            var prime = 2*primes[primes.Count - 1];
            while (bigMod<bound)
            {
                prime = nextPrime.GetNext(prime);
                var irreducibilityTest = new IrreducibilityTest();
                while (!irreducibilityTest.IsIrreducible(mod, prime))
                {
                    prime = nextPrime.GetNext(prime + 1);
                }
                bigMod *= prime;
                primes.Add(prime);
            }
            var sqrts=new List<Polynomial>();
            foreach (var fieldChar in primes)
            {
                var polyMath = new PolynomialMath(fieldChar);
                var four = new Polynomial(new BigInteger[] { 4 });
                Polynomial b;
                var isSqr = false;
                for (int i = 0; true; i++)
                {
                    b = new Polynomial(new BigInteger[] { i });
                    var tmp = polyMath.ModPow(polyMath.Sub(polyMath.Mul(b, b), polyMath.Mul(four, sqr)),
                        (BigInteger.Pow(fieldChar, mod.Deg) - 1) / 2, mod);
                    isSqr = (tmp[0] == 1 || tmp[0] == -(fieldChar - 1)) && (tmp.Deg == 0);
                    if (!isSqr)
                    {
                        break;
                    }
                }
                var gfMath = new FiniteFieldMath(mod, fieldChar);
                var modPoly = new PolynomialOverFiniteField(new Polynomial[] { sqr, b, new Polynomial(new BigInteger[] { 1 }) });
                var y = new PolynomialOverFiniteField(new Polynomial[] { new Polynomial(new BigInteger[] { 0 }), new Polynomial(new BigInteger[] { 1 }) });
                var sqrt= gfMath.ModPow(y, (BigInteger.Pow(fieldChar, mod.Deg) + 1) / 2, modPoly)[0];
                var sqrtNormMod=sqrtNorm * polyMath.ModPow(modDerivative, (BigInteger.Pow(fieldChar, mod.Deg) - 1) / (fieldChar - 1), mod)[0];
                var norm = polyMath.ModPow(sqrt, (BigInteger.Pow(fieldChar, mod.Deg) - 1) / (fieldChar - 1), mod)[0];
                sqrtNormMod %= fieldChar;
                if (sqrtNormMod < 0)
                {
                    sqrtNormMod = sqrtNormMod+fieldChar;
                }
                if (norm < 0)
                {
                    norm = norm+fieldChar;
                }
                if (norm != sqrtNormMod)
                {
                    sqrt = polyMath.ConstMul(-1, sqrt);
                }
                sqrts.Add(sqrt);
            }
            BigInteger[] result=new BigInteger[mod.Deg];
            var crt=new GarnerCrt();
            for (int i = 0; i < mod.Deg; i++)
            {
                var coefficients=new List<BigInteger>();
                for (int j = 0; j < primes.Count; j++)
                {
                    coefficients.Add(sqrts[j][i]);
                }
                result[i] = crt.Calculate(coefficients, primes);
            }
            var resultPoly=new Polynomial(result);
            for (int i = 0; i <= resultPoly.Deg; i++)
            {
                var coeff = BigInteger.Abs(resultPoly[i]);
                if (coeff > max)
                {
                    if (resultPoly[i] < 0)
                    {
                        resultPoly[i] += bigMod;
                    }
                    else
                    {
                        resultPoly[i] -= bigMod;
                    }
                }
            }
            return resultPoly;
        }
        public bool IsSqr(Polynomial sqr, Polynomial mod, BigInteger fieldChar)
        {
            var polyMath=new PolynomialMath(fieldChar);
            var pow = polyMath.ModPow(sqr, (BigInteger.Pow(fieldChar, mod.Deg) - 1)/2, mod)[0];
            var isSqr= pow == 1||pow==-(fieldChar-1);
            return isSqr;
        }
    }
}
