using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GNFS.Polynomial_arithmetic
{
    public class Polynomial
    {
        private int _deg;
        private BigInteger[] _coefficients;


        public BigInteger[] Coefficients
        {
            get { return _coefficients; }
        }

        public Polynomial(BigInteger[] arr):this(arr,-1)
        {
        }

        public Polynomial(BigInteger[] arr, BigInteger mod)
        {
            if (mod < 2)
            {
                _deg = arr.Length - 1;
                for (int i = _deg; i >= 0; i--)
                {
                    if (arr[i] == 0)
                        _deg--;
                    else
                        break;
                }

                var length = _deg + 1;
                if (length == 0)
                {
                    length++;
                }
                _coefficients = new BigInteger[length];
                for (int i = 0; i <= _deg; i++)
                {
                    _coefficients[i] = arr[i];
                }
            }
            else
            {
                _deg = arr.Length - 1;
                for (int i = _deg; i >= 0; i--)
                {
                    if (arr[i] % mod == 0)
                        _deg--;
                    else
                        break;
                }

                var length = _deg + 1;
                _coefficients = new BigInteger[length];
                for (int i = 0; i <= _deg; i++)
                {
                    _coefficients[i] = arr[i] % mod;
                }
            }
           
        }

        public Polynomial Reduce(BigInteger mod)
        {
            var reducedPoly = new BigInteger[Deg + 1];
            for (int i = 0; i <= Deg; i++)
            {
                reducedPoly[i] = _coefficients[i]%mod;
            }
            return new Polynomial(reducedPoly);
        }


        public int Deg
        {
            get { return _deg; }
        }

        public BigInteger this[int index]
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

        public BigInteger Value(BigInteger point)
        {
            BigInteger bn, bn_1 = 0;

            bn = this[Deg];
            for (int i = 1; i <= Deg; i++)
            {
                bn_1 = point*bn + this[Deg - i];
                bn = bn_1;
            }
            return bn_1;
        }


        public override string ToString()
        {
            var res = new StringBuilder();
            if (_deg == -1)
            {
                return "0";
            }
            for (int index = _deg; index >= 0; index--)
            {
                if (_coefficients[index] == 0)
                    continue;


                if (_coefficients[index] == 1)
                {
                    if (index == _deg)
                    {
                        if (index == 0)
                        {
                            res.Append("1");
                        }
                        else if (index == 1)
                        {
                            res.Append("x");
                        }
                        else
                        {
                            res.Append("x^" + index);
                        }
                    }
                    else
                    {
                        if (index == 0)
                        {
                            if (_coefficients[index] > 0)
                            {
                                res.Append("+1");
                            }
                            else
                            {
                                res.Append("-1");
                            }
                        }
                        else if (index == 1)
                        {
                            if (_coefficients[index] > 0)
                            {
                                res.Append("+" + "x");
                            }
                            else
                            {
                                res.Append("-" + "x");
                            }
                        }
                        else
                        {
                            if (_coefficients[index] > 0)
                            {
                                res.Append("+" + "x^" + index);
                            }
                            else
                            {
                                res.Append("-" + "x^" + index);
                            }
                        }
                    }
                }
                else
                {
                    if (index == _deg)
                    {
                        if (index == 0)
                        {
                            res.Append(_coefficients[index]);
                        }
                        else if (index == 1)
                        {
                            res.Append(_coefficients[index] + "x");
                        }
                        else
                        {
                            res.Append(_coefficients[index] + "x^" + index);
                        }
                    }
                    else
                    {
                        if (index == 0)
                        {
                            if (_coefficients[index] > 0)
                            {
                                res.Append("+" + _coefficients[index]);
                            }
                            else
                            {
                                res.Append(+_coefficients[index]);
                            }
                        }
                        else if (index == 1)
                        {
                            if (_coefficients[index] > 0)
                            {
                                res.Append("+" + _coefficients[index] + "x");
                            }
                            else
                            {
                                res.Append(_coefficients[index] + "x");
                            }
                        }
                        else
                        {
                            if (_coefficients[index] > 0)
                            {
                                res.Append("+" + _coefficients[index] + "x^" + index);
                            }
                            else
                            {
                                res.Append(+_coefficients[index] + "x^" + index);
                            }
                        }
                    }
                }
            }
            return res.ToString();
        }

        public static bool operator ==(Polynomial firstArg, Polynomial secondArg)
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

        public static bool operator !=(Polynomial firstArg, Polynomial secondArg)
        {
            return !(firstArg == secondArg);
        }

        public override bool Equals(object obj)
        {
            return this == (Polynomial) obj;
        }
    }
    
}
