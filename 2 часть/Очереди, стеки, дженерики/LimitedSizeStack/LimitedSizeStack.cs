using System;
using System.Collections.Generic;

namespace LimitedSizeStack;

public class LimitedSizeStack<T>
{
	private readonly LinkedList<T> _list;
	private readonly int _limit;

	public LimitedSizeStack(int undoLimit)
	{
		_list = new LinkedList<T>();
		_limit = undoLimit;
	}

	public void Push(T item)
	{
		_list.AddLast(item);
		if (_list.Count > _limit)
		{
			_list.RemoveFirst();
		}
	}

	public T Pop()
	{
		var last = _list.Last;
		if (last == null)
		{
			throw new InvalidOperationException("Stack is empty");
		}

		_list.RemoveLast();
		return last.Value;
	}

	public int Count
	{
		get { return _list.Count; }
	}
}