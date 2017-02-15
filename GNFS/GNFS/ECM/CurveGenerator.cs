using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using GNFS.Integer_arithmetic;

namespace GNFS.ECM
{
    public class CurveGenerator
    {
        static Random _rnd = new Random();
        public Curve GenerateMontgomeryCurve(BigInteger mod,out BigInteger gcd)
        {
            gcd = 0;
            BigInteger u = 0;
            BigInteger v = 0;
            BigInteger x = 0;
            BigInteger z = 0;
            BigInteger T1 = 0;
            BigInteger sigma = 0;
            ModularInverse modInverse=new ModularInverse();
            while (gcd == 0)
            {
                sigma = _rnd.Next(7, int.MaxValue);
                u = sigma * sigma - 5;
                v = 4 * sigma;
                x = u * u * u;
                z = v * v * v;

                T1 = modInverse.Inverse(4 * u * u * u * v, mod,out gcd);

                if (gcd != 1 && gcd != 0)
                {
                   return new Curve(0,0,0,new ProjectivePoint(0,0,0));
                }
            }
            var T2 = (v - u);
            var a = T2 * T2 * T2 * (3 * u + v) * T1 - 2;
            return new Curve(a%mod,0,mod,new ProjectivePoint(x % mod, 0, z % mod));

        }
    }
}
