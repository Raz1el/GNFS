using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GNFS.Linear_algebra
{
    public class GaussianEliminationOverGf2:IMatrixSolver
    {
        public List<int[]> Solve(Matrix matrix)
        {

            var c = new int[matrix.RowsCount];
            var d = new int[matrix.ColumnsCount];
            int r = 0;

            for (int i = 0; i < matrix.RowsCount; i++)
            {
                c[i] = -1;

            }

            for (int k = 0;  k< matrix.ColumnsCount; k++)
            {

                int j = -1;
                for (int index=0; index < matrix.RowsCount; index++)
                {
                    if (matrix[index, k] !=0&&c[index] ==-1)
                    {
                        j = index;
                        break;
                    }
                }
                if (j != -1)
                {

                    for (int i= 0; i < matrix.RowsCount; i++)
                    {
                        if (i != j)
                        {
                            var tmp = matrix[i,k];
                            if (tmp != 0)
                            {
                                matrix[i, k] = 0;
                                for (int s = k + 1; s < matrix.ColumnsCount; s++)
                                {
                                    matrix[i, s] = matrix[i, s] ^ (matrix[j, s]);
                                }
                            }
                        }
                    }
                    c[j] = k;
                    d[k] = j;

                }
                else
                {
                    r++;
                    d[k] = -1;
                }

            }
            var res=new List<int[]>();
            for (int k = 0; k < matrix.ColumnsCount; k++)
            {
                if (d[k] == -1)
                {
                    var vector = new int[matrix.ColumnsCount];
                    for (int i = 0; i < matrix.ColumnsCount; i++)
                    {
                        if (d[i] >= 0)
                        {
                            vector[i] = matrix[d[i], k];
                        }
                        else if (i == k)
                        {
                            vector[i] = 1;
                        }

                    }
                    res.Add(vector);
                }
            }

            return res;
        }

        public void CheckSolutions(Matrix matrix, List<int[]> solutions)
        {
            for (int i = 0; i < solutions.Count; i++)
            {
                var solution = solutions[i];
                for (int j = 0; j < matrix.RowsCount; j++)
                {
                    var shouldBeZero = 0;
                    for (int k = 0; k < matrix.ColumnsCount; k++)
                    {
                        shouldBeZero += matrix[j, k] * solution[k];
                    }
                    shouldBeZero %= 2;
                    if (shouldBeZero != 0) throw new Exception();
                }
            }
        }



    }
}
