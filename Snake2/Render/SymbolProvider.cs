namespace Snake2.Render;

public interface SymbolProvider
{
    public char GetSymbol(Symbols symbol);

    public ConsoleColor GetSymbolColor(Symbols symbol);
}