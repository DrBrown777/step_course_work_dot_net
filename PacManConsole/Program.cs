using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace PacManConsole
{
    class Program
    {
        enum Dir { Right = 1, Left, Down, Up}
        static readonly int speedGame = 150;
        static ConsoleKeyInfo k;
        static bool gameover = false;

        static void Menu()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.SetCursorPosition(15, 8);
            Console.WriteLine(@" 
              ____  ____  ____  _      ____  _          
            /  __\/  _ \/   _\/ \__/|/  _ \/ \  /|     
            |  \/|| / \||  /  | |\/||| / \|| |\ ||     
            |  __/| |-|||  \_ | |  ||| |-||| | \||     
            \_/   \_/ \|\____/\_/  \|\_/ \|\_/  \|     
                                           
           ____  ____  _      ____  ____  _     _____
          /   _\/  _ \/ \  /|/ ___\/  _ \/ \   /  __/
          |  /  | / \|| |\ |||    \| / \|| |   |  \  
          |  \__| \_/|| | \||\___ || \_/|| |_/\|  /_ 
          \____/\____/\_/  \|\____/\____/\____/\____\
                                           
");
            Console.SetCursorPosition(18, 26);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Press Enter to Start Game");
            k = Console.ReadKey(true);
            
            while (Console.KeyAvailable == false)
            {
                if (k.Key == ConsoleKey.Enter)
                    break;
            }
        }

        static void GameOver()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.SetCursorPosition(15, 8);
            Console.WriteLine(@" 
                 _____ ____  _      _____
                /  __//  _ \/ \__/|/  __/
                | |  _| / \|| |\/|||  \  
                | |_//| |-||| |  |||  /_ 
                \____\\_/ \|\_/  \|\____\
                         
                 ____  _     _____ ____  
                /  _ \/ \ |\/  __//  __\ 
                | / \|| | //|  \  |  \/| 
                | \_/|| \// |  /_ |    / 
                \____/\__/  \____\\_/\_\ 
                                                              
");
            Console.SetCursorPosition(18, 26);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Press Enter to Menu");
            k = Console.ReadKey(true);

            while (Console.KeyAvailable == false)
            {
                if (k.Key == ConsoleKey.Enter)
                    break;
            }
        }

        static void DrawStatistics(int score, int life)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.SetCursorPosition(35, 2);
            Console.Write("Score: {0}", score);
            Console.SetCursorPosition(35, 3);
            Console.Write("Life: {0}", life);
        }

        static void ReloadLevel(ref List<Ghost> ghost, ref PacMan pacMan, ref int[,] map, ref Level level)
        {
            ghost.Clear();
            ghost.Add(new Red(ref map));
            ghost.Add(new Blue(ref map));
            ghost.Add(new Green(ref map));
            ghost.Add(new Magenta(ref map));
            Console.Clear();
            level.DrawLevel();
            pacMan.PosX = 13; pacMan.PosY = 23;
            pacMan.Dir = 0;
        }

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

            Menu();

            level.DrawLevel();

            while (true)
            {
                if(gameover == true)
                {
                    Menu();
                    ReloadLevel(ref ghost, ref pacMan, ref map, ref level);
                    gameover = false;
                }
                while (Console.KeyAvailable == false)
                {
                    pacMan.Update(speedGame);

                    foreach (var item in ghost)
                    {
                       
                        item.Update(pacMan.PosX, pacMan.PosY, PacMan.PacmanAttack);
                       
                        item.Draw();
                        if (item.PosX == pacMan.PosX & item.PosY == pacMan.PosY & !PacMan.PacmanAttack)
                        {
                            pacMan.Live--;
                            Console.SetCursorPosition(35, 5);
                            Console.WriteLine("YOU DEAD!");
                            Thread.Sleep(1000);
                            ReloadLevel(ref ghost, ref pacMan, ref map, ref level);
                            break;
                        }   
                    }
                    DrawStatistics(pacMan.Score, pacMan.Live);
                    if (pacMan.Live == 0)
                    {
                        pacMan.Live = 3;
                        pacMan.Score = 0;
                        map = level.GetMap();
                        GameOver();
                        gameover = true;
                        break;
                    } 
                }
                
                k = Console.ReadKey(true);

                switch (k.Key)
                {
                    case ConsoleKey.RightArrow when Global.checkItemMap.Contains(map[pacMan.PosY, pacMan.PosX + 1 != 28 ? pacMan.PosX + 1 : pacMan.PosX]):
                        pacMan.Dir = (int)Dir.Right;
                        break;
                    case ConsoleKey.LeftArrow when Global.checkItemMap.Contains(map[pacMan.PosY, pacMan.PosX - 1 != -1 ? pacMan.PosX - 1 : pacMan.PosX]):
                        pacMan.Dir = (int)Dir.Left;
                        break;
                    case ConsoleKey.DownArrow when Global.checkItemMap.Contains(map[pacMan.PosY + 1, pacMan.PosX]):
                        pacMan.Dir = (int)Dir.Down;
                        break;
                    case ConsoleKey.UpArrow when Global.checkItemMap.Contains(map[pacMan.PosY - 1, pacMan.PosX]):
                        pacMan.Dir = (int)Dir.Up;
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
