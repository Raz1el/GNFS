using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using GNFS.GNFS.Factor_bases.Builders;
using GNFS.GNFS.Polynomial_generator;
using GNFS.GNFS.Sieve;
using GNFS.Integer_arithmetic;
using GNFS.Linear_algebra;
using GNFS.Polynomial_arithmetic;

namespace GNFS.GNFS
{
    public class Snfs
    {

        SpecialNumber _number;
        PolynomialInfo _polyInfo;
        IPolynomialGenerator _generator;
        int _kerDim;
        long _primeBound;

        public Snfs(SpecialNumber number,IPolynomialGenerator generator)
        {
            _number = number;
            _generator = generator;
            _primeBound =1000;
            _kerDim = 10;
        }



        public BigInteger FindFactor()
        {
            _polyInfo = _generator.GeneratePolynomial();
            var rootFinder=new BruteforceRootFinder();
            var algFbBuilder=new AlgebraicFactorbaseBuilder(_polyInfo.Polynomial, rootFinder, _primeBound );
            var rationalFbBuilder=new RationalFactorbaseBuilder(_polyInfo.Root,_primeBound);
            var quadraticCharFbBuilder=new QuadraticCharactersBuilder(_polyInfo.Polynomial, rootFinder, _primeBound,_primeBound+4);

            var rationalFb = rationalFbBuilder.Build();



            var quadraticCharFb = quadraticCharFbBuilder.Build();
            var algFb = algFbBuilder.Build();
            var sieve=new SimpleSieveAlgorithm();
            Console.Clear();
            var pairs =
                sieve.Sieve(
                    (ulong) (algFb.Elements.Count + rationalFb.Elements.Count + quadraticCharFb.Elements.Count+_kerDim),
                    new SieveOptions(1400, -1400, algFb, rationalFb, _polyInfo.Polynomial, _polyInfo.Root));


            var matrixBuilder=new MatrixBuilder();
            var matrix = matrixBuilder.Build(pairs,rationalFb,algFb,quadraticCharFb,_polyInfo.Root,_polyInfo.Polynomial);
            var matrixSolver=new GaussianElimination();
            var solutions = matrixSolver.Solve(matrix);
            var polyMath = new PolynomialMath(_number.Value());
            var df=new PolynomialDerivative().Derivative(_polyInfo.Polynomial);
            var sqrDf = polyMath.Mul(df, df);

            foreach (var solution in solutions)
            {
                
                var sqr=new Polynomial(new BigInteger[] {1});
                for (int i = 0; i < solution.Length; i++)
                {
                    if (solution[i] == 1)
                    {
                        var tmp=new Polynomial(new BigInteger[] {pairs[i].Item1,pairs[i].Item2});
                        sqr = polyMath.Rem(polyMath.Mul(tmp, sqr),_polyInfo.Polynomial);

                    }
                    
                }
               
                sqr =polyMath.Rem( polyMath.Mul(sqrDf, sqr),_polyInfo.Polynomial);
                var val = sqr.Value(_polyInfo.Root);
                var y = new IntegerSquareRoot().Sqrt(val);

                if (y*y == sqr.Value(_polyInfo.Root))
                {
                    throw new Exception();
                }
            }


            throw new NotImplementedException();
        }
    }
}
