using System.Collections.Generic;

namespace yield;

public static class ExpSmoothingTask
{
	public static IEnumerable<DataPoint> SmoothExponentialy(this IEnumerable<DataPoint> data, double alpha)
	{
		var isFirst = true;
		double previousSmoothedValue = 0;

		foreach (var point in data)
		{
			double currentSmoothedValue;
			if (isFirst)
			{
				currentSmoothedValue = point.OriginalY;
				isFirst = false;
			}
			else
			{
				currentSmoothedValue = alpha * point.OriginalY + (1 - alpha) * previousSmoothedValue;
			}

			previousSmoothedValue = currentSmoothedValue;
			yield return point.WithExpSmoothedY(currentSmoothedValue);
		}
	}
}
