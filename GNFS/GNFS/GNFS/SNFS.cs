using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using GNFS.GNFS.Polynomial_generator;
using GNFS.GNFS.Polynomial_selector;
using GNFS.Polynomial_arithmetic.GaloisFieldLib;

namespace GNFS.GNFS
{
    public class Snfs
    {

        SpecialNumber _number;
        PolynomialInfo _polyInfo;
        IPolynomialGenerator _generator;

        public Snfs(SpecialNumber number,IPolynomialGenerator generator)
        {
            _number = number;
            _generator = generator;
        }



        public BigInteger FindFactor()
        {
            _polyInfo = _generator.GeneratePolynomial();


            throw new NotImplementedException();
        }
    }
}
