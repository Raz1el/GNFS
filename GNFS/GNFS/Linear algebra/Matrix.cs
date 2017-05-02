using System;

namespace GNFS.Linear_algebra
{
    public class Matrix
    {
        readonly int[,] _matrix;
        int[] _rowOrder;

        public int RowsCount => _matrix.GetLength(0);
        public int ColumnsCount => _matrix.GetLength(1);

        public Matrix(int[,] matrix)
        {
            _rowOrder=new int[matrix.GetLength(0)];
            for (int i = 0; i < _rowOrder.Length; i++)
            {
                _rowOrder[i] = i;
            }
            _matrix = matrix;
            
        }

        public int this[int row, int column]
        {
            get { return _matrix[_rowOrder[row], column]; }
            set { _matrix[_rowOrder[row], column] = value; }
        }

 

        internal void SwapRows(int k, int i)
        {
           if(k==i)
                return;
            var tmp = _rowOrder[k];
            _rowOrder[k] = _rowOrder[i];
            _rowOrder[i] = tmp;
        }

        public void Print()
        {
            Console.WriteLine("\n\n");
            for (int i = 0; i < RowsCount; i++)
            {
                for (int j = 0; j < ColumnsCount; j++)
                {
                    Console.Write(this[i, j] + " ");
                }
                Console.WriteLine();
            }
        }
    }
}
