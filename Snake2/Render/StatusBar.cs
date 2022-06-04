using System.Drawing;
using Snake2.Game;

namespace Snake2.Render;

public class StatusBar
{
    private readonly Point _topLeft;

    public StatusBar(Point topLeft)
    {
        _topLeft = topLeft;
    }

    public void DrawStatus(GameState gameState)
    {
        var stateStatusMessage = gameState.Current switch
        {
            GameState.State.Paused => "Pause",
            GameState.State.Finished => "Game over",
            _ => new string(' ', 8) // set count to longest status message
        };

        Console.SetCursorPosition(_topLeft.X, _topLeft.Y);

        DrawLine(gameState.Level.ToString(), "Level");
        DrawLine(gameState.Score.ToString(), "Score");
        DrawLine(stateStatusMessage);
    }

    private void DrawLine(string message, string? label = null)
    {
        if (label != null)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"{label}: ");

            Console.SetCursorPosition(_topLeft.X, Console.CursorTop);
            Console.ForegroundColor = ConsoleColor.White;
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
        }

        Console.WriteLine(message);
        Console.SetCursorPosition(_topLeft.X, Console.CursorTop + 1);
    }
}