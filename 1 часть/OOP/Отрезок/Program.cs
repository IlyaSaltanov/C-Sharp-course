// Поместите сюда классы Vector и Geometry

namespace Geometry
{
    public class Vector
    {
        public double X;
        public double Y;
    }

    public class Segment
    {
        public Vector Begin;
        public Vector End;
    }

    public static class Geometry
    {
        public static double GetLength(Vector vector)
        {
            return Math.Sqrt(GetLengthSquared(vector));
        }

        public static double GetLength(Segment segment)
        {
            double dx = segment.Begin.X - segment.End.X;
            double dy = segment.Begin.Y - segment.End.Y;
            return Math.Sqrt(dx * dx + dy * dy);
        }

        public static Vector Add(Vector vector1, Vector vector2)
        {
            return CreateVector(vector1.X + vector2.X, vector1.Y + vector2.Y);
        }

        public static bool IsVectorInSegment(Vector vector, Segment segment)
        {
            double segmentLength = GetLength(segment);
            double lengthToBegin = Math.Sqrt(GetLengthSquared(new Vector { 
                X = vector.X - segment.Begin.X, 
                Y = vector.Y - segment.Begin.Y 
            }));
            double lengthToEnd = Math.Sqrt(GetLengthSquared(new Vector { 
                X = vector.X - segment.End.X, 
                Y = vector.Y - segment.End.Y 
            }));
            
            const double epsilon = 1e-10;
            return Math.Abs((lengthToBegin + lengthToEnd) - segmentLength) < epsilon;
        }

        private static double GetLengthSquared(Vector vector)
        {
            return vector.X * vector.X + vector.Y * vector.Y;
        }

        private static Vector CreateVector(double x, double y)
        {
            return new Vector { X = x, Y = y };
        }
    }
}