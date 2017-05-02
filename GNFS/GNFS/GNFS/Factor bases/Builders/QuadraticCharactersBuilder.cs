using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using GNFS.Integer_arithmetic;
using GNFS.Polynomial_arithmetic;

namespace GNFS.GNFS.Factor_bases.Builders
{
    public class QuadraticCharactersBuilder
    {
        Polynomial _polynomial;
        IRootFinder _rootFinder;
        long _lowerBound;
        long _size;


        public QuadraticCharactersBuilder(Polynomial polynomial, IRootFinder rootFinder, long lowerBound, long size)
        {
            _lowerBound = lowerBound;
            _size = size;
            _rootFinder = rootFinder;
            _polynomial = polynomial;
        }

        public QuadraticCharacters Build()
        {
            var sieve = new EratosthenesSieve();
            var primes = sieve.GetPrimes(_lowerBound+1,_lowerBound+ 100*_size);
            var derivative=new PolynomialDerivative().Derivative(_polynomial);
            var result = new List<Pair>();

            List<long> roots;

            for (int i = 0; i < primes.Length; i++)
            {
                roots = _rootFinder.FindRoots(_polynomial, primes[i]);

                for (int j = 0; j < roots.Count; j++)
                {
                    if (derivative.Value(roots[j])%primes[i] != 0)
                    {
                        result.Add(new Pair(roots[j], primes[i]));
                    }
                    if(result.Count==_size)
                        return new QuadraticCharacters(result);
                }
            }
            return new QuadraticCharacters(result);
        }
    }
}
