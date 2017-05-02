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
using GNFS.QS;


namespace GNFS
{
    class Program
    {
        static void Main(string[] args)
        {
            //22455983949710645412x2+54100105785512562427x+22939402657683071224
            //216366620575959221 x 438925910071081891 x 33696009303094673 x 33696009303095179 x14029308060317546154181 x 37280713718589679646221
            //var p1 = BigInteger.Parse("216366620575959221");// BigInteger.Parse("216366620575959221");
            //var p2 = BigInteger.Parse("438925910071081891");
            //var p3 = BigInteger.Parse("33696009303094673");
            //var p4 = BigInteger.Parse("33696009303095179");
            //var p5 = BigInteger.Parse("14029308060317546154181");
            //var p6 = BigInteger.Parse("37280713718589679646221");
            //var n = p5 * p6;
            //10000000000000000001231254125695123124241000000070181485132132018081737
            //1522605027922533360535618378132637429718068114961380688657908494580122963258952897654000350692006139
            ////1135421042954259800567202166881467
            //BigInteger.Parse("25602452817775159059194687644564881010593683"),
            //80: 45261371639976440147856072490845590811635113169755543286071538119010508358468757
            //51: 556158012756522140970101270050308458769458529626977
            //58: 1000000000000000000000000036290000000000000000000000105589 
            ////1135421042954259800567202166881467
            /// 105963439434940945466724710985809
            /// 98059678006604446847234312569523
            /// 2273315755446568584255314994581
            /// 482923494310788761323853885701
            /// 10976388499557984324984058439
            /// 6859038167262341961920676409
            /// 107863234983600330243839377
            /// 97965478833685375120845037
            /// 5565990898925529810152251
            /// 124145163990833424303911
            /// 54870913303062151617209
            /// 7196557138390478870831
            /// 440319813883378517749
            /// 37125895053318175103
            /// 4403261957931795781
            /// 348502809153085921
            /// 10128397120599323
            /// 7683116086849193
            /// 214899179042879
            /// 99474890689379
            /// 3748037878609
            /// 266686300733
            /// 33202661729
            /// 3435223721
            /// 128894237
            /// 12809161



            var n = BigInteger.Parse("1135421042954259800567202166881467");
            var nSpec=new SpecialNumber(2,128,-1);


            var gnfs=new Gnfs(n,new GnfsPolynomialGenerator(n,3),15000,15000,1000000);
            var snfs = new Snfs(nSpec,new SnfsPolynomialGenerator(nSpec,3),4000,4000,100000);
            var siqs = new Siqs(120000, 50000, 10);

            Console.WriteLine();
            Console.WriteLine(gnfs.FindFactor());
          
            Console.ReadKey();

        }
        static BigInteger EcmSandbox(BigInteger n)
        {
            Console.WriteLine(string.Format("n={0} ({1} digits)",n,n.ToString().Length));

            CurveGenerator g = new CurveGenerator();
            BigInteger gcd = 1;
            var timer = new Stopwatch();
            var parallelismLimit = 6;
            ulong b1 = 2000000;
            ulong b2 = b1*1000000;
            ulong b3 = b2 + b1*100;
            ulong curveCount = 10;
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





        static void tmp()
        {
            //var poly = new Polynomial(new BigInteger[]
            //{
            //    BigInteger.Parse("22939402657683071224"),
            //    BigInteger.Parse("54100105785512562427"),
            //    BigInteger.Parse("22455983949710645412")
            //});
            //var modPoly = new Polynomial(new BigInteger[] { 8,29,15,1 });
            //var algSqrt = new AlgebraicSqrt();
            //var res = algSqrt.Sqrt(poly, modPoly);
            //Console.WriteLine(res);
            //Console.ReadKey();
            //var fieldChar = BigInteger.Parse("4665721");
            //var polyMath = new PolynomialMath(fieldChar);
            //var four = new Polynomial(new BigInteger[] { 4 });
            //var b = new Polynomial(new BigInteger[] { 3 });

            //for (int i = 0; i < 100; i++)
            //{
            //    b = new Polynomial(new BigInteger[] { i });

            //    var t = polyMath.ModPow(poly, (fieldChar * fieldChar * fieldChar - 1) / 2, modPoly);
            //    var res = polyMath.ModPow(polyMath.Sub(polyMath.Mul(b, b), polyMath.Mul(four, poly)), (fieldChar * fieldChar * fieldChar - 1) / 2, modPoly);
            //    if (res.Deg == 0 && res[0] == -1)
            //    {
            //        break;

            //    }
            //}





            //var gfMath = new FiniteFieldMath(modPoly, fieldChar);
            //var mod = new PolynomialOverFiniteField(new Polynomial[] { poly, b, new Polynomial(new BigInteger[] { 1 }) });

            //var y = new PolynomialOverFiniteField(new Polynomial[] { new Polynomial(new BigInteger[] { 0 }), new Polynomial(new BigInteger[] { 1 }) });

            //var res1 = gfMath.ModPow(y, (BigInteger.Pow(fieldChar, 3) + 1) / 2, mod);
            //Console.WriteLine(res1);

            //Console.ReadKey();


            //216366620575959221 x 438925910071081891 x 33696009303094673 x 33696009303095179 x14029308060317546154181 x 37280713718589679646221
            //var p1 = BigInteger.Parse("216366620575959221");// BigInteger.Parse("216366620575959221");
            //var p2 = BigInteger.Parse("438925910071081891");
            //var p3 = BigInteger.Parse("33696009303094673");
            //var p4 = BigInteger.Parse("33696009303095179");
            //var p5 = BigInteger.Parse("14029308060317546154181");
            //var p6 = BigInteger.Parse("37280713718589679646221");
            //var n = p5 * p6;
            //var n = BigInteger.Parse("10000000000000000001231254125695123124241000000070181485132132018081737");
            //EcmSandbox(n);

            //var gauss = new FastGaussianEliminationOverGf2();
            //var matr = new int[12, 13];
            //var row0 = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            //var row1 = new int[] { 0, 0, 1, 0, 1, 0, 0, 0, 0, 0, 0, 1, 0 };
            //var row2 = new int[] { 1, 0, 1, 0, 0, 0, 1, 1, 1, 0, 1, 0, 1 };
            //var row3 = new int[] { 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0 };
            //var row4 = new int[] { 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 1, 1, 0 };
            //var row5 = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            //var row6 = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            //var row7 = new int[] { 1, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0 };
            //var row8 = new int[] { 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0 };
            //var row9 = new int[] { 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 1, 0, 0 };
            //var row10 = new int[] { 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1 };
            //var row11 = new int[] { 0, 0, 1, 1, 1, 0, 0, 0, 1, 0, 1, 0, 0 };


            //for (int i = 0; i < row0.Length; i++)
            //{
            //    matr[0, i] = row0[i];
            //    matr[1, i] = row1[i];
            //    matr[2, i] = row2[i];
            //    matr[3, i] = row3[i];
            //    matr[4, i] = row4[i];
            //    matr[5, i] = row5[i];
            //    matr[6, i] = row6[i];
            //    matr[7, i] = row7[i];
            //    matr[8, i] = row8[i];
            //    matr[9, i] = row9[i];
            //    matr[10, i] = row10[i];
            //    matr[11, i] = row11[i];
            //}

            //var matrix = new Matrix(matr);
            //var s = gauss.Solve(matrix);

            //gauss.CheckSolutions(matrix, s);

            //matrix.Print();
            //Console.WriteLine("Press any key... \n");
            //Console.ReadKey();





            //var sqrt = new AlgebraicSqrt();

            //var poly = new Polynomial(new BigInteger[]
            //{
            //    BigInteger.Parse("25602452817775159059194687644564881010593683"),
            //});





            //var f = new Polynomial(new BigInteger[] { -36, 0, 0, 1 });
            //var df = new Polynomial(new BigInteger[] { 0, 0, 3 });
            ////  var s = new Polynomial(new BigInteger[] { -23048, -28552, 37681 });

            //var p = new Polynomial(new BigInteger[] { 46656, -8748, 972 });

            //var algSqrt = new AlgebraicSqrt();
            //var res = algSqrt.Sqrt(p, f, df, 315);
            //var polymath = new PolynomialMath();

            //var tmp = polymath.Mul(res, res);
            //tmp = polymath.Rem(tmp, f);

            //var num = new SpecialNumber(17, 25, 4);
            //Console.WriteLine(num.Value());
            //var snfs = new Snfs(num, new SnfsPolynomialGenerator(num, 5));
            //snfs.FindFactor();

            //var primes=new List<BigInteger> {5,7};
            //var coeff=new List<BigInteger> {1,5};
            //var alg=new GarnerCrt();
            //var x=alg.Calculate(coeff, primes);





            var timer = new Stopwatch();
            ////BigInteger.Parse("1522605027922533360535618378132637429718068114961380688657908494580122963258952897654000350692006139");

            ////556158012756522140970101270050308458769458529626977
            ////1135421042954259800567202166881467
            var num = BigInteger.Parse(@"1000000000000000000000000036290000000000000000000000105589");
            Console.WriteLine("Number: " + num);
            timer.Reset();
            timer.Start();
            var gnfs = new Gnfs(num, new GnfsPolynomialGenerator(num, 3), 50000, 150000, 5000000);
            Console.WriteLine("\nFactor found:" + gnfs.FindFactor());
            Console.WriteLine("\nTotal time:" + timer.Elapsed);


            Console.ReadKey();




            //var num = new SpecialNumber(2, 128, -1);
            //Console.WriteLine("Number: " + num.Value());
            //timer.Reset();
            //timer.Start();
            //var snfs = new Snfs(num, new SnfsPolynomialGenerator(num, 3),5000,2000,100000);
            //Console.WriteLine("\nFactor found:" + snfs.FindFactor());
            //Console.WriteLine("\nTotal time:" + timer.Elapsed);
            //Console.ReadKey();




            //80: 45261371639976440147856072490845590811635113169755543286071538119010508358468757
            //51: 556158012756522140970101270050308458769458529626977
            //58: 1000000000000000000000000036290000000000000000000000105589 
            //var timer = new Stopwatch();

            //var num = BigInteger.Parse(@"556158012756522140970101270050308458769458529626977");
            //Console.WriteLine("Number: " + num);
            //timer.Reset();
            //timer.Start();
            //var siqs = new Siqs();
            //Console.WriteLine("\nFactor found:" + siqs.FindFactor(num));
            //Console.WriteLine("\nTotal time:" + timer.Elapsed);


            //Console.ReadKey();
        }

    }
}
