using System.Collections.Generic;

namespace func.brainfuck
{
	public class BrainfuckLoopCommands
	{
		public static void RegisterTo(IVirtualMachine vm)
		{
			var brackets = BuildBracketPairs(vm.Instructions);

			vm.RegisterCommand('[', b =>
			{
				if (b.Memory[b.MemoryPointer] == 0)
					b.InstructionPointer = brackets[b.InstructionPointer];
			});

			vm.RegisterCommand(']', b =>
			{
				if (b.Memory[b.MemoryPointer] != 0)
					b.InstructionPointer = brackets[b.InstructionPointer];
			});
		}

		private static Dictionary<int, int> BuildBracketPairs(string instructions)
		{
			var result = new Dictionary<int, int>();
			var stack = new Stack<int>();
			for (int i = 0; i < instructions.Length; i++)
			{
				var c = instructions[i];
				if (c == '[') stack.Push(i);
				else if (c == ']')
				{
					var open = stack.Pop();
					result[open] = i;
					result[i] = open;
				}
			}
			return result;
		}
	}
}