using System;
using System.Threading;

namespace PacManConsole
{
    class PacMan
    {
        private static PacMan instance;
        static readonly char icon = Convert.ToChar(0x263A);
        static int[,] map;

        public int PosX { get; set; }
        public int PosY { get; set; }
        public int Dir { get; set; }
        private PacMan(ref int[,] _map)
        {
            PosX = 13; PosY = 23;
            Dir = 0;
            map = _map;
        }
        public static PacMan GetInstance(ref int[,] _map)
        {
            if (instance == null)
            {
                instance = new PacMan(ref _map);
            }
            return instance;
        }
        private void Draw()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.SetCursorPosition(PosX, PosY);
            if (map[Console.CursorTop, Console.CursorLeft] == (int)Figures.Eat)
            {
                map[Console.CursorTop, Console.CursorLeft] = (int)Figures.EmptySpace;
            }    
            Console.Write(icon);
        }
        private void ClearTheTrack()
        {
            Console.SetCursorPosition(PosX, PosY);
            Console.Write(" ");
        }
        public void Update(int speed)
        {
            Draw();
           
            Thread.Sleep(speed);

            switch (Dir)
            {
                case 1:
                    if (PosX + 1 == 28 && PosY == 14)
                    {
                        ClearTheTrack();
                        PosX -= 27;
                    }
                    else if (map[PosY, PosX + 1] == (int)Figures.Eat || map[PosY, PosX + 1] == (int)Figures.EmptySpace)
                    {
                        ClearTheTrack();
                        PosX++;
                    }
                    break;
                case 2:
                    if (PosX == 0 && PosY == 14)
                    {
                        ClearTheTrack();
                        PosX += 27;
                    }
                    else if (map[PosY, PosX - 1] == (int)Figures.Eat || map[PosY, PosX - 1] == (int)Figures.EmptySpace)
                    {
                        ClearTheTrack();
                        PosX--;
                    }
                    break;
                case 3:
                    if (map[PosY + 1, PosX] == (int)Figures.Eat || map[PosY + 1, PosX] == (int)Figures.EmptySpace)
                    {
                        ClearTheTrack();
                        PosY++;
                    }
                    break;
                case 4:
                    if (map[PosY - 1, PosX] == (int)Figures.Eat || map[PosY - 1, PosX] == (int)Figures.EmptySpace)
                    {
                        ClearTheTrack();
                        PosY--;
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
