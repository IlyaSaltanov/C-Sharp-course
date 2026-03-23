using BeeRun.Model;

namespace BeeRun.Tests;

public class GameModelTests
{
    [Fact]
    public void NewModel_HasZeroSize_UntilLevelLoaded()
    {
        GameModel m = new GameModel();
        Assert.Equal(0, m.Width);
        Assert.Equal(0, m.Height);
        Assert.Equal(GamePhase.Menu, m.Phase);
    }

    [Fact]
    public void LoadLevel_SetsDimensions_FromTutorial()
    {
        GameModel m = new GameModel();
        m.LoadLevel(0);
        Assert.True(m.Width > 0);
        Assert.True(m.Height > 0);
    }

    [Fact]
    public void MovePlayer_IntoWall_DoesNotMove()
    {
        string[] lines =
        {
            "###",
            "#P#",
            "###"
        };
        GameModel m = new GameModel();
        LevelLoader.ApplyToModel(m, lines);
        m.ConfigureRules(10, 0, 3, 3, 2);
        m.Phase = GamePhase.Playing;
        int x0 = m.PlayerX;
        int y0 = m.PlayerY;
        m.MovePlayer(-1, 0);
        Assert.Equal(x0, m.PlayerX);
        Assert.Equal(y0, m.PlayerY);
    }

    [Fact]
    public void MovePlayer_IntoEmpty_Moves()
    {
        string[] lines =
        {
            "#####",
            "#P..#",
            "#####"
        };
        GameModel m = new GameModel();
        LevelLoader.ApplyToModel(m, lines);
        m.ConfigureRules(10, 0, 3, 3, 2);
        m.Phase = GamePhase.Playing;
        m.MovePlayer(1, 0);
        Assert.Equal(2, m.PlayerX);
    }

    [Fact]
    public void TryCollectNectar_IncreasesCarry_AndDecreasesFlower()
    {
        string[] lines =
        {
            "###",
            "#F2#",
            "#P.#",
            "###"
        };
        GameModel m = new GameModel();
        LevelLoader.ApplyToModel(m, lines);
        m.ConfigureRules(10, 0, 3, 3, 2);
        m.Phase = GamePhase.Playing;
        m.MovePlayer(0, -1);
        m.TryCollectNectar();
        Assert.Equal(1, m.CarriedNectar);
        Cell c = m.GetCell(1, 1);
        Assert.Equal(CellKind.Flower, c.Kind);
        Assert.Equal(1, c.Nectar);
    }

    [Fact]
    public void TryCollectNectar_DoesNotExceedMaxCarry()
    {
        string[] lines =
        {
            "#####",
            "#F5#",
            "#P.#",
            "#####"
        };
        GameModel m = new GameModel();
        LevelLoader.ApplyToModel(m, lines);
        m.ConfigureRules(10, 0, maxCarry: 2, lives: 3, waspMovePeriod: 2);
        m.Phase = GamePhase.Playing;
        m.MovePlayer(0, -1);
        m.TryCollectNectar();
        m.TryCollectNectar();
        m.TryCollectNectar();
        Assert.Equal(2, m.CarriedNectar);
        Cell c = m.GetCell(1, 1);
        Assert.Equal(3, c.Nectar);
    }

    [Fact]
    public void TryDepositAtHive_AddsScore_AndClearsCarry()
    {
        string[] lines =
        {
            "#####",
            "#F2#",
            "#P.#",
            "#H.#",
            "#####"
        };
        GameModel m = new GameModel();
        LevelLoader.ApplyToModel(m, lines);
        m.ConfigureRules(10, 0, 3, 3, 2);
        m.Phase = GamePhase.Playing;
        m.MovePlayer(0, -1);
        m.TryCollectNectar();
        m.TryCollectNectar();
        m.MovePlayer(0, 1);
        m.MovePlayer(0, 1);
        m.TryDepositAtHive();
        Assert.Equal(0, m.CarriedNectar);
        Assert.Equal(2, m.Score);
    }

    [Fact]
    public void Deposit_WithCarry_IncreasesScore()
    {
        string[] lines =
        {
            "#######",
            "#F2..H#",
            "#..P..#",
            "#######"
        };
        GameModel m = new GameModel();
        LevelLoader.ApplyToModel(m, lines);
        m.ConfigureRules(10, 0, 3, 3, 2);
        m.Phase = GamePhase.Playing;
        m.MovePlayer(-1, 0);
        m.MovePlayer(-1, 0);
        m.MovePlayer(0, -1);
        m.TryCollectNectar();
        m.TryCollectNectar();
        m.MovePlayer(1, 0);
        m.MovePlayer(1, 0);
        m.MovePlayer(1, 0);
        m.MovePlayer(1, 0);
        m.PlayerAction();
        Assert.Equal(2, m.Score);
        Assert.Equal(0, m.CarriedNectar);
    }

    [Fact]
    public void Score_ReachesTarget_SetsWon()
    {
        string[] lines =
        {
            "#######",
            "#F3..H#",
            "#..P..#",
            "#######"
        };
        GameModel m = new GameModel();
        LevelLoader.ApplyToModel(m, lines);
        m.ConfigureRules(targetScore: 2, timeLimitTicks: 0, maxCarry: 3, lives: 3, waspMovePeriod: 2);
        m.Phase = GamePhase.Playing;
        m.MovePlayer(-1, 0);
        m.MovePlayer(-1, 0);
        m.MovePlayer(0, -1);
        m.TryCollectNectar();
        m.TryCollectNectar();
        m.MovePlayer(1, 0);
        m.MovePlayer(1, 0);
        m.MovePlayer(1, 0);
        m.MovePlayer(1, 0);
        m.PlayerAction();
        Assert.Equal(GamePhase.Won, m.Phase);
    }

    [Fact]
    public void Wasp_MovesTowardPlayer_OverTicks()
    {
        string[] lines =
        {
            "#####",
            "#P..O#",
            "#####"
        };
        GameModel m = new GameModel();
        LevelLoader.ApplyToModel(m, lines);
        m.ConfigureRules(10, 0, 3, 3, waspMovePeriod: 1);
        m.Phase = GamePhase.Playing;
        int d0 = Distance(m);
        m.Tick();
        int d1 = Distance(m);
        Assert.True(d1 < d0);
    }

    private static int Distance(GameModel m)
    {
        if (m.Wasps.Count == 0)
        {
            return 999;
        }

        WaspState w = m.Wasps[0];
        return Math.Abs(w.X - m.PlayerX) + Math.Abs(w.Y - m.PlayerY);
    }

    [Fact]
    public void WaspHit_OutsideBush_LosesLife_AndResetsBee()
    {
        string[] lines =
        {
            "####",
            "#PO#",
            "####"
        };
        GameModel m = new GameModel();
        LevelLoader.ApplyToModel(m, lines);
        m.ConfigureRules(10, 0, 3, lives: 2, waspMovePeriod: 1);
        m.Phase = GamePhase.Playing;
        int sx = m.PlayerX;
        int sy = m.PlayerY;
        m.Tick();
        Assert.Equal(1, m.Lives);
        Assert.Equal(sx, m.PlayerX);
        Assert.Equal(sy, m.PlayerY);
    }

    [Fact]
    public void WaspHit_InsideBush_DoesNotLoseLife()
    {
        string[] lines =
        {
            "#######",
            "#PBO###",
            "#######"
        };
        GameModel m = new GameModel();
        LevelLoader.ApplyToModel(m, lines);
        m.ConfigureRules(10, 0, 3, lives: 2, waspMovePeriod: 1);
        m.Phase = GamePhase.Playing;
        m.MovePlayer(1, 0);
        int livesBefore = m.Lives;
        m.Tick();
        Assert.Equal(livesBefore, m.Lives);
    }

    [Fact]
    public void LevelLoader_ParsesFlowerWithDigit()
    {
        string[] lines =
        {
            "###",
            "#F3#",
            "#P#",
            "###"
        };
        GameModel m = new GameModel();
        LevelLoader.ApplyToModel(m, lines);
        Cell c = m.GetCell(1, 1);
        Assert.Equal(CellKind.Flower, c.Kind);
        Assert.Equal(3, c.Nectar);
    }

    [Fact]
    public void TimeRunsOut_SetsGameOver()
    {
        string[] lines =
        {
            "###",
            "#P#",
            "###"
        };
        GameModel m = new GameModel();
        LevelLoader.ApplyToModel(m, lines);
        m.ConfigureRules(10, timeLimitTicks: 1, 3, 3, 2);
        m.Phase = GamePhase.Playing;
        m.Tick();
        Assert.Equal(GamePhase.GameOver, m.Phase);
    }

    [Fact]
    public void StartNewGameFromMenu_SetsPlaying()
    {
        GameModel m = new GameModel();
        m.StartNewGameFromMenu();
        Assert.Equal(GamePhase.Playing, m.Phase);
    }

    [Fact]
    public void TryStartNextLevelAfterWin_LoadsNext_WhenAvailable()
    {
        GameModel m = new GameModel();
        m.LoadLevel(0);
        m.Phase = GamePhase.Won;
        m.TryStartNextLevelAfterWin();
        Assert.Equal(1, m.CurrentLevelIndex);
        Assert.Equal(GamePhase.Playing, m.Phase);
    }

    [Fact]
    public void TryStartNextLevelAfterWin_OnLastLevel_GoesToMenu()
    {
        GameModel m = new GameModel();
        m.LoadLevel(1);
        m.Phase = GamePhase.Won;
        m.TryStartNextLevelAfterWin();
        Assert.Equal(GamePhase.Menu, m.Phase);
    }

    [Fact]
    public void LoadLevel_LoadsEveryEmbeddedLevel_WithoutThrow()
    {
        for (int i = 0; i < GameLevels.All.Length; i++)
        {
            GameModel m = new GameModel();
            m.LoadLevel(i);
            Assert.True(m.Width > 0);
            Assert.True(m.Height > 0);
        }
    }
}
