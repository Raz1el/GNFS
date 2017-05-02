using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using GNFS.GNFS.Factor_bases;
using GNFS.Polynomial_arithmetic;

namespace GNFS.GNFS.Sieve
{
    public class SieveOptions
    {
        public long UpperBound { get; set; }
        public long LowerBound { get; set; }
        public BigInteger IntegerRoot { get; set; }
        public Polynomial Polynomial { get; set; }

        public AlgebraicFactorbase AlgebraicFactorbase { get; set; }
        public RationalFactorbase RationalFactorbase { get; set; }
        public long IntervalLength { get { return (long)(UpperBound - LowerBound); } }

        public SieveOptions(long upperBound, long lowerBound, AlgebraicFactorbase algebraicFactorbase, RationalFactorbase rationalFactorbase, Polynomial polynomial,BigInteger integerRoot)
        {
            UpperBound = upperBound;
            LowerBound = lowerBound;
            AlgebraicFactorbase = algebraicFactorbase;
            RationalFactorbase = rationalFactorbase;
            Polynomial = polynomial;
            IntegerRoot = integerRoot;
        }

    }
}
