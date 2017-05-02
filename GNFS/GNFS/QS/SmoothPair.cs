using System.Collections.Generic;
using System.Numerics;

namespace GNFS.QS
{
    public class SmoothPair
    {
        //Пара (x,y), где x - полный квадрат, а y - гладкое число

        BigInteger _x;
        Dictionary<long,int> _y;


        public BigInteger X
        {
            get { return _x; }
        }
        public Dictionary<long,int> Y
        {
            get { return _y; }
        }

        public SmoothPair(BigInteger x, Dictionary<long, int> y)
        {
            _x = x;
            _y = y;
        }
    }
}
