using System;
using System.Collections.Generic;
using System.Linq;

namespace Autocomplete;

public class RightBorderTask
{
	/// <returns>
	/// Возвращает индекс правой границы. 
	/// То есть индекс минимального элемента, который не начинается с prefix и большего prefix.
	/// Если такого нет, то возвращает items.Length
	/// </returns>
	/// <remarks>
	/// Функция должна быть НЕ рекурсивной
	/// и работать за O(log(items.Length)*L), где L — ограничение сверху на длину фразы
	/// </remarks>
	static int MyCompare(string currentPhrase, string prefix)
	{
		return string.Compare(currentPhrase, prefix, StringComparison.InvariantCultureIgnoreCase);
	}
	static bool MyStartsWith(string currentPhrase, string prefix)
    {
        return !currentPhrase.StartsWith(prefix, StringComparison.InvariantCultureIgnoreCase);
    }
	public static int GetRightBorderIndex(IReadOnlyList<string> phrases, string prefix, int left, int right)
	{
		// IReadOnlyList похож на List, но у него нет методов модификации списка.
		int itemsLength = phrases.Count;
		bool flag = itemsLength > 0;

		while (flag && left + 1 < right)
		{
			var m = left + (right - left) / 2;
			var currentPhrase = phrases[m];

			var comparison = MyCompare(currentPhrase, prefix);
			var variablStartWith = MyStartsWith(currentPhrase, prefix);

			if (comparison > 0 && variablStartWith)
			{
				right = m;
			}
			else
			{
				left = m;
			}
		}

		bool lastComparison = MyCompare(phrases[left], prefix) > 0;
		var lastVariablStartWith = MyStartsWith(phrases[left], prefix);
		// bool result = lastComparison && lastVariablStartWith;
		bool result = lastComparison;
		if (flag && result)
		{
			return left;
		}
		else if (flag && !result)
		{
			return right;
		}
		else return itemsLength;
	}
}