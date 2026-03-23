namespace BeeRun.Model;

public struct Cell
{
    public CellKind Kind;
    public int Nectar;

    public Cell(CellKind kind, int nectar)
    {
        Kind = kind;
        Nectar = nectar;
    }
}
