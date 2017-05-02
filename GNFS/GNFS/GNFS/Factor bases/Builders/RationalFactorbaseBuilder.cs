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
        BigInteger _root;
        long _bound;
        public RationalFactorbaseBuilder(BigInteger root, long bound)
        {
            _root = root;
            _bound = bound;
        }
        public RationalFactorbase Build()
        {

            var sieve=new EratosthenesSieve();
            var result = new List<Pair>();
           
            long interval = 10000000;
            if (_bound < interval)
                interval = _bound;
                for (long j = 3; j < _bound; j+=interval)
            {
                var primes = sieve.GetPrimes(j, interval+j);

                

                result.Add(new Pair(_root % 2, 2));
                for (int i = 0; i < primes.Length; i++)
                {
                    result.Add(new Pair(_root % primes[i], primes[i]));
                }
            }
           
            return new RationalFactorbase(result);
        }
    }
}
