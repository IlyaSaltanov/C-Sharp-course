using System.Collections.Generic;

namespace Clones;

public class CloneVersionSystem : ICloneVersionSystem
{
	/// <summary>
	/// Узел стека на связном списке. При клонировании копируем только ссылку на голову — клон получает общую историю (Θ(1)).
	/// Push создаёт новый узел, Pop только сдвигает голову — существующие узлы не меняются.
	/// </summary>
	private class StackNode
	{
		public readonly string Value;
		public readonly StackNode Next;

		public StackNode(string value, StackNode next)
		{
			Value = value;
			Next = next;
		}
	}

	private class Clone
	{
		private StackNode _learnedHead;
		private StackNode _rollbackHead;

		public void Learn(string program)
		{
			_learnedHead = new StackNode(program, _learnedHead);
			_rollbackHead = null;
		}

		public void Rollback()
		{
			var program = _learnedHead.Value;
			_learnedHead = _learnedHead.Next;
			_rollbackHead = new StackNode(program, _rollbackHead);
		}

		public void Relearn()
		{
			var program = _rollbackHead.Value;
			_rollbackHead = _rollbackHead.Next;
			_learnedHead = new StackNode(program, _learnedHead);
		}

		public string Check() =>
			_learnedHead == null ? "basic" : _learnedHead.Value;

		/// <summary>
		/// Клонирование за Θ(1): копируем только ссылки на головы стеков.
		/// </summary>
		public Clone CreateClone() =>
			new Clone
			{
				_learnedHead = _learnedHead,
				_rollbackHead = _rollbackHead
			};
	}

	private readonly List<Clone> _clones = new() { new Clone() };

	public string Execute(string query)
	{
		var parts = query.Split(' ', 3);
		var cmd = parts[0];
		var ci = int.Parse(parts[1]);
		var clone = _clones[ci - 1];

		return cmd switch
		{
			"learn" => RunLearn(parts, clone),
			"rollback" => RunRollback(clone),
			"relearn" => RunRelearn(clone),
			"clone" => RunClone(clone),
			"check" => RunCheck(clone),
			_ => null
		};
	}

	private static string RunLearn(string[] parts, Clone clone)
	{
		clone.Learn(parts.Length > 2 ? parts[2] : "");
		return null;
	}

	private static string RunRollback(Clone clone)
	{
		clone.Rollback();
		return null;
	}

	private static string RunRelearn(Clone clone)
	{
		clone.Relearn();
		return null;
	}

	private string RunClone(Clone clone)
	{
		_clones.Add(clone.CreateClone());
		return null;
	}

	private static string RunCheck(Clone clone) => clone.Check();
}
