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
    public class LogSieve:ISievingAlgorithm
    {
        public List<Pair> Sieve(long pairsCount, SieveOptions options)
        {
            var result = new List<Pair>();
            var intervalLength = (int)(options.UpperBound - options.LowerBound);
            var fudge = 0.7;
            var smoothTester = new SmoothTester();


            for (int b = 1; true; b++)
            {
                var norm = new FirstDegreeElementsNormCalculator(options.Polynomial, b);
                var rationalSmoothBound =fudge* BigInteger.Log(b * options.IntegerRoot);



                var rationalElements = new double[intervalLength];
                var algebraicElements = new double[intervalLength];

                  
                RationalSieve(rationalElements, b, options);
                AlgebraicSieve(algebraicElements, b, options);
                if (b%2 == 0)
                {
                    for (int i = (options.LowerBound%2==0?1:0); i < intervalLength; i+=2)
                    {
                        if (rationalElements[i] >= rationalSmoothBound)
                        {
                            var firstComponent = options.LowerBound + i;
                            var secondComponent = b;
                            var algebraicSmoothBound = fudge *
                                                       BigInteger.Log(BigInteger.Abs(norm.CalculateNorm(firstComponent)));
                            if (algebraicElements[i] >= algebraicSmoothBound)
                            {
                                if (BigInteger.GreatestCommonDivisor(firstComponent, secondComponent) != 1)
                                    continue;

                                if (smoothTester.IsSmoothOverRationalFactorbase(firstComponent, secondComponent,
                                    options.IntegerRoot, options.RationalFactorbase))
                                {
                                    if (smoothTester.IsSmoothOverAlgebraicFactorbase(firstComponent, secondComponent,
                                        options.Polynomial, options.AlgebraicFactorbase))
                                    {

                                        result.Add(new Pair(firstComponent, secondComponent));

                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < intervalLength; i++)
                    {
                        if (rationalElements[i] >= rationalSmoothBound)
                        {
                            var firstComponent = options.LowerBound + i;
                            var secondComponent = b;
                            var algebraicSmoothBound = fudge *
                                                       BigInteger.Log(BigInteger.Abs(norm.CalculateNorm(firstComponent)));
                            if (algebraicElements[i] >= algebraicSmoothBound)
                            {
                                if (BigInteger.GreatestCommonDivisor(firstComponent, secondComponent) != 1)
                                    continue;

                                if (smoothTester.IsSmoothOverRationalFactorbase(firstComponent, secondComponent,
                                    options.IntegerRoot, options.RationalFactorbase))
                                {
                                    if (smoothTester.IsSmoothOverAlgebraicFactorbase(firstComponent, secondComponent,
                                        options.Polynomial, options.AlgebraicFactorbase))
                                    {

                                        result.Add(new Pair(firstComponent, secondComponent));

                                    }
                                }
                            }
                        }
                    }
                }
              
                Console.Write("\r{0}/{1}   [b={2}]                        ",result.Count, pairsCount, b);
                if (result.Count >= pairsCount)
                {
                    break;
                }
            }
            Console.WriteLine();
            Console.WriteLine();
            return result;
        }



        void RationalSieve(double[] elements, BigInteger b, SieveOptions options)
        {
            for (int i = 0; i < options.RationalFactorbase.Elements.Count; i++)
            {
                var currentPair = options.RationalFactorbase.Elements[i];
                var startPoint = (options.LowerBound + b * currentPair.Item1) % currentPair.Item2;
                if (startPoint < 0)
                {
                    startPoint = -startPoint;
                }
                else
                {
                    startPoint = currentPair.Item2 - startPoint;
                }
                var log = BigInteger.Log(currentPair.Item2);
                var prime = (long)currentPair.Item2;
                for (long j = (long)startPoint; j < options.IntervalLength; j += prime)
                {
                    elements[j] += log;
                }

            }
        }
        void AlgebraicSieve(double[] elements, BigInteger b, SieveOptions options)
        {
            for (int i = 0; i < options.AlgebraicFactorbase.Elements.Count; i++)
            {
                var currentPair = options.AlgebraicFactorbase.Elements[i];
                var startPoint = (options.LowerBound + b * currentPair.Item1) % currentPair.Item2;
                if (startPoint <= 0)
                {
                    startPoint = -startPoint;
                }
                else
                {
                    startPoint = currentPair.Item2 - startPoint;
                }
                var log = BigInteger.Log(currentPair.Item2);
                var prime = (long)currentPair.Item2;
                for (long j = (long)startPoint; j < options.IntervalLength; j += prime)
                {
                    elements[j] += log;
                }

            }
        }
    }
}
