using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GNFS.Polynomial_arithmetic
{
    public class PolynomialOverFiniteField
    {
        private int _deg;
        private Polynomial[] _coefficients;


        public Polynomial[] Coefficients
        {
            get { return _coefficients; }
        }


        public PolynomialOverFiniteField(Polynomial[] arr)
        {
            _deg = arr.Length - 1;
            for (int i = _deg; i >= 0; i--)
            {
                if ((arr[i]) == new Polynomial(new BigInteger[] { 0 }))
                    _deg--;
                else
                    break;
            }

            var length = _deg + 1;
            if (length == 0)
            {
                length++;
            }
            _coefficients = new Polynomial[length];
            for (int i = 0; i <= _deg; i++)
            {
                _coefficients[i] = arr[i];
            }
        }

        public PolynomialOverFiniteField(Polynomial[] arr,Polynomial mod,BigInteger fieldChar)
        {
            var polyMath=new PolynomialMath(fieldChar);
            _deg = arr.Length - 1;
            for (int i = _deg; i >= 0; i--)
            {
                if (polyMath.Rem(arr[i],mod) == new Polynomial(new BigInteger[] { 0 }))
                    _deg--;
                else
                    break;
            }

            var length = _deg + 1;
            if (length == 0)
            {
                length++;
            }
            _coefficients = new Polynomial[length];
            for (int i = 0; i <= _deg; i++)
            {
                _coefficients[i] =polyMath.Rem( arr[i],mod);
            }
        }


        public int Deg
        {
            get { return _deg; }
        }

        public Polynomial this[int index]
        {
            get
            {
                return _coefficients[index];
            }
            set
            {
                if (index > Deg)
                {
                    if (Deg == -1 && index == 0)
                    {
                        _coefficients[index] = value;
                        _deg = 1;
                    }
                    else
                    {
                        throw new InvalidOperationException();
                    }
                }
                else
                {
                    _coefficients[index] = value;
                }
            }
        }


        public override string ToString()
        {
            var result = new StringBuilder();
            return _coefficients.Aggregate(result,(x,y)=>x.Append(" ["+y?.ToString()+"] ")).ToString();

        }

        public static bool operator ==(PolynomialOverFiniteField firstArg, PolynomialOverFiniteField secondArg)
        {
            if (ReferenceEquals(firstArg, null) && ReferenceEquals(secondArg, null))
            {
                return true;
            }
            if (ReferenceEquals(firstArg, null) || ReferenceEquals(secondArg, null))
            {
                return false;
            }
            if (firstArg.Deg != secondArg.Deg)
            {
                return false;
            }
            for (int i = 0; i <= firstArg.Deg; i++)
            {
                if (firstArg._coefficients[i] != secondArg._coefficients[i])
                {
                    return false;
                }
            }
            return true;
        }

        public static bool operator !=(PolynomialOverFiniteField firstArg, PolynomialOverFiniteField secondArg)
        {
            return !(firstArg == secondArg);
        }

        public override bool Equals(object obj)
        {
            return this == (PolynomialOverFiniteField)obj;
        }
    }
}
