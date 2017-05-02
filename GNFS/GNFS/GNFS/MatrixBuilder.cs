using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using GNFS.GNFS.Factor_bases;
using GNFS.Integer_arithmetic;
using GNFS.Linear_algebra;
using GNFS.Linear_algebra.tmp;
using GNFS.Polynomial_arithmetic;

namespace GNFS.GNFS
{
    public class MatrixBuilder
    {
        public Matrix Build(List<Pair> pairs,RationalFactorbase rfb,AlgebraicFactorbase afb,QuadraticCharacters qfb,BigInteger integerRoot,Polynomial polynomial)
        {


            var matrix = new int[rfb.Elements.Count + afb.Elements.Count + qfb.Elements.Count + 1, pairs.Count];
            for (int index = 0; index < pairs.Count; index++)
            {
                var pair = pairs[index];
                var rational = pair.Item1 + pair.Item2 * integerRoot;
                if (rational < 0)
                {
                    matrix[0, index] = 1;
                    rational = -rational;
                }
                for (int i = 0; i < rfb.Elements.Count; i++)
                {
                    while (rational % rfb.Elements[i].Item2 == 0)
                    {
                        matrix[i + 1, index]++;
                        rational /= rfb.Elements[i].Item2;
                    }
                    matrix[i + 1, index] %= 2;
                    if (rational == 1)
                    {
                        break;
                    }

                }

                var normCalculator = new FirstDegreeElementsNormCalculator(polynomial, pair.Item2);
                var algebraic = normCalculator.CalculateNorm(pair.Item1);

                for (int i = 0; i < afb.Elements.Count; i++)
                {
                    var primeIdeal = afb.Elements[i];
                    while (algebraic % primeIdeal.Item2 == 0 && (pair.Item1 + pair.Item2 * primeIdeal.Item1) % primeIdeal.Item2 == 0)
                    {
                        matrix[i + rfb.Elements.Count, index]++;
                        algebraic /= afb.Elements[i].Item2;
                    }
                    matrix[i + rfb.Elements.Count, index] %= 2;
                    if (algebraic == 1 || algebraic == -1)
                    {
                        break;
                    }
                }

                var jacobiSymbol = new JacobiSymbol();

                for (int i = 0; i < qfb.Elements.Count; i++)
                {
                    if (jacobiSymbol.Calculate(pair.Item1 + pair.Item2 * qfb.Elements[i].Item1, qfb.Elements[i].Item2) == -1)
                    {
                        matrix[rfb.Elements.Count + afb.Elements.Count + 1 + i, index] = 1;
                    }
                }
            }


            return new Matrix(matrix);
        }
        public BinaryMatrix TmpBuild(List<Pair> pairs, RationalFactorbase rfb, AlgebraicFactorbase afb, QuadraticCharacters qfb, BigInteger integerRoot, Polynomial polynomial)
        {


            var matrix = new int[rfb.Elements.Count + afb.Elements.Count + qfb.Elements.Count + 1, pairs.Count];
            for (int index = 0; index < pairs.Count; index++)
            {
                var pair = pairs[index];
                var rational = pair.Item1 + pair.Item2 * integerRoot;
                if (rational < 0)
                {
                    matrix[0, index] = 1;
                    rational = -rational;
                }
                for (int i = 0; i < rfb.Elements.Count; i++)
                {
                    while (rational % rfb.Elements[i].Item2 == 0)
                    {
                        matrix[i + 1, index]++;
                        rational /= rfb.Elements[i].Item2;
                    }
                    matrix[i + 1, index] %= 2;
                    if (rational == 1)
                    {
                        break;
                    }

                }
                if (rational != 1)
                {
                    throw new Exception();
                }
                var normCalculator = new FirstDegreeElementsNormCalculator(polynomial, pair.Item2);
                var algebraic = normCalculator.CalculateNorm(pair.Item1);

                for (int i = 0; i < afb.Elements.Count; i++)
                {
                    var primeIdeal = afb.Elements[i];
                    while (algebraic % primeIdeal.Item2 == 0 && (pair.Item1 + pair.Item2 * primeIdeal.Item1) % primeIdeal.Item2 == 0)
                    {
                        matrix[i + rfb.Elements.Count, index]++;
                        algebraic /= afb.Elements[i].Item2;
                    }
                    matrix[i + rfb.Elements.Count, index] %= 2;
                    if (algebraic == 1 || algebraic == -1)
                    {
                        break;
                    }
                }
                var jacobiSymbol = new JacobiSymbol();

                for (int i = 0; i < qfb.Elements.Count; i++)
                {
                    if (jacobiSymbol.Calculate(pair.Item1 + pair.Item2 * qfb.Elements[i].Item1, qfb.Elements[i].Item2) == -1)
                    {
                        matrix[rfb.Elements.Count + afb.Elements.Count + 1 + i, index] = 1;
                    }
                }
            }


            return new BinaryMatrix(matrix);
        }
    }
}
