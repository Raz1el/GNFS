using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GNFS.Integer_arithmetic
{
    public class NextPrime
    {
        public  BigInteger GetNext(BigInteger number)
        {
            var primeTest=new RabinMillerTest();
            number++;
            if (number < 2)
                return 2;
            if (number % 2 == 0)
                number++;
            while (!primeTest.IsStrongPseudoPrime(number, 10))
            {
                number += 2;
            }
            return number;
        }
    }
}
