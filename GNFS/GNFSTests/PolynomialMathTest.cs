using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using GNFS.Integer_arithmetic;
using GNFS.Polynomial_arithmetic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GNFSTests
{
    [TestClass]
    public class PolynomialMathTest
    {
        long[] _primes;
        Random _generator;
        public PolynomialMathTest()
        {
            var sieve=new EratosthenesSieve();
            _primes = sieve.GetPrimes(2, 1000);
            _generator=new Random();
        }
        [TestMethod]
        public void AddTest()
        {
            foreach (var prime in _primes)
            {
                var firstPoly = GeneratePoly(20, prime);
                var secondPoly = GeneratePoly(30, prime);


                var polyMath = new PolynomialMath(prime);

                Polynomial res;
                res = polyMath.Add(firstPoly, secondPoly);

                var checkPoly = polyMath.Sub(res, secondPoly);

                for (int i = 0; i <= firstPoly.Deg; i++)
                {
                    if ((firstPoly[i] + prime*prime)%prime != (checkPoly[i] + prime*prime)%prime)
                    {
                        throw new Exception();
                    }
                }

            }
        }
        [TestMethod]
        public void DivTest()
        {
            foreach (var prime in _primes)
            {
                var firstPoly = GeneratePoly(40, prime);
                var secondPoly = GeneratePoly(30, prime);

                var polyMath = new PolynomialMath(prime);


                while (secondPoly.Deg == -1)
                {
                    secondPoly = GeneratePoly(30, prime);
                }

                var res = polyMath.Div(firstPoly, secondPoly);
                var rem = polyMath.Rem(firstPoly, secondPoly);


                var checkPoly = polyMath.Add(rem, polyMath.Mul(secondPoly, res));
                for (int i = 0; i <= firstPoly.Deg; i++)
                {
                    if ((firstPoly[i] + prime * prime) % prime != (checkPoly[i] + prime * prime) % prime)
                    {
                        throw new Exception();
                    }
                }


            }
        }

        [TestMethod]
        public void MulTest()
        {
            foreach (var prime in _primes)
            {
                var firstPoly = GeneratePoly(20, prime);
                var secondPoly = GeneratePoly(30, prime);

                while (firstPoly.Deg == -1)
                {
                    firstPoly = GeneratePoly(20, prime);
                }


                while (secondPoly.Deg == -1)
                {
                    secondPoly = GeneratePoly(30, prime);
                }

                var polyMath = new PolynomialMath(prime);
                Polynomial res;
                res = polyMath.Mul(firstPoly, secondPoly);








                var checkPoly = polyMath.Div(res, secondPoly);
                for (int i = 0; i <= firstPoly.Deg; i++)
                {
                    if ((firstPoly[i] + prime * prime) % prime != (checkPoly[i] + prime * prime) % prime)
                    {
                        throw new Exception();
                    }
                }

            }
        }
        [TestMethod]
        public void SubTest()
        {
            foreach (var prime in _primes)
            {
                var firstPoly = GeneratePoly(20, prime);
                var secondPoly = GeneratePoly(30, prime);


                var polyMath = new PolynomialMath(prime);

                Polynomial res;
                res = polyMath.Sub(firstPoly, secondPoly);

                var checkPoly = polyMath.Add(res, secondPoly);

                for (int i = 0; i <= firstPoly.Deg; i++)
                {
                    if ((firstPoly[i] + prime * prime) % prime != (checkPoly[i] + prime * prime) % prime)
                    {
                        throw new Exception();
                    }
                }

            }
        }


        Polynomial GeneratePoly(int n, BigInteger mod)
        {

            var arr = new BigInteger[n];
            for (int i = 0; i < n; i++)
            {
                arr[i] = _generator.Next();
            }

            return new Polynomial(arr, mod);

        }
    }
}
