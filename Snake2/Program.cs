// See https://aka.ms/new-console-template for more information

using Snake2.Game;
using Snake2.Render;

Console.Clear();
Console.CursorVisible = false;

var gameCreator = new GameCreator();
var game = gameCreator.CreateGame(new DefaultSymbols());

var score = game.Play();

Console.Clear();
Console.WriteLine($"Final score: {score}");
Console.CursorVisible = true;