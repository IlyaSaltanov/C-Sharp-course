using System.Drawing.Drawing2D;
using BeeRun.Model;

namespace BeeRun.View;

public class GameForm : Form
{
    private readonly GameModel _model;
    private readonly GameController _controller;
    private readonly System.Windows.Forms.Timer _timer;
    private readonly Panel _gamePanel;
    private const int CellPx = 36;

    public GameForm()
    {
        Text = "Пчелиный рейс";
        DoubleBuffered = true;
        KeyPreview = true;
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        StartPosition = FormStartPosition.CenterScreen;

        _model = new GameModel();

        _gamePanel = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = Color.FromArgb(245, 250, 255)
        };

        _controller = new GameController(_model, () => _gamePanel.Invalidate());

        _timer = new System.Windows.Forms.Timer();
        _timer.Interval = 120;
        _timer.Tick += (_, _) => _controller.OnTimerTick();

        FlowLayoutPanel buttons = new FlowLayoutPanel
        {
            Dock = DockStyle.Bottom,
            AutoSize = true,
            Padding = new Padding(8),
            WrapContents = false
        };

        Button start = new Button { Text = "Играть", AutoSize = true };
        start.Click += (_, _) => _controller.StartGame();
        Button menu = new Button { Text = "Меню", AutoSize = true };
        menu.Click += (_, _) => _controller.GoMenu();
        Button restart = new Button { Text = "Заново", AutoSize = true };
        restart.Click += (_, _) => _controller.RestartLevel();
        Button next = new Button { Text = "След. уровень", AutoSize = true };
        next.Click += (_, _) => _controller.NextLevel();

        buttons.Controls.Add(start);
        buttons.Controls.Add(menu);
        buttons.Controls.Add(restart);
        buttons.Controls.Add(next);

        _gamePanel.Paint += OnPaintField;

        Controls.Add(_gamePanel);
        Controls.Add(buttons);

        ClientSize = new Size(CellPx * 22 + 32, CellPx * 14 + buttons.Height + 32);

        KeyDown += (_, e) => _controller.OnKeyDown(e.KeyCode);
        Shown += (_, _) => _timer.Start();
    }

    private void OnPaintField(object? sender, PaintEventArgs e)
    {
        Graphics g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;

        int offsetX = 12;
        int offsetY = 12;

        if (_model.Phase == GamePhase.Menu)
        {
            DrawCenteredText(g, _gamePanel.ClientSize, "Стрелки — движение. Пробел — собрать нектар или сдать в улей.\n" +
                "Куст защищает от осы. Наберите очки до цели. R — заново уровень.\n" +
                "Нажмите «Играть».", offsetY + 40);
            return;
        }

        string banner = "";
        if (_model.Phase == GamePhase.GameOver)
        {
            banner = "Игра окончана. R — заново.";
        }
        else if (_model.Phase == GamePhase.Won)
        {
            banner = "Уровень пройден! N — следующий.";
        }

        if (banner.Length > 0)
        {
            g.DrawString(banner, Font, Brushes.DarkRed, offsetX, 4);
        }

        for (int y = 0; y < _model.Height; y++)
        {
            for (int x = 0; x < _model.Width; x++)
            {
                Rectangle r = new Rectangle(offsetX + x * CellPx, offsetY + y * CellPx, CellPx - 2, CellPx - 2);
                Cell cell = _model.GetCell(x, y);
                Brush brush = Brushes.White;
                if (cell.Kind == CellKind.Wall)
                {
                    brush = new SolidBrush(Color.FromArgb(90, 90, 90));
                }
                else if (cell.Kind == CellKind.Flower)
                {
                    brush = new SolidBrush(Color.FromArgb(255, 120, 200));
                }
                else if (cell.Kind == CellKind.Hive)
                {
                    brush = new SolidBrush(Color.FromArgb(255, 200, 60));
                }
                else if (cell.Kind == CellKind.Bush)
                {
                    brush = new SolidBrush(Color.FromArgb(40, 160, 70));
                }

                g.FillRectangle(brush, r);
                g.DrawRectangle(Pens.DimGray, r);

                if (cell.Kind == CellKind.Flower && cell.Nectar > 0)
                {
                    string s = cell.Nectar.ToString();
                    g.DrawString(s, Font, Brushes.Black, r.X + 4, r.Y + 4);
                }
            }
        }

        for (int i = 0; i < _model.Wasps.Count; i++)
        {
            WaspState w = _model.Wasps[i];
            DrawEllipseCell(g, offsetX, offsetY, w.X, w.Y, Color.OrangeRed, Color.DarkRed);
        }

        DrawEllipseCell(g, offsetX, offsetY, _model.PlayerX, _model.PlayerY, Color.Gold, Color.DarkGoldenrod);

        string hud = "Очки: " + _model.Score + " / " + _model.TargetScore +
            "   Нектар с собой: " + _model.CarriedNectar + " / " + _model.MaxCarry +
            "   Жизни: " + _model.Lives;
        if (_model.TimeLimitTicks > 0)
        {
            hud += "   Время: " + _model.TimeLeftTicks;
        }

        g.DrawString(hud, Font, Brushes.Black, offsetX, offsetY + _model.Height * CellPx + 6);
    }

    private void DrawEllipseCell(Graphics g, int ox, int oy, int cx, int cy, Color fill, Color edge)
    {
        int px = ox + cx * CellPx + 6;
        int py = oy + cy * CellPx + 6;
        int s = CellPx - 14;
        using SolidBrush b = new SolidBrush(fill);
        using Pen p = new Pen(edge, 2);
        g.FillEllipse(b, px, py, s, s);
        g.DrawEllipse(p, px, py, s, s);
    }

    private void DrawCenteredText(Graphics g, Size panelSize, string text, int top)
    {
        using StringFormat sf = new StringFormat();
        sf.Alignment = StringAlignment.Center;
        sf.LineAlignment = StringAlignment.Near;
        RectangleF area = new RectangleF(0, top, panelSize.Width, 200);
        g.DrawString(text, Font, Brushes.Black, area, sf);
    }
}
