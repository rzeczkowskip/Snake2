using System.Drawing;
using Snake2.Actor;
using Snake2.Render;

namespace Snake2
{
    enum GameState
    {
        None,
        Running,
        Paused,
        Finished,
        GameOver,
        Unpausing,
    }
    
    public class Game
    {
        private readonly World _world;
        private readonly Renderer _renderer;

        private readonly (int, int) _minConsoleSize;

        private GameState _state;
        private Snake _snake;
        private DifficultyCalculator _difficulty;
        private int _score = 0;
        private Food _food;

        public Game(World world, Renderer renderer)
        {
            _world = world;
            _renderer = renderer;

            _minConsoleSize = (_world.Width * 2, _world.Height);
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

            switch (_state)
            {
                case GameState.Running:
                case GameState.Unpausing:
                    throw new Exception("Game already running.");
                case GameState.None:
                case GameState.Finished:
                case GameState.GameOver:
                    Init();
                    break;
                case GameState.Paused:
                    Unpause();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            GameLoop();

            RedrawScreen();
            return WaitForExit();
        }

        private void Init()
        {
            _difficulty = new DifficultyCalculator();
            _snake = new Snake(
                new Point(0, 0),
                new Point(Console.WindowWidth - 1, Console.WindowHeight - 2)
            );

            _food = new Food(
                _snake,
                new Point(0, 0),
                new Point(Console.WindowWidth - 1, Console.WindowHeight - 2),
                new DateTime().GetHashCode()
            );

            _state = GameState.Running;
            RedrawScreen();
        }

        private void RedrawScreen()
        {
            Console.Clear();

            foreach (var p in _snake.GetBody())
            {
                _renderer.Render(p, Symbols.SnakeBody);
            }

            _renderer.Render(_snake.GetHead(), Symbols.SnakeHead);
            DrawStatusBar();
        }

        private void GameLoop()
        {
            var previousHead = _snake.GetHead();
            var foodPosition = _food.GetNewFoodPosition();

            while (true)
            {
                if (_state == GameState.Finished)
                {
                    break;
                }

                ProcessUserInput();
                DrawStatusBar();

                _renderer.Render(foodPosition, Symbols.Food);

                if (_state == GameState.Running)
                {
                    var snakeEdges = _snake.Move();
                    if (_snake.GetBody().SkipLast(1).Contains(snakeEdges.Head))
                    {
                        _state = GameState.Finished;
                        break;
                    }

                    if (snakeEdges.Head == foodPosition)
                    {
                        _snake.Grow();
                        _score += 1;

                        if (_score % 3 == 0)
                        {
                            _difficulty.IncreaseLevel();
                        }

                        _renderer.ClearPoint(foodPosition);
                        
                        foodPosition = _food.GetNewFoodPosition();
                        _renderer.Render(foodPosition, Symbols.Food);
                    }

                    _renderer.Render(snakeEdges.Head, Symbols.SnakeHead);
                    _renderer.ClearPoint(snakeEdges.Tail);
                    _renderer.Render(previousHead, Symbols.SnakeBody);

                    previousHead = snakeEdges.Head;
                }

                Thread.Sleep(TimeSpan.FromMilliseconds(_difficulty.SleepTime));
            }
        }

        private void ProcessUserInput()
        {
            if (_state != GameState.Paused && !Console.KeyAvailable)
            {
                return;
            }

            var pressedKey = Console.ReadKey(true).Key;

            if (pressedKey == ConsoleKey.Escape)
            {
                PauseOrExit();
                return;
            }

            if (_state == GameState.Paused)
            {
                Unpause();
            }

            ProcessSnakeDirectionInput(pressedKey);
        }

        private void PauseOrExit()
        {
            _state = _state switch
            {
                GameState.Paused => GameState.Finished,
                GameState.Running => GameState.Paused,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private int WaitForExit()
        {
            while (true)
            {
                var key = Console.ReadKey(true).Key;
                if (key == ConsoleKey.Escape)
                {
                    return _score;
                }

                if (key == ConsoleKey.Enter)
                {
                    return Play();
                }
            }
        }

        private void Unpause()
        {
            _state = GameState.Unpausing;
            RedrawScreen();
            _state = GameState.Running;
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

            _snake.UpdateDirection(newDirection);
        }

        private void DrawStatusBar()
        {
            Console.SetCursorPosition(1, Console.WindowHeight - 1);

            Console.ForegroundColor = ConsoleColor.Cyan;
            if (_state is GameState.Paused or GameState.Finished)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write(_state == GameState.Paused ? "Pause" : "Finished");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(" | ");
            }

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("Level: ");

            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($"{_difficulty.Level} ");

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("Score: ");

            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($"{_score} ");
        }
    }
}