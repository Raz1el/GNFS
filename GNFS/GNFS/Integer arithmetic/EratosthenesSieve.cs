using System;

namespace GNFS.Integer_arithmetic
{
    public class EratosthenesSieve
    {
        public ulong[] GetPrimes(ulong lowerBound, ulong upperBound)
        {
            ValidateBounds(ref lowerBound,ref upperBound);


            ulong intervalLength = ((upperBound - lowerBound) / 2) + 1;
            var primesCount = intervalLength;
            bool[] isNotPrime = new bool[intervalLength];
            for (ulong d = 3; d * d <= upperBound; d = d + 2)
            {
                ulong j;
                ulong r = lowerBound % d;
                if (r != 0)
                {
                    if (r % 2 == 1)
                    {
                        j = (d - r) / 2;
                    }
                    else
                    {
                        j = d - r / 2;
                    }
                }
                else
                {
                    j = 0;
                }
                if (d >= lowerBound)
                {
                    j = j + d;
                }
                for (ulong i = j; i < intervalLength; i += d)
                {
                    if (!isNotPrime[i])
                    {
                        primesCount--;
                    }
                    isNotPrime[i] = true;
                }
                if (d % 6 == 1)
                    d += 2;
            }
            ulong[] result = new ulong[primesCount];
            var nextIndex = 0;
            for (ulong i = 0; i < intervalLength; i++)
            {
                if (!isNotPrime[i])
                {
                    ulong p = lowerBound + 2 * i;
                    result[nextIndex++] = p;
                }
            }
            return result;
        }

        void ValidateBounds(ref ulong lowerBound,ref ulong upperBound)
        {
            if (lowerBound >= upperBound || lowerBound < 2)
            {
                throw new ArgumentException();
            }
            if (lowerBound % 2 == 0)
                lowerBound++;
            if (upperBound % 2 == 0)
                upperBound--;
        }
    }
}
