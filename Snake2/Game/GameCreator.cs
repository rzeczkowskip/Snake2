using System.Drawing;
using Snake2.Environment;
using Snake2.Render;

namespace Snake2.Game;

public class GameCreator
{
    private const int WorldWidth = 40;
    private const int WorldHeight = 20;

    private const int MinConsoleWidth = WorldWidth * 2;
    private const int MinConsoleHeight = WorldHeight;

    public Game CreateGame(SymbolProvider symbolProvider)
    {
        while (!ValidateGameWindow())
        {
            Console.WriteLine(
                "Zły rozmiar okna gry. Minimalne wymiary wymagane do uruchomienia to " +
                $"{MinConsoleWidth}x{MinConsoleHeight}");
            Console.WriteLine("Zmień rozmiar i naciśnij enter");
            Console.ReadKey();
        }

        var world = new World(WorldWidth, WorldHeight);
        var renderer = new Renderer(symbolProvider);
        var statusBar = new StatusBar(new Point(WorldWidth + 3, 2));

        return new SimpleSnake(world, renderer, statusBar);
    }

    private bool ValidateGameWindow()
    {
        return Console.WindowWidth > MinConsoleWidth && Console.WindowHeight > MinConsoleHeight;
    }
}