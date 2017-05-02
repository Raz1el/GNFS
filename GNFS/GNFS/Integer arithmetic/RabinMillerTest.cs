using System;
using System.Numerics;

namespace GNFS.Integer_arithmetic
{
    public class RabinMillerTest
    {
        Random _prng;

        public RabinMillerTest()
        {
            _prng=new Random();
        }
        public bool IsStrongPseudoPrime(BigInteger number, int iterationsCount)
        {
            if (number < 2)
                return false;
            if (number == 2 || number == 3)
                return true;
            BigInteger q = number - 1;
            uint s = 0;
            while (q % 2 == 0)
            {
                s++;
                q = q / 2;
            }
            for (int i = 0; i < iterationsCount; i++)
            {
                BigInteger b = GenerateNumber(number-2);
                b = BigInteger.ModPow(b, q, number);
                if (b == 1 || b == number - 1)
                    continue;
                bool nextStep = false;
                for (int j = 0; j < s; j++)
                {
                    b = BigInteger.ModPow(b, 2, number);
                    if (b == 1)
                        return false;
                    if (b == number - 1)
                    {
                        nextStep = true;
                        break;
                    }
                }
                if (nextStep)
                    continue;
                return false;
            }
            return true;
        }
        int GenerateNumber(BigInteger upperBound)
        {
            if (upperBound > int.MaxValue)
            {
                return _prng.Next(2, int.MaxValue);
            }
            else
            {
                return _prng.Next(2, (int)upperBound);
            }

          
        }
    }
}
