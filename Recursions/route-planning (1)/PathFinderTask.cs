using System.Drawing;
// using System.Collections.Generic;
using System;

namespace RoutePlanning;

public static class PathFinderTask
{
	private static int[] bestOrder;
    private static double bestLength;

	public static int[] FindBestCheckpointsOrder(Point[] checkpoints)
	{
		bestOrder = MakeTrivialPermutation(checkpoints.Length);
		bestLength = checkpoints.GetPathLength(bestOrder);

		var currentOrder = new int[checkpoints.Length];
        currentOrder[0] = 0;

		MakePermutations(checkpoints, currentOrder, 1, 0.0);

		return bestOrder;
	}

	private static int[] MakeTrivialPermutation(int size)
	{
		var bestOrder = new int[size];
		for (var i = 0; i < bestOrder.Length; i++)
			bestOrder[i] = i;
		return bestOrder;
	}

	static void MakePermutations(Point[] checkpoints, int[] currentOrder, int position, double currentLength)
	{
		if (currentLength >= bestLength)
			return;
			
		if (position == currentOrder.Length)
		{
      		bestLength = currentLength;
            Array.Copy(currentOrder, bestOrder, currentOrder.Length);
            return;
		}

		for (int i = 1; i < currentOrder.Length; i++)
		{
			var index = Array.IndexOf(currentOrder, i, 0, position);
			if (index != -1)
				continue;
			currentOrder[position] = i;
			double additionalLength = checkpoints[currentOrder[position - 1]].DistanceTo(checkpoints[i]);
            double newLength = currentLength + additionalLength;
			MakePermutations(checkpoints, currentOrder, position + 1, newLength);
		}
	}
}