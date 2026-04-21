using System;
using System.Collections.Generic;
using System.Linq;

namespace Dungeon;

public class DungeonTask
{
	public static MoveDirection[] FindShortestPath(Map map)
	{
		var fromStart = BuildBfsData(map, map.InitialPosition);
		if (fromStart.Distance[map.Exit.X, map.Exit.Y] < 0)
			return Array.Empty<MoveDirection>();

		if (map.Chests.Length == 0)
			return BuildDirectionsFromStartToTarget(fromStart, map.Exit);

		var fromExit = BuildBfsData(map, map.Exit);
		var bestChest = FindBestChest(map.Chests, fromStart.Distance, fromExit.Distance);
		if (bestChest == null)
			return BuildDirectionsFromStartToTarget(fromStart, map.Exit);

		var pathStartToChest = BuildDirectionsFromStartToTarget(fromStart, bestChest.Location);
		var pathChestToExit = BuildDirectionsFromPointToBfsOrigin(fromExit, bestChest.Location);
		return CombinePaths(pathStartToChest, pathChestToExit);
	}

	private sealed class BfsData
	{
		public readonly int[,] Distance;
		public readonly Point[,] Parent;

		public BfsData(int[,] distance, Point[,] parent)
		{
			Distance = distance;
			Parent = parent;
		}
	}

	private static BfsData BuildBfsData(Map map, Point start)
	{
		var width = map.Dungeon.GetLength(0);
		var height = map.Dungeon.GetLength(1);

		InitializeBfsBuffers(width, height, out var distance, out var parent);
		var queue = new Queue<Point>();
		EnqueueStart(queue, distance, start);

		while (queue.Count > 0)
		{
			var current = queue.Dequeue();
			var currentDistance = distance[current.X, current.Y];

			for (var i = 0; i < Walker.PossibleDirections.Count; i++)
			{
				var next = current + Walker.PossibleDirections[i];
				TryVisitNeighbor(map, queue, distance, parent, current, currentDistance, next);
			}
		}

		return new BfsData(distance, parent);
	}

	private static void InitializeBfsBuffers(int width, int height, out int[,] distance, out Point[,] parent)
	{
		distance = new int[width, height];
		parent = new Point[width, height];

		for (var x = 0; x < width; x++)
		for (var y = 0; y < height; y++)
		{
			distance[x, y] = -1;
			parent[x, y] = Point.Null;
		}
	}

	private static void EnqueueStart(Queue<Point> queue, int[,] distance, Point start)
	{
		queue.Enqueue(start);
		distance[start.X, start.Y] = 0;
	}

	private static void TryVisitNeighbor(
		Map map,
		Queue<Point> queue,
		int[,] distance,
		Point[,] parent,
		Point current,
		int currentDistance,
		Point next)
	{
		if (!CanStepOn(map, next))
			return;
		if (distance[next.X, next.Y] >= 0)
			return;

		distance[next.X, next.Y] = currentDistance + 1;
		parent[next.X, next.Y] = current;
		queue.Enqueue(next);
	}

	private static bool CanStepOn(Map map, Point point)
	{
		if (!map.InBounds(point))
			return false;
		return map.Dungeon[point.X, point.Y] != MapCell.Wall;
	}

	private static Chest FindBestChest(Chest[] chests, int[,] distanceFromStart, int[,] distanceFromExit)
	{
		Chest bestChest = null;
		var bestLength = int.MaxValue;
		var bestChestValue = -1;

		for (var i = 0; i < chests.Length; i++)
		{
			var chest = chests[i];
			var totalLength = GetTotalLengthIfReachable(chest.Location, distanceFromStart, distanceFromExit);
			if (totalLength == null)
				continue;

			bestChest = UpdateBestChestIfBetter(chest, totalLength.Value, bestChest, ref bestLength, ref bestChestValue);
		}

		return bestChest;
	}

	private static int? GetTotalLengthIfReachable(Point chestLocation, int[,] distanceFromStart, int[,] distanceFromExit)
	{
		var d1 = distanceFromStart[chestLocation.X, chestLocation.Y];
		if (d1 < 0)
			return null;
		var d2 = distanceFromExit[chestLocation.X, chestLocation.Y];
		if (d2 < 0)
			return null;
		return d1 + d2;
	}

	private static Chest UpdateBestChestIfBetter(
		Chest chest,
		int totalLength,
		Chest bestChest,
		ref int bestLength,
		ref int bestChestValue)
	{
		var isBetterByLength = totalLength < bestLength;
		var isBetterByChestValue = totalLength == bestLength && chest.Value > bestChestValue;
		if (!isBetterByLength && !isBetterByChestValue)
			return bestChest;

		bestLength = totalLength;
		bestChestValue = chest.Value;
		return chest;
	}

	private static MoveDirection[] BuildDirectionsFromStartToTarget(BfsData bfs, Point target)
	{
		var points = BuildPointsFromNodeToBfsOrigin(bfs, target);
		points.Reverse();
		return ConvertPointsToDirections(points);
	}

	private static MoveDirection[] BuildDirectionsFromPointToBfsOrigin(BfsData bfs, Point startPoint)
	{
		var points = BuildPointsFromNodeToBfsOrigin(bfs, startPoint);
		return ConvertPointsToDirections(points);
	}

	private static List<Point> BuildPointsFromNodeToBfsOrigin(BfsData bfs, Point startPoint)
	{
		if (bfs.Distance[startPoint.X, startPoint.Y] < 0)
			return new List<Point>();

		var points = new List<Point>();
		var current = startPoint;
		points.Add(current);

		while (bfs.Distance[current.X, current.Y] > 0)
		{
			var p = bfs.Parent[current.X, current.Y];
			points.Add(p);
			current = p;
		}

		return points;
	}

	private static MoveDirection[] ConvertPointsToDirections(List<Point> points)
	{
		if (points.Count <= 1)
			return Array.Empty<MoveDirection>();

		var directions = new MoveDirection[points.Count - 1];
		for (var i = 0; i < points.Count - 1; i++)
		{
			var offset = points[i + 1] - points[i];
			directions[i] = Walker.ConvertOffsetToDirection(offset);
		}

		return directions;
	}

	private static MoveDirection[] CombinePaths(MoveDirection[] first, MoveDirection[] second)
	{
		if (first.Length == 0)
			return second.ToArray();
		if (second.Length == 0)
			return first.ToArray();

		var result = new MoveDirection[first.Length + second.Length];
		Array.Copy(first, 0, result, 0, first.Length);
		Array.Copy(second, 0, result, first.Length, second.Length);
		return result;
	}
}