using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacManConsole
{
    class Level
    {
        private static Level instance;
        private static StreamReader rd;

        private const string path = @"level.txt";
        private const int mapHeight = 31;
        private const int mapWight = 28;

        private readonly char[,] Map;
        private int[,] _map;

        private Level()
        {
            Map = new char[mapHeight, mapWight];

            rd = new StreamReader(path, Encoding.UTF8);

            Random rand = new Random();

            string tmpLine;

            string[] line = new string[mapHeight];

            for (int i = 0; (tmpLine = rd.ReadLine()) != null; i++)
            {
                line[i] = tmpLine;
            }

            for (int i = 0; i < line.Length; i++)
            {
                char[] tmp = line[i].ToCharArray();

                for (int j = 0; j < tmp.Length; j++)
                {
                    if (tmp[j] == ' ') 
                        Map[i, j] = Convert.ToChar(0x2219);
                    else
                        Map[i, j] = tmp[j];
                }
            }
            _map = new int[Map.GetLength(0), Map.GetLength(1)];
            
            int[] bonus = new int[] { 1, 5, 8, 20, 26, 29 };

            for (int i = 0; i < mapHeight; i++)
            {
                int j = rand.Next(1, 28);
                
                if (bonus.Contains(i) && Map[i, j] == 8729)
                {
                    Map[i, j] = Convert.ToChar(0xA4);
                }
            }
        }
        public static Level GetInstance()
        {
            if (instance == null)
            {
                instance = new Level();
            }
            return instance;
        }
        public int[,] GetMap()
        {
            for (int i = 0; i < Map.GetLength(0); i++)
            {
                for (int j = 0; j < Map.GetLength(1); j++)
                {
                    if (Map[i, j] == ' ' || Map[i, j] == '1')
                        _map[i, j] = (int)Figures.EmptySpace;
                    else if (Map[i, j] == 8729)
                        _map[i, j] = (int)Figures.Eat;
                    else if (Map[i, j] == 164)
                        _map[i, j] = (int)Figures.Bonus;
                    else
                        _map[i, j] = (int)Figures.Barrier;
                }
            }

            return _map;
        }
        public void DrawLevel()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.CursorVisible = false;
            Console.SetCursorPosition(0, 0);

            for (int i = 0; i < mapHeight; i++)
            {
                for (int j = 0; j < mapWight; j++)
                {
                    if (_map[i, j] == (int)Figures.EmptySpace)
                        Console.Write(' ');
                    else if (_map[i, j] == (int)Figures.Bonus)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        Console.Write(Convert.ToChar(0xA4));
                        Console.ForegroundColor = ConsoleColor.Cyan;
                    }
                    else
                        Console.Write(Map[i, j]);
                }
                Console.WriteLine();
            }
            Console.ResetColor();
        }
    }
}
