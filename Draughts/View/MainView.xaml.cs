using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using Draughts.Library;
using Draughts.Library.Exceptions;
using Draughts.ViewModel;

namespace Draughts.View
{

    public partial class MainView : Window
    {

        private DraughtsGame game;
        private MainViewModel mainViewModel;

        private Rectangle[,] cells;
        private Ellipse[,] draughts;

        private Brush blackBrush;
        private Brush whiteBrush;
        private Brush playerOneBrush;
        private Brush playerTwoBrush;
        private Brush playerOneQueenBrush;
        private Brush playerTwoQueenBrush;

        private Ellipse ellipse;
        private bool move;
        private Point startPoint;
        private int oldX;
        private int oldY;

        public MainView(MainViewModel mainViewModel)
        {
            InitializeComponent();

            this.mainViewModel = mainViewModel;
            game = mainViewModel.Game;
            mainViewModel.PropertyChanged += PropertyChanged;

            InitGradients();
            ReDraw();
        }

        private void PropertyChanged(object o, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "BoardChanged")
            {
                ReDraw();
            }
            else if (args.PropertyName == "Load")
            {
                game = ((MainViewModel)o).Game;
                ReDraw();
            }
        }

        private void InitGradients()
        {
            //blackBrush = new SolidColorBrush(Color.FromRgb(126, 92, 54));
            //whiteBrush = new SolidColorBrush(Color.FromRgb(236, 206, 168));
            blackBrush = Brushes.Black;
            whiteBrush = Brushes.White;

            GradientStopCollection collection = new GradientStopCollection
                                                    {
                                                        new GradientStop(Colors.Blue, 1),
                                                        new GradientStop(Colors.DarkBlue, 0.6),
                                                        new GradientStop(Colors.AliceBlue, 0)
                                                    };
            playerOneBrush = new RadialGradientBrush(collection);

            collection = new GradientStopCollection
                             {
                                 new GradientStop(Colors.Red, 1),
                                 new GradientStop(Colors.DarkRed, 0.6),
                                 new GradientStop(Colors.OrangeRed, 0)
                             };
            playerTwoBrush = new RadialGradientBrush(collection);

            collection = new GradientStopCollection
                             {
                                 new GradientStop(Colors.Gold, 1),
                                 new GradientStop(Colors.Blue, 0.6),
                                 new GradientStop(Colors.DarkBlue, 0.5),
                                 new GradientStop(Colors.AliceBlue, 0)
                             };
            playerOneQueenBrush = new RadialGradientBrush(collection);

            collection = new GradientStopCollection
                             {
                                 new GradientStop(Colors.Gold, 1),
                                 new GradientStop(Colors.Red, 0.6),
                                 new GradientStop(Colors.DarkRed, 0.5),
                                 new GradientStop(Colors.OrangeRed, 0)
                             };
            playerTwoQueenBrush = new RadialGradientBrush(collection);
        }

        private void ReDraw()
        {
            canvas.Children.Clear();
            DrawCells();
            DrawDraughts();
        }

        private void DrawCells()
        {
            cells = new Rectangle[8, 8];

            int y = 0;
            for (int i = 0; i < 8; i++)
            {
                int x = 0;
                for (int j = 0; j < 8; j++)
                {
                    cells[i, j] = new Rectangle
                                      {
                                          Width = 50,
                                          Height = 50,
                                          Fill = (i + j) % 2 != 0 ? blackBrush : whiteBrush
                                      };
                    cells[i, j].MouseMove += DraughtsMouseMove;


                    Canvas.SetLeft(cells[i, j], x);
                    Canvas.SetTop(cells[i, j], y);
                    canvas.Children.Add(cells[i, j]);

                    x += 50;
                }

                y += 50;
            }
        }

        private void DrawDraughts()
        {
            draughts = new Ellipse[8, 8];
            int z = 1000;

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (game.GameBoard[i, j] != null)
                    {
                        draughts[i, j] = new Ellipse()
                        {
                            Width = 40,
                            Height = 40
                        };
                        draughts[i, j].MouseLeftButtonDown += DraughtsMouseDown;
                        draughts[i, j].MouseMove += DraughtsMouseMove;
                        draughts[i, j].MouseUp += DraughtsMouseUp;

                        if (game.GameBoard[i, j].Player == Players.PlayerOne)
                        {
                            draughts[i, j].Fill = game.GameBoard[i, j].Type == DraughtType.None ? playerOneBrush : playerOneQueenBrush;
                        }
                        else
                        {
                            draughts[i, j].Fill = game.GameBoard[i, j].Type == DraughtType.None ? playerTwoBrush : playerTwoQueenBrush;
                        }

                        Canvas.SetLeft(draughts[i, j], ((game.GameBoard[i, j].X) * 50) + 5);
                        Canvas.SetTop(draughts[i, j], ((game.GameBoard[i, j].Y) * 50) + 5);
                        Canvas.SetZIndex(draughts[i, j], z);
                        canvas.Children.Add(draughts[i, j]);

                        z++;
                    }
                }
            }
        }

        private void DraughtsMouseDown(object o, MouseButtonEventArgs args)
        {
            ellipse = (Ellipse)o;
            oldX = ((int)Canvas.GetLeft(ellipse) - 5) / 50;
            oldY = ((int)Canvas.GetTop(ellipse) - 5) / 50;

            startPoint = args.GetPosition(null);

            double r = 15;
            double Cx = Canvas.GetLeft(ellipse) + 20;
            double Cy = Canvas.GetTop(ellipse) + 20;

            if (game.GameBoard[oldY, oldX].Player == game.CurrentTurn && Math.Pow(startPoint.X - Cx, 2) + Math.Pow(startPoint.Y - Cy, 2) < Math.Pow(r, 2))
            {
                move = true;
                Canvas.SetZIndex(ellipse, 10000);
            }
        }

        private void DraughtsMouseMove(object o, MouseEventArgs args)
        {
            if (move && args.LeftButton == MouseButtonState.Pressed)
            {
                Point currentPoint = args.GetPosition(null);
                Canvas.SetLeft(ellipse, Canvas.GetLeft(ellipse) + currentPoint.X - startPoint.X);
                Canvas.SetTop(ellipse, Canvas.GetTop(ellipse) + currentPoint.Y - startPoint.Y);

                startPoint = currentPoint;
            }
        }

        private void DraughtsMouseUp(object o, MouseButtonEventArgs args)
        {
            if (move && args.LeftButton == MouseButtonState.Released)
            {
                Point currentPoint = args.GetPosition(null);
                int currentX = (int)(currentPoint.X / 50);
                int currentY = (int)(currentPoint.Y / 50);

                mainViewModel.Turn(oldX, oldY, currentX, currentY);
                move = false;
            }
        }

    }

}
