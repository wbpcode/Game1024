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

namespace SnakeGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public SnakeSection snakeFood;
        public Snake snake;
        DispatcherTimer timer;
        Random foodPoint;

        public MainWindow()
        {
            InitializeComponent();

            foodPoint = new Random();
            snake = new Snake(mapCanvas);
            snakeFood = new SnakeSection(foodPoint.Next(10,(int)mapCanvas.Width-10),foodPoint.Next(10,(int)mapCanvas.Height-10));
            timer = new DispatcherTimer();

            foreach (var ss in snake.body)
            {
                ViewUpdate(ss);
            }
            ViewUpdate(snakeFood);

            mapCanvas.MouseDown += MapCanvas_MouseDown;

            timer.Interval = new TimeSpan(0,0,0,0,25);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void MapCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Point targetPosition=new Point();
            targetPosition = e.GetPosition(this);
            double temp= snake.speed / Math.Sqrt(
                Math.Pow((targetPosition.X - snake.header.point.X), 2) +
                Math.Pow((targetPosition.Y - snake.header.point.Y), 2));
            snake.orientation.X = temp * (targetPosition.X - snake.header.point.X);
            snake.orientation.Y = temp * (targetPosition.Y - snake.header.point.Y);
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            snake.SnakeUpdate();
            if (GetFood())
            {
                snake.GetFood(snakeFood);
                snakeFood = new SnakeSection(foodPoint.Next(10, (int)mapCanvas.Width - 10), foodPoint.Next(10, (int)mapCanvas.Height - 10));
                ViewUpdate(snakeFood);
            }

            for (int i=0;i<snake.body.Count;i++)
            {
                SnakeSection ss = snake.body[i];
                mapCanvas.Children.Remove(ss.shape);
                ViewUpdate(ss);
            }
        }

        private bool GetFood()
        {
            if (Math.Sqrt(Math.Pow((snake.header.point.X - snakeFood.point.X), 2) + Math.Pow((snake.header.point.Y - snakeFood.point.Y), 2))<=10){
                return true;
            }
            else { return false; }
        }

        private void ViewUpdate(SnakeSection ss)
        {
            ss.shape.SetValue(Canvas.LeftProperty, ss.point.X);
            ss.shape.SetValue(Canvas.TopProperty, ss.point.Y);
            mapCanvas.Children.Add(ss.shape);
        }
    }
    public class Snake
    {
        public SnakeSection header = new SnakeSection();
        public Point orientation = new Point();
        public List<SnakeSection> body=new List<SnakeSection>();
        public Point lastpoint=new Point();
        public Canvas map;
        int stepNum = 0;

        public int length { get { return body.Count; } }
        public double speed { get; set; }

        public Snake(Canvas c)
        {
            header.point.X = c.Width / 2;
            header.point.Y = c.Height / 2;
            speed = 10;
            orientation.X = speed;
            orientation.Y = 0;

            body.Add(header);
            lastpoint.X= body[body.Count - 1].point.X;
            lastpoint.Y = body[body.Count - 1].point.Y;
            map = c;
        }
        public Snake(Canvas c,int s)
        {
            header.point.X = c.Width / 2;
            header.point.Y = c.Height / 2;
            speed = s;
            orientation.X = speed;
            orientation.Y = 0;
            body.Add(header);
            header.shape.Fill = Brushes.Blue;
            lastpoint.X = body[body.Count - 1].point.X;
            lastpoint.Y = body[body.Count - 1].point.Y;
            map = c;
        }

        public void SnakeUpdate()
        {
            SnakeSection temp = body[body.Count - 1];
            lastpoint.X = temp.point.X;
            lastpoint.Y = temp.point.Y;

            stepNum++;

            temp.point.X = header.point.X+orientation.X;
            temp.point.Y = header.point.Y+orientation.Y;

            body.RemoveAt(body.Count - 1);

            body.Insert(0, temp);

            header.shape.Fill = Brushes.Azure;

            header = temp;

            header.shape.Fill = Brushes.Blue;


        }

        public void GetFood(SnakeSection f)
        {
            body.Add(f);
            f.point.X = lastpoint.X;
            f.point.Y = lastpoint.Y;
        }

    }


    public class SnakeSection
    {
        public Point point;
        public Ellipse shape;
        public SnakeSection()
        {
            point = new Point();
            shape = new Ellipse();
            point.X = 0;
            point.Y = 0;
            shape.Height = 10;
            shape.Width = 10;
            shape.Fill = Brushes.Azure;
        }
        public SnakeSection(double x,double y)
        {
            point = new Point();
            shape = new Ellipse();
            point.X = x;
            point.Y = y;
            shape.Height = 10;
            shape.Width = 10;
            shape.Fill = Brushes.Azure;
        }
        public SnakeSection(double x, double y,double w)
        {
            point = new Point();
            shape = new Ellipse();
            point.X = x;
            point.Y = y;
            shape.Height = w;
            shape.Width = w;
            shape.Fill = Brushes.Azure;
        }
        public SnakeSection(double x, double y, double w,SolidColorBrush c)
        {
            point = new Point();
            shape = new Ellipse();
            point.X = x;
            point.Y = y;
            shape.Height = w;
            shape.Width = w;
            shape.Stroke = c;
        }
    }
}
