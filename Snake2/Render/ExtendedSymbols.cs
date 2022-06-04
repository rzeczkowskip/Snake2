namespace Snake2.Render;

public class ExtendedSymbols : DefaultSymbols
{
    private const char Food = '\u25CD';

    public override char GetSymbol(Symbols symbol)
    {
        return symbol switch
        {
            Symbols.Food => Food,
            _ => base.GetSymbol(symbol)
        };
    }
}