using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using GNFS.Integer_arithmetic;

namespace GNFS.GNFS.Factor_bases.Builders
{
    public class RationalFactorbaseBuilder
    {
        ulong _root;
        ulong _bound;
        public RationalFactorbaseBuilder(ulong root, ulong bound)
        {
            _root = root;
            _bound = bound;
        }
        public RationalFactorbase Build()
        {

            var sieve=new EratosthenesSieve();
            var primes = sieve.GetPrimes(3, _bound);

            var result=new List<FactorbaseElement>();

            result.Add(new FactorbaseElement(_root%2,2));
            for (int i = 0; i < primes.Length; i++)
            {
                result.Add(new FactorbaseElement(_root % primes[i], primes[i]));
            }
            return new RationalFactorbase(result);
        }
    }
}
