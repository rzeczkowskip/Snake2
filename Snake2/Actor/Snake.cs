using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Snake2.Actor
{
    public enum SnakeDirection
    {
        Up,
        Down,
        Left,
        Right,
        Keep
    }

    public enum SnakeDirectionAxis
    {
        Y,
        X
    }

    public class Snake 
    {
        private SnakeDirection _direction;
        public SnakeDirectionAxis DirectionAxis { get; private set; }
        private int _length = 3;

        private readonly List<Point> _snake = new();

        private readonly Point _topLeft;
        private readonly Point _bottomRight;

        public Snake(Point topLeft, Point bottomRight)
        {
            _topLeft = topLeft;
            _bottomRight = bottomRight;

            UpdateDirection(SnakeDirection.Right);
            _snake.Add(new Point(
                (_bottomRight.X - _topLeft.X) / 2,
                (_bottomRight.Y - _topLeft.Y) / 2
            ));
        }

        public List<Point> GetBody()
        {
            return _snake;
        }

        public void UpdateDirection(SnakeDirection direction)
        {
            if (direction == SnakeDirection.Keep)
            {
                return;
            }
            
            SnakeDirectionAxis newAxis = direction is SnakeDirection.Down or SnakeDirection.Up
                ? SnakeDirectionAxis.Y
                : SnakeDirectionAxis.X;

            if (newAxis == DirectionAxis)
            {
                return;
            }

            _direction = direction;
            DirectionAxis = newAxis;
        }

        public SnakeEdges Move()
        {
            Point head = GetHead();
            Point tail = GetTail();

            switch (_direction)
            {
                case SnakeDirection.Up:
                    head.Y -= 1;
                    break;
                case SnakeDirection.Down:
                    head.Y += 1;
                    break;
                case SnakeDirection.Left:
                    head.X -= 1;
                    break;
                case SnakeDirection.Right:
                    head.X += 1;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (head.Y < _topLeft.Y)
            {
                head.Y = _bottomRight.Y;
            }
            else if (head.Y > _bottomRight.Y)
            {
                head.Y = _topLeft.Y;
            }
            
            if (head.X < _topLeft.Y)
            {
                head.X = _bottomRight.X;
            }
            else if (head.X > _bottomRight.X)
            {
                head.X = _topLeft.X;
            }

            _snake.Add(head);

            if (_snake.Count > _length)
            {
                _snake.RemoveAt(0);
            }
            else
            {
                tail = Point.Empty;
            }

            return new SnakeEdges(head, tail);
        }

        public void Grow()
        {
            _length += 1;
        }

        public Point GetHead()
        {
            return _snake.Last();
        }

        private Point GetTail()
        {
            return _snake.First();
        }
    }
}
