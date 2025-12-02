class Program
{
    static string[] ReadAndSplit()
    {
        string input = Console.ReadLine();
        string[] parts = input.Split(' ');
        return parts;
    }
    class Domino
    {
        public int One;
        public int Two;

        public Domino(int d1, int d2)
        {
            One = d1;
            Two = d2;
        }
    }

    class CreateDesk
    {
        public string One;
        public string Two;
        public string Three;

        public CreateDesk(string d1, string d2, string d3)
        {
            One = d1;
            Two = d2;
            Three = d3;
        }
    }
    class CreatePlayer
    {
        public string One;
        public string Two;
        public string Three;
        
        public CreatePlayer(string d1, string d2)
        {
            One = d1;
            Two = d2;
        }
    }
    
    static void Main(string[] args)
    {
        Console.WriteLine("Для того чтобы начать игру, нужно создать двух игроков и три стола.");

        Console.WriteLine("Создайте двух игроков (два имени через пробел): ");

        var parts = ReadAndSplit();

        var player1 = parts[0];
        var player2 = parts[1];
        
        CreatePlayer CreatePlayer = new CreatePlayer(player1, player2);
        Console.WriteLine($"Congartulation! Вы создали своих игроков: {CreatePlayer.One}:{CreatePlayer.Two}");

        Console.WriteLine("Создайте три игы (три названия через пробел): ");

        parts = ReadAndSplit();

        var desk1 = parts[0];
        var desk2 = parts[1];
        var desk3 = parts[2];
        
        CreateDesk CreateDesk = new CreateDesk(desk1, desk2, desk3);
        Console.WriteLine($"Созданы следующие столы: {CreateDesk.One}:{CreateDesk.Two}:{CreateDesk.Three}");

        Console.WriteLine("Введите доминошку (два числа через пробел): ");

        parts = ReadAndSplit();

        var num1 = int.Parse(parts[0]);
        var num2 = int.Parse(parts[1]);
        
        Domino domino = new Domino(num1, num2);
        Console.WriteLine($"Создана доминошка: {domino.One}:{domino.Two}");
    }
}