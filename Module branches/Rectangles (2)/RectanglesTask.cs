using System;

namespace Rectangles;

public static class RectanglesTask
{
    // Пересекаются ли два прямоугольника (пересечение только по границе также считается пересечением)
    public static bool AreIntersected(Rectangle r1, Rectangle r2)
    {
        // Прямоугольники пересекаются, если:
        // - левая граница первого не правее правой границы второго И
        // - правая граница первого не левее левой границы второго И  
        // - верхняя граница первого не ниже нижней границы второго И
        // - нижняя граница первого не выше верхней границы второго
        return r1.Left <= r2.Right && 
               r1.Right >= r2.Left && 
               r1.Top <= r2.Bottom && 
               r1.Bottom >= r2.Top;
    }

    // Площадь пересечения прямоугольников
    public static int IntersectionSquare(Rectangle r1, Rectangle r2)
    {
        if (!AreIntersected(r1, r2))
            return 0;

        // Находим координаты прямоугольника пересечения:
        int left = Math.Max(r1.Left, r2.Left);
        int right = Math.Min(r1.Right, r2.Right);
        int top = Math.Max(r1.Top, r2.Top);
        int bottom = Math.Min(r1.Bottom, r2.Bottom);

        int width = right - left;
        int height = bottom - top;

        return width * height;
    }

    // Если один из прямоугольников целиком находится внутри другого — вернуть номер (с нуля) внутреннего.
    // Иначе вернуть -1
    // Если прямоугольники совпадают, можно вернуть номер любого из них.
    public static int IndexOfInnerRectangle(Rectangle r1, Rectangle r2)
    {
        // Выясняем кто где находится или кто в ком?
        bool r1InR2 = r1.Left >= r2.Left && 
                      r1.Right <= r2.Right && 
                      r1.Top >= r2.Top && 
                      r1.Bottom <= r2.Bottom;

        bool r2InR1 = r2.Left >= r1.Left && 
                      r2.Right <= r1.Right && 
                      r2.Top >= r1.Top && 
                      r2.Bottom <= r1.Bottom;

        if (r1InR2)
            return 0;
        else if (r2InR1)
            return 1; 
        else
            return -1;
    }
}