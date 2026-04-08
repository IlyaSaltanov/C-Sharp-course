// Пример использования текстового редактора с Undo и Redo

var editor = new TextEditor();

editor.InsertChar('H');
editor.InsertChar('i');
editor.InsertChar('!');
Console.WriteLine(editor.GetText()); // Hi!

editor.Undo();
Console.WriteLine(editor.GetText()); // Hi

editor.Undo();
editor.Undo();
Console.WriteLine(editor.GetText()); // (пусто)

editor.Redo();
editor.Redo();
Console.WriteLine(editor.GetText()); // Hi

editor.InsertChar('\n');
editor.InsertChar('C');
editor.InsertChar('#');
Console.WriteLine(editor.GetText()); // H + newline + C#

editor.Undo();
editor.Undo();
editor.Undo();
Console.WriteLine(editor.GetText()); // H

editor.Redo();
editor.Redo();
Console.WriteLine(editor.GetText()); // H + newline + C

editor.MoveCursorTo(0, 1);
editor.DeleteChar(); // удалили перевод строки между "H" и "C"
Console.WriteLine(editor.GetText()); // HC

editor.Undo(); // вернули \n
Console.WriteLine(editor.GetText()); // H + newline + C

Console.WriteLine("Done.");
