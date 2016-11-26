using System;
using System.Numerics;

namespace GNFS.Integer_arithmetic
{
    public class IntegerSquareRoot
    {
        public BigInteger Sqrt(BigInteger number)
        {
            if (number == 0) return 0;
            if (number > 0)
            {
                int bitLength = Convert.ToInt32(Math.Ceiling(BigInteger.Log(number, 2)));
                BigInteger root = BigInteger.One << (bitLength / 2);

                while (!IsSqrt(number, root))
                {
                    root += number / root;
                    root /= 2;
                }
                
                return root;
            }
            throw new ArithmeticException("NaN");
        }

     

        private bool IsSqrt(BigInteger n, BigInteger root)
        {
            BigInteger lowerBound = root * root;
            BigInteger upperBound = (root + 1) * (root + 1);

            return (n >= lowerBound && n < upperBound);
        }
    }
}
