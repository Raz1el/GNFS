using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GNFS.Linear_algebra
{
    public interface IMatrixSolver
    {
        List<int[]> Solve(Matrix matrix);
    }
}
