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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Game2048
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        NumberBlock[,] numberArray = new NumberBlock[4, 4];
        Random ran = new Random();
        public int myScore = 0;

        public MainWindow()
        {
            InitializeComponent();
            GameInit();
            NewRandomBlock();
            NewRandomBlock();
            numberZone.KeyDown += NumberZone_KeyDown;
            gameRestart.Click += GameRestart_Click;
            gameEnd.Click += GameEnd_Click;

        }

        private void GameEnd_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void GameRestart_Click(object sender, RoutedEventArgs e)
        {
            GameRestart();
        }
        private void NumberZone_KeyDown(object sender, KeyEventArgs e)
        {
            Key inputKey = e.Key;
            switch (inputKey)
            {
                case Key.S:
                    DownEvent();
                    break;
                case Key.W:
                    UpEvent();
                    break;
                case Key.A:
                    LeftEvent();
                    break;
                case Key.D:
                    RightEvent();
                    break;
            }
        }

        private void GameInit()
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    numberArray[i, j].num = 0;

                    Button newButton = new Button();
                    newButton.BorderBrush = null;
                    newButton.Background = Brushes.LightCyan;
                    newButton.FontSize = 18;
                    numberZone.Children.Add(newButton);
                    Grid.SetRow(newButton, i);
                    Grid.SetColumn(newButton, j);
                    numberArray[i, j].button = newButton;
                }
            }
        }
        public void GameRestart()
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    numberArray[i, j].num = 0;
                    ViewUpdate(numberArray[i, j]);
                }
            }
            NewRandomBlock();
            NewRandomBlock();

            ScoreUpdate();
        }

        private void NewRandomBlock()
        {
            List<int> blankList = new List<int>();
            blankList.Clear();
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (numberArray[i, j].num == 0)
                    {
                        blankList.Add(i * 4 + j);
                    }
                }
            }
            int random = ran.Next(0, blankList.Count);
            numberArray[blankList[random] / 4, blankList[random] % 4].num = 2;
            numberArray[blankList[random] / 4, blankList[random] % 4].button.Content = 2;
        }

        private void GameEndCheck()
        {
            int flag = 0;
            for(int i = 0; i < 4; i++)
            {
                for(int j = 0; j < 4; j++)
                {
                    if (numberArray[i, j].num == 0)
                    {
                        flag = 1;
                        break;
                    }
                }
                if (flag == 1) { break; }
            }
            for(int i = 0; i < 4; i++)
            {
                for(int j = 0; j < 3; j++)
                {
                    if (numberArray[i, j].num == numberArray[i, j + 1].num)
                    {
                        flag = 1;
                        break;
                    }
                }
                if (flag == 1) { break; }
            }
            for(int i = 0; i < 4; i++)
            {
                for(int j = 0; j < 3; j++)
                {
                    if (numberArray[j, i].num == numberArray[j + 1, i].num)
                    {
                        flag = 1;
                        break;
                    }
                }
                if (flag == 1) { break; }

            }
            if (flag == 0) { GameEndTip(); }

        }
        private void GameEndTip()
        {
            SubWindow gameEndTip = new SubWindow(this);
            gameEndTip.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            gameEndTip.Owner = this;
            gameEndTip.ShowDialog();
        }

        private void ScoreUpdate()
        {
            int num = 0;
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (numberArray[i, j].num != 0)
                    {
                        num += numberArray[i, j].num * (int)(Math.Log(numberArray[i, j].num / 2, 2));
                    }
                }
            }
            myScore = num;
            scoreTextBlock.Text = num.ToString();
        }
        private void ViewUpdate(NumberBlock block)
        {
            if (block.num != 0)
            {
                block.button.Content = block.num;
            }
            else
            {
                block.button.Content = "";
            }
        }

        private void UpEvent()
        {
            int flag = 0;
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    for (int m = j; m > 0; m--)
                    {
                        if (numberArray[m, i].num == numberArray[m - 1, i].num && numberArray[m - 1, i].num == 0)
                        {
                            continue;
                        }
                        if (numberArray[m, i].num == numberArray[m - 1, i].num || numberArray[m - 1, i].num == 0)
                        {
                            numberArray[m - 1, i].num += numberArray[m, i].num;
                            numberArray[m, i].num = 0;

                            ViewUpdate(numberArray[m - 1, i]);
                            ViewUpdate(numberArray[m, i]);

                            flag = 1;
                        }
                    }
                }
            }
            ScoreUpdate();
            if (flag == 1) { NewRandomBlock(); }
            GameEndCheck();
        }
        private void LeftEvent()
        {
            int flag = 0;
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    for (int m = j; m > 0; m--)
                    {
                        if (numberArray[i, m].num == numberArray[i, m - 1].num && numberArray[i, m].num == 0)
                        {
                            continue;
                        }
                        if (numberArray[i, m].num == numberArray[i, m - 1].num || numberArray[i, m - 1].num == 0)
                        {
                            numberArray[i, m - 1].num += numberArray[i, m].num;
                            numberArray[i, m].num = 0;

                            ViewUpdate(numberArray[i, m - 1]);
                            ViewUpdate(numberArray[i, m]);

                            flag = 1;
                        }
                    }
                }
            }
            ScoreUpdate();
            if (flag == 1) { NewRandomBlock(); }
            GameEndCheck();
        }
        private void RightEvent()
        {
            int flag = 0;
            for (int i = 0; i < 4; i++)
            {
                for (int j = 3; j >= 0; j--)
                {
                    for (int m = j; m < 3; m++)
                    {
                        if (numberArray[i, m].num == numberArray[i, m + 1].num && numberArray[i, m + 1].num == 0)
                        {
                            continue;
                        }
                        if (numberArray[i, m].num == numberArray[i, m + 1].num || numberArray[i, m + 1].num == 0)
                        {
                            numberArray[i, m + 1].num += numberArray[i, m].num;
                            numberArray[i, m].num = 0;

                            ViewUpdate(numberArray[i, m + 1]);
                            ViewUpdate(numberArray[i, m]);

                            flag = 1;
                        }
                    }
                }
            }
            ScoreUpdate();
            if (flag == 1) { NewRandomBlock(); }
            GameEndCheck();
        }
        private void DownEvent()
        {
            int flag = 0;
            for (int i = 0; i < 4; i++)
            {
                for (int j = 3; j >= 0; j--)
                {
                    for (int m = j; m < 3; m++)
                    {
                        if (numberArray[m, i].num == numberArray[m + 1, i].num && numberArray[m + 1, i].num == 0)
                        {
                            continue;
                        }
                        if (numberArray[m, i].num == numberArray[m + 1, i].num || numberArray[m + 1, i].num == 0)
                        {
                            numberArray[m + 1, i].num += numberArray[m, i].num;
                            numberArray[m, i].num = 0;

                            ViewUpdate(numberArray[m + 1, i]);
                            ViewUpdate(numberArray[m, i]);

                            flag = 1;
                        }
                    }
                }
            }
            ScoreUpdate();
            if (flag == 1) { NewRandomBlock(); }
            GameEndCheck();
        }



        private struct NumberBlock
        {
            public int num;
            public Button button;
        }
    }

}
