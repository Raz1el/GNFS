using System;
using System.Collections.Generic;
using System.Linq;

namespace GNFS.Linear_algebra
{
    public class GaussianElimination : IMatrixSolver
    {
        private Matrix _matrix;
        private int _algorithmIteration;

        /// <summary>
        /// Алгоритм Гаусса
        /// </summary>
        /// <param name="matrix">Должна содержать только нули и единицы. Строк меньше, чем столбцов.</param>
        public List<int[]> Solve(Matrix matrix)
        {
            CloneMatrix(matrix);

            for (_algorithmIteration = 0; _algorithmIteration < matrix.RowsCount; _algorithmIteration++)
            {
                var isOnlyZeros = !LiftRowWithNonZeroDiagonalElement();
                if (isOnlyZeros) continue;

                PerformAddition();
            }
            var basis = FindBasis();
            return basis;
        }

        private List<int[]> FindBasis()
        {
            var setOfEquations = GetSetOfEquations();
            var countOfSolutions = _matrix.ColumnsCount - _matrix.RowsCount;
            var basis = new List<int[]>(countOfSolutions);
            for (int i = 0; i < countOfSolutions; i++)
            {
                var solution = new int[_matrix.ColumnsCount];
                solution[_matrix.ColumnsCount - i - 1] = 1;
                for (int j = setOfEquations.Count - 1; j >= 0; j--)
                {
                    if (setOfEquations[j][j] == 0) continue;
                    var temp = new int[_matrix.ColumnsCount];
                    Array.Copy(setOfEquations[j], temp, temp.Length);
                    for (int k = 0; k < setOfEquations[j].Length; k++)
                    {
                        temp[k] *= solution[k];
                    }
                    solution[j] = temp.Sum() % 2;
                }
                basis.Add(solution);
            }
            return basis;
        }

        private List<int[]> GetSetOfEquations()
        {
            var list = new List<int[]>(_matrix.RowsCount);
            for (int i = 0; i < _matrix.RowsCount; i++)
            {
                var equation = new int[_matrix.ColumnsCount];
                if (_matrix[i, i] == 0)
                {
                    list.Add(equation);
                    continue;
                }
                for (int j = i; j < _matrix.ColumnsCount; j++)
                {
                    equation[j] = _matrix[i, j];
                }
                list.Add(equation);
            }
            return list;
        }

        private void PerformAddition() //сложение и вычитание по модулю два эквивалентны
        {
            for (int i = _algorithmIteration + 1; i < _matrix.RowsCount; i++)
            {
                if (_matrix[i, _algorithmIteration] == 0) continue;
                AddRows(_algorithmIteration, i);
            }
        }

        private void AddRows(int main, int slave)
        {
            for (int i = _algorithmIteration; i < _matrix.ColumnsCount; i++)
            {
                _matrix[slave, i] += _matrix[main, i];
                if (_matrix[slave, i] == 2) _matrix[slave, i] = 0;
            }
        }

        private bool LiftRowWithNonZeroDiagonalElement()
        {
            if (_matrix[_algorithmIteration, _algorithmIteration] != 0) return true;

            for (int i = _algorithmIteration + 1; i < _matrix.RowsCount; i++)
            {
                if (_matrix[i, _algorithmIteration] == 0) continue;

                SwapRows(_algorithmIteration, i);
                return true;
            }
            return false;
        }

        private void SwapRows(int first, int second)
        {
            for (int i = 0; i < _matrix.ColumnsCount; i++)
            {
                var temp = _matrix[first, i];
                _matrix[first, i] = _matrix[second, i];
                _matrix[second, i] = temp;
            }
        }

        private void CloneMatrix(Matrix matrix)
        {
            var temp = new int[matrix.RowsCount, matrix.ColumnsCount];
            for (int i = 0; i < matrix.RowsCount; i++)
            {
                for (int j = 0; j < matrix.ColumnsCount; j++)
                {
                    temp[i, j] = matrix[i, j];
                }
            }

            _matrix = new Matrix(temp);
        }
    }
}
