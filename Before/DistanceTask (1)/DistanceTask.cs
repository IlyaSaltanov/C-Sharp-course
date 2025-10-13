using System;

namespace DistanceTask;

public static class DistanceTask
{
    // Расстояние от точки (x, y) до отрезка AB с координатами A(ax, ay), B(bx, by)
    public static double GetDistanceToSegment(double ax, double ay, double bx, double by, double x, double y)
    {
        // double DX = bx - ax;
        // double DY = by - ay;

        // double len = DX * DX + DY * DY;

        // if (len != 0)
        // {
        //     if (dlina(defaultVector) * dlina(newVector) * )
        // }
        double dlinaVectoraA = Math.Sqrt(((x - ax) * (x - ax)) + ((y - ay) * (y - ay)));
        double dlinaVectoraB = Math.Sqrt(((x - bx) * (x - bx)) + ((y - by) * (y - by)));
        double dlinaVectoraCOsnovanie = Math.Sqrt(((ax - bx) * (ax - bx)) + ((ay - by) * (ay - by)));
        double ifPerpendicular = 100000000;
        if (dlinaVectoraCOsnovanie != 0)
        {
            double poluSquare = (dlinaVectoraA + dlinaVectoraB + dlinaVectoraCOsnovanie) / 2.0;
            double square = Math.Sqrt(poluSquare * (poluSquare - dlinaVectoraA) * (poluSquare - dlinaVectoraB) * (poluSquare - dlinaVectoraCOsnovanie));
            ifPerpendicular = 2.0 * square / dlinaVectoraCOsnovanie;
        }

        var ifNetPerpendiculara = Math.Min(dlinaVectoraA, dlinaVectoraB);
        return Math.Min(ifPerpendicular, ifNetPerpendiculara);
    }
}