using System.Collections.Generic;
using System.Text;

// Конвертация инфиксной записи в постфиксную (обратная польская нотация)
// и вычисление выражения с использованием стека (Stack<T>)

string infix = "(2+1)*3+5";
Console.WriteLine($"Инфиксная запись:  {infix}");

string postfix = InfixToPostfix(infix);
Console.WriteLine($"Постфиксная запись: {postfix}");

double result = EvaluatePostfix(postfix);
Console.WriteLine($"Результат:         {result}");

// Нормализация: подмена Unicode-вариантов операторов на ASCII,
// чтобы не терять знак между скобками (например, "＋" или "−" из Word/браузера)
static string NormalizeInfix(string s)
{
    return s
        .Replace('\uFF0B', '+')  // fullwidth plus ＋
        .Replace('\u2212', '-')  // minus sign −
        .Replace('\u00D7', '*')  // multiplication ×
        .Replace('\u00F7', '/'); // division ÷
}

// Алгоритм сортировочной станции (Dijkstra)
static string InfixToPostfix(string infix)
{
    infix = NormalizeInfix(infix);

    var output = new StringBuilder();
    var operators = new Stack<char>(); // стек операторов (generic Stack<T>)

    int i = 0;
    while (i < infix.Length)
    {
        char c = infix[i];

        if (char.IsDigit(c))
        {
            // Собираем многозначное число
            while (i < infix.Length && char.IsDigit(infix[i]))
            {
                output.Append(infix[i]);
                i++;
            }
            output.Append(' ');
            continue;
        }

        if (c == '(')
        {
            operators.Push(c);
            i++;
            continue;
        }

        if (c == ')')
        {
            while (operators.Count > 0 && operators.Peek() != '(')
                output.Append(operators.Pop()).Append(' ');
            if (operators.Count > 0)
                operators.Pop(); // убираем '('
            i++;
            continue;
        }

        if (IsOperator(c))
        {
            while (operators.Count > 0 &&
                   operators.Peek() != '(' &&
                   Precedence(operators.Peek()) >= Precedence(c))
            {
                output.Append(operators.Pop()).Append(' ');
            }
            operators.Push(c);
            i++;
            continue;
        }

        if (char.IsWhiteSpace(c))
        {
            i++;
            continue;
        }

        // Неизвестный символ (например, Unicode-оператор не из списка нормализации)
        throw new ArgumentException($"Нераспознанный символ в выражении: '{c}' (код U+{(int)c:X4}). " +
            "Используйте обычные знаки + - * / или вставьте выражение как обычный текст.");
    }

    while (operators.Count > 0)
        output.Append(operators.Pop()).Append(' ');

    return output.ToString().TrimEnd();
}

static double EvaluatePostfix(string postfix)
{
    var operands = new Stack<double>(); // стек операндов (generic Stack<T>)

    int i = 0;
    while (i < postfix.Length)
    {
        if (postfix[i] == ' ')
        {
            i++;
            continue;
        }

        if (char.IsDigit(postfix[i]))
        {
            int start = i;
            while (i < postfix.Length && (char.IsDigit(postfix[i]) || postfix[i] == '.'))
                i++;
            string numStr = postfix[start..i];
            if (double.TryParse(numStr, out double num))
                operands.Push(num);
            continue;
        }

        if (IsOperator(postfix[i]))
        {
            double b = operands.Pop();
            double a = operands.Pop();
            double res = postfix[i] switch
            {
                '+' => a + b,
                '-' => a - b,
                '*' => a * b,
                '/' => a / b,
                _ => throw new ArgumentException($"Неизвестный оператор: {postfix[i]}")
            };
            operands.Push(res);
            i++;
        }
    }

    return operands.Pop();
}

static bool IsOperator(char c) => c == '+' || c == '-' || c == '*' || c == '/';

static int Precedence(char op)
{
    return op switch
    {
        '+' or '-' => 1,
        '*' or '/' => 2,
        _ => 0
    };
}
