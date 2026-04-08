// Простой текстовый редактор с операциями Undo и Redo

// Базовый класс обратимого действия (паттерн Команда)
internal abstract class EditorAction
{
    public abstract void Execute(TextEditor editor);
}

// Действие "вставить символ" (применяется при Redo или при отмене Delete)
internal class InsertAction : EditorAction
{
    public int Row { get; }
    public int Col { get; }
    public char Char { get; }

    public InsertAction(int row, int col, char c)
    {
        Row = row;
        Col = col;
        Char = c;
    }

    public override void Execute(TextEditor editor)
    {
        editor.ApplyInsert(Row, Col, Char);
    }
}

// Действие "удалить символ" (применяется при Undo вставки или при Redo удаления)
internal class DeleteAction : EditorAction
{
    public int Row { get; }
    public int Col { get; }

    public DeleteAction(int row, int col)
    {
        Row = row;
        Col = col;
    }

    public override void Execute(TextEditor editor)
    {
        editor.ApplyDelete(Row, Col);
    }
}

internal class TextEditor
{
    private readonly List<string> _lines = new() { "" };
    private int _row;
    private int _col;
    private readonly Stack<EditorAction> _undoStack = new();
    private readonly Stack<EditorAction> _redoStack = new();

    public void MoveCursorTo(int row, int col)
    {
        _row = Math.Clamp(row, 0, _lines.Count - 1);
        _col = Math.Clamp(col, 0, _lines[_row].Length);
    }

    public void InsertChar(char c)
    {
        _redoStack.Clear();
        if (c == '\n')
        {
            // Разбиение строки
            var line = _lines[_row];
            var before = line[.._col];
            var after = line[_col..];
            _lines[_row] = before;
            _lines.Insert(_row + 1, after);
            _undoStack.Push(new InsertAction(_row, _col, '\n'));
            _row++;
            _col = 0;
        }
        else
        {
            _lines[_row] = _lines[_row].Insert(_col, c.ToString());
            _undoStack.Push(new DeleteAction(_row, _col));
            _col++;
        }
    }

    public void DeleteChar()
    {
        _redoStack.Clear();
        if (_col < _lines[_row].Length)
        {
            var deleted = _lines[_row][_col];
            _lines[_row] = _lines[_row].Remove(_col, 1);
            _undoStack.Push(new InsertAction(_row, _col, deleted));
        }
        else if (_row < _lines.Count - 1)
        {
            // Удаление перевода строки — склеиваем с следующей
            var nextLine = _lines[_row + 1];
            _lines[_row] += nextLine;
            _lines.RemoveAt(_row + 1);
            _undoStack.Push(new InsertAction(_row, _col, '\n'));
        }
    }

    public void Undo()
    {
        if (_undoStack.Count == 0) return;
        var action = _undoStack.Pop();
        PushInverseToRedoBeforeUndo(action);
        action.Execute(this);
    }

    public void Redo()
    {
        if (_redoStack.Count == 0) return;
        var action = _redoStack.Pop();
        PushInverseToUndoBeforeRedo(action);
        action.Execute(this);
    }

    // Внутренние методы для применения действий (без изменения стеков)
    internal void ApplyInsert(int row, int col, char c)
    {
        MoveCursorTo(row, col);
        if (c == '\n')
        {
            var line = _lines[_row];
            var before = line[.._col];
            var after = line[_col..];
            _lines[_row] = before;
            _lines.Insert(_row + 1, after);
            _row++;
            _col = 0;
        }
        else
        {
            _lines[_row] = _lines[_row].Insert(_col, c.ToString());
            _col++;
        }
    }

    internal void ApplyDelete(int row, int col)
    {
        MoveCursorTo(row, col);
        if (_col < _lines[_row].Length)
        {
            _lines[_row] = _lines[_row].Remove(_col, 1);
        }
        else if (_row < _lines.Count - 1)
        {
            var nextLine = _lines[_row + 1];
            _lines[_row] += nextLine;
            _lines.RemoveAt(_row + 1);
        }
    }

    private void PushInverseToRedoBeforeUndo(EditorAction actionToUndo)
    {
        switch (actionToUndo)
        {
            case InsertAction ins:
                _redoStack.Push(new DeleteAction(ins.Row, ins.Col));
                break;
            case DeleteAction del:
                var ch = GetCharAt(del.Row, del.Col);
                _redoStack.Push(new InsertAction(del.Row, del.Col, ch));
                break;
        }
    }

    private char GetCharAt(int row, int col)
    {
        if (row < 0 || row >= _lines.Count) return '\0';
        var line = _lines[row];
        if (col < line.Length) return line[col];
        if (col == line.Length && row < _lines.Count - 1) return '\n';
        return '\0';
    }

    private void PushInverseToUndoBeforeRedo(EditorAction actionToRedo)
    {
        switch (actionToRedo)
        {
            case InsertAction ins:
                _undoStack.Push(new DeleteAction(ins.Row, ins.Col));
                break;
            case DeleteAction del:
                var ch = GetCharAt(del.Row, del.Col);
                _undoStack.Push(new InsertAction(del.Row, del.Col, ch));
                break;
        }
    }

    public string GetText() => string.Join(Environment.NewLine, _lines);
    public (int row, int col) GetCursor() => (_row, _col);
}
