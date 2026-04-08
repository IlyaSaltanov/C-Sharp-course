using SQ2;

var queue = new QueueWithTwoStacks<int>();

queue.Enqueue(1);
queue.Enqueue(2);
queue.Enqueue(3);

Console.WriteLine($"Peek: {queue.Peek()}");  // 1
Console.WriteLine($"Dequeue: {queue.Dequeue()}");  // 1
Console.WriteLine($"Dequeue: {queue.Dequeue()}");  // 2
Console.WriteLine($"Count: {queue.Count}");  // 1
Console.WriteLine($"Dequeue: {queue.Dequeue()}");  // 3
