namespace BeeRun.Model;

public sealed class WaspState
{
    public int X;
    public int Y;
    public int SpawnX;
    public int SpawnY;

    public WaspState(int x, int y)
    {
        X = x;
        Y = y;
        SpawnX = x;
        SpawnY = y;
    }

    public void ResetToSpawn()
    {
        X = SpawnX;
        Y = SpawnY;
    }
}
