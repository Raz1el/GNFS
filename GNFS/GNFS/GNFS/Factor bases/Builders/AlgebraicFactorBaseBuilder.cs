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
            var result = new List<Pair>();
            long interval = 10000000;

            if (_primeBound < interval)
                interval = _primeBound;

            var roots = _rootFinder.FindRoots(_polynomial, 2);

            for (int index = 0; index < roots.Count; index++)
            {
                result.Add(new Pair(roots[index], 2));
            }




            for (long k = 3; k < _primeBound; k += interval)
            {
                var primes = sieve.GetPrimes(k, k+interval);

              

           

                for (int i = 0; i < primes.Length; i++)
                {
                    Console.Write("\r k="+k+"; i="+i+"; p="+primes[i]+"                                 ");
                    roots = _rootFinder.FindRoots(_polynomial, primes[i]);
                    for (int j = 0; j < roots.Count; j++)
                    {
                        result.Add(new Pair(roots[j], primes[i]));
                    }
                }
            }

            return new AlgebraicFactorbase(result);


















         

        }
    }
}
