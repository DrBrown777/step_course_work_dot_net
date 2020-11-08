using System;
using System.Linq;
using System.Threading;
using Timer = System.Timers.Timer;

namespace PacManConsole
{
    class PacMan
    {
        private static PacMan instance;
        static readonly char icon = Convert.ToChar(0x263A);
        static Timer timer;

        static int[,] map;
        public int Score { get; set; }
        public int Live { get; set; }
        public int PosX { get; set; }
        public int PosY { get; set; }
        public int Dir { get; set; }
        public bool PacmanAttack { get; set; }
        
        private PacMan(ref int[,] _map)
        {
            PosX = 13; PosY = 23;
            Dir = 0;
            map = _map;
            Score = 0;
            Live = 3;
            timer = new Timer(10000)
            {
                AutoReset = false
            };
            timer.Elapsed += Timer_Elapsed; 
        }

        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            PacmanAttack = false;
            timer.Stop();
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
                Score += 10;
                map[Console.CursorTop, Console.CursorLeft] = (int)Figures.EmptySpace;
            }
            else if (map[Console.CursorTop, Console.CursorLeft] == (int)Figures.Bonus)
            {
                PacmanAttack = true;
                timer.Start();
                Score += 50;
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
                    else if (Global.checkItemMap.Contains(map[PosY, PosX + 1]))
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
                    else if (Global.checkItemMap.Contains(map[PosY, PosX - 1]))
                    {
                        ClearTheTrack();
                        PosX--;
                    }
                    break;
                case 3:
                    if (Global.checkItemMap.Contains(map[PosY + 1, PosX]))
                    {
                        ClearTheTrack();
                        PosY++;
                    }
                    break;
                case 4:
                    if (Global.checkItemMap.Contains(map[PosY - 1, PosX]))
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
