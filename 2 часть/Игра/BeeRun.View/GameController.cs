using System.Windows.Forms;
using BeeRun.Model;

namespace BeeRun.View;

public sealed class GameController
{
    private readonly GameModel _model;
    private readonly Action _invalidate;

    public GameController(GameModel model, Action invalidate)
    {
        _model = model;
        _invalidate = invalidate;
    }

    public GameModel Model => _model;

    public void OnTimerTick()
    {
        _model.Tick();
        _invalidate();
    }

    public void OnKeyDown(Keys key)
    {
        if (key == Keys.Up)
        {
            _model.MovePlayer(0, -1);
        }
        else if (key == Keys.Down)
        {
            _model.MovePlayer(0, 1);
        }
        else if (key == Keys.Left)
        {
            _model.MovePlayer(-1, 0);
        }
        else if (key == Keys.Right)
        {
            _model.MovePlayer(1, 0);
        }
        else if (key == Keys.Space)
        {
            _model.PlayerAction();
        }
        else if (key == Keys.Escape)
        {
            _model.GoToMenu();
        }
        else if (key == Keys.R)
        {
            _model.RestartCurrentLevel();
        }
        else if (key == Keys.N)
        {
            _model.TryStartNextLevelAfterWin();
        }

        _invalidate();
    }

    public void StartGame()
    {
        _model.StartNewGameFromMenu();
        _invalidate();
    }

    public void GoMenu()
    {
        _model.GoToMenu();
        _invalidate();
    }

    public void RestartLevel()
    {
        _model.RestartCurrentLevel();
        _invalidate();
    }

    public void NextLevel()
    {
        _model.TryStartNextLevelAfterWin();
        _invalidate();
    }
}
