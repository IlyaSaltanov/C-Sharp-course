using System;
using System.Collections.Generic;
using System.Linq;

namespace yield;

public static class MovingMaxTask
{
	public static IEnumerable<DataPoint> MovingMax(this IEnumerable<DataPoint> data, int windowWidth)
	{
		var window = new Queue<double>();
		var potentialMaxs = new LinkedList<double>();

		foreach (var point in data)
		{
			window.Enqueue(point.OriginalY);
			AddPotentialMax(potentialMaxs, point.OriginalY);

			if (window.Count > windowWidth)
			{
				RemoveExpiredElement(window, potentialMaxs);
			}

			yield return point.WithMaxY(potentialMaxs.First.Value);
		}
	}

	private static void AddPotentialMax(LinkedList<double> potentialMaxs, double value)
	{
		while (potentialMaxs.Count > 0 && potentialMaxs.Last.Value < value)
		{
			potentialMaxs.RemoveLast();
		}
		potentialMaxs.AddLast(value);
	}

	private static void RemoveExpiredElement(Queue<double> window, LinkedList<double> potentialMaxs)
	{
		var removed = window.Dequeue();
		if (potentialMaxs.First.Value == removed)
		{
			potentialMaxs.RemoveFirst();
		}
	}
}