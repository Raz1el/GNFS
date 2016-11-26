using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using GNFS.GNFS.Polynomial_generator;
using GNFS.Polynomial_arithmetic.GaloisFieldLib;

namespace GNFS.GNFS.Polynomial_selector
{
    public interface IPolynomialGenerator
    {
        PolynomialInfo GeneratePolynomial();
    }
}
