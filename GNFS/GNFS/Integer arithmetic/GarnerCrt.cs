using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GNFS.Integer_arithmetic
{
    public class GarnerCrt
    {
        public BigInteger Calculate(List<BigInteger> coefficients,List<BigInteger> primes)
        {
            var m = new BigInteger[primes.Count];
            var c = new BigInteger[primes.Count];
            var inverse = new ModularInverse();
            m[0] = 1;
            for (int i = 1; i < primes.Count; i++)
            {
                m[i] = 1;
                for (int j = 0; j < i; j++)
                {
                    m[i] *= primes[j];

                }
                c[i] = inverse.Inverse(m[i], primes[i]);
            }
            var M = m[primes.Count - 1] * primes[primes.Count - 1];
            var n = coefficients[0];
            for (int i = 1; i < primes.Count; i++)
            {
                var u = (c[i] * (coefficients[i] - n)) % primes[i];
                n += u * m[i];
            }
            n = n % M;
            return n;
        }
    }
}
