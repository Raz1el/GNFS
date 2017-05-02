using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using GNFS.GNFS.Factor_bases.Builders;
using GNFS.GNFS.Polynomial_generator;
using GNFS.GNFS.Sieve;
using GNFS.GNFS.Square_root;
using GNFS.Integer_arithmetic;
using GNFS.Linear_algebra;
using GNFS.Polynomial_arithmetic;

namespace GNFS.GNFS
{
    public class Gnfs
    {
        BigInteger _number;
        PolynomialInfo _polyInfo;
        IPolynomialGenerator _generator;
        int _kerDim;
        long _rationalPrimeBound;
        long _algebraicPrimeBound;
        long _sieveSize;
        long _quadraticCharFbSize;

        public Gnfs(BigInteger number, IPolynomialGenerator generator)
        {
            _number = number;
            _generator = generator;

            var exp = Math.Pow(8 / 9.0, 1 / 3.0) * Math.Pow(BigInteger.Log(number), 1 / 3.0) * Math.Pow(Math.Log(BigInteger.Log(number)), 2 / 3.0);


            _rationalPrimeBound = Convert.ToInt64(Math.Exp(exp) / 2);
            _algebraicPrimeBound = _rationalPrimeBound;
            _kerDim = 10;
            _sieveSize = 5000000;
            _quadraticCharFbSize = Convert.ToInt64(3*BigInteger.Log10(number));
        }


        public Gnfs(BigInteger number, IPolynomialGenerator generator,long rfb,long afb,long sieveSize)
        {
            _number = number;
            _generator = generator;

            _rationalPrimeBound = rfb;
            _algebraicPrimeBound = afb;
            _kerDim = 10;
            _sieveSize = sieveSize;
            _quadraticCharFbSize = Convert.ToInt64(3 * BigInteger.Log10(number));
        }



        public BigInteger FindFactor()
        {

            var timer = new Stopwatch();
            timer.Start();
            _polyInfo = _generator.GeneratePolynomial();
            Console.WriteLine(_number);
            Console.WriteLine("f(x)=" + _polyInfo.Polynomial);
            Console.WriteLine("Root=" + _polyInfo.Root);
            Console.WriteLine("RB=" + _rationalPrimeBound);
            Console.WriteLine("AB=" + _algebraicPrimeBound);

            var rootFinder = new GcdRootFinder();
            var algFbBuilder = new AlgebraicFactorbaseBuilder(_polyInfo.Polynomial, rootFinder, _algebraicPrimeBound);
            var rationalFbBuilder = new RationalFactorbaseBuilder(_polyInfo.Root, _rationalPrimeBound);
            var quadraticCharFbBuilder = new QuadraticCharactersBuilder(_polyInfo.Polynomial, rootFinder, _algebraicPrimeBound, _quadraticCharFbSize);

            var rationalFb = rationalFbBuilder.Build();

            var quadraticCharFb = quadraticCharFbBuilder.Build();
            var algFb = algFbBuilder.Build();
            var sieve = new LogSieve();
            Console.WriteLine("\nRational factorbase size: " + rationalFb.Elements.Count);
            Console.WriteLine("Algebraic factorbase size: " + algFb.Elements.Count);
            Console.WriteLine("Quadratic characters factorbase size: " + quadraticCharFb.Elements.Count);
            Console.WriteLine("Init time: " + timer.Elapsed);
            timer.Reset();
            timer.Start();
            var pairs =
                sieve.Sieve(
                    (algFb.Elements.Count + rationalFb.Elements.Count + quadraticCharFb.Elements.Count + _kerDim + 1),
                    new SieveOptions(_sieveSize, -_sieveSize, algFb, rationalFb, _polyInfo.Polynomial, _polyInfo.Root));

            Console.WriteLine();
            Console.WriteLine(pairs.Count + " relations collected.");
            Console.WriteLine("Sieve time: " + timer.Elapsed);
            timer.Reset();
            timer.Start();
            var matrixBuilder = new MatrixBuilder();
            var matrix = matrixBuilder.Build(pairs, rationalFb, algFb, quadraticCharFb, _polyInfo.Root, _polyInfo.Polynomial);
            var matrixSolver=new GaussianEliminationOverGf2();


            Console.WriteLine("{0}x{1} matrix builded. ", matrix.RowsCount, matrix.ColumnsCount);
            var solutions = matrixSolver.Solve(matrix);

            Console.WriteLine();
            Console.WriteLine("Linear algebra time: " + timer.Elapsed);
            Console.WriteLine("{0} solution computed. ", solutions.Count);
            timer.Reset();
            timer.Start();


            var polyMath = new PolynomialMath(-1);
            var df = new PolynomialDerivative().Derivative(_polyInfo.Polynomial);
            var sqrDf = polyMath.Mul(df, df);

            var solutionsCheked = -1;
            foreach (var solution in solutions)
            {

                timer.Reset();
                timer.Start();
                var sqr = new Polynomial(new BigInteger[] { 1 });
                BigInteger sqrtNorm = 1;
                BigInteger x = 1;

                for (int i = 0; i < solution.Length; i++)
                {
                    if (solution[i] == 1)
                    {

                        var tmp = new Polynomial(new BigInteger[] { pairs[i].Item1, pairs[i].Item2 });
                        x *= pairs[i].Item1 + _polyInfo.Root * pairs[i].Item2;
                        sqr = polyMath.Rem(polyMath.Mul(tmp, sqr), _polyInfo.Polynomial);
                        var normCalculator = new FirstDegreeElementsNormCalculator(_polyInfo.Polynomial, pairs[i].Item2);
                        sqrtNorm *= normCalculator.CalculateNorm(pairs[i].Item1);

                    }

                }
                x *= sqrDf.Value(_polyInfo.Root);
                if (x < 0)
                {
                    throw new Exception();
                }
                sqr = polyMath.Rem(polyMath.Mul(sqrDf, sqr), _polyInfo.Polynomial);
                var integerSqrt = new IntegerSquareRoot();
                var sqrtX = integerSqrt.Sqrt(x);
                if (sqrtX * sqrtX != x)
                {
                    if (sqrtX * sqrtX != x)
                    {
                        if (sqrtX * sqrtX != x)
                        {
                            throw new Exception();
                        }
                    }
                }


                var algSqrt = new AlgebraicSqrt();


                sqrtNorm = BigInteger.Abs(sqrtNorm);
                var tmpNorm = sqrtNorm;
                sqrtNorm = integerSqrt.Sqrt(BigInteger.Abs(sqrtNorm));
                timer.Stop();
                Console.WriteLine(timer.Elapsed + " Умножение");



                if (sqrtNorm * sqrtNorm != tmpNorm)
                {
                    if (sqrtNorm * sqrtNorm != tmpNorm)
                    {
                        if (sqrtNorm * sqrtNorm != tmpNorm)
                        {
                            throw new Exception();
                        }
                    }
                }
                timer.Reset();
                timer.Start();
                var sqrt = algSqrt.Sqrt(sqr, _polyInfo.Polynomial, df, sqrtNorm);
                timer.Stop();
                Console.WriteLine(timer.Elapsed + " Квадратный корень");
                if (algSqrt.DontExist)
                {
                    Console.Write("\r{0}/{1} solutions cheked. (BAD SQRT)    ", ++solutionsCheked, solutions.Count);
                    continue;
                }
                var sqrtY = sqrt.Value(_polyInfo.Root);

                var check = polyMath.Rem(polyMath.Mul(sqrt, sqrt), _polyInfo.Polynomial);
                if (check != sqr)
                {
                    var primes = new EratosthenesSieve().GetPrimes(5, 10000);
                    int i;
                    for (i = 0; i < primes.Length; i++)
                    {
                        if (!algSqrt.IsSqr(sqr, _polyInfo.Polynomial, primes[i]))
                        {
                            break;
                        }
                    }
                    if (i == primes.Length)
                    {
                        throw new Exception();
                    }
                    Console.Write("\r{0}/{1} solutions cheked. (BAD SQRT)    ", ++solutionsCheked, solutions.Count);
                    continue;
                }
                if ((sqrtY * sqrtY - sqrtX * sqrtX) % _number != 0)
                {
                    throw new Exception();
                }
                var factor = BigInteger.GreatestCommonDivisor(sqrtX - sqrtY, _number);
                if (factor > 1 && factor < _number)
                {
                    Console.Write("\r{0}/{1} solutions cheked                ", ++solutionsCheked, solutions.Count);
                    return factor;
                }
                Console.Write("\r{0}/{1} solutions cheked. (BAD SOLUTION)", ++solutionsCheked, solutions.Count);
            }
            Console.Write("\r{0}/{1} solutions cheked.                ", solutionsCheked, solutions.Count);
            return 1;
        }
    }
}
