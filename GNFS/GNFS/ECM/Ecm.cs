using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using GNFS.Integer_arithmetic;

namespace GNFS.ECM
{
    public class Ecm
    {
        public BigInteger Calculate(Curve curve, ulong b1, ulong b2,ulong b3)
        {
            var arithmetic = new MontgomeryArithmetic();
            var sieve = new EratosthenesSieve();
            var primeManager=new PrimeManager();
            var point = curve.Point;
            ulong prime = 2;
            var mod = curve.Mod;
            BigInteger product = 1;

            while (prime < b1)
            {
                point = arithmetic.Double(point, curve);
                prime = prime*2;
                product = (point.Z*product)%mod;
            }


            var gcd = BigInteger.GreatestCommonDivisor(product, mod);
            if (gcd > 1)
            {
                return gcd;
            }



            ulong[] primes;
            ulong upPrimeLimit = 1000000;
            ulong lowPrimeLimit = 3;
            ulong sievestep = 10000000;
            if (b1 < upPrimeLimit)
            {
                upPrimeLimit = b1;
            }
            ulong partCount = b1/sievestep;



            for (ulong j = 0; j <= partCount; j++)
            {
                primes = sieve.GetPrimes(lowPrimeLimit, upPrimeLimit);
                for (int i = 0; i < primes.Length; i++)
                {
                    var p = primes[i];
                    prime = p;
                    while (prime < b1)
                    {
                        point = arithmetic.MontgomeryMul(point, p, curve);
                        prime = prime*p;
                        product = (point.Z*product)%mod;
                    }
                    gcd = BigInteger.GreatestCommonDivisor(product, mod);
                    if (gcd > 1)
                    {

                        return gcd;
                    }
                }
                lowPrimeLimit = upPrimeLimit;
                upPrimeLimit += sievestep;
            }


            if (b2 % 2 == 0)
            {
                b2--;
            }
           

            ulong precomputedDataSize = 1000;
            var precomputedData = new ProjectivePoint[precomputedDataSize];
            precomputedData[0] = arithmetic.Double(point, curve);
            precomputedData[1] = arithmetic.Double(precomputedData[0], curve);
            for (ulong i = 2; i < precomputedDataSize; i++)
            {
                precomputedData[i] = arithmetic.Add(precomputedData[i - 1], precomputedData[0], precomputedData[i - 2], curve);
            }

            if (b2 <= 2 * precomputedDataSize)
            {
                b2 = precomputedDataSize + 2;
            }

            product = 1;
            var R = arithmetic.MontgomeryMul(point, b2, curve);
            var T = arithmetic.MontgomeryMul(point, b2 - 2 * precomputedDataSize, curve);
            primeManager.Init(b2);
            for (ulong i = b2; i < b3; i = i + 2 * precomputedDataSize)
            {
                for (ulong j = 0; j < precomputedDataSize; j++)
                {
                    prime = primeManager.NextPrime();
                    var delta = (prime - i) / 2;
                    if(delta>=(ulong)precomputedData.Length)
                        break;
                    product = (product * (R.X * precomputedData[delta - 1].Z - R.Z * precomputedData[delta
                        - 1].X)) % curve.Mod;
                }



                gcd = BigInteger.GreatestCommonDivisor(curve.Mod, product);
                if (gcd > 1)
                {
                    return gcd;
                }
                var temp = T;
                T = R;
                R = arithmetic.Add(R, precomputedData[precomputedDataSize - 1], temp, curve);
            }
            return 1;

        }
    }
}

    
