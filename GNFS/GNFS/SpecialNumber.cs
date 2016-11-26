using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace GNFS
{
    public class SpecialNumber
    {
        // Число вида base^exp-const

        int _base;
        int _exp;
        int _const;
        bool _isInitialized;
        BigInteger _value;

        public SpecialNumber(int baseValue, int expValue, int constValue)
        {
            _base = baseValue;
            _exp = expValue;
            _const = constValue;
        }

        public int Base
        {
            get { return _base; }
        }

        public int Exp
        {
            get { return _exp; }
        }

        public int Const
        {
            get { return _const; }
        }

        public BigInteger Value()
        {
            if (!_isInitialized)
            {
                _value = BigInteger.Pow(_base, _exp) - _const;
                _isInitialized = true;
            }
            return _value;
        }
    }
}
