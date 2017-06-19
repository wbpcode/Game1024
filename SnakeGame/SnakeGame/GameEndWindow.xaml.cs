using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SnakeGame
{
    /// <summary>
    /// Interaction logic for GameEndWindow.xaml
    /// </summary>
    public partial class GameEndWindow : Window
    {
        MainWindow main;
        public GameEndWindow(MainWindow w)
        {
            InitializeComponent();

            main = w;
            scoreTextBlock.Text = main.snake.length.ToString();
            replayButton.Click += GameReplay;
            endButton.Click += GameEnd;
        }

        private void GameEnd(object sender, RoutedEventArgs e)
        {
            this.Close();
            main.Close();
        }

        private void GameReplay(object sender, RoutedEventArgs e)
        {
            main.timer.Stop();
            main.snake = new Snake(main.mapCanvas);
            this.Close();
            main.timer.Start();
        }
    }
}
