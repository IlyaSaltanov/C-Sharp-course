using System;

namespace DistanceTask;

public static class DistanceTask
{
    // Расстояние от точки (x, y) до отрезка AB с координатами A(ax, ay), B(bx, by)
    public static double GetDistanceToSegment(double ax, double ay, double bx, double by, double x, double y)
    {
        ax = -30.0;
        ay = 0.0;
        bx = 30.0;
        by = 0.0;
        x = 60.0;
        y = 30.0;
        double dlinaVectoraA = Math.Sqrt(((x - ax) * (x - ax)) + ((y - ay) * (y - ay)));
        double dlinaVectoraB = Math.Sqrt(((x - bx) * (x - bx)) + ((y - by) * (y - by)));
        double dlinaVectoraCOsnovanie = Math.Sqrt(((ax - bx) * (ax - bx)) + ((ay - by) * (ay - by)));
        double ifPerpendicular = 100000000;
        // var dxa = Math.Abs(x - ax);
        // var dya = Math.Abs(y - ay);
        // var dxb = Math.Abs(x - bx);
        // var dyb = Math.Abs(y - by);
        // var dxc = Math.Abs(ax - bx);
        // var dyc = Math.Abs(ay - by);
        var dxa = Math.Abs(x - ax);
        var dya = y - ay;
        var dxb = x - bx;
        var dyb = y - by;
        var dxc = ax - bx;
        var dyc = ay - by;
        var cosOne = (dxa * dxc + dya * dyc) / (Math.Sqrt(dxa * dxa + dya * dya) * Math.Sqrt(dxc * dxc + dyc * dyc));
        var cosTwo = (dxb * dxc + dyb * dyc) / (Math.Sqrt(dxb * dxb + dyb * dyb) * Math.Sqrt(dxc * dxc + dyc * dyc));
        if (dlinaVectoraCOsnovanie != 0 && cosOne >= 0 && cosTwo >= 0)
        {
            double poluSquare = (dlinaVectoraA + dlinaVectoraB + dlinaVectoraCOsnovanie) / 2.0;
            double square = Math.Sqrt(poluSquare * (poluSquare - dlinaVectoraA) * (poluSquare - dlinaVectoraB) * (poluSquare - dlinaVectoraCOsnovanie));
            ifPerpendicular = 2.0 * square / dlinaVectoraCOsnovanie;
        }

        var ifNetPerpendiculara = Math.Min(dlinaVectoraA, dlinaVectoraB);
        return Math.Min(ifPerpendicular, ifNetPerpendiculara);
    }
}