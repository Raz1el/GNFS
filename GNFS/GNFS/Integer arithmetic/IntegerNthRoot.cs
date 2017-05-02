using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GNFS.Integer_arithmetic
{
    public class IntegerNthRoot
    {
        public BigInteger Root(BigInteger number,int root)
        {
            if (number == 0) return 0;
            if (number > 0)
            {
                int bitLength = Convert.ToInt32(Math.Ceiling(BigInteger.Log(number, 2)))/root;
                BigInteger xi = BigInteger.One << (bitLength);
                BigInteger tmp = 1;
                while (bitLength>0)
                {
                    tmp = xi*(root-1) + number / BigInteger.Pow(xi, root - 1);
                    tmp /=root;
                    if (tmp == xi)
                    {
                        return tmp;
                    }
                    xi = tmp;
                    bitLength --;
                }
                return xi;
            }
            else if (number < 0 && root % 2 != 0)
            {
                number = -number;
                int bitLength = Convert.ToInt32(Math.Ceiling(BigInteger.Log(number, 2))) / root;
                BigInteger xi = BigInteger.One << (bitLength);
                BigInteger tmp = 1;
                while (bitLength > 0)
                {
                    tmp = xi * (root - 1) + number / BigInteger.Pow(xi, root - 1);
                    tmp /= root;
                    if (tmp == xi)
                    {
                        return -tmp;
                    }
                    xi = tmp;
                    bitLength--;
                }
                return xi;


                return -xi;
            }

            throw new ArithmeticException("NaN");
        }
    }
}
