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
            Snake snake = new Snake(mapCanvas);
            snakeFood = new SnakeSection(foodPoint.Next(0,(int)mapCanvas.Width),foodPoint.Next(0,(int)mapCanvas.Height));
            timer = new DispatcherTimer();

            foreach (var ss in snake.body)
            {
                ViewUpdate(ss);
            }

            timer.Interval = new TimeSpan(0,0,1);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            snake.SnakeUpdate();
            mapCanvas.Children.Clear();
            foreach(var ss in snake.body)
            {
                ViewUpdate(ss);
            }
            ViewUpdate(snakeFood);
        }

        private void ViewUpdate(SnakeSection ss)
        {
            ss.shape.SetValue(Canvas.LeftProperty, ss.point.X);
            ss.shape.SetValue(Canvas.TopProperty, ss.point.Y);
            ss.shape.SetValue(Canvas.ZIndexProperty, 1);

            mapCanvas.Children.Add(ss.shape);
        }
    }
    public class Snake
    {
        public SnakeSection header = new SnakeSection();
        public Vector orientation = new Vector();
        public List<SnakeSection> body=new List<SnakeSection>();
        public Point lastpoint=new Point();
        public Canvas map;

        public int length { get { return body.Count; } }
        public int width { get; private set; }
        public int speed { get; private set; }

        public Snake(Canvas c)
        {
            header.point.X = c.Width / 2;
            header.point.Y = c.Height / 2;
            orientation.X = 10;
            orientation.Y = 0;
            width = 10;
            speed = 10;
            body.Add(header);
            lastpoint.X= body[body.Count - 1].point.X;
            lastpoint.Y = body[body.Count - 1].point.Y;
            map = c;
        }
        public Snake(Canvas c, int w,int s)
        {
            header.point.X = c.Width / 2;
            header.point.Y = c.Height / 2;
            orientation.X = 10;
            orientation.Y = 0;
            width = w;
            speed = w;
            body.Add(header);
            lastpoint.X = body[body.Count - 1].point.X;
            lastpoint.Y = body[body.Count - 1].point.Y;
            map = c;
        }

        public void SnakeUpdate()
        {
            lastpoint.X = body[body.Count - 1].point.X;
            lastpoint.Y = body[body.Count - 1].point.Y;

            SnakeSection temp = body[body.Count - 1];
            temp.point.X = header.point.X;
            temp.point.Y = header.point.Y;

            body.RemoveAt(body.Count - 1);
            body.Insert(1, temp);

            header.point.X += orientation.X;
            header.point.Y += orientation.Y;

        }

        public void GetFood(SnakeSection f)
        {
            body.Insert(body.Count - 1, f);
            f.point.X = lastpoint.X;
            f.point.Y = lastpoint.Y;
        }

    }


    public class SnakeSection
    {
        public Point point=new Point();
        public Ellipse shape=new Ellipse();
        public SnakeSection()
        {
            point.X = 0;
            point.Y = 0;
            shape.Height = 10;
            shape.Width = 10;
            shape.Stroke = Brushes.Blue;
        }
        public SnakeSection(double x,double y)
        {
            point.X = x;
            point.Y = y;
            shape.Height = 10;
            shape.Width = 10;
            shape.Stroke = Brushes.Blue;
        }
        public SnakeSection(double x, double y,double w)
        {
            point.X = x;
            point.Y = y;
            shape.Height = w;
            shape.Width = w;
            shape.Stroke = Brushes.Blue;
        }
        public SnakeSection(double x, double y, double w,SolidColorBrush c)
        {
            point.X = x;
            point.Y = y;
            shape.Height = w;
            shape.Width = w;
            shape.Stroke = c;
        }
    }
}
