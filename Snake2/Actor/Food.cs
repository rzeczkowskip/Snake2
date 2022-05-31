﻿using System.Drawing;

namespace Snake2.Actor
{
    public class Food
    {
        public Point TopLeft
        {
            get => _topLeft;
            set => _topLeft = value;
        }
        private Snake _snake;
        private Point _topLeft;
        private Point _bottomRight;

        private Random _random;
        
        public Food(Snake snake, Point topLeft, Point bottomRight, int seed = 1337)
        {
            _snake = snake;
            _topLeft = topLeft;
            _bottomRight = bottomRight;

            _random = new Random(seed);
        }

        public Point GetNewFoodPosition()
        {
            Point point = Point.Empty;
            
            do
            {
                point = new Point(
                    _random.Next(_topLeft.X, _bottomRight.X),
                    _random.Next(_topLeft.Y, _bottomRight.Y)
                );
            } while (_snake.GetBody().Contains(point));

            return point;
        }
    }
}