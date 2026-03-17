using System;
using System.Collections.Generic;

namespace func.brainfuck
{
	public class VirtualMachine : IVirtualMachine
	{
		private readonly Dictionary<char, Action<IVirtualMachine>> commands = new();

		public string Instructions { get; }
		public int InstructionPointer { get; set; }
		public byte[] Memory { get; }
		public int MemoryPointer { get; set; }

		public VirtualMachine(string program, int memorySize)
		{
			Instructions = program ?? "";
			Memory = new byte[memorySize];
			InstructionPointer = 0;
			MemoryPointer = 0;
		}

		public void RegisterCommand(char symbol, Action<IVirtualMachine> execute)
		{
			if (execute == null) throw new ArgumentNullException(nameof(execute));
			commands[symbol] = execute;
		}

		public void Run()
		{
			while (InstructionPointer >= 0 && InstructionPointer < Instructions.Length)
			{
				var instruction = Instructions[InstructionPointer];
				if (commands.TryGetValue(instruction, out var execute))
					execute(this);
				InstructionPointer++;
			}
		}
	}
}