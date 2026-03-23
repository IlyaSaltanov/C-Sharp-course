namespace BeeRun.Model;

public static class GameLevels
{
    public static readonly string[][] All =
    {
        TutorialLines,
        Level2Lines
    };

    public static readonly int[] TargetScores = { 3, 6 };

    public static readonly int[] TimeLimits = { 900, 1200 };

    private static readonly string[] TutorialLines =
    {
        "###############",
        "#P....O.......#",
        "#.#.#.#.#.#.#.#",
        "#..F..B..H..F.#",
        "#..............#",
        "###############"
    };

    private static readonly string[] Level2Lines =
    {
        "###################",
        "#P......F.........#",
        "#.###.###.###.###.#",
        "#..F#..FO...#...H.#",
        "#.B.#.F...B...F...#",
        "#....F.O....O....F#",
        "###################"
    };
}
