using System;
using System.Collections.Generic;

namespace LimitedSizeStack;

public interface IUndoCommand
{
    void Undo();
}

public class AddItemCommand<TItem> : IUndoCommand
{
    private readonly List<TItem> _items;

    public AddItemCommand(List<TItem> items)
    {
        _items = items;
    }

    public void Undo()
    {
        if (_items.Count > 0)
        {
            _items.RemoveAt(_items.Count - 1);
        }
    }
}

public class RemoveItemCommand<TItem> : IUndoCommand
{
    private readonly List<TItem> _items;
    private readonly TItem _item;
    private readonly int _index;

    public RemoveItemCommand(List<TItem> items, TItem item, int index)
    {
        _items = items;
        _item = item;
        _index = index;
    }

    public void Undo()
    {
        if (_index <= _items.Count)
        {
            _items.Insert(_index, _item);
        }
    }
}

public class ListModel<TItem>
{
	public List<TItem> Items { get; }
	public int UndoLimit;
	private readonly LimitedSizeStack<IUndoCommand> _undoStack;

	public ListModel(int undoLimit): this(new List<TItem>(), undoLimit)
	{
	}

	public ListModel(List<TItem> items, int undoLimit)
	{
		Items = items;
		UndoLimit = undoLimit;
		_undoStack = new LimitedSizeStack<IUndoCommand>(undoLimit);
	}

	public void AddItem(TItem item)
	{
		Items.Add(item);
		_undoStack.Push(new AddItemCommand<TItem>(Items));
	}

	public void RemoveItem(int index)
	{
		var item = Items[index];
		Items.RemoveAt(index);
		_undoStack.Push(new RemoveItemCommand<TItem>(Items, item, index));
	}

	public bool CanUndo()
	{
		return _undoStack.Count > 0;
	}

	public void Undo()
	{
		if (CanUndo())
		{
			var command = _undoStack.Pop();
			command.Undo();
		}
	}
}