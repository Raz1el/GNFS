using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GNFS.Linear_algebra.tmp
{
    public class BinaryMatrix
    {
        int[,] _matrix;
        int[] _rowOrder;

        int _n;
        int _m;


        public int M { get { return _m; } }
        public int N { get { return _n; } }

        public int ElementAt(int i, int j)
        {
            return _matrix[i, j];
        }
        public BinaryMatrix(int[,] matrix):this(matrix,false)
        {
        }
        public BinaryMatrix(int[,] matrix, bool withFilter)
        {

            _n = matrix.GetLength(1);
            _m = matrix.GetLength(0);
            _matrix = matrix;
            for (int i = 0; i < _m; i++)
            {
                for (int j = 0; j < _n; j++)
                {
                    _matrix[i, j] = _matrix[i, j]  % 2;
                }
            }
            _rowOrder = new int[_n];
            for (int i = 0; i < _n; i++)
            {
                _rowOrder[i] = i;
            }


        }


        public List<int[]> Solve()
        {


            var temp = new int[_m,_n];
            for (int i = 0; i < _m; i++)
            {
                for (int j = 0; j < _n; j++)
                {
                    temp[i, j] = _matrix[i, j];
                }
            }






            int[] nonzeroElements = new int[_m];
            int[] leadElements = new int[_m];
            int nextRow = _n - 1;
            int rank = 0;
            for (int i = 0; i < _m; i++)
            {
                leadElements[i] = -1;
            }
            for (int i = 0; i < _n; i++)
            {
                int k = 0;
                for (; k < _m; k++)
                {
                    if (_matrix[k, _rowOrder[i]] != 0 && nonzeroElements[k] == 0)
                    {
                        leadElements[i] = k;
                        nonzeroElements[k] = 1;
                        rank++;
                        break;
                    }
                }
                if (k != _m)
                {
                    for (int j = 0; j < _m; j++)
                    {
                        if (j != k && (_matrix[j, _rowOrder[i]] != 0))
                        {
                            for (int r = i; r < _n; r++)
                            {
                                _matrix[j, _rowOrder[r]] = (_matrix[j, _rowOrder[r]] + _matrix[k, _rowOrder[r]]) % 2;
                            }
                        }


                    }
                }
                else
                {

                    var tmp = _rowOrder[nextRow];
                    _rowOrder[nextRow] = _rowOrder[i];
                    _rowOrder[i] = tmp;
                    nextRow--;
                    if (nextRow <= i)
                    {
                        break;
                    }
                    i--;

                }

            }

            var results = new List<int[]>();
            for (int j = rank; j < _n; j++)
            {
                int[] result = new int[_n];
                result[_rowOrder[j]] = 1;

                for (int i = 0; i < rank; i++)
                {
                    int index = leadElements[i];


                    int res = _matrix[index, _rowOrder[j]];

                    result[_rowOrder[i]] = (res) % 2;
                }
                results.Add(result);
            }

            _matrix = temp;
            return results;


        }
        public override string ToString()
        {
            var strb = new StringBuilder();
            for (int i = 0; i < _m; i++)
            {
                for (int j = 0; j < _n; j++)
                {
                    strb.Append(_matrix[i, _rowOrder[j]] + " ");
                }
                strb.AppendLine();
            }
            return strb.ToString();
        }

        internal void Check(int[] vector)
        {
            for (int j = 0; j < _m; j++)
            {
                var res = 0;
                for (int i = 0; i < _n; i++)
                {
                    res += _matrix[j, _rowOrder[i]] * (vector[_rowOrder[i]]);
                }
                res %= 2;
                if (res != 0)
                {
                    throw new Exception();
                }
            }
        }

    }
}
