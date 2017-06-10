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

namespace Game2048
{
    /// <summary>
    /// Interaction logic for SubWindow.xaml
    /// </summary>
    public partial class SubWindow : Window
    {
        MainWindow mainWindow;
        public SubWindow(MainWindow window)
        {
            InitializeComponent();
            mainWindow = window;
            scoreTextBlock.Text = mainWindow.myScore.ToString();
            gameRestart.Click += GameRestart_Click;
            gameEnd.Click += GameEnd_Click;
        }

        private void GameEnd_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.Close();
            this.Close();
            
        }

        private void GameRestart_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.GameRestart();
            this.Close();
        }
    }
}
