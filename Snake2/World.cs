using System.Drawing;

namespace Snake2;

public class World
{
    public int Width { get; }
    public int Height { get; }
    
    private List<Point> _walls;
    
    public World(int width, int height)
    {
        Width = width;
        Height = height;
        _walls = new List<Point>();
        for (var i = 0; i <= width; i++)
        {
            _walls.Add(new Point(i ,0));
            _walls.Add(new Point(i, height));
        }

        for (var i = 0; i < height; i++)
        {
            _walls.Add(new Point(0, i));
            _walls.Add(new Point(width, i));
        }
    }
    
    public List<Point> GetPoints()
    {
        return _walls;
    }
}