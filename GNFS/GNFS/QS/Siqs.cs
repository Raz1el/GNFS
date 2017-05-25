using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using GNFS.GNFS.Factor_bases;
using GNFS.Integer_arithmetic;
using GNFS.Linear_algebra;

namespace GNFS.QS
{
    public class Siqs
    {
        int _sieveBound;
        long _primeBound;
        int _polyIndex;
        long[] _factorBase;
        double[] _logp;
        Dictionary<long,long> _sqrtN;
        Dictionary<BigInteger, int> _partial; 
        double _thresh;
        int _kerSize ;
        bool _autoInit;




        public Siqs(int sieveBound,long primeBound,int kerSize)
        {
            _sieveBound = sieveBound;
            _primeBound = primeBound;
            _kerSize = kerSize;
            _autoInit = false;
        }
        public Siqs()
        {
            _autoInit = true;
        }

        public BigInteger FindFactor(BigInteger n)
        {
           Console.WriteLine(n);

            Init(n);


            var smoothPairs=new List<SmoothPair>();



            var timer=new Stopwatch();
            timer.Start();
            var polyCount = 0;
            while (smoothPairs.Count < _factorBase.Length+_kerSize)
            {
                var polynomials = GeneratePolynomials(n);
                if (polynomials == null)
                {
                    return 1;
                }
                foreach (var sievePolynomial in polynomials)
                {
                    smoothPairs.AddRange(LogSieve(sievePolynomial,n));
                    polyCount++;
                    Console.Write("\r"+smoothPairs.Count + "/" + _factorBase.Length+" [polynomials used:"+polyCount+"]                               ");
                    if (smoothPairs.Count >= _factorBase.Length + _kerSize)
                        break;

                }

            }
            if (smoothPairs.Count < _factorBase.Length+_kerSize)
                return 1;
            timer.Stop();
            Console.WriteLine();
            Console.WriteLine("sieve time: "+timer.Elapsed);

           timer.Restart();
            var matrix = BuildMatrix(smoothPairs);
            var matrixSolver = new GaussianEliminationOverGf2();
            var solutions = matrixSolver.Solve(matrix);
            Console.WriteLine("linear algebra time: "+timer.Elapsed);
            foreach (var solution in solutions)
            {
            
                var x = BigInteger.One;
                var y = new Dictionary<long,int>();
                y.Add(-1,0);
                for (int i = 0; i < _factorBase.Length; i++)
                {
                    y.Add(_factorBase[i],0);
                }


                for (int i = 0; i < solution.Length; i++)
                {
                    if (solution[i] == 1)
                    {
                        x = x * smoothPairs[i].X;

                        y[-1]+= smoothPairs[i].Y[-1];
                        for (int j = 0; j < _factorBase.Length; j++)
                        {
                            if (smoothPairs[i].Y.ContainsKey(_factorBase[j]))
                            {
                                y[_factorBase[j]] += smoothPairs[i].Y[_factorBase[j]];
                            }
                           
                        }
                       

                    }
                }
    



                if (y[-1]%2!=0)
                    throw new Exception();

                for (int j = 0; j < _factorBase.Length; j++)
                {
                    if (y[_factorBase[j]] % 2 != 0)
                        throw new Exception();
                    y[_factorBase[j]] /= 2;
                }

                BigInteger sqrtY = 1;

                foreach (var pair in y)
                {
                    sqrtY *= BigInteger.Pow(pair.Key, pair.Value);
                }

            



                var factor = BigInteger.GreatestCommonDivisor(sqrtY - x, n);


                if (factor < n && factor > 1)
                {
              
                    return factor;
                }




            }



            return 1;
        }

        private void Init(BigInteger n)
        {
            if (_autoInit)
            {
                _primeBound = (long)(12 * BigInteger.Log10(n) * BigInteger.Log10(n));
                _factorBase = BuildFactorBase(n);
                _sieveBound = (_factorBase.Length) * 60;
                _kerSize = 10;
            }
            else
            {
                _factorBase = BuildFactorBase(n);
            }
            _logp = _factorBase.Select(x => Math.Log10(x)).ToArray();
            var sqrt=new IntegerSquareRoot();
            _thresh =BigInteger.Log10((_sieveBound * sqrt.Sqrt(2*n)) >> 1)* 0.735;
            _partial=new Dictionary<BigInteger, int>();
            Console.WriteLine("Prime bound:"+_primeBound);
            Console.WriteLine("Sieve size:" + _sieveBound);
            Console.WriteLine("Factorbase size:" + _factorBase.Length);
        }

        private List<SmoothPair> LogSieve(SievePolynomial sievePolynomial,BigInteger n)
        {
            var result=new List<SmoothPair>();
            double[] sieveArray=new double[2*_sieveBound];
            var i = 0;
            for (; i < _factorBase.Length; i++)
            {
                var p = _factorBase[i];
                if (sievePolynomial.A%p == 0)
                    continue;


                var logp = _logp[i];


                var root1 = sievePolynomial.FirstRoot[p];
                var root2 = sievePolynomial.SecondRoot[p];

                if (root1 < 0)
                {
                    root1 += p;
                }
                if (root2 < 0)
                {
                    root2 += p;
                }


                long x = 0;
                while (x < _sieveBound)
                {
                    if (x + root1 < _sieveBound)
                    {
                        sieveArray[x + root1] += logp;
                    }
                    if (x + root2 < _sieveBound)
                    {
                        sieveArray[x + root2] += logp;
                    }
                    if (x != 0)
                    {
                        sieveArray[x - root1 + _sieveBound] += logp;
                        sieveArray[x - root2 + _sieveBound] += logp;
                    }

                    x += p;
                }
            }

            i = 0;
            foreach (var value in sieveArray)
            {
                if (value > _thresh)
                {
                    BigInteger x = i > _sieveBound ? _sieveBound - i : i;


                 

                    var vector = new Dictionary<long, int>();

                    var t = sievePolynomial.A*x + sievePolynomial.B;
                    var polyValue = t*t-n;

                    if (polyValue < 0)
                    {
                        vector.Add(-1, 1);

                        polyValue = -polyValue;

                    }
                    else
                    {
                        vector.Add(-1, 0);
                    }

                    foreach (var p in _factorBase)
                    {
                        while (polyValue % p == 0)
                        {


                            if (vector.ContainsKey(p))
                            {
                                vector[p]++;
                            }
                            else
                            {
                                vector.Add(p, 1);
                            }
                            polyValue = (polyValue / p);
                        }
                    }

                    var xVal = sievePolynomial.A * x + sievePolynomial.B;
                    if (polyValue == 1)
                    {
                      


                     
                        result.Add(new SmoothPair(xVal, vector));

                    }
                }
                i++;
            }


            return result;

        }
        private List<SmoothPair> Sieve(SievePolynomial sievePolynomial, BigInteger n)
        {
            var result = new List<SmoothPair>();
            var sieveArray = new BigInteger[2 * _sieveBound];
            for (int j = 0; j < sieveArray.Length; j++)
            {
                var x = j > _sieveBound ? _sieveBound - j : j;
                var t = sievePolynomial.A * x*x + 2*sievePolynomial.B*x+sievePolynomial.C;
                sieveArray[j] = t;
            }
     


            var i = 0;
            foreach (var p in _factorBase)
            {
                //Need skip small primes
                if (p == 7621)
                {
                    i++;
                    i--;
                }

                if (sievePolynomial.A % p == 0)
                    continue;


                var logp = _logp[i];


                var root1 = sievePolynomial.FirstRoot[p];
                var root2 = sievePolynomial.SecondRoot[p];

                if (root1 < 0)
                {
                    root1 += p;
                }
                if (root2 < 0)
                {
                    root2 += p;
                }



                long x = 0;
                while (x < _sieveBound)
                {
                    
                    if (x + root1 < _sieveBound)
                    {
                        if (sieveArray[x + root1]%p!=0)
                            throw new Exception();



                        while (sieveArray[x + root1] % p == 0)
                        {
                            sieveArray[x + root1] /= p;
                        }
                      
                    }
                    if (x + root2 < _sieveBound&&root1!=root2)
                    {
                        if (sieveArray[x + root2] % p != 0)
                            throw new Exception();






                        while (sieveArray[x + root2] % p == 0)
                        {
                            sieveArray[x + root2] /= p;
                        }
        
                    }
                    if (x != 0)
                    {
                        if (sieveArray[x - root1 + _sieveBound] % p != 0|| sieveArray[x - root2 + _sieveBound] % p != 0)
                            throw new Exception();



                        while (sieveArray[x - root1 + _sieveBound] % p == 0)
                        {
                            sieveArray[x - root1 + _sieveBound] /= p;
                        }

                        while (sieveArray[x - root2 + _sieveBound] % p == 0&&root1!=root2)
                        {
                            sieveArray[x - root2 + _sieveBound] /= p;
                        }
                  
                    }

                    x += p;
                }
                i += 1;
            }

            i = 0;
            foreach (var value in sieveArray)
            {
            
                if (value ==1||value==-1)
                {
                    var x = i > _sieveBound ? _sieveBound - i : i;
                    var vector = new Dictionary<long, int>();

                    var t = sievePolynomial.A * x + sievePolynomial.B;
                    var polyValue = t * t - n;

                    if (polyValue < 0)
                    {
                        vector.Add(-1, 1);

                        polyValue = -polyValue;

                    }
                    else
                    {
                        vector.Add(-1, 0);
                    }

                    foreach (var p in _factorBase)
                    {
                        while (polyValue % p == 0)
                        {


                            if (vector.ContainsKey(p))
                            {
                                vector[p]++;
                            }
                            else
                            {
                                vector.Add(p, 1);
                            }
                            polyValue = (polyValue / p);
                        }
                    }

                    if (polyValue == 1)
                    {
                        var xVal = sievePolynomial.A * x + sievePolynomial.B;
                        result.Add(new SmoothPair(xVal, vector));

                    }
                }
                i++;
            }


            return result;

        }

        private List<SievePolynomial> GeneratePolynomials(BigInteger n)
        {
            var result=new List<SievePolynomial>();
            var sqrt=new IntegerSquareRoot();
            var sizeBound = sqrt.Sqrt(2*n)/_sieveBound;
            var primes=new List<long>();
            BigInteger a = 1;
            var aFactorsBound = Math.Exp(BigInteger.Log(sizeBound)/13);
           
            var indexes=new HashSet<int>();
            while (a<sizeBound&&_polyIndex<_factorBase.Length)
            {
                if (_factorBase[_polyIndex] < aFactorsBound)
                {
                    _polyIndex++;
                    continue;
                }

                indexes.Add(_polyIndex);
                var p = _factorBase[_polyIndex++];
                a *= p;
                primes.Add(p);
   
            }
            var inv=new ModularInverse();
            var B=new BigInteger[primes.Count];
            var Bainv=new Dictionary<long,Dictionary<int,long>>();
            for (int i = 0; i < primes.Count; i++)
            {
                var q = primes[i];
                var aq = a/q;
                var gamma = _sqrtN[q]*inv.Inverse(aq, q);
                if (gamma > q/2)
                {
                    gamma = q - gamma;
                }
                B[i] = aq*gamma;

            }
            var ainv=new Dictionary<long,long>();


            BigInteger b = 0;
            for (int i = 0; i < B.Length; i++)
            {

                b += B[i];
            }


            var firstSln = new Dictionary<long, long>();
            var secondSln = new Dictionary<long, long>();
            for (int i = 0; i < _factorBase.Length; i++)
            {
                if (indexes.Contains(i))
                {
                    continue;
                }
                var p = _factorBase[i];
                ainv.Add(p,(long)inv.Inverse(a,p));
                Bainv[p] = new Dictionary<int, long>();
                for (int j = 0; j < primes.Count; j++)
                {

                    Bainv[p][j] = (long)(2*B[j]*ainv[p]%p);
                }
                firstSln[p] = (long)(ainv[p] * (_sqrtN[p] - b) % p);
                secondSln[p] = (long)(ainv[p] * (-_sqrtN[p] - b) % p);
            }

            if (primes.Count == 0)
            {
                return null;
            }

            result.Add(new SievePolynomial() {A = a,B=b,C=(b*b-n)/a,FirstRoot = firstSln,SecondRoot = secondSln,InvA = ainv,MinFactorA = primes[0],MaxFactorA = primes[primes.Count-1]});


            var polyCount = Math.Pow(2, primes.Count - 1)-1;
            for (int i = 1; i <= polyCount; i++)
            {
                var v = 1;
                for (;  (i&(0x1<<(v-1)))==0; v++) { }


                var tmp =(long)Math.Ceiling( i/Math.Pow(2, v));

                if (tmp%2 == 0)
                {
                    b = b + 2*B[v];
                }
                else
                {
                    b = b - 2*B[v];
                }



                firstSln = new Dictionary<long, long>();
                secondSln = new Dictionary<long, long>();

                for (var k = 0; k < _factorBase.Length; k++)
                {
                    if (indexes.Contains(k))
                    {
                        continue;
                    }
                    var p = _factorBase[k];
                    firstSln[p] = (long)(ainv[p] * (_sqrtN[p] - b) % p);
                    secondSln[p] = (long)(ainv[p] * (-_sqrtN[p] - b) % p);
                }
                result.Add(new SievePolynomial() { A = a, B = b,C= (b * b - n) / a, FirstRoot = firstSln, SecondRoot = secondSln, InvA = ainv, MinFactorA = primes[0], MaxFactorA = primes[primes.Count-1] });


            }
            _polyIndex += 3-primes.Count;
            return result;

        }

        private long[] BuildFactorBase(BigInteger n)
        {
            var sieve = new EratosthenesSieve();
            var legandre = new JacobiSymbol();
            var sqt=new ModularSqrt();
            _sqrtN=new Dictionary<long, long>();
            
            return sieve.GetPrimes(2, _primeBound).Where(p =>
            {
                if (legandre.Calculate(n, p) == 1)
                {
                    _sqrtN.Add(p,(long)sqt.Sqrt(n,p));
                    return true;
                }
                return false;
            }).ToArray();
        }

        private SievePolynomial GetDefaultPolynomial(BigInteger n)
        {
            var m=new IntegerSquareRoot().Sqrt(n);
            var ainv = new Dictionary<long, long>();
            foreach (var p in _factorBase)
            {
                ainv.Add(p,1);
            }

            var firstSln = new Dictionary<long, long>();
            var secondSln = new Dictionary<long, long>();

            for (int i = 0; i < _factorBase.Length; i++)
            {
                var p = _factorBase[i];
                firstSln[p] = (long)(ainv[p] * (_sqrtN[p] - m) % p);
                secondSln[p] = (long)(ainv[p] * (-_sqrtN[p] - m) % p);
            }



            return new SievePolynomial()
            {
                A = 1,
                B = m,
                C = (m*m - n),
                FirstRoot = firstSln,
                SecondRoot = secondSln,
                InvA = ainv
            };
        }

        private Matrix BuildMatrix(List<SmoothPair> smoothPairs)
        {
            int m = _factorBase.Length + 1;
            int n = smoothPairs.Count;
            int[,] matrix = new int[m, n];
            for (int i = 0; i < n; i++)
            {
                if (smoothPairs[i].Y[-1] ==0)
                {
                    matrix[0, i] = 0;
                }
                else
                {
                    matrix[0, i] = 1;
                }
                for (int j = 0; j < _factorBase.Length; j++)
                {
                    if (smoothPairs[i].Y.ContainsKey(_factorBase[j]))
                    {
                        matrix[j + 1, i] = (int) (smoothPairs[i].Y[_factorBase[j]])%2;
                    }
                }
            }
           return new Matrix(matrix);
        }
    }
}
