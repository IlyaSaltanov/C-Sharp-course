using System.Collections.Generic;

namespace SQ1;

/// <summary>
/// Стек с операцией GetMax за O(1).
/// Используется вспомогательный стек, хранящий текущий максимум на каждом уровне.
///
/// Аналогично за O(1) можно реализовать:
/// - GetMin — вспомогательный стек минимумов (при Push: min(текущий минимум, value));
/// - GetSum — одна переменная суммы (при Push += value, при Pop -= value);
/// - GetProduct — переменная произведения (при Push *= value, при Pop /= value, осторожно с 0).
/// </summary>
public class StackWithGetMax<T> where T : IComparable<T>
{
    private readonly Stack<T> _data = new Stack<T>();
    private readonly Stack<T> _max = new Stack<T>();

    public int Count => _data.Count;
    public bool IsEmpty => _data.Count == 0;

    /// <summary>Добавляет элемент. O(1).</summary>
    public void Push(T value)
    {
        _data.Push(value);
        if (_max.Count == 0 || value.CompareTo(_max.Peek()) >= 0)
            _max.Push(value);
        else
            _max.Push(_max.Peek());
    }

    /// <summary>Удаляет и возвращает верхний элемент. O(1).</summary>
    public T Pop()
    {
        _max.Pop();
        return _data.Pop();
    }

    /// <summary>Возвращает верхний элемент без удаления. O(1).</summary>
    public T Peek() => _data.Peek();

    /// <summary>Возвращает максимум среди всех элементов стека. O(1).</summary>
    /// <exception cref="InvalidOperationException">Стек пуст.</exception>
    public T GetMax()
    {
        if (_max.Count == 0)
            throw new InvalidOperationException("Стек пуст. Невозможно получить максимум.");
        return _max.Peek();
    }
}
