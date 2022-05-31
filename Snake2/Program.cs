// See https://aka.ms/new-console-template for more information

using Snake2;
using Snake2.Render;

Console.Clear();
Console.CursorVisible = false;

var game = new Game(
    new World(40, 20),
    new Renderer(new DefaultSymbols())
);
var score = game.Play();
            
Console.Clear();
Console.WriteLine($"Final score: {score}");
Console.CursorVisible = true;