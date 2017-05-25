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
using GNFS.GNFS.Sieve;
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
            Console.WindowWidth=170;
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

            //1565478606270488556559294655961056969707482486703
            //78325683705012095897299536068804821

            //{ 101, 953, 96253}  0.28 | 0.05 | 0.002 | 0.006
            //{ 1009, 9511, 9596599} 0.33 | 0.051 | 0.002 | 0.01
            //{ 10007, 95003, 950695021} 0.53 | 0.056  | 0.003 | 0.013
            //{ 100003, 950009, 95003750027} 0.72 | 0.065 | 0.01 | 0.015
            //{ 1000003, 9500021, 9500049500063} 0.95 | 0.069 | 0.07 | 0.02
            //{ 10000019, 95000011, 950001915000209} 1.31 | 0.06 | 0.7 | 0.01
            //{ 100000007, 950000017, 95000008350000119} 1.80 | 0.07 | 6.5 | 0.02
            //{ 1000000007, 9500000021, 9500000087500000147} 1.82 | 0.08 | 66.59 | 0.03
            //{ 10000000019, 95000000033, 950000002135000000627} 2.70 | 0.09 | 1191 | 0.06
            //{ 100000000003, 950000000003, 95000000003150000000009} 4.32 | 0.11 | ... | 0.09
            //{ 1000000000039, 9500000000039,9500000000409500000001521} 7.2|0.16| ... | 0.4
            // { 10000000000037, 95000000000029,950000000003805000000001073} 10.5 | 0.26| ... | 0.2
            //{ 100000000000031, 950000000000021, 95000000000031550000000000651} 15.3 | 0.3 | ...| 0.2
            //{ 1000000000000037, 9500000000000017, 9500000000000368500000000000629} 42.13 | 0.4 | ... | 0.8
            //{ 10000000000000061, 95000000000000009, 950000000000005885000000000000549} 62.76 | 0.5 | ... | 0.8
            //{ 100000000000000003, 950000000000000021, 95000000000000004950000000000000063} 103.65 | 0.9 | ... | 0.5
            //{ 1000000000000000003, 9500000000000000003, 9500000000000000031500000000000000009} 303.32 | 1.5 | ...| 6.6
            //{ 10000000000000000051, 95000000000000000021, 950000000000000005055000000000000001071} 961.51 | 1.9 |...2.9

            var n = BigInteger.Parse("95000000003150000000009");
            var nSpec=new SpecialNumber(2,512,-1);


            var gnfs=new Gnfs(n,new GnfsPolynomialGenerator(n,3), 4000, 5000, 122500,new LogSieve());
            var snfs = new Snfs(nSpec,new SnfsPolynomialGenerator(nSpec,5), 1294973, 1294973, 500000);
            var siqs = new Siqs(122000, 10000, 10);

            Console.WriteLine();
            var timer=new Stopwatch();
            timer.Start();
            var factor =gnfs.FindFactor();
            Console.WriteLine("factor found: "+factor);
            timer.Stop();
            Console.WriteLine();
            if (n % factor != 0)
            {
                Console.WriteLine("ERROR!");
            }
            Console.WriteLine("___________________________________________________");
          Console.WriteLine("TOTAL TIME:"+timer.Elapsed);

            Console.ReadKey();

        }
        static BigInteger EcmSandbox(BigInteger n,ulong bound, ulong curveCount)
        {
            Console.WriteLine(string.Format("n={0} ({1} digits)",n,n.ToString().Length));

            CurveGenerator g = new CurveGenerator();
            BigInteger gcd = 1;
            var timer = new Stopwatch();
            ulong b1 = bound;
            ulong b2 = b1*100;
            ulong b3 = b2 + b1*100;
       
            timer.Start();
            var curves = new Curve[curveCount];
            BigInteger factor =1;
            curves = curves.Select(x =>
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
                foreach (var curve in curves)
                {
                   
                        var ecm = new Ecm();
                        factor = ecm.Calculate(curve, b1, b2, b3);
                    


                    if (n % factor != 0)
                        Console.WriteLine("ERROR!");
                    else if (factor > 1)
                    {
                        Console.WriteLine();
                        Console.WriteLine("Factor found!");
                        Console.WriteLine(factor);
                        timer.Stop();
                        Console.WriteLine(timer.Elapsed);
                        break;
                    }
                    else
                    {

                            Console.Write("\r{0}/{1} curves   ", count++, curveCount);

                    }
                }
          
            }
            return factor;
        }





        static BigInteger TrialDivision(BigInteger n)
        {
            var sqrtN=new IntegerSquareRoot().Sqrt(n);
            if (n % 2 == 0)
            {
                return 2;
            }
            for (BigInteger i = 3; i < sqrtN; i+=2)
            {
                if (n%i == 0)
                {
                    return i;
                }
            }
            return 1;
        }

    }
}
