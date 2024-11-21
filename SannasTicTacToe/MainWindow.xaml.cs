using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace SannasTicTacToe
{
    public partial class MainWindow : Window
    {
        DispatcherTimer cpuTimer;
        Player currentPlayer;
        Random random = new Random();
        int playerWinCount = 0;
        int cpuWinCount = 0;
        List<Button> buttons = new List<Button>();
      
        public MainWindow()
        {
            InitializeComponent();
            cpuTimer = new DispatcherTimer();
            cpuTimer.Interval = TimeSpan.FromSeconds(1);
            cpuTimer.Tick += cpuMoveTimer_Tick;
            StartNewGame();
        }
        public enum Player
        {
            X, O
        }
        private void StartNewGame()
        {
            buttons = new List<Button> { btn1, btn2, btn3, btn4, btn5, btn6, btn7, btn8, btn9 };

            foreach (Button button in buttons)
            {
                button.IsEnabled = true;
                button.IsHitTestVisible = true;
                button.Content = "";
                button.Background = new SolidColorBrush(Colors.Pink);
            }
            currentPlayer = Player.X;
        }
        private void StartNewGame_Click(object sender, RoutedEventArgs e)
        {
            StartNewGame();
        }
        private void PlayerMakesMove(Button button)
        {
            currentPlayer = Player.X;
            button.Content = currentPlayer.ToString();
            //button.Background = Brushes.LightBlue;
            button.IsEnabled = false;
            buttons.Remove(button);
            CheckGame();
            ButtonsAreEnabled(false);
            cpuTimer.Start();
        }
        private void cpuMoveTimer_Tick(object? sender, EventArgs e)
        {
            cpuTimer?.Stop();

            if (buttons.Count > 0)
            {
                int index = random.Next(buttons.Count);
                Button button = buttons[index];
                buttons.RemoveAt(index);
                button.Content = Player.O.ToString();
                //button.Background = Brushes.LightBlue;
                button.IsEnabled = false;
                currentPlayer = Player.X;
                CheckGame();
            }
            ButtonsAreEnabled(true);
        }
        private void CheckGame()
        {
            var winningCombinations = new List<List<Button>>
            {
                new List<Button> { btn1, btn2, btn3 },
                new List<Button> { btn4, btn5, btn6 },
                new List<Button> { btn7, btn8, btn9 },
                new List<Button> { btn1, btn4, btn7 },
                new List<Button> { btn2, btn5, btn8 },
                new List<Button> { btn3, btn6, btn9 },
                new List<Button> { btn1, btn5, btn9 },
                new List<Button> { btn3, btn5, btn7 }
            };

            foreach (var combination in winningCombinations)
            {
                if (combination[0].Content.ToString() != "" &&
                    combination[0].Content.ToString() == combination[1].Content.ToString() &&
                    combination[0].Content.ToString() == combination[2].Content.ToString())
                {
                    DeclareWinner(combination[0].Content.ToString());
                }
            }
            if (buttons.All(b => b.Content.ToString() != ""))
            {
                DeclareDraw();
            }
        }
        private void DeclareWinner(string winner)
        {
            if (winner == "X")
            {
                playerWinCount++;
                txtPlayerWins.Text = $"Player Wins: {playerWinCount}";
                MessageBox.Show("Congratulations, you won!");
            }
            else if (winner == "O")
            {
                cpuWinCount++;
                txtCPUWins.Text = $"CPU Wins: {cpuWinCount}";
                MessageBox.Show("CPU won!");
            }
            StartNewGame();
        }
        private void DeclareDraw()
        {
            MessageBox.Show("It's a tie!");
            StartNewGame();
        }
        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;

            if (button == null || button == btnNewGame || !button.IsEnabled)
            {
                return;
            }
            if (currentPlayer == Player.X)
            {                
                PlayerMakesMove(button);
            }
        }
        private void ButtonsAreEnabled(bool isEnabled)
        {
            foreach (Button button in buttons)
            {
                if (string.IsNullOrEmpty(button.Content?.ToString()))
                {
                    button.IsHitTestVisible = isEnabled;
                }
            }
        }
    }
}