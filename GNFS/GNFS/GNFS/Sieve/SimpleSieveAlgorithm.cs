using System;
using System.Collections.Generic;
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
        public List<Pair> Sieve(ulong pairsCount,SieveOptions options)
        {
            var result=new List<Pair>();
            var intervalLength =(ulong) (options.UpperBound - options.LowerBound);
            for (BigInteger b = 1;; b++)
            {
                var rationalElements = new BigInteger[intervalLength];
                var norms=new BigInteger[intervalLength];
                InitSieve(rationalElements,norms,b,options);
                RationalSieve(rationalElements,b,options);
                AlgebraicSieve(norms,b,options);
                for (ulong i = 0; i < intervalLength; i++)
                {
                 
                    if (BigInteger.Abs(rationalElements[i]) == 1&& (BigInteger.Abs(norms[i]) == 1))
                    {
                        var firstComponent = options.LowerBound + i;
                        var secondComponent = b;
                        var d = BigInteger.GreatestCommonDivisor(firstComponent,secondComponent);
                        if (d == 1)
                        {
                            result.Add(new Pair(firstComponent, secondComponent));
                        }
                        if (d == -1)
                        {
                            throw new Exception();
                        }
                        if ((ulong)result.Count == pairsCount)
                        {
                            return result;
                        }
                    }
                }
            }
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
                var log = BigInteger.Log(currentPair.Item2);
                var prime = (ulong) currentPair.Item2;
                for (ulong j = (ulong)startPoint; j <options.IntervalLength; j+= prime)
                {
                    if (rationalElements[j]%prime!=0)
                        throw new Exception();
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
                var log = BigInteger.Log(currentPair.Item2);
                var prime = (ulong)currentPair.Item2;
                for (ulong j = (ulong)startPoint; j < options.IntervalLength; j += prime)
                {
                    if (algebraicElements[j] % prime != 0)
                        throw new Exception();
                    while (algebraicElements[j] % prime == 0&& algebraicElements[j] != 0)
                    {
                        algebraicElements[j] /= prime;
                    }

                }

            }
        }

    }
}
