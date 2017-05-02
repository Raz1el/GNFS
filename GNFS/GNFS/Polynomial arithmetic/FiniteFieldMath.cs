using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GNFS.Polynomial_arithmetic
{
    public class FiniteFieldMath
    {
        public Polynomial Mod { get; }
        public BigInteger FieldChar { get; }


        private PolynomialMath _polynomialMath;

        public FiniteFieldMath(Polynomial mod,BigInteger fieldChar)
        {
            Mod = mod;
            FieldChar = fieldChar;
            _polynomialMath=new PolynomialMath(FieldChar);
        }
        public PolynomialOverFiniteField Mul(PolynomialOverFiniteField firstArg, PolynomialOverFiniteField secondArg)
        {
            int firstDeg = firstArg.Deg;
            int secondDeg = secondArg.Deg;
            if (firstDeg == -1 || secondDeg == -1)
                return new PolynomialOverFiniteField(new Polynomial[] {new Polynomial(new BigInteger[] {0}) });
            int maxdegree = firstDeg + secondDeg;
            Polynomial[] result = new Polynomial[maxdegree + 1];
            for (int i = 0; i <= firstDeg; i++)
            {
                for (int j = 0; j <= secondDeg; j++)
                {
                    if (result[i + j] == null)
                    {
                        result[i + j]=new Polynomial(new BigInteger[] {0});
                    }
                    result[i + j] = _polynomialMath.Add(result[i + j] ,_polynomialMath.Mul(firstArg[i] , secondArg[j]));
                }
            }
            return new PolynomialOverFiniteField(result,Mod,FieldChar);
        }
        public PolynomialOverFiniteField Pow(PolynomialOverFiniteField polynomial, BigInteger power)
        {
            if (power == 0)
                return new PolynomialOverFiniteField( new Polynomial[] { new Polynomial(new BigInteger[] { 1 }) });
            var res = new PolynomialOverFiniteField(new Polynomial[] { new Polynomial(new BigInteger[] { 1 }) });
            var a = polynomial;
            while (power != 0)
            {

                if (power % 2 != 0)
                {
                    res = Mul(res, a);
                }
                a = Mul(a, a);
                power >>= 1;
            }
            return res;
        }


        public PolynomialOverFiniteField ModPow(PolynomialOverFiniteField polynomial, BigInteger power,PolynomialOverFiniteField mod)
        {
            if (power == 0)
                return new PolynomialOverFiniteField(new Polynomial[] { new Polynomial(new BigInteger[] { 1 }) });
            var res = new PolynomialOverFiniteField(new Polynomial[] { new Polynomial(new BigInteger[] { 1 }) });
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


        public PolynomialOverFiniteField Div(PolynomialOverFiniteField firstArg, PolynomialOverFiniteField secondArg)
        {
            if (secondArg.Deg == -1)
                throw new DivideByZeroException();
            if (firstArg.Deg < secondArg.Deg)
                return new PolynomialOverFiniteField(new Polynomial[] { new Polynomial(new BigInteger[] { 0 })});


            var firstDeg = firstArg.Deg;
            var secondDeg = secondArg.Deg;
            var newDeg = firstDeg - secondDeg;
            var result = new Polynomial[newDeg + 1];
            var reminder = (Polynomial[])firstArg.Coefficients.Clone();
            Polynomial firstLc;
            Polynomial secondLc = secondArg[secondDeg];

            var inverse = new PolynomialInverse();
            secondLc = inverse.Inverse(secondLc, Mod);


            for (int i = 0; i <= newDeg; i++)
            {
                firstLc = reminder[firstDeg - i];
                result[newDeg - i] =_polynomialMath.Mul(firstLc , secondLc);
                for (int j = 0; j <= secondDeg; j++)
                {
                    reminder[firstDeg - secondDeg + j - i] =
                       _polynomialMath.Sub (reminder[firstDeg - secondDeg + j - i] , (_polynomialMath.Mul(result[newDeg - i] , secondArg[j])));
                }
            }
            return new PolynomialOverFiniteField(result, Mod,FieldChar);

        }
        public PolynomialOverFiniteField Rem(PolynomialOverFiniteField firstArg, PolynomialOverFiniteField secondArg)
        {
            if (secondArg.Deg == -1)
                throw new DivideByZeroException();
            if (firstArg.Deg < secondArg.Deg)
                return firstArg;


            var firstDeg = firstArg.Deg;
            var secondDeg = secondArg.Deg;
            var newDeg = firstDeg - secondDeg;
            var result = new Polynomial[newDeg + 1];
            var reminder = (Polynomial[])firstArg.Coefficients.Clone();
            Polynomial firstLc;
            Polynomial secondLc = secondArg[secondDeg];

            var inverse = new PolynomialInverse();
            secondLc = inverse.Inverse(secondLc, Mod);


            for (int i = 0; i <= newDeg; i++)
            {
                firstLc = reminder[firstDeg - i];
                result[newDeg - i] = _polynomialMath.Mul(firstLc, secondLc);
                for (int j = 0; j <= secondDeg; j++)
                {
                    reminder[firstDeg - secondDeg + j - i] =
                       _polynomialMath.Sub(reminder[firstDeg - secondDeg + j - i], (_polynomialMath.Mul(result[newDeg - i], secondArg[j])));
                }
            }
            return new PolynomialOverFiniteField(reminder, Mod, FieldChar);

        }
        public PolynomialOverFiniteField Sub(PolynomialOverFiniteField firstArg, PolynomialOverFiniteField secondArg)
        {
            int firstDeg = firstArg.Deg;
            int secondDeg = secondArg.Deg;

            var maxDeg = Math.Max(firstDeg, secondDeg);
            if (maxDeg == -1)
            {
                return new PolynomialOverFiniteField(new Polynomial[] { new Polynomial(new BigInteger[1])});
            }

            var result = new Polynomial[maxDeg + 1];
            if (firstDeg > secondDeg)
            {
                int i;
                for (i = 0; i <= secondDeg; i++)
                {
                    result[i] = _polynomialMath.Sub(firstArg[i] , secondArg[i]);
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
                    result[i] = _polynomialMath.Sub(firstArg[i] , secondArg[i]);
                }
                for (; i <= secondDeg; i++)
                {
                    result[i] =_polynomialMath.ConstMul(-1 ,secondArg[i]);
                }

            }

            return new PolynomialOverFiniteField(result, Mod,FieldChar);
        }
        public PolynomialOverFiniteField Add(PolynomialOverFiniteField firstArg, PolynomialOverFiniteField secondArg)
        {
            int firstDeg = firstArg.Deg;
            int secondDeg = secondArg.Deg;

            var maxDeg = Math.Max(firstDeg, secondDeg);
            if (maxDeg == -1)
            {
                return new PolynomialOverFiniteField(new Polynomial[] { new Polynomial(new BigInteger[1]) });
            }

            var result = new Polynomial[maxDeg + 1];
            if (firstDeg > secondDeg)
            {
                int i;
                for (i = 0; i <= secondDeg; i++)
                {
                    result[i] = _polynomialMath.Add(firstArg[i], secondArg[i]);
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
                    result[i] = _polynomialMath.Add(firstArg[i], secondArg[i]);
                }
                for (; i <= secondDeg; i++)
                {
                    result[i] = secondArg[i];
                }

            }

            return new PolynomialOverFiniteField(result, Mod, FieldChar);
        }

    }
}
