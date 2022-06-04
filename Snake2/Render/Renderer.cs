using System.Drawing;

namespace Snake2.Render;

public class Renderer
{
    private readonly SymbolProvider _symbolProvider;

    public Renderer(SymbolProvider symbolProvider)
    {
        _symbolProvider = symbolProvider;
    }

    public void Render(Point point, Symbols symbol)
    {
        Console.ForegroundColor = _symbolProvider.GetSymbolColor(symbol);
        renderSymbol(point, symbol);
    }

    public void Render(List<Point> points, Symbols symbol)
    {
        Console.ForegroundColor = _symbolProvider.GetSymbolColor(symbol);
        foreach (var point in points) renderSymbol(point, symbol);
    }

    public void ClearPoint(Point point)
    {
        renderSymbol(point, Symbols.Empty);
    }

    private void renderSymbol(Point point, Symbols symbol)
    {
        Console.SetCursorPosition(point.X, point.Y);
        Console.Write(_symbolProvider.GetSymbol(symbol));
    }
}