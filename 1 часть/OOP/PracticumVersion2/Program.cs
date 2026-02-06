using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    // Перечисление мастей карт
    enum Suit
    {
        Пики,    // ♠
        Трефы,   // ♣
        Черви,   // ♥
        Бубны    // ♦
    }

    // Перечисление достоинств карт
    enum Rank
    {
        Шесть = 6,
        Семь = 7,
        Восемь = 8,
        Девять = 9,
        Десять = 10,
        Валет = 11,
        Дама = 12,
        Король = 13,
        Туз = 14
    }

    // Класс карты
    class Card
    {
        public Suit Suit { get; }
        public Rank Rank { get; }
        public bool IsTrump { get; set; }

        public Card(Suit suit, Rank rank)
        {
            Suit = suit;
            Rank = rank;
            IsTrump = false;
        }

        public override string ToString()
        {
            string rankStr = Rank switch
            {
                Rank.Шесть => "6",
                Rank.Семь => "7",
                Rank.Восемь => "8",
                Rank.Девять => "9",
                Rank.Десять => "10",
                Rank.Валет => "В",
                Rank.Дама => "Д",
                Rank.Король => "К",
                Rank.Туз => "Т",
                _ => Rank.ToString()
            };

            string suitStr = Suit switch
            {
                Suit.Пики => "♠",
                Suit.Трефы => "♣",
                Suit.Черви => "♥",
                Suit.Бубны => "♦",
                _ => Suit.ToString()
            };

            return $"{rankStr}{suitStr}";
        }

        // Сравнение карт (учитывает козырь)
        public bool CanBeat(Card attackingCard, Suit trumpSuit)
        {
            // Если карта защиты - козырь, а карта атаки - нет
            if (this.Suit == trumpSuit && attackingCard.Suit != trumpSuit)
                return true;
            
            // Если обе карты козыри или обе не козыри
            if (this.Suit == attackingCard.Suit)
                return (int)this.Rank > (int)attackingCard.Rank;
            
            // Если карта защиты не той же масти и не козырь
            return false;
        }
    }

    // Класс колоды
    class Deck
    {
        private List<Card> cards;
        private Random random;

        public Card TrumpCard { get; private set; }
        public Suit TrumpSuit { get; private set; }

        public Deck()
        {
            cards = new List<Card>();
            random = new Random();
            InitializeDeck();
        }

        private void InitializeDeck()
        {
            // Создаем колоду из 36 карт
            foreach (Suit suit in Enum.GetValues(typeof(Suit)))
            {
                foreach (Rank rank in Enum.GetValues(typeof(Rank)))
                {
                    cards.Add(new Card(suit, rank));
                }
            }
        }

        public void Shuffle()
        {
            // Тасуем колоду
            for (int i = cards.Count - 1; i > 0; i--)
            {
                int j = random.Next(i + 1);
                Card temp = cards[i];
                cards[i] = cards[j];
                cards[j] = temp;
            }
        }

        public void SetTrump()
        {
            // Определяем козырь (последняя карта в колоде)
            TrumpCard = cards.Last();
            TrumpSuit = TrumpCard.Suit;
            
            // Отмечаем все карты этой масти как козыри
            foreach (var card in cards.Where(c => c.Suit == TrumpSuit))
            {
                card.IsTrump = true;
            }
            
            Console.WriteLine($"Козырь: {TrumpCard}");
        }

        public Card DrawCard()
        {
            if (cards.Count == 0)
                return null;
                
            Card card = cards[0];
            cards.RemoveAt(0);
            return card;
        }

        public int CardsRemaining => cards.Count;
    }

    // Класс игрока
    class Player
    {
        public string Name { get; }
        public List<Card> Hand { get; }
        public bool IsAttacker { get; set; }

        public Player(string name)
        {
            Name = name;
            Hand = new List<Card>();
        }

        public void TakeCard(Card card)
        {
            if (card != null)
            {
                Hand.Add(card);
                // Сортируем карты в руке: сначала по масти, затем по достоинству
                Hand.Sort((c1, c2) => 
                {
                    if (c1.Suit != c2.Suit)
                        return c1.Suit.CompareTo(c2.Suit);
                    return c1.Rank.CompareTo(c2.Rank);
                });
            }
        }

        public void ShowHand()
        {
            Console.WriteLine($"\nКарты игрока {Name}:");
            for (int i = 0; i < Hand.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {Hand[i]} {(Hand[i].IsTrump ? "(козырь)" : "")}");
            }
        }

        public bool HasCards => Hand.Count > 0;

        public Card PlayCard(int index)
        {
            if (index >= 0 && index < Hand.Count)
            {
                Card card = Hand[index];
                Hand.RemoveAt(index);
                return card;
            }
            return null;
        }
    }

    // Класс игрового стола
    class GameTable
    {
        public List<Card> AttackCards { get; }
        public List<Card> DefenseCards { get; }
        public List<Card> DiscardPile { get; }

        public GameTable()
        {
            AttackCards = new List<Card>();
            DefenseCards = new List<Card>();
            DiscardPile = new List<Card>();
        }

        public void ClearTable()
        {
            DiscardPile.AddRange(AttackCards);
            DiscardPile.AddRange(DefenseCards);
            AttackCards.Clear();
            DefenseCards.Clear();
        }

        public void ShowTable()
        {
            Console.WriteLine("\n=== ИГРОВОЙ СТОЛ ===");
            Console.WriteLine("Карты атаки: " + string.Join(", ", AttackCards));
            Console.WriteLine("Карты защиты: " + string.Join(", ", DefenseCards));
        }
    }

    // Основная игровая логика
    static string[] ReadAndSplit()
    {
        string input = Console.ReadLine();
        string[] parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        return parts;
    }

    static void Main(string[] args)
    {
        Console.WriteLine("=== КЛАССИЧЕСКИЙ ДУРАК ===\n");

        // Создаем игроков
        Console.WriteLine("Введите имена двух игроков через пробел:");
        string[] playerNames = ReadAndSplit();

        if (playerNames.Length < 2)
        {
            Console.WriteLine("Нужно ввести два имени!");
            return;
        }

        Player player1 = new Player(playerNames[0]);
        Player player2 = new Player(playerNames[1]);

        Console.WriteLine($"\nСозданы игроки: {player1.Name} и {player2.Name}");

        // Создаем и инициализируем колоду
        Deck deck = new Deck();
        deck.Shuffle();
        deck.SetTrump();

        // Раздаем карты
        Console.WriteLine("\nРаздача карт...");
        for (int i = 0; i < 6; i++)
        {
            player1.TakeCard(deck.DrawCard());
            player2.TakeCard(deck.DrawCard());
        }

        // Определяем, кто ходит первым (у кого младший козырь)
        Player attacker = player1;
        Player defender = player2;

        // Создаем игровой стол
        GameTable table = new GameTable();

        // Игровой цикл
        bool gameOver = false;
        int round = 1;

        while (!gameOver)
        {
            Console.WriteLine($"\n=== РАУНД {round} ===");
            Console.WriteLine($"Ходит: {attacker.Name} (атакует)");
            Console.WriteLine($"Защищается: {defender.Name}");

            // Показываем карты игроков
            attacker.ShowHand();
            defender.ShowHand();

            // Фаза атаки
            Console.WriteLine($"\n{attacker.Name}, выберите карту для атаки (номер):");
            if (int.TryParse(Console.ReadLine(), out int attackIndex) && attackIndex > 0 && attackIndex <= attacker.Hand.Count)
            {
                Card attackCard = attacker.PlayCard(attackIndex - 1);
                table.AttackCards.Add(attackCard);
                Console.WriteLine($"{attacker.Name} атакует картой: {attackCard}");

                // Фаза защиты
                Console.WriteLine($"\n{defender.Name}, выберите карту для защиты (номер) или введите 0, чтобы взять карты:");
                if (int.TryParse(Console.ReadLine(), out int defenseIndex))
                {
                    if (defenseIndex == 0)
                    {
                        // Защитник берет карты
                        Console.WriteLine($"{defender.Name} берет карты со стола!");
                        defender.Hand.AddRange(table.AttackCards);
                        defender.Hand.Sort((c1, c2) => c1.Suit.CompareTo(c2.Suit));
                    }
                    else if (defenseIndex > 0 && defenseIndex <= defender.Hand.Count)
                    {
                        Card defenseCard = defender.PlayCard(defenseIndex - 1);
                        
                        if (defenseCard.CanBeat(attackCard, deck.TrumpSuit))
                        {
                            table.DefenseCards.Add(defenseCard);
                            Console.WriteLine($"{defender.Name} отбивается картой: {defenseCard}");
                            
                            // Карты уходят в отбой
                            table.ClearTable();
                            Console.WriteLine("Карты ушли в отбой!");
                        }
                        else
                        {
                            Console.WriteLine("Этой картой нельзя отбиться! Попробуйте еще раз.");
                            defender.TakeCard(defenseCard); // Возвращаем карту
                            continue;
                        }
                    }
                }

                // Добираем карты из колоды
                while (attacker.Hand.Count < 6 && deck.CardsRemaining > 0)
                    attacker.TakeCard(deck.DrawCard());
                    
                while (defender.Hand.Count < 6 && deck.CardsRemaining > 0)
                    defender.TakeCard(deck.DrawCard());

                // Меняем роли
                (attacker, defender) = (defender, attacker);

                // Проверяем условия окончания игры
                if (deck.CardsRemaining == 0)
                {
                    if (!player1.HasCards || !player2.HasCards)
                    {
                        gameOver = true;
                        Console.WriteLine("\n=== ИГРА ОКОНЧЕНА ===");
                        
                        if (!player1.HasCards && !player2.HasCards)
                            Console.WriteLine("НИЧЬЯ!");
                        else if (!player1.HasCards)
                            Console.WriteLine($"ПОБЕДИТЕЛЬ: {player1.Name}!");
                        else
                            Console.WriteLine($"ПОБЕДИТЕЛЬ: {player2.Name}!");
                    }
                }

                round++;
            }
            else
            {
                Console.WriteLine("Неверный ввод! Попробуйте еще раз.");
            }

            Console.WriteLine("\nНажмите любую клавишу для продолжения...");
            Console.ReadKey();
            Console.Clear();
        }
    }
}