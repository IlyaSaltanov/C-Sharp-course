namespace BeeRun.Model;

public static class LevelLoader
{
    public static void ApplyToModel(GameModel model, string[] lines)
    {
        if (lines.Length == 0)
        {
            throw new ArgumentException("Пустой уровень.");
        }

        int height = lines.Length;
        int width = 0;
        for (int r = 0; r < height; r++)
        {
            ParsedRow parsed = ParseRow(lines[r]);
            if (parsed.Cells.Count == 0)
            {
                throw new ArgumentException("Пустая строка уровня.");
            }

            if (width == 0)
            {
                width = parsed.Cells.Count;
            }
            else if (parsed.Cells.Count != width)
            {
                throw new ArgumentException("Строки уровня разной длины.");
            }
        }

        model.InitEmptyField(width, height);

        int playerCount = 0;
        for (int y = 0; y < height; y++)
        {
            ParsedRow parsed = ParseRow(lines[y]);
            for (int x = 0; x < width; x++)
            {
                TileToken t = parsed.Cells[x];
                if (t.Kind == ParsedTile.Player)
                {
                    playerCount++;
                    model.SetCell(x, y, CellKind.Empty, 0);
                    model.SetPlayerSpawn(x, y);
                }
                else if (t.Kind == ParsedTile.Wasp)
                {
                    model.SetCell(x, y, CellKind.Empty, 0);
                    model.AddWaspSpawn(x, y);
                }
                else if (t.Kind == ParsedTile.Wall)
                {
                    model.SetCell(x, y, CellKind.Wall, 0);
                }
                else if (t.Kind == ParsedTile.Empty)
                {
                    model.SetCell(x, y, CellKind.Empty, 0);
                }
                else if (t.Kind == ParsedTile.Hive)
                {
                    model.SetCell(x, y, CellKind.Hive, 0);
                }
                else if (t.Kind == ParsedTile.Bush)
                {
                    model.SetCell(x, y, CellKind.Bush, 0);
                }
                else if (t.Kind == ParsedTile.Flower)
                {
                    int n = t.Nectar;
                    if (n < 1)
                    {
                        n = 1;
                    }

                    model.SetCell(x, y, CellKind.Flower, n);
                }
            }
        }

        if (playerCount != 1)
        {
            throw new ArgumentException("На уровне должен быть ровно один символ P (пчела).");
        }
    }

    private enum ParsedTile
    {
        Wall,
        Empty,
        Player,
        Hive,
        Bush,
        Wasp,
        Flower
    }

    private struct TileToken
    {
        public ParsedTile Kind;
        public int Nectar;
    }

    private sealed class ParsedRow
    {
        public List<TileToken> Cells = new List<TileToken>();
    }

    private static ParsedRow ParseRow(string line)
    {
        ParsedRow row = new ParsedRow();
        for (int i = 0; i < line.Length;)
        {
            char c = line[i];
            if (c == 'F')
            {
                int nectar = 1;
                if (i + 1 < line.Length && char.IsDigit(line[i + 1]))
                {
                    nectar = line[i + 1] - '0';
                    i += 2;
                }
                else
                {
                    i += 1;
                }

                row.Cells.Add(new TileToken { Kind = ParsedTile.Flower, Nectar = nectar });
            }
            else if (c == '#')
            {
                row.Cells.Add(new TileToken { Kind = ParsedTile.Wall, Nectar = 0 });
                i += 1;
            }
            else if (c == '.')
            {
                row.Cells.Add(new TileToken { Kind = ParsedTile.Empty, Nectar = 0 });
                i += 1;
            }
            else if (c == 'P')
            {
                row.Cells.Add(new TileToken { Kind = ParsedTile.Player, Nectar = 0 });
                i += 1;
            }
            else if (c == 'H')
            {
                row.Cells.Add(new TileToken { Kind = ParsedTile.Hive, Nectar = 0 });
                i += 1;
            }
            else if (c == 'B')
            {
                row.Cells.Add(new TileToken { Kind = ParsedTile.Bush, Nectar = 0 });
                i += 1;
            }
            else if (c == 'O')
            {
                row.Cells.Add(new TileToken { Kind = ParsedTile.Wasp, Nectar = 0 });
                i += 1;
            }
            else if (char.IsWhiteSpace(c))
            {
                i += 1;
            }
            else
            {
                throw new ArgumentException("Неизвестный символ в уровне: " + c);
            }
        }

        return row;
    }
}
