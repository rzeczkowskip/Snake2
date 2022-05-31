namespace Snake2.Render;

public class DefaultSymbols : SymbolProvider
{
    private const char SnakeBody = '\u2593';
    private const char SnakeHead = '\u2588';
    private const char SnakeDead = 'X';
    private const char Empty = ' ';
    private const char Food = 'x';
    private const char StatusBarBg = '\u2593';

    public char GetSymbol(Symbols symbol)
    {
        return symbol switch
        {
            Symbols.SnakeBody => SnakeBody,
            Symbols.SnakeHead => SnakeHead,
            Symbols.SnakeDead => SnakeDead,
            Symbols.Empty => Empty,
            Symbols.Food => Food,
            Symbols.StatusBarBg => StatusBarBg,
            _ => Empty
        };
    }

    public ConsoleColor GetSymbolColor(Symbols symbol)
    {
        return symbol switch
        {
            Symbols.SnakeHead => ConsoleColor.Cyan,
            Symbols.Food => ConsoleColor.Yellow,
            _ => ConsoleColor.White,
        };
    }
}
