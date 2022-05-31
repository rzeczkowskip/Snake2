using System.Drawing;
using Snake2.Actor;
using Snake2.Render;

namespace Snake2
{
    public class Game
    {
        private readonly World _world;
        private readonly Renderer _renderer;
        private readonly GameState _gameState;

        private readonly (int, int) _minConsoleSize;
        
        public Game(World world, Renderer renderer)
        {
            _world = world;
            _renderer = renderer;

            _minConsoleSize = (world.Width * 2, world.Height);
            _gameState = new GameState(world);
        }

        private bool ValidateGameWindow()
        {
            return Console.WindowWidth > _minConsoleSize.Item1 && Console.WindowHeight > _minConsoleSize.Item2;
        }

        public int Play()
        {
            while (!ValidateGameWindow())
            {
                Console.WriteLine(
                    $"Zły rozmiar okna gry. Minimalne wymiary wymagane do uruchomienia to " +
                    $"{_minConsoleSize.Item1}x{_minConsoleSize.Item1}");
                Console.WriteLine("Zmień rozmiar i naciśnij enter");
                Console.ReadKey();
            }
            
            switch (_gameState.Current)
            {
                case GameState.State.None:
                    _gameState.Start();
                    RedrawScreen();
                    break;
                case GameState.State.Finished:
                case GameState.State.GameOver:
                    _gameState.Reload();
                    _gameState.Start();
                    RedrawScreen();
                    break;
                case GameState.State.Paused:
                    _gameState.Unpause();
                    RedrawScreen();
                    break;
                case GameState.State.Running:
                case GameState.State.Unpausing:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            GameLoop();

            RedrawScreen();
            return WaitForExit();
        }

        private void RedrawScreen()
        {
            Console.Clear();

            foreach (var p in _world.GetPoints())
            {
                _renderer.Render(p, Symbols.Wall);
            }

            foreach (var p in _gameState.Snake.GetBody())
            {
                _renderer.Render(p, Symbols.SnakeBody);
            }

            _renderer.Render(_gameState.Snake.GetHead(), Symbols.SnakeHead);
            DrawStatusBar();
        }

        private void GameLoop()
        {
            var previousHead = _gameState.Snake.GetHead();
            var foodPosition = _gameState.GenerateFood();

            while (true)
            {
                if (_gameState.Current == GameState.State.Finished)
                {
                    break;
                }

                ProcessUserInput();
                DrawStatusBar();

                _renderer.Render(foodPosition, Symbols.Food);

                if (_gameState.Current == GameState.State.Running)
                {
                    var snakeEdges = _gameState.Snake.Move();
                    if (_gameState.Snake.GetBody().SkipLast(1).Contains(snakeEdges.Head))
                    {
                        _gameState.Finish();
                        break;
                    }

                    if (snakeEdges.Head == foodPosition)
                    {
                        _gameState.Snake.Grow();
                        _gameState.AddPoint();

                        if (_gameState.Score % 3 == 0)
                        {
                            _gameState.IncreaseLevel();
                        }

                        _renderer.ClearPoint(foodPosition);
                        
                        foodPosition = _gameState.GenerateFood();
                        _renderer.Render(foodPosition, Symbols.Food);
                    }

                    _renderer.Render(snakeEdges.Head, Symbols.SnakeHead);
                    _renderer.ClearPoint(snakeEdges.Tail);
                    _renderer.Render(previousHead, Symbols.SnakeBody);

                    previousHead = snakeEdges.Head;
                }

                Thread.Sleep(TimeSpan.FromMilliseconds(_gameState.FrameDelay));
            }
        }

        private void ProcessUserInput()
        {
            if (_gameState.Current != GameState.State.Paused && !Console.KeyAvailable)
            {
                return;
            }

            var pressedKey = Console.ReadKey(true).Key;

            if (pressedKey == ConsoleKey.Escape)
            {
                PauseOrExit();
                return;
            }

            if (_gameState.Current == GameState.State.Paused)
            {
                _gameState.Unpause();
            }

            ProcessSnakeDirectionInput(pressedKey);
        }

        private void PauseOrExit()
        {
            switch (_gameState.Current)
            {
                case GameState.State.Paused:
                    _gameState.Finish();
                    break;
                case GameState.State.Running:
                    _gameState.Pause();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private int WaitForExit()
        {
            while (true)
            {
                var key = Console.ReadKey(true).Key;
                switch (key)
                {
                    case ConsoleKey.Escape:
                        return _gameState.Score;
                    case ConsoleKey.Enter:
                        return Play();
                }
            }
        }

        private void ProcessSnakeDirectionInput(ConsoleKey pressedKey)
        {
            SnakeDirection newDirection = pressedKey switch
            {
                ConsoleKey.UpArrow => SnakeDirection.Up,
                ConsoleKey.DownArrow => SnakeDirection.Down,
                ConsoleKey.LeftArrow => SnakeDirection.Left,
                ConsoleKey.RightArrow => SnakeDirection.Right,
                _ => SnakeDirection.Keep,
            };

            _gameState.Snake.UpdateDirection(newDirection);
        }

        private void DrawStatusBar()
        {
            Console.SetCursorPosition(1, Console.WindowHeight - 1);

            Console.ForegroundColor = ConsoleColor.Cyan;
            if (_gameState.Current is GameState.State.Paused or GameState.State.Finished)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write(_gameState.Current == GameState.State.Paused ? "Pause" : "Finished");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(" | ");
            }

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("Level: ");

            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($"{_gameState.Level} ");

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("Score: ");

            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($"{_gameState.Score} ");
            
            Console.Write(_gameState.FrameDelay);
        }
    }
}