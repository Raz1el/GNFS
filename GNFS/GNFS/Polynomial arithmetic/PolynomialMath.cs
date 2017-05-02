using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using GNFS.Integer_arithmetic;

namespace GNFS.Polynomial_arithmetic
{
    public class PolynomialMath
    {
        public BigInteger Mod { get; }

        public PolynomialMath(BigInteger mod)
        {
            Mod = mod;
        }

        public PolynomialMath():this(-1)
        {
            
        }
        public Polynomial Mul(Polynomial firstArg, Polynomial secondArg)
        {
            int firstDeg = firstArg.Deg;
            int secondDeg = secondArg.Deg;
            if (firstDeg == -1 || secondDeg == -1)
                return new Polynomial(new BigInteger[] { 0 });
            int maxdegree = firstDeg + secondDeg;
            BigInteger[] result = new BigInteger[maxdegree + 1];
            for (int i = 0; i <= firstDeg; i++)
            {
                for (int j = 0; j <= secondDeg; j++)
                {
                    result[i + j] = result[i + j] + firstArg[i] * secondArg[j];
                }
            }
            return new Polynomial(result,Mod);
        }

        public Polynomial Normalize(Polynomial poly)
        {
            if(Mod==-1)
                throw new NotImplementedException();
            var coefficient = poly[poly.Deg];
            if (coefficient == 1)
                return poly;
            var inverse=new ModularInverse();
            coefficient = inverse.Inverse(coefficient, Mod);
            var newPoly=new BigInteger[poly.Deg+1];
            for (int i = 0; i <= poly.Deg; i++)
            {
                newPoly[i] = poly[i]*coefficient;
            }
            return new Polynomial(newPoly,Mod);
        }

        public Polynomial Pow(Polynomial polynomial, BigInteger power)
        {
            if (power == 0)
                return new Polynomial(new BigInteger[] { 1});
            var res = new Polynomial(new BigInteger[] { 1 });
            var a = polynomial;
            while (power != 0)
            {

                if (power % 2 != 0)
                {
                    res = Mul(res , a);
                }
                a = Mul(a , a);
                power >>= 1;
            }
            return res;
        }
        public Polynomial ModPow(Polynomial polynomial, BigInteger power,  Polynomial mod)
        {
            if (power == 0)
                return new Polynomial(new BigInteger[] { 1 });
            var res = new Polynomial(new BigInteger[] { 1 });
            var a = polynomial;
            while (power != 0)
            {

                if (power % 2 != 0)
                {
                    res = Rem(Mul(res, a),mod);
                }
                a = Rem(Mul(a, a),mod);
                power >>= 1;
            }
            return res;
        }

        public Polynomial Div(Polynomial firstArg, Polynomial secondArg)
        {
            if (secondArg.Deg == -1)
                throw new DivideByZeroException();
            if (firstArg.Deg < secondArg.Deg)
                return new Polynomial(new BigInteger[] {0});


            var firstDeg = firstArg.Deg;
            var secondDeg = secondArg.Deg;
            var newDeg = firstDeg - secondDeg;
            var result = new BigInteger[newDeg + 1];
            var reminder = (BigInteger[])firstArg.Coefficients.Clone();
            BigInteger firstLc;
            BigInteger secondLc = secondArg[secondDeg];
        
            if (Mod < 2)
            {
                if (secondLc != 1)
                    throw new InvalidOperationException();
            }
            else
            {
                var inverse = new ModularInverse();
                secondLc = inverse.Inverse(secondLc, Mod);
            }

            for (int i = 0; i <= newDeg; i++)
            {
                firstLc = reminder[firstDeg - i];
                result[newDeg - i] = (firstLc * secondLc);
                for (int j = 0; j <= secondDeg; j++)
                {
                    reminder[firstDeg - secondDeg + j - i] =
                        (reminder[firstDeg - secondDeg + j - i] + ( - (result[newDeg - i] * secondArg[j]) )) ;
                }
            }
            return new Polynomial(result, Mod);

        }
        public Polynomial Rem(Polynomial firstArg, Polynomial secondArg)
        {
            if (secondArg.Deg == -1)
                throw new DivideByZeroException();
            if (firstArg.Deg < secondArg.Deg)
                return new Polynomial(firstArg.Coefficients);

            var firstDeg = firstArg.Deg;
            var secondDeg = secondArg.Deg;
            var newDeg = firstDeg - secondDeg;
            var result = new BigInteger[newDeg + 1];
            var reminder = (BigInteger[])firstArg.Coefficients.Clone();


     
            BigInteger firstLc;
            BigInteger secondLc = secondArg[secondDeg];
            if (Mod < 2)
            {
                if (secondLc != 1)
                    throw new InvalidOperationException();
            }
            else
            {
                var inverse = new ModularInverse();
                secondLc = inverse.Inverse(secondLc, Mod);
            }
         


            for (int i = 0; i <= newDeg; i++)
            {
                firstLc = reminder[firstDeg - i];
                result[newDeg - i] = (firstLc * secondLc);
                for (int j = 0; j <= secondDeg; j++)
                {
                    reminder[firstDeg - secondDeg + j - i] =
                        (reminder[firstDeg - secondDeg + j - i] + ( - (result[newDeg - i] * secondArg[j])));
                }
            }
            return new Polynomial(reminder, Mod);

        }
        public Polynomial Sub(Polynomial firstArg, Polynomial secondArg)
        {
            int firstDeg = firstArg.Deg;
            int secondDeg = secondArg.Deg;

            var maxDeg = Math.Max(firstDeg, secondDeg);
            if (maxDeg == -1)
            {
                return new Polynomial(new BigInteger[1]);
            }

            var result = new BigInteger[maxDeg + 1];
            if (firstDeg > secondDeg)
            {
                int i;
                for (i = 0; i <= secondDeg; i++)
                {
                    result[i] = firstArg[i] - secondArg[i];
                }
                for (; i <= firstDeg; i++)
                {
                    result[i] = firstArg[i];
                }
            }
            else
            {
                int i;
                for (i = 0; i <= firstDeg; i++)
                {
                    result[i] = +firstArg[i] - secondArg[i];
                }
                for (; i <= secondDeg; i++)
                {
                    result[i] = -secondArg[i];
                }

            }

            return new Polynomial(result, Mod);
        }

        public Polynomial ConstMul(BigInteger number, Polynomial polynomial)
        {
            var result = new BigInteger[polynomial.Deg+1];
            for (int i = 0; i <= polynomial.Deg; i++)
            {
                result[i] = number*polynomial[i];
            }
            return new Polynomial(result,Mod);
        }

        public Polynomial Add(Polynomial firstArg, Polynomial secondArg)
        {
            int firstDeg = firstArg.Deg;
            int secondDeg = secondArg.Deg;
            var maxDeg = Math.Max(firstDeg, secondDeg);
            if (maxDeg == -1)
            {
                return new Polynomial(new BigInteger[1]);
            }
            var result = new BigInteger[maxDeg + 1];
            if (firstDeg > secondDeg)
            {
                int i;
                for (i = 0; i <= secondDeg; i++)
                {
                    result[i] = firstArg[i] + secondArg[i];
                }
                for (; i <= firstDeg; i++)
                {
                    result[i] = firstArg[i];
                }
            }
            else
            {
                int i;
                for (i = 0; i <= firstDeg; i++)
                {
                    result[i] = firstArg[i] + secondArg[i];
                }
                for (; i <= secondDeg; i++)
                {
                    result[i] = secondArg[i];
                }

            }

            return new Polynomial(result, Mod);
        }

      
    }
}
