using SQ1;

var stack = new StackWithGetMax<int>();

stack.Push(3);
stack.Push(1);
stack.Push(4);
stack.Push(1);
stack.Push(5);

Console.WriteLine($"GetMax: {stack.GetMax()}"); // 5
stack.Pop();
Console.WriteLine($"GetMax после Pop: {stack.GetMax()}"); // 4
stack.Pop();
stack.Pop();
Console.WriteLine($"GetMax после трёх Pop: {stack.GetMax()}"); // 3
