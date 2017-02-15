namespace GNFS.Linear_algebra
{
    public class Matrix
    {
        private readonly int[,] _matrix;

        public int RowsCount => _matrix.GetLength(0);
        public int ColumnsCount => _matrix.GetLength(1);

        public Matrix(int[,] matrix)
        {
            _matrix = matrix;
        }

        public int this[int row, int column]
        {
            get { return _matrix[row, column]; }
            set { _matrix[row, column] = value; }
        }
    }
}
