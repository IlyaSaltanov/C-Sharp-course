using System;

namespace func.brainfuck
{
	public class BrainfuckBasicCommands
	{
		public static void RegisterTo(IVirtualMachine vm, Func<int> read, Action<char> write)
		{
			if (vm == null) throw new ArgumentNullException(nameof(vm));
			if (read == null) throw new ArgumentNullException(nameof(read));
			if (write == null) throw new ArgumentNullException(nameof(write));

			RegisterIo(vm, read, write);
			RegisterArithmetic(vm);
			RegisterShift(vm);
			RegisterConstants(vm);
		}

		private static void RegisterIo(IVirtualMachine vm, Func<int> read, Action<char> write)
		{
			vm.RegisterCommand('.', b => write((char)b.Memory[b.MemoryPointer]));

			vm.RegisterCommand(',', b =>
			{
				var value = read();
				if (value < 0) value = 0;
				b.Memory[b.MemoryPointer] = (byte)value;
			});
		}

		private static void RegisterArithmetic(IVirtualMachine vm)
		{
			vm.RegisterCommand('+', b =>
			{
				unchecked { b.Memory[b.MemoryPointer]++; }
			});

			vm.RegisterCommand('-', b =>
			{
				unchecked { b.Memory[b.MemoryPointer]--; }
			});
		}

		private static void RegisterShift(IVirtualMachine vm)
		{
			vm.RegisterCommand('>', b =>
			{
				var length = b.Memory.Length;
				b.MemoryPointer = (b.MemoryPointer + 1) % length;
			});

			vm.RegisterCommand('<', b =>
			{
				var length = b.Memory.Length;
				b.MemoryPointer = (b.MemoryPointer - 1 + length) % length;
			});
		}

		private static void RegisterConstants(IVirtualMachine vm)
		{
			const string constants = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
			foreach (var c in constants)
			{
				var constant = c;
				vm.RegisterCommand(constant, b => b.Memory[b.MemoryPointer] = (byte)constant);
			}
		}
	}
}