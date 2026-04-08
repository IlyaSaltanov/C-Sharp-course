using System.Collections.Generic;

namespace SQ2;

/// <summary>
/// Очередь, реализованная с помощью двух стеков.
/// Enqueue: O(1), Dequeue/Peek: O(1) в среднем (амортизированно).
/// </summary>
public class QueueWithTwoStacks<T>
{
    private readonly Stack<T> _input;   // стек для добавления элементов
    private readonly Stack<T> _output;  // стек для извлечения элементов

    public QueueWithTwoStacks()
    {
        _input = new Stack<T>();
        _output = new Stack<T>();
    }

    public int Count => _input.Count + _output.Count;

    /// <summary>
    /// Добавляет элемент в конец очереди.
    /// </summary>
    public void Enqueue(T item)
    {
        _input.Push(item);
    }

    /// <summary>
    /// Удаляет и возвращает элемент из начала очереди.
    /// </summary>
    public T Dequeue()
    {
        EnsureOutputHasItems();
        return _output.Pop();
    }

    /// <summary>
    /// Возвращает элемент из начала очереди без удаления.
    /// </summary>
    public T Peek()
    {
        EnsureOutputHasItems();
        return _output.Peek();
    }

    /// <summary>
    /// Переносит элементы из стека ввода в стек вывода (если вывод пуст).
    /// Так мы получаем FIFO: первый добавленный оказывается наверху _output.
    /// </summary>
    private void EnsureOutputHasItems()
    {
        if (_output.Count == 0)
        {
            while (_input.Count > 0)
            {
                _output.Push(_input.Pop());
            }
        }

        if (_output.Count == 0)
            throw new InvalidOperationException("Очередь пуста.");
    }
}
