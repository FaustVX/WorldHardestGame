using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Shapes = System.Windows.Shapes;
using System.Windows.Threading;
using WorldHardestGame.Core;
using System.Diagnostics;
using System.Threading.Tasks;
using WorldHardestGame.Core.Entities;
using System.IO;

namespace WorldHardestGame.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private double _blockSize;
        private readonly Stopwatch _timer;
        private TimeSpan _deltaTime, _lastTime;
        private readonly FileInfo[] _levelFiles;
        private int _currentLevel;
        private readonly float _playerSpeed = .2f;

        public MainWindow()
        {
            InitializeComponent();
            
            _levelFiles = new DirectoryInfo(Environment.GetCommandLineArgs()[1]).GetFiles("*.whg.map");

            _timer = new Stopwatch();
            
            ResetMap();

            var timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1 / 60d)
            };
            timer.Tick += Timer_Tick;
            timer.Start();

            CanvasBackground.SizeChanged += CanvasBackground_SizeChanged;

#if !DEBUG
            TxtInfo.Visibility = Visibility.Collapsed;
#endif
        }

        public Map Map { get; set; }

        public float FPS => 1 / (float)_deltaTime.TotalSeconds;

        public Player Player { get; private set; }

        private void ResetMap()
        {
            lock (this)
                using (var fileStream = _levelFiles[_currentLevel].OpenText())
                {
                    Map = Map.Parse(fileStream);
                    Title = Map.Name;

                    Player = Map.Entities.OfType<Player>().First();
                    _timer.Restart();
                    _deltaTime = _lastTime = TimeSpan.Zero;
                    CanvasBackground.Children.Clear();
                    CanvasBackground.Children.Capacity = Map.Size.Width * Map.Size.Height;
                }
        }

        private void NextLevel()
        {
            ++_currentLevel;
            _currentLevel %= _levelFiles.Length;
            _currentLevel += _levelFiles.Length;
            _currentLevel %= _levelFiles.Length;

            if (!_levelFiles[_currentLevel].Exists)
                Environment.Exit(0);
            else
                ResetMap();
        }

        private void CanvasBackground_SizeChanged(object sender, SizeChangedEventArgs e)
            => DrawBackground();

        private void Timer_Tick(object sender, EventArgs e)
        {
            lock (this)
            {
                UpdateFPS();
                UpdateForeground();
                DrawForeground();
            }
        }

        private void DrawBackground()
        {
            _blockSize = Math.Min(CanvasBackground.ActualWidth / Map.Size.Width, CanvasBackground.ActualHeight / Map.Size.Height);
            CanvasBackground.Children.Clear();
            DrawForeground();

            for (var x = 0; x < Map.Size.Width; x++)
            {
                for (var y = 0; y < Map.Size.Height; y++)
                {
                    var rect = new Shapes.Rectangle()
                    {
                        Width = _blockSize,
                        Height = _blockSize,
                        Fill = new SolidColorBrush(Map.Blocks[x, y] switch
                        {
                            Core.Blocks.Floor _ => ((x + y) % 2 == 0) ? Colors.DimGray : Colors.Gray,
                            Core.Blocks.Start _ => Colors.PaleGreen,
                            Core.Blocks.Finish _ => Colors.PaleTurquoise,
                            _ => Colors.Wheat,
                        })
                    };

                    CanvasBackground.Children.Add(rect);
                    Canvas.SetLeft(rect, x * _blockSize);
                    Canvas.SetTop(rect, y * _blockSize);
                }
            }
        }

        private void UpdateFPS()
        {
            _deltaTime = -(_lastTime - (_lastTime = _timer.Elapsed));
        }

        private void UpdateForeground()
        {
            if (Keyboard.IsKeyDown(Key.Up))
                Player.Position += new Position(0, -_playerSpeed);

            if (Keyboard.IsKeyDown(Key.Down))
                Player.Position += new Position(0,_playerSpeed);

            if (Keyboard.IsKeyDown(Key.Left))
                Player.Position += new Position(-_playerSpeed, 0);

            if (Keyboard.IsKeyDown(Key.Right))
                Player.Position += new Position(_playerSpeed, 0);

            if (Keyboard.IsKeyDown(Key.F3))
                TxtInfo.Visibility = TxtInfo.Visibility is Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
            else if (Map.Finished)
                NextLevel();
            else if (Keyboard.IsKeyToggled(Key.R) || Player.HasBennKilledBy is { })
                ResetMap();

            foreach (var entity in Map.Entities)
                entity.Update(_deltaTime);
        }

        private void DrawForeground()
        {
            var nl = Environment.NewLine;
            if (TxtInfo.Visibility is Visibility.Visible)
                TxtInfo.Text = $@"BlockSize: {_blockSize:0.###}" + nl +
                    $@"FPS: {FPS:0.###}" + nl +
                    $@"Player Speed: {_playerSpeed:0.###}" + nl +
                    $@"Player Position: {Player.Position.X:0.###}  {Player.Position.Y:0.###}"+
                    Map.Entities.OfType<Ball>().Select(b => b.Position).Aggregate("", (t, pos) => t + nl + $@"Ball Position: {pos.X:0.###}  {pos.Y:0.###}");

            CanvasForeground.Children.Clear();

            foreach (var entity in Map.Entities)
            {
                var element = entity switch
                {
                    Player p => (FrameworkElement)new Shapes.Rectangle()
                    {
                        Width = p.BoundingBox.BottomRight.X - p.BoundingBox.TopLeft.X,
                        Height = p.BoundingBox.BottomRight.Y - p.BoundingBox.TopLeft.Y,
                        Fill = new SolidColorBrush(p.HasBennKilledBy is null ? Colors.Red : Colors.Transparent),
                    },
                    Ball b => new Shapes.Ellipse()
                    {
                        Width = b.BoundingBox.BottomRight.X - b.BoundingBox.TopLeft.X,
                        Height = b.BoundingBox.BottomRight.Y - b.BoundingBox.TopLeft.Y,
                        Fill = new SolidColorBrush(Colors.Blue)
                    },
                    _ => throw new Exception(),
                };

                element.Width *= _blockSize;
                element.Height *= _blockSize;
                CanvasForeground.Children.Add(element);
                Canvas.SetLeft(element, (entity.Position.X - entity.BoundingBox.BottomRight.X) * _blockSize);
                Canvas.SetTop(element, (entity.Position.Y - entity.BoundingBox.BottomRight.Y) * _blockSize);
            }
        }
    }
}
