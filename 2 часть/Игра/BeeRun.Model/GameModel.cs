namespace BeeRun.Model;

public sealed class GameModel
{
    private Cell[,] _cells = new Cell[0, 0];
    private readonly List<WaspState> _wasps = new List<WaspState>();

    public int Width { get; private set; }
    public int Height { get; private set; }

    public int PlayerX { get; private set; }
    public int PlayerY { get; private set; }

    private int _spawnX;
    private int _spawnY;

    public int CarriedNectar { get; private set; }
    public int MaxCarry { get; private set; } = 3;

    public int Score { get; private set; }
    public int TargetScore { get; private set; }

    public int Lives { get; private set; } = 3;

    public int WorldTicks { get; private set; }

    public int TimeLimitTicks { get; private set; }
    public int TimeLeftTicks { get; private set; }

    public int WaspMovePeriod { get; private set; } = 2;

    public GamePhase Phase { get; private set; } = GamePhase.Menu;

    public int CurrentLevelIndex { get; private set; }

    public IReadOnlyList<WaspState> Wasps => _wasps;

    public void InitEmptyField(int width, int height)
    {
        Width = width;
        Height = height;
        _cells = new Cell[width, height];
        _wasps.Clear();
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                _cells[x, y] = new Cell(CellKind.Empty, 0);
            }
        }
    }

    public void SetCell(int x, int y, CellKind kind, int nectar)
    {
        _cells[x, y] = new Cell(kind, nectar);
    }

    public Cell GetCell(int x, int y)
    {
        return _cells[x, y];
    }

    public void SetPlayerSpawn(int x, int y)
    {
        _spawnX = x;
        _spawnY = y;
    }

    public void AddWaspSpawn(int x, int y)
    {
        _wasps.Add(new WaspState(x, y));
    }

    public void ConfigureRules(int targetScore, int timeLimitTicks, int maxCarry, int lives, int waspMovePeriod)
    {
        TargetScore = targetScore;
        TimeLimitTicks = timeLimitTicks;
        TimeLeftTicks = timeLimitTicks;
        MaxCarry = maxCarry;
        Lives = lives;
        WaspMovePeriod = waspMovePeriod;
    }

    public void LoadLevel(int levelIndex)
    {
        if (levelIndex < 0 || levelIndex >= GameLevels.All.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(levelIndex));
        }

        CurrentLevelIndex = levelIndex;
        string[] lines = GameLevels.All[levelIndex];
        LevelLoader.ApplyToModel(this, lines);
        ConfigureRules(
            GameLevels.TargetScores[levelIndex],
            GameLevels.TimeLimits[levelIndex],
            maxCarry: 3,
            lives: 3,
            waspMovePeriod: 2);
        Score = 0;
        CarriedNectar = 0;
        WorldTicks = 0;
        PlayerX = _spawnX;
        PlayerY = _spawnY;
        for (int i = 0; i < _wasps.Count; i++)
        {
            _wasps[i].ResetToSpawn();
        }
    }

    public void StartNewGameFromMenu()
    {
        Phase = GamePhase.Playing;
        LoadLevel(0);
    }

    public void RestartCurrentLevel()
    {
        if (Phase != GamePhase.Playing && Phase != GamePhase.GameOver && Phase != GamePhase.Won)
        {
            return;
        }

        LoadLevel(CurrentLevelIndex);
        Phase = GamePhase.Playing;
    }

    public void GoToMenu()
    {
        Phase = GamePhase.Menu;
    }

    public void TryStartNextLevelAfterWin()
    {
        if (Phase != GamePhase.Won)
        {
            return;
        }

        int next = CurrentLevelIndex + 1;
        if (next >= GameLevels.All.Length)
        {
            Phase = GamePhase.Menu;
            return;
        }

        LoadLevel(next);
        Phase = GamePhase.Playing;
    }

    public bool IsPlayerSafeFromWasps()
    {
        Cell c = _cells[PlayerX, PlayerY];
        return c.Kind == CellKind.Bush;
    }

    public void MovePlayer(int dx, int dy)
    {
        if (Phase != GamePhase.Playing)
        {
            return;
        }

        int nx = PlayerX + dx;
        int ny = PlayerY + dy;
        if (nx < 0 || ny < 0 || nx >= Width || ny >= Height)
        {
            return;
        }

        Cell target = _cells[nx, ny];
        if (target.Kind == CellKind.Wall)
        {
            return;
        }

        PlayerX = nx;
        PlayerY = ny;
        CheckWaspHits();
        CheckWin();
    }

    public void PlayerAction()
    {
        if (Phase != GamePhase.Playing)
        {
            return;
        }

        Cell here = _cells[PlayerX, PlayerY];
        if (here.Kind == CellKind.Hive)
        {
            TryDepositAtHive();
        }
        else if (here.Kind == CellKind.Flower)
        {
            TryCollectNectar();
        }

        CheckWaspHits();
        CheckWin();
    }

    public void TryCollectNectar()
    {
        if (Phase != GamePhase.Playing)
        {
            return;
        }

        Cell here = _cells[PlayerX, PlayerY];
        if (here.Kind != CellKind.Flower)
        {
            return;
        }

        if (here.Nectar <= 0)
        {
            return;
        }

        if (CarriedNectar >= MaxCarry)
        {
            return;
        }

        int take = 1;
        if (CarriedNectar + take > MaxCarry)
        {
            take = MaxCarry - CarriedNectar;
        }

        int left = here.Nectar - take;
        CarriedNectar += take;
        if (left <= 0)
        {
            _cells[PlayerX, PlayerY] = new Cell(CellKind.Empty, 0);
        }
        else
        {
            _cells[PlayerX, PlayerY] = new Cell(CellKind.Flower, left);
        }
    }

    public void TryDepositAtHive()
    {
        if (Phase != GamePhase.Playing)
        {
            return;
        }

        Cell here = _cells[PlayerX, PlayerY];
        if (here.Kind != CellKind.Hive)
        {
            return;
        }

        if (CarriedNectar <= 0)
        {
            return;
        }

        Score += CarriedNectar;
        CarriedNectar = 0;
    }

    public void Tick()
    {
        if (Phase != GamePhase.Playing)
        {
            return;
        }

        WorldTicks++;
        if (TimeLimitTicks > 0)
        {
            TimeLeftTicks--;
            if (TimeLeftTicks <= 0)
            {
                TimeLeftTicks = 0;
                Phase = GamePhase.GameOver;
                return;
            }
        }

        if (WaspMovePeriod > 0 && WorldTicks % WaspMovePeriod == 0)
        {
            MoveAllWaspsTowardPlayer();
        }

        CheckWaspHits();
        CheckWin();
    }

    private void MoveAllWaspsTowardPlayer()
    {
        for (int i = 0; i < _wasps.Count; i++)
        {
            WaspState w = _wasps[i];
            int dx = 0;
            int dy = 0;
            if (PlayerX > w.X)
            {
                dx = 1;
            }
            else if (PlayerX < w.X)
            {
                dx = -1;
            }

            if (dx == 0)
            {
                if (PlayerY > w.Y)
                {
                    dy = 1;
                }
                else if (PlayerY < w.Y)
                {
                    dy = -1;
                }
            }

            int nx = w.X + dx;
            int ny = w.Y + dy;
            if (nx >= 0 && ny >= 0 && nx < Width && ny < Height)
            {
                Cell c = _cells[nx, ny];
                if (c.Kind != CellKind.Wall)
                {
                    w.X = nx;
                    w.Y = ny;
                }
            }
        }
    }

    private void CheckWaspHits()
    {
        if (Phase != GamePhase.Playing)
        {
            return;
        }

        if (IsPlayerSafeFromWasps())
        {
            return;
        }

        for (int i = 0; i < _wasps.Count; i++)
        {
            WaspState w = _wasps[i];
            if (w.X == PlayerX && w.Y == PlayerY)
            {
                HurtPlayer();
                return;
            }
        }
    }

    private void HurtPlayer()
    {
        Lives--;
        if (Lives <= 0)
        {
            Lives = 0;
            Phase = GamePhase.GameOver;
            return;
        }

        PlayerX = _spawnX;
        PlayerY = _spawnY;
        CarriedNectar = 0;
        for (int i = 0; i < _wasps.Count; i++)
        {
            _wasps[i].ResetToSpawn();
        }
    }

    private void CheckWin()
    {
        if (Phase != GamePhase.Playing)
        {
            return;
        }

        if (Score >= TargetScore)
        {
            Phase = GamePhase.Won;
        }
    }
}
