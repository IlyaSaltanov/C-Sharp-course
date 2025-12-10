using System.Collections.Generic;
using Mazes.TestCases;

namespace Mazes.UI;

public partial class App
{
    private IEnumerable<TestCase> TestCases => CreateTestCases();

    private static IEnumerable<TestCase> CreateTestCases()
    {
        yield return new MazeTestCase("empty1", EmptyMazeTask.LookingForWayOut);
        yield return new MazeTestCase("empty2", EmptyMazeTask.LookingForWayOut);
        yield return new MazeTestCase("empty3", EmptyMazeTask.LookingForWayOut);
        yield return new MazeTestCase("empty4", EmptyMazeTask.LookingForWayOut);
        yield return new MazeTestCase("empty5", EmptyMazeTask.LookingForWayOut);
        yield return new MazeTestCase("snake1", SnakeMazeTask.LookingForWayOut);
        yield return new MazeTestCase("snake2", SnakeMazeTask.LookingForWayOut);
        yield return new MazeTestCase("snake3", SnakeMazeTask.LookingForWayOut);
        /*
        yield return new MazeTestCase("pyramid1", PyramidMazeTask.LookingForWayOut);
        yield return new MazeTestCase("pyramid2", PyramidMazeTask.LookingForWayOut);
        yield return new MazeTestCase("pyramid3", PyramidMazeTask.LookingForWayOut);
        yield return new MazeTestCase("pyramid4", PyramidMazeTask.LookingForWayOut);
        */
        yield return new MazeTestCase("diagonal1", DiagonalMazeTask.LookingForWayOut);
        yield return new MazeTestCase("diagonal2", DiagonalMazeTask.LookingForWayOut);
        yield return new MazeTestCase("diagonal3", DiagonalMazeTask.LookingForWayOut);
    }
}