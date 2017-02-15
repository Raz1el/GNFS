using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GNFS.Integer_arithmetic;
using GNFS.Polynomial_arithmetic;

namespace GNFS.GNFS.Factor_bases.Builders
{
    public class AlgebraicFactorbaseBuilder
    {
        Polynomial _polynomial;
        IRootFinder _rootFinder;
        long _primeBound;


        public AlgebraicFactorbaseBuilder(Polynomial polynomial,IRootFinder rootFinder, long primeBound)
        {
            _primeBound = primeBound;
            _rootFinder = rootFinder;
            _polynomial = polynomial;
        }

        public AlgebraicFactorbase Build()
        {
            var sieve = new EratosthenesSieve();
            var primes = sieve.GetPrimes(3, _primeBound);

            var result = new List<Pair>();

            var roots = _rootFinder.FindRoots(_polynomial, 2);

            for (int index = 0; index < roots.Count; index++)
            {
                result.Add(new Pair(roots[index], 2));
            }


            for (int i = 0; i < primes.Length; i++)
            {
                Console.Write("\r {0}/{1}",i,primes.Length);
                roots = _rootFinder.FindRoots(_polynomial, primes[i]);

                for (int j = 0; j < roots.Count; j++)
                {
                    result.Add(new Pair(roots[j], primes[i]));
                }
            }
            return new AlgebraicFactorbase(result);

        }
    }
}
