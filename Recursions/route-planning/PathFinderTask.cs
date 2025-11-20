using System;
using System.Drawing;
using System.Collections.Generic;

namespace RoutePlanning;

public static class PathFinderTask
{
    private static int[] bestOrder;
    private static double minPathLength = double.MaxValue;
    private static double[,] distanceMatrix;
    private static Point[] checkpoints;

    public static int[] FindBestCheckpointsOrder(Point[] checkpoints)
    {
        if (checkpoints.Length == 0) return new int[0];
        if (checkpoints.Length == 1) return new int[] { 0 };

        // Инициализация
        PathFinderTask.checkpoints = checkpoints;
        bestOrder = MakeTrivialPermutation(checkpoints.Length);
        minPathLength = double.MaxValue;
        
        // Создаем матрицу расстояний
        BuildDistanceMatrix(checkpoints);
        
        // Начинаем перебор с тривиальной перестановки
        var currentOrder = new int[checkpoints.Length];
        currentOrder[0] = 0; // начинаем с первой точки
        
        MakePermutations(currentOrder, 1, 0.0);
        
        return bestOrder;
    }

    static void MakePermutations(int[] permutation, int position, double currentPathLength)
    {
        if (position == permutation.Length)
        {
            // Нашли полный маршрут
            if (currentPathLength < minPathLength)
            {
                minPathLength = currentPathLength;
                Array.Copy(permutation, bestOrder, permutation.Length);
            }
        }
        else
        {
            for (int i = 1; i < permutation.Length; i++)
            {
                var index = Array.IndexOf(permutation, i, 0, position);
                //если i не встречается среди первых position элементов массива permutation, то index == -1
                //иначе index — это номер позиции элемента i в массиве.
                if (index == -1)
                {
                    // если число i ещё не было использовано, то...
                    // Вычисляем длину пути до новой точки
                    var lastPoint = checkpoints[permutation[position - 1]];
                    var nextPoint = checkpoints[i];
                    var segmentLength = distanceMatrix[permutation[position - 1], i];
                    var newPathLength = currentPathLength + segmentLength;

                    // Отсечение: если текущий путь уже длиннее лучшего, не продолжаем
                    if (newPathLength >= minPathLength)
                        continue;

                    permutation[position] = i;
                    MakePermutations(permutation, position + 1, newPathLength);
                }
            }
        }
    }

    // Построение матрицы расстояний
    private static void BuildDistanceMatrix(Point[] checkpoints)
    {
        int n = checkpoints.Length;
        distanceMatrix = new double[n, n];
        
        for (int i = 0; i < n; i++)
        {
            for (int j = i + 1; j < n; j++)
            {
                double distance = Distance(checkpoints[i], checkpoints[j]);
                distanceMatrix[i, j] = distance;
                distanceMatrix[j, i] = distance; // Симметричная матрица
            }
        }
    }

    private static double Distance(Point a, Point b)
    {
        var dx = a.X - b.X;
        var dy = a.Y - b.Y;
        return Math.Sqrt(dx * dx + dy * dy);
    }

    private static int[] MakeTrivialPermutation(int size)
    {
        var order = new int[size];
        for (var i = 0; i < order.Length; i++)
            order[i] = i;
        return order;
    }
}