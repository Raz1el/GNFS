using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GNFS.Integer_arithmetic;
using GNFS.Polynomial_arithmetic;
using GNFS.Polynomial_arithmetic.GaloisFieldLib;

namespace GNFS.GNFS.Factor_bases.Builders
{
    public class QuadraticCharactersBuilder
    {
        Polynomial _polynomial;
        IRootFinder _rootFinder;
        ulong _lowerBound;
        ulong _upperBound;


        public QuadraticCharactersBuilder(Polynomial polynomial, IRootFinder rootFinder, ulong lowerBound,ulong upperBound)
        {
            _lowerBound = lowerBound;
            _upperBound = upperBound;
            _rootFinder = rootFinder;
            _polynomial = polynomial;
        }

        public QuadraticCharacters Build()
        {
            var sieve = new EratosthenesSieve();
            var primes = sieve.GetPrimes(_lowerBound, _upperBound);
            var derivative=new PolynomialDerivative().Derivative(_polynomial);
            var result = new List<Pair>();

            List<ulong> roots;

            for (int i = 0; i < primes.Length; i++)
            {
                roots = _rootFinder.FindRoots(_polynomial, primes[i]);

                for (int j = 0; j < roots.Count; j++)
                {
                    if (derivative.Value(roots[j])%primes[i] != 0)
                    {
                        result.Add(new Pair(roots[j], primes[i]));
                    }
                }
            }
            return new QuadraticCharacters(result);
        }
    }
}
