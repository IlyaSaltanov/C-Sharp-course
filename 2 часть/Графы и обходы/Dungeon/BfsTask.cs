using System.Collections.Generic;

namespace Dungeon;

public class BfsTask
{
	public static IEnumerable<SinglyLinkedList<Point>> FindPaths(Map map, Point start, Chest[] chests)
	{
		var pathsFromStart = BuildShortestPathsFromStart(map, start);

		for (var i = 0; i < chests.Length; i++)
		{
			var chestLocation = chests[i].Location;
			if (pathsFromStart.TryGetValue(chestLocation, out var path))
				yield return path;
		}
	}

	private static Dictionary<Point, SinglyLinkedList<Point>> BuildShortestPathsFromStart(Map map, Point start)
	{
		var width = map.Dungeon.GetLength(0);
		var height = map.Dungeon.GetLength(1);

		InitializeBfsState(width, height, out var visited, out var paths, out var queue);
		EnqueueStart(start, visited, queue, paths);
		RunBfs(map, visited, queue, paths);

		return paths;
	}

	private static void InitializeBfsState(
		int width,
		int height,
		out bool[,] visited,
		out Dictionary<Point, SinglyLinkedList<Point>> paths,
		out Queue<Point> queue)
	{
		visited = new bool[width, height];
		paths = new Dictionary<Point, SinglyLinkedList<Point>>();
		queue = new Queue<Point>();
	}

	private static void EnqueueStart(
		Point start,
		bool[,] visited,
		Queue<Point> queue,
		Dictionary<Point, SinglyLinkedList<Point>> paths)
	{
		MarkAsVisited(start, visited, queue, paths, previousPath: null);
	}

	private static void RunBfs(
		Map map,
		bool[,] visited,
		Queue<Point> queue,
		Dictionary<Point, SinglyLinkedList<Point>> paths)
	{
		while (queue.Count > 0)
		{
			ProcessBfsLayer(map, visited, queue, paths);
		}
	}

	private static void ProcessBfsLayer(
		Map map,
		bool[,] visited,
		Queue<Point> queue,
		Dictionary<Point, SinglyLinkedList<Point>> paths)
	{
		var current = queue.Dequeue();
		var currentPath = paths[current];

		for (var i = 0; i < Walker.PossibleDirections.Count; i++)
		{
			var neighbor = current + Walker.PossibleDirections[i];
			TryVisitNeighbor(map, neighbor, visited, queue, paths, currentPath);
		}
	}

	private static void TryVisitNeighbor(
		Map map,
		Point neighbor,
		bool[,] visited,
		Queue<Point> queue,
		Dictionary<Point, SinglyLinkedList<Point>> paths,
		SinglyLinkedList<Point> currentPath)
	{
		if (!CanVisit(map, neighbor, visited))
			return;

		MarkAsVisited(neighbor, visited, queue, paths, currentPath);
	}

	private static bool CanVisit(Map map, Point point, bool[,] visited)
	{
		if (!map.InBounds(point))
			return false;
		if (visited[point.X, point.Y])
			return false;
		return map.Dungeon[point.X, point.Y] != MapCell.Wall;
	}

	private static void MarkAsVisited(
		Point point,
		bool[,] visited,
		Queue<Point> queue,
		Dictionary<Point, SinglyLinkedList<Point>> paths,
		SinglyLinkedList<Point> previousPath)
	{
		visited[point.X, point.Y] = true;
		paths[point] = new SinglyLinkedList<Point>(point, previousPath);
		queue.Enqueue(point);
	}
}