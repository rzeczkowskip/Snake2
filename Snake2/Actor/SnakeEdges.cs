using System.Drawing;

namespace Snake2.Actor
{
    public class SnakeEdges
    {
        public Point Head { get; }
        public Point Tail { get; }

        public SnakeEdges(Point head, Point tail)
        {
            Head = head;
            Tail = tail;
        }
    }
}
