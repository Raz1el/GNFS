using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using GNFS.GNFS.Factor_bases;

namespace GNFS.GNFS.Sieve
{
    public class LogSieve : ISievingAlgorithm
    {
        public List<Pair> Sieve(long pairsCount, SieveOptions options)
        {
            var result = new List<Pair>();
            var intervalLength = (int)(options.UpperBound - options.LowerBound);
            var badRel = 0;
            for (int b = 1;true; b++)
            {
                var norm = new FirstDegreeElementsNormCalculator(options.Polynomial, b);

                var rationalSmoothBound =  BigInteger.Log((intervalLength / 2 + b * options.IntegerRoot));
                var algebraicSmoothBound = 0.7*BigInteger.Log(BigInteger.Abs(norm.CalculateNorm(options.LowerBound)));
                var timer=new Stopwatch();
                Console.Write("\r {0}                  |", b);
                Console.Write("\r {0}/{1}   [b={2}]", result.Count, pairsCount, b);
                var bad = 0;
                var smoothTester = new SmoothTester();
                var tmp = new List<Pair>();
                var elements = new double[intervalLength];
             //   double algebraicFudge, rationalFudge;
                timer.Start();
              //  InitSieve(elements, b, options, out algebraicFudge, out rationalFudge);
                timer.Stop();
                //Console.WriteLine();
                //Console.WriteLine("INIT:" + timer.Elapsed);
                //timer.Reset();
                //timer.Start();
                RationalSieve(elements, b, options);
                //timer.Stop();
                //Console.WriteLine("Rational:" + timer.Elapsed);
                //timer.Reset();
                //timer.Start();
                AlgebraicSieve(elements, b, options);
                //timer.Stop();
                //Console.WriteLine("Algebraic:" + timer.Elapsed);
                //timer.Reset();
                //timer.Start();
                
                for (int i = 0; i < intervalLength; i++)
                {
                    if (elements[i]>=algebraicSmoothBound+rationalSmoothBound)
                    {
                        var firstComponent = options.LowerBound + i;
                        var secondComponent = b;
                        if (smoothTester.IsSmoothOverRationalFactorbase(firstComponent, secondComponent, options.IntegerRoot, options.RationalFactorbase))
                        {

                            if (smoothTester.IsSmoothOverAlgebraicFactorbase(firstComponent, secondComponent,
                                options.Polynomial, options.AlgebraicFactorbase))
                            {
                                tmp.Add(new Pair(firstComponent, secondComponent));
                               
                            }

                        }
                        else
                        {
                            bad++;
                        }
                    }
                }
                lock (result)
                {
                    badRel += bad;
                    result.AddRange(tmp);
                    if (result.Count >= pairsCount)
                    {
                        break;
                    }
                }
               // Console.WriteLine("Other: "+timer.Elapsed);
               // Console.ReadKey();
            }
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine(badRel + " псевдогладких чисел");
            return result;

        }


     



        public List<Pair> ParallelSieve(long pairsCount, SieveOptions options)
        {
            var result = new List<Pair>();
            var intervalLength = (int)(options.UpperBound - options.LowerBound);

            Parallel.For(1, intervalLength, (b, state) =>
            {
              

                var smoothTester = new SmoothTester();
                var tmp = new List<Pair>();
                var elements = new double[intervalLength];


                var norm = new FirstDegreeElementsNormCalculator(options.Polynomial, b);

                var rationalSmoothBound = BigInteger.Log((intervalLength / 2 + b * options.IntegerRoot));
                var algebraicSmoothBound = 0.7 * BigInteger.Log(BigInteger.Abs(norm.CalculateNorm(options.LowerBound)));
                var smoothBound = rationalSmoothBound + algebraicSmoothBound;

                RationalSieve(elements, b, options);
                AlgebraicSieve(elements, b, options);
                for (int i = 0; i < intervalLength; i++)
                {
                   if(elements[i]> smoothBound)
                    {
                        var firstComponent = options.LowerBound + i;
                        var secondComponent = b;
                        if (smoothTester.IsSmoothOverRationalFactorbase(firstComponent, secondComponent, options.IntegerRoot, options.RationalFactorbase))
                        {

                            if (smoothTester.IsSmoothOverAlgebraicFactorbase(firstComponent, secondComponent,
                                options.Polynomial, options.AlgebraicFactorbase))
                            {
                                tmp.Add(new Pair(firstComponent, secondComponent));
                                Console.Write("\r {0}/{1}                         ", result.Count, pairsCount);
                            }

                        }
                    }
                }


                lock (result)
                {
                    result.AddRange(tmp);
                    Console.Write("\r {0}/{1}                         ", result.Count, pairsCount);
                    if (result.Count >= pairsCount)
                    {
                        state.Stop();
                    }
                }


            });
            return result;

        }

        void InitSieve(double[] elements, BigInteger b, SieveOptions options, out double algebraicFudge, out double rationalFudge)
        {
            var a = options.LowerBound;
            var normCalculator = new FirstDegreeElementsNormCalculator(options.Polynomial, b);
            var log = BigInteger.Log(BigInteger.Abs(b * options.IntegerRoot));
            rationalFudge = 0.3 * log;
            algebraicFudge = 0;
            for (var i = 0; i < elements.Length; i++)
            {
                var normLog = BigInteger.Log(BigInteger.Abs(normCalculator.CalculateNorm(a)));
                elements[i] = -normLog - log; ;
                a++;
                if (normLog > algebraicFudge)
                    algebraicFudge = normLog;
            }
            algebraicFudge *= 0.2;
        }

        void RationalSieve(double[] elements, BigInteger b, SieveOptions options)
        {
            for (int i = 0; i < options.RationalFactorbase.Elements.Count; i++)
            {
                var currentPair = options.RationalFactorbase.Elements[i];
                var startPoint = (currentPair.Item2 - (b * currentPair.Item1) % currentPair.Item2) % currentPair.Item2 -
                    (options.LowerBound % currentPair.Item2 + currentPair.Item2) % currentPair.Item2;
                startPoint = startPoint + currentPair.Item2;
                startPoint %= currentPair.Item2;
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
                var startPoint = (currentPair.Item2 - (b * currentPair.Item1) % currentPair.Item2) % currentPair.Item2 -
                    (options.LowerBound % currentPair.Item2 + currentPair.Item2) % currentPair.Item2;
                startPoint = startPoint + currentPair.Item2;
                startPoint %= currentPair.Item2;
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
