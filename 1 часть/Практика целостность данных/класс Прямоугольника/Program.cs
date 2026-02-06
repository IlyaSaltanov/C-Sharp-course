using System;
using System.Collections.Generic;
using System.Linq;

// Структура для представления точки/вектора в 2D пространстве
public struct Vector
{
    public double X { get; }
    public double Y { get; }
    
    public Vector(double x, double y)
    {
        X = x;
        Y = y;
    }
    
    public double Length => Math.Sqrt(X * X + Y * Y);
    
    public static Vector operator +(Vector a, Vector b) => new Vector(a.X + b.X, a.Y + b.Y);
    public static Vector operator -(Vector a, Vector b) => new Vector(a.X - b.X, a.Y - b.Y);
    public static Vector operator *(Vector v, double scalar) => new Vector(v.X * scalar, v.Y * scalar);
    public static Vector operator /(Vector v, double scalar) => new Vector(v.X / scalar, v.Y / scalar);
    
    public override string ToString() => $"({X:F2}, {Y:F2})";
}

// Структура для представления отрезка (стороны прямоугольника)
public struct Segment
{
    public Vector Start { get; }
    public Vector End { get; }
    
    public Segment(Vector start, Vector end)
    {
        Start = start;
        End = end;
    }
    
    public double Length => (End - Start).Length;
    public Vector Direction => (End - Start) / Length;
    
    public override string ToString() => $"[{Start} -> {End}]";
}

// Основной класс Прямоугольника
public class Rectangle
{
    private Vector[] vertices = new Vector[4];
    
    // Конструктор по 4 вершинам (в порядке обхода)
    public Rectangle(Vector v0, Vector v1, Vector v2, Vector v3)
    {
        vertices[0] = v0;
        vertices[1] = v1;
        vertices[2] = v2;
        vertices[3] = v3;
    }
    
    // Конструктор по центру, размерам и углу поворота
    public Rectangle(Vector center, double width, double height, double angle = 0)
    {
        double halfWidth = width / 2;
        double halfHeight = height / 2;
        
        // Вершины без поворота (относительно центра)
        Vector[] baseVertices = new Vector[]
        {
            new Vector(-halfWidth, -halfHeight),
            new Vector(halfWidth, -halfHeight),
            new Vector(halfWidth, halfHeight),
            new Vector(-halfWidth, halfHeight)
        };
        
        // Применяем поворот и сдвигаем к центру
        double cos = Math.Cos(angle);
        double sin = Math.Sin(angle);
        
        for (int i = 0; i < 4; i++)
        {
            Vector v = baseVertices[i];
            double x = v.X * cos - v.Y * sin + center.X;
            double y = v.X * sin + v.Y * cos + center.Y;
            vertices[i] = new Vector(x, y);
        }
    }
    
    // Статический метод для создания прямоугольника по двум точкам
    // (предполагаем, что точки - это концы одной из сторон)
    public static Rectangle FromTwoPoints(Vector p1, Vector p2, double sideLength)
    {
        // Получаем вектор стороны и его длину
        Vector sideVector = p2 - p1;
        double currentSideLength = sideVector.Length;
        
        // Нормализуем вектор стороны
        Vector unitSide = sideVector / currentSideLength;
        
        // Поворачиваем на 90 градусов для получения перпендикулярного вектора
        Vector perpendicular = new Vector(-unitSide.Y, unitSide.X);
        
        // Вычисляем оставшиеся две вершины
        Vector p3 = p2 + perpendicular * sideLength;
        Vector p4 = p1 + perpendicular * sideLength;
        
        return new Rectangle(p1, p2, p3, p4);
    }
    
    // Метод, который возвращает длину стороны и вектор между двумя вершинами
    public (double length, Vector vector) GetSideInfo(int vertexIndex1, int vertexIndex2)
    {
        if (vertexIndex1 < 0 || vertexIndex1 >= 4 || vertexIndex2 < 0 || vertexIndex2 >= 4)
            throw new ArgumentException("Индексы вершин должны быть от 0 до 3");
        
        Vector v1 = vertices[vertexIndex1];
        Vector v2 = vertices[vertexIndex2];
        Vector vector = v2 - v1;
        
        return (vector.Length, vector);
    }
    
    // Получение массива вершин
    public Vector[] GetVertices() => vertices.ToArray();
    
    // Получение массива сторон
    public Segment[] GetSides()
    {
        return new Segment[]
        {
            new Segment(vertices[0], vertices[1]),
            new Segment(vertices[1], vertices[2]),
            new Segment(vertices[2], vertices[3]),
            new Segment(vertices[3], vertices[0])
        };
    }
    
    // Методы изменения прямоугольника:
    
    // 1. Перемещение (сдвиг)
    public void Translate(Vector offset)
    {
        for (int i = 0; i < 4; i++)
        {
            vertices[i] = vertices[i] + offset;
        }
    }
    
    // 2. Поворот относительно центра
    public void Rotate(double angle, Vector? rotationCenter = null)
    {
        Vector center = rotationCenter ?? GetCenter();
        double cos = Math.Cos(angle);
        double sin = Math.Sin(angle);
        
        for (int i = 0; i < 4; i++)
        {
            Vector v = vertices[i] - center;
            double x = v.X * cos - v.Y * sin + center.X;
            double y = v.X * sin + v.Y * cos + center.Y;
            vertices[i] = new Vector(x, y);
        }
    }
    
    // 3. Масштабирование относительно центра
    public void Scale(double scaleFactor, Vector? scalingCenter = null)
    {
        Vector center = scalingCenter ?? GetCenter();
        
        for (int i = 0; i < 4; i++)
        {
            Vector v = vertices[i] - center;
            vertices[i] = center + v * scaleFactor;
        }
    }
    
    // 4. Изменение размеров (ширины и высоты) с сохранением центра
    public void Resize(double newWidth, double newHeight)
    {
        Vector center = GetCenter();
        double currentWidth = (vertices[1] - vertices[0]).Length;
        double currentHeight = (vertices[2] - vertices[1]).Length;
        
        double widthRatio = newWidth / currentWidth;
        double heightRatio = newHeight / currentHeight;
        
        for (int i = 0; i < 4; i++)
        {
            Vector v = vertices[i] - center;
            vertices[i] = center + new Vector(v.X * widthRatio, v.Y * heightRatio);
        }
    }
    
    // 5. Деформация (сдвиг одной из сторон)
    public void Deform(int sideIndex, Vector offset)
    {
        int[] vertexIndices = GetSideVertexIndices(sideIndex);
        vertices[vertexIndices[0]] = vertices[vertexIndices[0]] + offset;
        vertices[vertexIndices[1]] = vertices[vertexIndices[1]] + offset;
    }
    
    // Вспомогательные методы
    private Vector GetCenter()
    {
        double x = (vertices[0].X + vertices[1].X + vertices[2].X + vertices[3].X) / 4;
        double y = (vertices[0].Y + vertices[1].Y + vertices[2].Y + vertices[3].Y) / 4;
        return new Vector(x, y);
    }
    
    private int[] GetSideVertexIndices(int sideIndex)
    {
        return sideIndex switch
        {
            0 => new int[] { 0, 1 }, // нижняя сторона
            1 => new int[] { 1, 2 }, // правая сторона
            2 => new int[] { 2, 3 }, // верхняя сторона
            3 => new int[] { 3, 0 }, // левая сторона
            _ => throw new ArgumentException("Индекс стороны должен быть от 0 до 3")
        };
    }
    
    public override string ToString()
    {
        return $"Прямоугольник с вершинами: {vertices[0]}, {vertices[1]}, {vertices[2]}, {vertices[3]}";
    }
}

// Пример использования
class Program
{
    static void Main()
    {
        // Создаем прямоугольник по двум точкам и длине стороны
        Vector p1 = new Vector(0, 0);
        Vector p2 = new Vector(4, 0);
        double sideLength = 3;

        Rectangle rect = Rectangle.FromTwoPoints(p1, p2, sideLength);
        Console.WriteLine($"Создан прямоугольник: {rect}");

        // Получаем информацию о стороне между вершинами 0 и 1
        var sideInfo = rect.GetSideInfo(0, 1);
        Console.WriteLine($"Сторона между вершинами 0 и 1:");
        Console.WriteLine($"  Длина: {sideInfo.length:F2}");
        Console.WriteLine($"  Вектор: {sideInfo.vector}");

        // Получаем все вершины
        Console.WriteLine("\nВсе вершины:");
        foreach (var vertex in rect.GetVertices())
        {
            Console.WriteLine($"  {vertex}");
        }

        // Получаем все стороны
        Console.WriteLine("\nВсе стороны:");
        foreach (var side in rect.GetSides())
        {
            Console.WriteLine($"  {side} (длина: {side.Length:F2})");
        }

        // Применяем методы изменения
        Console.WriteLine("\nПосле перемещения на (1, 1):");
        rect.Translate(new Vector(1, 1));
        Console.WriteLine(rect);

        Console.WriteLine("\nПосле поворота на 45 градусов:");
        rect.Rotate(Math.PI / 4);
        Console.WriteLine(rect);

        Console.WriteLine("\nПосле масштабирования в 2 раза:");
        rect.Scale(2);
        Console.WriteLine(rect);

        Console.WriteLine("\nПосле изменения размеров (ширина=5, высота=4):");
        rect.Resize(5, 4);
        Console.WriteLine(rect);
    }
}



