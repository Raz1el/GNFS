using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GNFS.ECM
{
    public class MontgomeryArithmetic
    {

        public ProjectivePoint Add(ProjectivePoint point1, ProjectivePoint point2, ProjectivePoint point3,Curve curve)
        {
            var T1 = (point1.X * point2.X - point1.Z * point2.Z) % curve.Mod;
            var T2 = (point1.X * point2.Z - point1.Z * point2.X)% curve.Mod ;
            var newX = (point3.Z * T1 * T1);
            var newZ = (point3.X * T2 * T2);
            return new ProjectivePoint(newX % curve.Mod, 0, newZ % curve.Mod);
        }
        public ProjectivePoint Double(ProjectivePoint point,Curve curve)
        {
            var XX = point.X * point.X ;
            var ZZ = point.Z * point.Z ;
            var XZ = point.X * point.Z ;
            var newX = (XX - ZZ) % curve.Mod;
            newX = newX * newX ;
            var tmp = (XX + curve.A * XZ + ZZ) % curve.Mod;
            var newZ = 4 * XZ * tmp;
            return new ProjectivePoint(newX % curve.Mod, 0, newZ % curve.Mod);
        }

        public ProjectivePoint MontgomeryMul(ProjectivePoint point,ulong power,Curve curve)
        {
            if (power == 1)
                return point;
            if (power == 2)
            {
                return Double(point,curve);
            }
            var binary = Convert.ToString((long)power, 2);
            var U = point;
            var T =Double(point, curve);
            for (int i = 1; i <= binary.Length - 1; i++)
            {
                if (binary[i] == '1')
                {
                    U = Add(T, U, point,curve);
                    T = Double(T,curve);
                }
                else
                {
                    T =Add(U, T, point,curve);
                    U = Double(U,curve);
                }
            }
            return U;
        }
    }
}
