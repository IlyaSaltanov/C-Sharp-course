using System;
using System.Drawing;
using System.Windows.Forms;

namespace SimpleGame
{
    public partial class MainForm : Form
    {
        private Button targetButton = null!;
        private Label scoreLabel = null!;
        private Label timerLabel = null!;
        private int score = 0;
        private int timeLeft = 30;
        private System.Windows.Forms.Timer gameTimer = null!;
        private Random random = new Random();

        public MainForm()
        {
            InitializeComponent();
            SetupGame();
        }

        private void InitializeComponent()
        {
            this.Text = "Простая игра";
            this.Size = new Size(800, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.LightGray;
        }

        private void SetupGame()
        {
            // Создаем кнопку для кликов
            targetButton = new Button
            {
                Text = "Нажми меня!",
                Size = new Size(100, 50),
                Location = new Point(350, 250),
                BackColor = Color.Red,
                ForeColor = Color.White,
                Font = new Font("Arial", 10, FontStyle.Bold)
            };
            targetButton.Click += TargetButton_Click;
            this.Controls.Add(targetButton);

            // Создаем метку для счета
            scoreLabel = new Label
            {
                Text = "Счет: 0",
                Location = new Point(10, 10),
                Size = new Size(100, 30),
                Font = new Font("Arial", 12, FontStyle.Bold),
                BackColor = Color.White
            };
            this.Controls.Add(scoreLabel);

            // Создаем метку для таймера
            timerLabel = new Label
            {
                Text = "Время: 30",
                Location = new Point(10, 50),
                Size = new Size(100, 30),
                Font = new Font("Arial", 12, FontStyle.Bold),
                BackColor = Color.White
            };
            this.Controls.Add(timerLabel);

            // Создаем кнопку старта
            Button startButton = new Button
            {
                Text = "Старт",
                Location = new Point(10, 100),
                Size = new Size(80, 30),
                BackColor = Color.Green,
                ForeColor = Color.White
            };
            startButton.Click += StartButton_Click;
            this.Controls.Add(startButton);

            // Создаем таймер
            gameTimer = new System.Windows.Forms.Timer();
            gameTimer.Interval = 1000; // 1 секунда
            gameTimer.Tick += GameTimer_Tick;

            // Изначально кнопка неактивна
            targetButton.Enabled = false;
        }

        private void StartButton_Click(object? sender, EventArgs e)
        {
            // Сбрасываем игру
            score = 0;
            timeLeft = 30;
            UpdateScore();
            UpdateTimer();
            
            // Активируем игровую кнопку
            targetButton.Enabled = true;
            targetButton.BackColor = Color.Red;
            
            // Запускаем таймер
            gameTimer.Start();
            
            // Делаем кнопку старта неактивной
            if (sender is Button startBtn)
            {
                startBtn.Enabled = false;
            }
        }

        private void TargetButton_Click(object? sender, EventArgs e)
        {
            if (gameTimer.Enabled)
            {
                // Увеличиваем счет
                score++;
                UpdateScore();
                
                // Перемещаем кнопку в случайное место
                MoveButtonToRandomPosition();
                
                // Меняем цвет кнопки на случайный
                targetButton.BackColor = Color.FromArgb(
                    random.Next(100, 255),
                    random.Next(100, 255),
                    random.Next(100, 255));
                
                // Меняем текст кнопки
                string[] texts = { "Клик!", "Сюда!", "Быстрее!", "Еще!", "Молодец!" };
                targetButton.Text = texts[random.Next(texts.Length)];
            }
        }

        private void GameTimer_Tick(object? sender, EventArgs e)
        {
            timeLeft--;
            UpdateTimer();
            
            if (timeLeft <= 0)
            {
                // Игра окончена
                gameTimer.Stop();
                targetButton.Enabled = false;
                targetButton.BackColor = Color.Gray;
                targetButton.Text = "Игра окончена";
                
                MessageBox.Show($"Игра окончена! Ваш счет: {score}", 
                    "Конец игры", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Information);
                
                // Активируем кнопку старта снова
                foreach (Control control in this.Controls)
                {
                    if (control is Button btn && btn.Text == "Старт")
                    {
                        btn.Enabled = true;
                        break;
                    }
                }
            }
        }

        private void MoveButtonToRandomPosition()
        {
            int maxX = this.ClientSize.Width - targetButton.Width;
            int maxY = this.ClientSize.Height - targetButton.Height;
            
            if (maxX > 0 && maxY > 0)
            {
                int newX = random.Next(0, maxX);
                int newY = random.Next(0, maxY);
                targetButton.Location = new Point(newX, newY);
            }
        }

        private void UpdateScore()
        {
            scoreLabel.Text = $"Счет: {score}";
        }

        private void UpdateTimer()
        {
            timerLabel.Text = $"Время: {timeLeft}";
        }

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}