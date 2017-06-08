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
        int myScore = 0;

        public MainWindow()
        {
            InitializeComponent();
            NumberArrayInit();
            NumberZoneInit();
            NewRandomBlock();
            NewRandomBlock();
            numberZone.KeyDown += NumberZone_KeyDown;
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

        private void NumberArrayInit()
        {
            for(int i = 0; i < 4; i++)
            {
                for(int j = 0; j < 4; j++)
                {
                    numberArray[i, j].num = 0;
                    numberArray[i, j].button = null;
                }
            }
        }
        private void NumberZoneInit()
        {
            for(int i = 0; i < 4; i++)
            {
                for(int j = 0; j < 4; j++)
                {
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
        private void NewRandomBlock()
        {
            while (true) {
                int random = ran.Next(0, 15);
                if (numberArray[random / 4, random % 4].num == 0)
                {
                    numberArray[random / 4, random % 4].num = 2;
                    numberArray[random / 4, random % 4].button.Content = 2;
                    break;
                }
            }

        }
        private int CheckIfGameEnd()
        {
            return 0;
        }
        private void GameEndMess()
        {

        }
        private void ScoreUpdate()
        {
            int num = 0;
            for(int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (numberArray[i, j].num != 0)
                    {
                        num += numberArray[i, j].num * (int)(Math.Log(numberArray[i, j].num, 2));
                    }
                }
            }
            myScore = num;
            scoreText.Text = num.ToString();
        }

        private void UpEvent()
        {
            int flag = 0;
            for (int i = 0; i < 4; i++)
            {
                for(int j = 0; j < 4; j++)
                {
                    for (int m = j; m > 0; m--)
                    {
                        if (numberArray[m, i].num == numberArray[m - 1, i].num && numberArray[m - 1, i].num == 0)
                        {
                            continue;
                        }
                        if (numberArray[m, i].num == numberArray[m - 1, i].num || numberArray[m-1,i].num==0)
                        {
                            numberArray[m - 1, i].num += numberArray[m, i].num;
                            numberArray[m, i].num = 0;

                            if (numberArray[m - 1, i].num != 0)
                            {
                                numberArray[m - 1, i].button.Content = numberArray[m - 1, i].num;
                            }
                            else
                            {
                                numberArray[m - 1, i].button.Content = "";
                            }
                            if (numberArray[m, i].num != 0)
                            {
                                numberArray[m, i].button.Content = numberArray[m, i].num;
                            }
                            else
                            {
                                numberArray[m, i].button.Content = "";
                            }
                            flag = 1;
                        }
                    }
                }
            }
            ScoreUpdate();
            if (flag == 1) { NewRandomBlock(); }
        }
        private void LeftEvent()
        {
            int flag = 0;
            for(int i = 0; i < 4; i++)
            {
                for(int j = 0; j < 4; j++)
                {
                    for(int m = j; m > 0; m--)
                    {
                        if(numberArray[i, m].num == numberArray[i, m - 1].num && numberArray[i, m].num == 0)
                        {
                            continue;
                        }
                        if(numberArray[i,m].num==numberArray[i,m-1].num || numberArray[i, m - 1].num == 0)
                        {
                            numberArray[i, m - 1].num += numberArray[i, m].num;
                            numberArray[i, m].num = 0;
                            if (numberArray[i, m-1].num != 0)
                            {
                                numberArray[i, m-1].button.Content = numberArray[i, m-1].num;
                            }
                            else
                            {
                                numberArray[i, m-1].button.Content = "";
                            }
                            if (numberArray[i, m].num != 0)
                            {
                                numberArray[i, m].button.Content = numberArray[i, m].num;
                            }
                            else
                            {
                                numberArray[i, m].button.Content = "";
                            }
                            flag = 1;
                        }
                    }
                }
            }
            ScoreUpdate();
            if (flag == 1) { NewRandomBlock(); }
        }
        private void RightEvent()
        {
            int flag = 0;
            for (int i = 0; i < 4; i++) 
            {
                for(int j = 3; j >= 0; j--)
                {
                    for(int m = j; m < 3; m++)
                    {
                        if (numberArray[i, m].num == numberArray[i, m + 1].num && numberArray[i, m + 1].num == 0)
                        {
                            continue;
                        }
                        if (numberArray[i,m].num==numberArray[i,m+1].num || numberArray[i, m + 1].num == 0)
                        {
                            numberArray[i, m + 1].num += numberArray[i, m].num;
                            numberArray[i, m].num = 0;
                            if (numberArray[i, m+1].num != 0)
                            {
                                numberArray[i, m+1].button.Content = numberArray[i, m+1].num;
                            }
                            else
                            {
                                numberArray[i, m+1].button.Content = "";
                            }
                            if (numberArray[i, m].num != 0)
                            {
                                numberArray[i, m].button.Content = numberArray[i, m].num;
                            }
                            else
                            {
                                numberArray[i, m].button.Content = "";
                            }
                            flag = 1;
                        }
                    }
                }
            }
            ScoreUpdate();
            if (flag == 1) { NewRandomBlock(); }
        }
        private void DownEvent()
        {
            int flag = 0;
            for (int i = 0; i < 4; i++)
            {
                for (int j = 3; j >=0; j--)
                {
                    for (int m = j; m <3; m++)
                    {
                        if (numberArray[m, i].num == numberArray[m + 1, i].num && numberArray[m + 1, i].num == 0)
                        {
                            continue;
                        }
                        if (numberArray[m, i].num == numberArray[m + 1, i].num || numberArray[m + 1, i].num == 0)
                        {
                            numberArray[m + 1, i].num += numberArray[m, i].num;
                            numberArray[m, i].num = 0;
                            if (numberArray[m + 1, i].num != 0)
                            {
                                numberArray[m + 1, i].button.Content = numberArray[m + 1, i].num;
                            }
                            else
                            {
                                numberArray[m + 1, i].button.Content = "";
                            }
                            if (numberArray[m, i].num != 0)
                            {
                                numberArray[m, i].button.Content = numberArray[m, i].num;
                            }
                            else
                            {
                                numberArray[m, i].button.Content = "";
                            }
                            flag = 1;
                        }
                    }
                }
            }
            ScoreUpdate();
            if (flag == 1) { NewRandomBlock(); }
        }

        private struct NumberBlock
        {
            public int num;
            public Button button;
        }
    }
    


}
