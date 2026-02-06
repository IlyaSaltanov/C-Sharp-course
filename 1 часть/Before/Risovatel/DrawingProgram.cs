using System;
using System.Security.Cryptography.X509Certificates;
using Avalonia.Media;
using RefactorMe.Common;

namespace RefactorMe
{
    class Risovatel
    {
        static float x, y;
        static IGraphics Graphics;

        public static void Initialization ( IGraphics newGraphics )
        {
            Graphics = newGraphics;
            //Graphics.SmoothingMode = SmoothingMode.None;
            Graphics.Clear(Colors.Black);
        }

        public static void SetPosition(float x0, float y0)
        {
            x = x0;
            y = y0;
        }

        public static void MakeIt(Pen pen, double length, double angle)
        {
            //Делает шаг длиной dlina в направлении angle и рисует пройденную траекторию
            var x1 = (float)(x + length * Math.Cos(angle));
            var y1 = (float)(y + length * Math.Sin(angle));
            Graphics.DrawLine(pen, x, y, x1, y1);
            x = x1;
            y = y1;
        }

        public static void Change(double length, double angle)
        {
            x = (float)(x + length * Math.Cos(angle)); 
            y = (float)(y + length * Math.Sin(angle));
        }
    }
    
    public class ImpossibleSquare
    {
    public static void Draw(int wide, int high, double angleRotation, IGraphics Graphics)
    {
        // angleRotation пока не используется, но будет использоваться в будущем
        Risovatel.Initialization(Graphics);

        var sz = Math.Min(wide, high);

        

        var diagonalLength = Math.Sqrt(2) * (sz * 0.375f + sz * 0.04f) / 2;
        var boxOne = (float)diagonalLength * Math.Cos(Math.PI / 4 + Math.PI);
        var x0 = boxOne + wide / 2f;
        var y0 = boxOne + high / 2f;

        Risovatel.SetPosition((float)x0, (float)y0);
        //Рисуем 1-ую сторону
        Risovatel.MakeIt(new Pen(Brushes.Yellow), sz * 0.375f, 0);
        Risovatel.MakeIt(new Pen(Brushes.Yellow), sz * 0.04f * Math.Sqrt(2), Math.PI / 4);
        Risovatel.MakeIt(new Pen(Brushes.Yellow), sz * 0.375f, Math.PI);
        Risovatel.MakeIt(new Pen(Brushes.Yellow), sz * 0.375f - sz * 0.04f, Math.PI / 2);

        Risovatel.Change(sz * 0.04f, -Math.PI);
        Risovatel.Change(sz * 0.04f * Math.Sqrt(2), 3 * Math.PI / 4);

        //Рисуем 2-ую сторону
        Risovatel.MakeIt(new Pen(Brushes.Yellow), sz * 0.375f, -Math.PI / 2);
        Risovatel.MakeIt(new Pen(Brushes.Yellow), sz * 0.04f * Math.Sqrt(2), -Math.PI / 2 + Math.PI / 4);
        Risovatel.MakeIt(new Pen(Brushes.Yellow), sz * 0.375f, -Math.PI / 2 + Math.PI);
        Risovatel.MakeIt(new Pen(Brushes.Yellow), sz * 0.375f - sz * 0.04f, -Math.PI / 2 + Math.PI / 2);

        Risovatel.Change(sz * 0.04f, -Math.PI / 2 - Math.PI);
        Risovatel.Change(sz * 0.04f * Math.Sqrt(2), -Math.PI / 2 + 3 * Math.PI / 4);

        //Рисуем 3-ю сторону
        Risovatel.MakeIt(new Pen(Brushes.Yellow), sz * 0.375f, Math.PI);
        Risovatel.MakeIt(new Pen(Brushes.Yellow), sz * 0.04f * Math.Sqrt(2), Math.PI + Math.PI / 4);
        Risovatel.MakeIt(new Pen(Brushes.Yellow), sz * 0.375f, Math.PI + Math.PI);
        Risovatel.MakeIt(new Pen(Brushes.Yellow), sz * 0.375f - sz * 0.04f, Math.PI + Math.PI / 2);

        Risovatel.Change(sz * 0.04f, 0);
        Risovatel.Change(sz * 0.04f * Math.Sqrt(2), Math.PI + 3 * Math.PI / 4);

        //Рисуем 4-ую сторону
        Risovatel.MakeIt(new Pen(Brushes.Yellow), sz * 0.375f, Math.PI / 2);
        Risovatel.MakeIt(new Pen(Brushes.Yellow), sz * 0.04f * Math.Sqrt(2), Math.PI / 2 + Math.PI / 4);
        Risovatel.MakeIt(new Pen(Brushes.Yellow), sz * 0.375f, Math.PI / 2 + Math.PI);
        Risovatel.MakeIt(new Pen(Brushes.Yellow), sz * 0.375f - sz * 0.04f, Math.PI / 2 + Math.PI / 2);

        Risovatel.Change(sz * 0.04f, Math.PI / 2 - Math.PI);
        Risovatel.Change(sz * 0.04f * Math.Sqrt(2), Math.PI / 2 + 3 * Math.PI / 4);
    }
}
}