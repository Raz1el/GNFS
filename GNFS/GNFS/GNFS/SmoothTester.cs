using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using GNFS.GNFS.Factor_bases;
using GNFS.Polynomial_arithmetic;

namespace GNFS.GNFS
{
    public class SmoothTester
    {
        public bool IsSmoothOverRationalFactorbase(BigInteger a,BigInteger b,BigInteger integerRoot,RationalFactorbase factorbase)
        {

            var element = a + b*integerRoot;
            if (element == 0)
                return false;
            for (int i = 0; i < factorbase.Elements.Count; i++)
            {
                if (element == 1 || element == -1)
                    return true;
                var currentPair = factorbase.Elements[i];
                while (element%currentPair.Item2 == 0)
                {
                    element /= currentPair.Item2;
                }
                
            }
            return element == 1 || element == -1;
        }
        public bool IsSmoothOverAlgebraicFactorbase(BigInteger a, BigInteger b, Polynomial polynomial, AlgebraicFactorbase factorbase)
        {
            var normCalculator=new FirstDegreeElementsNormCalculator(polynomial,b);
            var element = normCalculator.CalculateNorm(a);
            if (element == 0)
                return false;
            for (int i = 0; i < factorbase.Elements.Count; i++)
            {
                if (element == 1 || element == -1)
                    return true;
                var currentPair = factorbase.Elements[i];
                while (element % currentPair.Item2 == 0)
                {
                    element /= currentPair.Item2;
                }

            }
            return element == 1 || element == -1;
        }
    }
}
