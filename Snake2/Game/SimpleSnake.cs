using Snake2.Actor;
using Snake2.Environment;
using Snake2.Render;

namespace Snake2.Game
{
    public class SimpleSnake : Game
    {
        private readonly World _world;
        private readonly Renderer _renderer;
        private readonly StatusBar _statusBar;
        private readonly GameState _gameState;
        
        public SimpleSnake(World world, Renderer renderer, StatusBar statusBar)
        {
            _world = world;
            _renderer = renderer;
            _statusBar = statusBar;

            _gameState = new GameState(world);
        }

        public int Play()
        {
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

            _renderer.Render(_world.GetPoints(), Symbols.Wall);
            _renderer.Render(_gameState.Snake.GetBody(), Symbols.SnakeBody);
            _renderer.Render(_gameState.Snake.GetHead(), Symbols.SnakeHead);
            
            _statusBar.DrawStatus(_gameState);
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
                _statusBar.DrawStatus(_gameState);

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
    }
}