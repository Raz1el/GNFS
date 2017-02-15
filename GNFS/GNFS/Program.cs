using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using GNFS.ECM;
using GNFS.GNFS.Polynomial_generator;
using GNFS.Integer_arithmetic;
using System.Numerics;
using GNFS.GNFS;
using GNFS.GNFS.Square_root;
using GNFS.Linear_algebra;
using GNFS.Polynomial_arithmetic;


namespace GNFS
{
    class Program
    {
        static void Main(string[] args)
        {
            //216366620575959221 x 438925910071081891 x 33696009303094673 x 33696009303095179 x14029308060317546154181 x 37280713718589679646221
            //var p1 = BigInteger.Parse("216366620575959221");// BigInteger.Parse("216366620575959221");
            //var p2 = BigInteger.Parse("438925910071081891");
            //var p3 = BigInteger.Parse("33696009303094673");
            //var p4 = BigInteger.Parse("33696009303095179");
            //var p5 = BigInteger.Parse("14029308060317546154181");
            //var p6 = BigInteger.Parse("37280713718589679646221");
            //var n = p5 * p6;
            //n = BigInteger.Parse("248969293387880645247065237773");
            //EcmSandbox(n);

            var gauss=new GaussianElimination();
            var matr = new int[10, 12];
            var row0 = new int[] {0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1};
            var row1 = new int[] {1, 0, 1, 0, 0, 0, 1, 1, 1, 0, 1, 0};
            var row2 = new int[] {0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0};
            var row3 = new int[] {0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 1, 1};
            var row4 = new int[] {1, 1, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0};
            var row5 = new int[] {0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0};
            var row6 = new int[] { 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0 };
            var row7 = new int[] {0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 1, 0};
            var row8 = new int[] {0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0};
            var row9 = new int[] {0, 0, 1, 1, 1, 0, 0, 0, 1, 0, 1, 0};
            for (int i = 0; i < row0.Length; i++)
            {
                matr[0, i] = row0[i];
                matr[1, i] = row1[i];
                matr[2, i] = row2[i];
                matr[3, i] = row3[i];
                matr[4, i] = row4[i];
                matr[5, i] = row5[i];
                matr[6, i] = row6[i];
                matr[7, i] = row7[i];
                matr[8, i] = row8[i];
                matr[9, i] = row9[i];
            }

            var matrix = new Matrix(matr);
            var s = gauss.Solve(matrix);
            Console.WriteLine(s);
            //Console.WriteLine("Press any key... \n");
            //Console.ReadKey();





            //var sqrt=new AlgebraicSqrt();

            //var poly=new Polynomial(new BigInteger[]
            //{
            //    BigInteger.Parse("25602452817775159059194687644564881010593683"), 
            //});





            //var f=new Polynomial(new BigInteger[] {-2,0,0,1});
            //var df = new Polynomial(new BigInteger[] { 0, 0, 3 });
            //var s = new Polynomial(new BigInteger[] { -23048, -28552, 37681 });
            //var res = sqrt.Sqrt(s, f, 2047, 16);


            //var r=new Polynomial(new BigInteger[] {1});
            //var one=new Polynomial(new BigInteger[] {-1});
            //var s=new Polynomial(new BigInteger[] {1});
            //var x=new Tuple<Polynomial,Polynomial>(r,one);
            //var res=sqrt.Pow(x, 4, f, 101, s);

            //var num = new SpecialNumber(17, 25, 4);
            //Console.WriteLine(num.Value());
            //var snfs = new Snfs(num, new SnfsPolynomialGenerator(num, 5));
            //snfs.FindFactor();




            //var num = new SpecialNumber(3, 4, 4);
            //Console.WriteLine(num.Value());
            //var snfs = new Snfs(num, new SnfsPolynomialGenerator(num, 3));
            //snfs.FindFactor();
            Console.ReadKey();
        }
        static BigInteger EcmSandbox(BigInteger n)
        {
            Console.WriteLine(string.Format("n={0} ({1} digits)",n,n.ToString().Length));

            CurveGenerator g = new CurveGenerator();
            BigInteger gcd = 1;
            var timer = new Stopwatch();
            var parallelismLimit = 6;
            ulong b1 = 1000;
            ulong b2 = b1*1000000;
            ulong b3 = b2 + b1*100;
            ulong curveCount = 50;
            timer.Start();
            var curve = new Curve[curveCount];
            BigInteger factor =1;
            curve = curve.Select(x =>
            {
                var c=g.GenerateMontgomeryCurve(n, out gcd);
                if(gcd>1)
                   factor=gcd;
                return c;

            }).ToArray();
            if (factor > 1)
            {
                Console.WriteLine("Factor found!");
                Console.WriteLine(factor);
                timer.Stop();
                Console.WriteLine(timer.Elapsed);
                return factor;
            }
            else
            {
                var count = 0;
                Console.Write("\r{0}/{1} curves   ", count++, curveCount);
                Parallel.ForEach(curve, new ParallelOptions { MaxDegreeOfParallelism = parallelismLimit }, (c, state) =>
                {
                    if (!state.IsStopped)
                    {
                        var ecm = new Ecm();
                        factor = ecm.Calculate(c, b1, b2,b3);
                    }
           

                    if (n % factor != 0)
                        Console.WriteLine("ERROR!");
                    else if (factor > 1 && !state.IsStopped)
                    {
                        Console.WriteLine();
                        Console.WriteLine("Factor found!");
                        Console.WriteLine(factor);
                        timer.Stop();
                        Console.WriteLine(timer.Elapsed);
                        Console.WriteLine("Waiting for killing threads...");
                        state.Stop();

                    }
                    else
                    {


                        if (!state.IsStopped)
                        {
                            Console.Write("\r{0}/{1} curves   ", count++, curveCount);
                        }
                      
                    }
                });
            }
            return factor;
        }

    }
}
