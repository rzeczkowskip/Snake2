using System.Drawing;
using Snake2.Actor;
using Snake2.Environment;

namespace Snake2.Game;

public class GameState
{
    public enum State
    {
        None,
        Running,
        Paused,
        Finished,
        GameOver,
    }
    
    public State Current { get; private set; }
    public Snake Snake { get; private set; }
    
    public int Score { get; private set; }

    public int Level { get; private set; } = 1;
    private int _frameDelay = 250;
    
    public double FrameDelay => Snake.DirectionAxis == SnakeDirectionAxis.Y ? _frameDelay * 1.5 : _frameDelay;

    private readonly Random _random;
    
    private readonly Point _gameTopLeft;
    private readonly Point _gameBottomRight;
    
    public GameState(World world, int seed = 1337)
    {
        _random = new Random(seed);
        
        _gameTopLeft = new Point(1, 1);
        _gameBottomRight = new Point(world.Width - 1, world.Height - 1);
        
        Snake = new Snake(
            _gameTopLeft,
            _gameBottomRight
        );
        
        Reload();
    }

    public void Reload()
    {
        Level = 1;
        _frameDelay = 250;
        
        Snake = new Snake(
            _gameTopLeft,
            _gameBottomRight
        );

        Current = State.None;
    }

    public void Start()
    {
        if (Current is State.Running)
        {
            throw new Exception("Game already running.");
        }

        Current = State.Running;
    }

    public void Pause()
    {
        Current = State.Paused;
    }

    public void Unpause()
    {
        Current = State.Running;
    }

    public void Finish()
    {
        Current = State.Finished;
    }

    public Point GenerateFood()
    {
        Point point;
        
        do
        {
            point = new Point(
                _random.Next(_gameTopLeft.X, _gameBottomRight.X),
                _random.Next(_gameTopLeft.Y, _gameBottomRight.Y)
            );
        } while (Snake.GetBody().Contains(point));

        return point;
    }

    public void AddPoint()
    {
        Score += 1;
    }

    public void IncreaseLevel()
    {
        if (_frameDelay == 1)
        {
            return;
        }
            
        Level += 1;
        _frameDelay = Math.Max(_frameDelay - (_frameDelay / 2), 1);
    }
}