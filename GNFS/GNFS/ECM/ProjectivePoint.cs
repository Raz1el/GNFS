using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GNFS.ECM
{
    public class ProjectivePoint
    {
        public BigInteger X { get; set; }
        public BigInteger Y { get; set; }
        public BigInteger Z { get; set; }


        public ProjectivePoint(BigInteger x,BigInteger y,BigInteger z)
        {
            X = x;
            Y = y;
            Z = z;
        }
    }
}
