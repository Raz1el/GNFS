using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using GNFS.GNFS.Factor_bases;

namespace GNFS.GNFS.Sieve
{
    public class SimpleSieveAlgorithm : ISievingAlgorithm
    {
        public List<Pair> Sieve(long pairsCount,SieveOptions options)
        {
            var result=new List<Pair>();
            var intervalLength =(long) (options.UpperBound - options.LowerBound);
            for (BigInteger b = 1;b<100; b++)
            {
                var timer=new Stopwatch();
                Console.Write("\r {0}                  |", b);
                Console.Write("\r {0}/{1}   [b={2}]",result.Count,pairsCount,b);
                var rationalElements = new BigInteger[intervalLength];
                var norms=new BigInteger[intervalLength];
                timer.Start();
                InitSieve(rationalElements,norms,b,options);
                Console.WriteLine("\nINIT: "+timer.Elapsed);
                timer.Reset();
                timer.Start();
                RationalSieve(rationalElements,b,options);
                Console.WriteLine("Rational: " + timer.Elapsed);
                timer.Reset();
                timer.Start();
                AlgebraicSieve(norms,b,options);
                Console.WriteLine("Algebraic: " + timer.Elapsed);
                timer.Reset();
                timer.Start();
                for (long i = 0; i < intervalLength; i++)
                {

                    if (BigInteger.Abs(rationalElements[i]) == 1 && (BigInteger.Abs(norms[i]) == 1))
                    {
                        var firstComponent = options.LowerBound + i;
                        var secondComponent = b;
                        var d = BigInteger.GreatestCommonDivisor(firstComponent, secondComponent);
                        if (d == 1)
                        {
                            result.Add(new Pair(firstComponent, secondComponent));
                        }
                        if (result.Count == pairsCount)
                        {
                            Console.Write("\r {0}/{1}", result.Count, pairsCount);
                            return result;
                        }
                    }
                }
                Console.WriteLine("Other: " + timer.Elapsed);
              //  Console.ReadKey();
            }
            return result;
        }

        void InitSieve(BigInteger[] rationalElements, BigInteger[] norms, BigInteger b,SieveOptions options)
        {
            var a = options.LowerBound;
            var normCalculator=new FirstDegreeElementsNormCalculator(options.Polynomial,b);
            
            for (var i = 0; i < rationalElements.Length; i++)
            {
               
                rationalElements[i] = a+b*options.IntegerRoot;
                norms[i] = normCalculator.CalculateNorm(a);
                a++;
            }
        }

        void RationalSieve(BigInteger[] rationalElements, BigInteger b,SieveOptions options)
        {
            for (int i = 0; i < options.RationalFactorbase.Elements.Count; i++)
            {
                var currentPair = options.RationalFactorbase.Elements[i];
                var startPoint = (currentPair.Item2-(b*currentPair.Item1)%currentPair.Item2)%currentPair.Item2-
                    (options.LowerBound % currentPair.Item2 + currentPair.Item2) % currentPair.Item2;
                startPoint =startPoint+ currentPair.Item2;
                startPoint %= currentPair.Item2;
                var prime = (long) currentPair.Item2;
                for (long j = (long)startPoint; j <options.IntervalLength; j+= prime)
                {
                    while (rationalElements[j] % prime == 0&& rationalElements[j]!=0)
                    {
                        rationalElements[j] /= prime;
                    }
                }

            }
        }
        void AlgebraicSieve(BigInteger[] algebraicElements, BigInteger b, SieveOptions options)
        {
            for (int i = 0; i < options.AlgebraicFactorbase.Elements.Count; i++)
            {
                var currentPair = options.AlgebraicFactorbase.Elements[i];
                var startPoint = (currentPair.Item2 - (b * currentPair.Item1) % currentPair.Item2) % currentPair.Item2 -
                    (options.LowerBound % currentPair.Item2 + currentPair.Item2) % currentPair.Item2;
                startPoint = startPoint + currentPair.Item2;
                startPoint %= currentPair.Item2;
                var prime = (long)currentPair.Item2;
                for (long j = (long)startPoint; j < options.IntervalLength; j += prime)
                {
              
                    while (algebraicElements[j] % prime == 0&& algebraicElements[j] != 0)
                    {
                        algebraicElements[j] /= prime;
                    }

                }

            }
        }

    }
}
