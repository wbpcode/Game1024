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
using System.Windows.Threading;

namespace MineSweeper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        MineInfo[,] mineTable = new MineInfo[22, 17];
        enum MineLevel { low=20, medium=40, high=60 };
        MineLevel level = MineLevel.low;
        DispatcherTimer timer = new DispatcherTimer();

        public MainWindow()
        {
            InitializeComponent();
            MineTableInit();
            MineGridInit();
            TipsInfoInit();
            TimerInit();
        }

        private void MineTableInit()
        {
            int mineNum = (int)level;
            int[,] randomArray = new int[mineNum, 2];
            int arrayNum=0;

            Random r = new Random();
            while (mineNum > 0)
            {
                int randomRow = r.Next(1, 20);
                int randomColumn = r.Next(1, 15);
                bool inArray = false;
                for(int i = 0; i < arrayNum; i++)
                {
                    if ((randomRow == randomArray[i,0])&&(randomColumn==randomArray[i,1]))
                    {
                        inArray = true;
                        break;
                    }
                }
                if (inArray){continue; }
                else
                {
                    randomArray[arrayNum, 0] = randomRow;
                    randomArray[arrayNum, 1] = randomColumn;
                    mineNum--;
                    arrayNum++;
                }
            }

            for(int i = 0; i < 22; i++)
            {
                for(int j = 0; j < 17; j++)
                {
                    mineTable[i, j].isMine = 0;
                    mineTable[i, j].flag = 0;
                    mineTable[i, j].roundMineNum = 0;
                    mineTable[i, j].isclicked = 0;
                    mineTable[i, j].button = null;
                }
            }

            for(int i=0;i<(int)(level);i++)
            {
                mineTable[randomArray[i,0], randomArray[i,1]].isMine = 1;
            }

            for (int i = 1; i < 21; i++)
            {
                for (int j = 1; j < 16; j++)
                {
                    mineTable[i, j].roundMineNum = mineTable[i-1,j-1].isMine+mineTable[i-1,j].isMine+mineTable[i-1,j+1].isMine+
                        mineTable[i+1,j-1].isMine+mineTable[i+1,j].isMine+mineTable[i+1,j+1].isMine+mineTable[i,j-1].isMine+mineTable[i,j+1].isMine;
                    mineTable[i, j].flag = 0;
                }
            }
        }

        private void MineTableRefresh()
        {
            MineTableInit();
        }

        private void MineGridInit()
        {
            for(int i = 0; i < 20; i++)
            {
                RowDefinition row = new RowDefinition();
                row.Height = new GridLength(20);
                mineGrid.RowDefinitions.Add(row);
            }
            for(int i = 0; i < 15; i++)
            {
                ColumnDefinition column = new ColumnDefinition();
                column.Width = new GridLength(20);
                mineGrid.ColumnDefinitions.Add(column);
            }
            for(int i = 0; i < 20; i++)
            {
                for(int j = 0; j < 15; j++)
                {
                    Button mineButton = new Button();
                    mineButton.Background = Brushes.Gray;
                    mineGrid.Children.Add(mineButton);
                    Grid.SetRow(mineButton, i);
                    Grid.SetColumn(mineButton, j);
                    mineButton.Click += mineButton_Click;
                    mineButton.MouseRightButtonDown += mineButton_MouseRightButtonDown;
                    mineTable[i + 1, j + 1].button = mineButton;
                }
            }
        }

        private void MineGridRefresh()
        {
            foreach(Button mineButton in mineGrid.Children)
            {
                mineButton.Content = "";
                mineButton.Background = Brushes.Gray;
                int row = Grid.GetRow(mineButton);
                int column = Grid.GetColumn(mineButton);
                mineTable[row + 1, column + 1].button = mineButton;
                mineButton.Click -= mineButton_Click;
                mineButton.MouseRightButtonDown -= mineButton_MouseRightButtonDown;
                mineButton.Click += mineButton_Click;
                mineButton.MouseRightButtonDown += mineButton_MouseRightButtonDown;

            }
        }

        private void TipsInfoInit()
        {
            mineNumTextBox.Text = ((int)level).ToString();
            flagNumTextBox.Text = "0";
            mineTimeContentButton.Text = "0";
            endTextBox.Text = "加油，Fight！！！";
        }

        private void TipsInfoRefresh()
        {
            TipsInfoInit();
        }

        private void TimerInit()
        {
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Tick += timer_Tick;
            timer.Start();
        }

        private void TimerRefresh()
        {
            mineTimeContentButton.Text = "0";
        }

        private void mineButton_Click(object sender, RoutedEventArgs e)
        {
            endTextBox.Text = "加油，Fight！！！";
            Button mineButton=(Button)sender;
            int gridRow = Grid.GetRow(mineButton);
            int gridColumn = Grid.GetColumn(mineButton);
            if (mineTable[gridRow + 1, gridColumn + 1].isMine == 1)
            {
                mineButton.Background = Brushes.Red;
                foreach(Button i in mineGrid.Children)
                {
                    i.Click -= mineButton_Click;
                    i.MouseRightButtonDown -= mineButton_MouseRightButtonDown;
                }
                endTextBox.Text = "即使小心翼翼，偶尔也会出问题呢！";
            }
            else
            {
                if (mineTable[gridRow + 1, gridColumn + 1].roundMineNum == 0)
                {
                    endTextBox.Text = "踩到白块(￣y▽￣)╭ Ohohoho.....";
                    clickBlankClear(gridRow + 1, gridColumn + 1);
                }
                else
                {
                    mineButton.Background = Brushes.LightBlue;
                    mineButton.Content = (mineTable[gridRow + 1, gridColumn + 1].roundMineNum).ToString();
                    mineTable[gridRow + 1, gridColumn + 1].isclicked = 1;
                    mineButton.Click -= mineButton_Click;
                    mineButton.MouseRightButtonDown -= mineButton_MouseRightButtonDown;
                }
            }
        }

        private void mineButton_MouseRightButtonDown(object sender,RoutedEventArgs e)
        {
            Button mineButton = (Button)sender;
            int row = Grid.GetRow(mineButton);
            int column = Grid.GetColumn(mineButton);
            mineTable[row + 1, column + 1].flag = (mineTable[row + 1, column + 1].flag + 1) % 2;
            if (mineTable[row + 1, column + 1].flag == 1)
            {
                endTextBox.Text = "我已经看穿真相，看我眼神(⓿_⓿)";
                mineButton.Background = Brushes.Yellow;
                mineButton.Click -= mineButton_Click;
                flagNumTextBox.Text = (int.Parse(flagNumTextBox.Text) + 1).ToString();
            }
            else
            {
                endTextBox.Text = "刚刚好像猜错了一个，咳咳~~~";
                mineButton.Background = Brushes.Gray;
                mineButton.Click += mineButton_Click;
                flagNumTextBox.Text = (int.Parse(flagNumTextBox.Text) - 1).ToString();
            }
        }

        private void gameRestartItem_Click(object sender, RoutedEventArgs e)
        {
            gameRefresh();
            endTextBox.Text = "再战___*( ￣皿￣)/#____";
        }

        private void gameSubmitItem_Click(object sender, RoutedEventArgs e)
        {
            int end = 0;
            for(int i = 1; i < 21; i++)
            {
                for(int j = 1; j < 16; j++)
                {
                    if (mineTable[i, j].isMine != mineTable[i, j].flag)
                    {
                        end=1;
                        break;
                    }
 
                }
                if (end == 1) { break; }
            }
            if (end == 0)
            {
                foreach (Button i in mineGrid.Children)
                {
                    i.Click -= mineButton_Click;
                    i.MouseRightButtonDown -= mineButton_MouseRightButtonDown;
                }
                endTextBox.Text = "搞定，哎，无敌，多么寂寞！";
            }
            else
            {
                foreach (Button i in mineGrid.Children)
                {
                    i.Click -= mineButton_Click;
                    i.MouseRightButtonDown -= mineButton_MouseRightButtonDown;
                }
                endTextBox.Text = "额，失算，就当不知道吧！";
            }
        }

        private void lowLevel_Click(object sender, RoutedEventArgs e)
        {
            level = MineLevel.low;
            lowLevel.Header = "简单*";
            mediumLevel.Header = "中等";
            highLevel.Header = "困难";
            gameRefresh();
            endTextBox.Text = "做人最重要的就是开心啦！";
        }

        private void mediumLevel_Click(object sender, RoutedEventArgs e)
        {
            level = MineLevel.medium;
            lowLevel.Header = "简单";
            mediumLevel.Header = "中等*";
            highLevel.Header = "困难";
            gameRefresh();
            endTextBox.Text = "我，也是会变强的o((>ω< ))o";
        }

        private void highLevel_Click(object sender, RoutedEventArgs e)
        {
            level = MineLevel.high;
            lowLevel.Header = "简单";
            mediumLevel.Header = "中等";
            highLevel.Header = "困难*";
            gameRefresh();
            endTextBox.Text = "看我搞点事情(ง •_•)ง";
        }

        private void gameHelpItem_Click(object sender, RoutedEventArgs e)
        {
            endTextBox.Text = "扫雷还要什么帮助(╯‵□′)╯︵┻━┻";
        }

        private void timer_Tick(object sender,EventArgs e)
        {
            mineTimeContentButton.Text = (int.Parse(mineTimeContentButton.Text) + 1).ToString();
        }

        private void clickBlankClear(int tableRow,int tableColumn)
        {
            Button mineButton = mineTable[tableRow, tableColumn].button;
            mineButton.Background = Brushes.LightBlue;
            mineTable[tableRow, tableColumn].isclicked = 1;
            mineButton.Click -= mineButton_Click;
            mineButton.MouseRightButtonDown -= mineButton_MouseRightButtonDown;

            for (int i = tableRow - 1; i <= tableRow + 1; i++)
            {
                for(int j = tableColumn - 1; j <= tableColumn + 1; j++)
                {
                    if (i != tableRow || j != tableColumn)
                    {
                        if (mineTable[i, j].roundMineNum == 0)
                        {
                            if (mineTable[i, j].button != null && mineTable[i, j].isclicked == 0)
                            {
                                clickBlankClear(i, j);
                            }
                        }
                        else
                        {
                            mineTable[i, j].button.Background = Brushes.LightBlue;
                            mineTable[i,j].button.Content= (mineTable[i, j].roundMineNum).ToString();
                            mineTable[i, j].isclicked = 1;
                            mineTable[i, j].button.Click -= mineButton_Click;
                            mineTable[i, j].button.MouseRightButtonDown -= mineButton_MouseRightButtonDown;
                        }
                    }

                }
            }
        }

        private void gameRefresh()
        {
            MineTableRefresh();
            MineGridRefresh();
            TipsInfoRefresh();
            TimerRefresh();
        }
    }

    public struct MineInfo
    {
        public int isMine;
        public int roundMineNum;
        public int flag;
        public int isclicked;
        public Button button;
    }
}
