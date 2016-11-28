using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using GNFS.GNFS.Factor_bases;
using GNFS.Polynomial_arithmetic.GaloisFieldLib;

namespace GNFS.GNFS.Sieve
{
    public class SieveOptions
    {
        public BigInteger UpperBound { get; set; }
        public BigInteger LowerBound { get; set; }
        public BigInteger IntegerRoot { get; set; }
        public Polynomial Polynomial { get; set; }

        public AlgebraicFactorbase AlgebraicFactorbase { get; set; }
        public RationalFactorbase RationalFactorbase { get; set; }
        public ulong IntervalLength { get { return (ulong)(UpperBound - LowerBound); } }

        public SieveOptions(BigInteger upperBound, BigInteger lowerBound, AlgebraicFactorbase algebraicFactorbase, RationalFactorbase rationalFactorbase, Polynomial polynomial,BigInteger integerRoot)
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
