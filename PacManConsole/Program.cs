using System;
using System.Collections.Generic;

namespace PacManConsole
{
    class Program
    {
        enum Dir { Right = 1, Left, Down, Up}
        static readonly int speedGame = 150;
        static ConsoleKeyInfo k;
        static void Main(string[] args)
        {
            Console.Title = "PacMan Console";
            Console.WindowHeight = 40; Console.BufferHeight = 40;
            Console.WindowWidth = 60; Console.BufferWidth = 60;
            Console.CursorVisible = false;

            Level level = Level.GetInstance();
            int[,] map = level.GetMap();

            PacMan pacMan = PacMan.GetInstance(ref map);

            List<Ghost> ghost = new List<Ghost> { new Red(ref map), new Blue(ref map), new Green(ref map), new Magenta(ref map) };

            level.DrawLevel();

            while (true)
            {
                while (Console.KeyAvailable == false)
                {
                    pacMan.Update(speedGame);

                    foreach (var item in ghost)
                    {
                        item.Update();
                        item.Draw();
                    }
                }
                
                k = Console.ReadKey(true);

                switch (k.Key)
                {
                    case ConsoleKey.RightArrow when map[pacMan.PosY, pacMan.PosX + 1 != 28 ? pacMan.PosX + 1 : pacMan.PosX] == (int)Figures.Eat 
                                                    || map[pacMan.PosY, pacMan.PosX + 1 != 28 ? pacMan.PosX + 1 : pacMan.PosX] == (int)Figures.EmptySpace:
                        pacMan.Dir = (int)Dir.Right;
                        break;
                    case ConsoleKey.LeftArrow when (map[pacMan.PosY, pacMan.PosX - 1 != -1 ? pacMan.PosX - 1 : pacMan.PosX] == (int)Figures.Eat)
                                                    || (map[pacMan.PosY, pacMan.PosX - 1 != -1 ? pacMan.PosX - 1 : pacMan.PosX] == (int)Figures.EmptySpace):
                        pacMan.Dir = (int)Dir.Left;
                        break;
                    case ConsoleKey.DownArrow when (map[pacMan.PosY + 1, pacMan.PosX] == (int)Figures.Eat) || (map[pacMan.PosY + 1, pacMan.PosX] == (int)Figures.EmptySpace):
                        pacMan.Dir = (int)Dir.Down;
                        break;
                    case ConsoleKey.UpArrow when map[pacMan.PosY - 1, pacMan.PosX] == (int)Figures.Eat || map[pacMan.PosY - 1, pacMan.PosX] == (int)Figures.EmptySpace:
                        pacMan.Dir = (int)Dir.Up;
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
