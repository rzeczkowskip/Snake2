using System.Drawing;
using Snake2.Environment;
using Snake2.Render;

namespace Snake2.Game;

public class GameCreator
{
    private const int WORLD_WIDTH = 40;
    private const int WORLD_HEIGHT = 20;

    private const int MIN_CONSOLE_WIDTH = WORLD_WIDTH * 2;
    private const int MIN_CONSOLE_HEIGHT = WORLD_HEIGHT;

    public Game CreateGame(SymbolProvider symbolProvider)
    {
        while (!ValidateGameWindow())
        {
            Console.WriteLine(
                $"Zły rozmiar okna gry. Minimalne wymiary wymagane do uruchomienia to " +
                $"{MIN_CONSOLE_WIDTH}x{MIN_CONSOLE_HEIGHT}");
            Console.WriteLine("Zmień rozmiar i naciśnij enter");
            Console.ReadKey();
        }

        var world = new World(WORLD_WIDTH, WORLD_HEIGHT);
        var renderer = new Renderer(symbolProvider);
        var statusBar = new StatusBar(new Point(WORLD_WIDTH + 3, 2));
        
        return new SimpleSnake(world, renderer, statusBar);        
    }
    
    private bool ValidateGameWindow()
    {
        return Console.WindowWidth > MIN_CONSOLE_WIDTH && Console.WindowHeight > MIN_CONSOLE_HEIGHT;
    }
}
