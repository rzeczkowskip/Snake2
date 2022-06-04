using System.Drawing;

namespace Snake2.Actor;

public class SnakeEdges
{
    public SnakeEdges(Point head, Point tail)
    {
        Head = head;
        Tail = tail;
    }

    public Point Head { get; }
    public Point Tail { get; }
}