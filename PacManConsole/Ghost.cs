using System;
using System.Collections.Generic;

namespace PacManConsole
{
    abstract class Ghost
    {
        protected List<Tuple<int, int>> visited;
        protected static int[,] map;

        public Ghost(ref int[,] _map)
        {
            Icon = 'W';
            Dir = 4;
            visited = new List<Tuple<int, int>>() { new Tuple<int, int>(PosX, PosY) };
            map = _map;
        }

        protected char Icon { get; set; }
        protected int PosX { get; set; }
        protected int PosY { get; set; }
        protected int Dir { get; set; }
        public virtual void Draw()
        {
            visited.Add(new Tuple<int, int>(PosX, PosY));
            Console.SetCursorPosition(PosX, PosY);
            Console.Write(Icon);
        }
        protected virtual void ClearTheTrack()
        {
            Console.SetCursorPosition(PosX, PosY);
            if (map[Console.CursorTop, Console.CursorLeft] == (int)Figures.Eat)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write(Convert.ToChar(0x2219));
            }
            else 
                Console.Write(" ");
        }
        abstract public void Update();
    }

    class Red : Ghost
    {
        public Red(ref int [,] _map) : base(ref _map)
        {
            PosX = 13; PosY = 13;
        }
        public override void Draw()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            base.Draw();
        }

        private void FindDir()
        {
            for (int x = PosX - 1; x <= PosX + 1; x++)
            {
                for (int y = PosY + 1; y >= PosY - 1; y--)
                {
                    if (x == 0 && y == 14 || x == 27 && y == 14) continue;
                    if (x == PosX && y == PosY) continue;
                    if (x != PosX && y != PosY) continue;

                    var targetPoint = new Tuple<int, int>(x, y);

                    if (map[y, x] == (int)Figures.EmptySpace && !visited.Contains(targetPoint) || map[y, x] == (int)Figures.Eat && !visited.Contains(targetPoint))
                    {
                        if (x == PosX + 1) Dir = 1;
                        if (x == PosX - 1) Dir = 2;
                        if (y == PosY + 1) Dir = 3;
                        if (y == PosY - 1) Dir = 4;

                        return;
                    }
                }
            }
            visited.Clear();
        }

        public override void Update()
        {
            switch (Dir)
            {
                case 1:
                    if (map[PosY, PosX + 1] == (int)Figures.EmptySpace || map[PosY, PosX + 1] == (int)Figures.Eat)
                    {
                        ClearTheTrack();
                        PosX++;
                    }
                    else 
                        FindDir();
                    break;
                case 2:
                    if (map[PosY, PosX - 1] == (int)Figures.EmptySpace || map[PosY, PosX - 1] == (int)Figures.Eat)
                    {
                        ClearTheTrack();
                        PosX--;
                    }
                    else 
                        FindDir();
                    break;
                case 3:
                    if (map[PosY + 1, PosX] == (int)Figures.EmptySpace || map[PosY + 1, PosX] == (int)Figures.Eat)
                    {
                        ClearTheTrack();
                        PosY++;
                    }
                    else 
                        FindDir();
                    break;
                case 4:
                    if (map[PosY - 1, PosX] == (int)Figures.EmptySpace || map[PosY - 1, PosX] == (int)Figures.Eat)
                    {
                        ClearTheTrack();
                        PosY--;
                    }
                    else 
                        FindDir();
                    break;
                default:
                    break;
            }
        }
    }

    class Blue : Ghost
    {
        public Blue(ref int[,] _map) : base(ref _map)
        {
            PosX = 14; PosY = 13;
        }
        public override void Draw()
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            base.Draw();
        }

        private void FindDir()
        {
            for (int x = PosX + 1; x >= PosX - 1; x--)
            {
                for (int y = PosY - 1; y <= PosY + 1; y++)
                {
                    if (x == 0 && y == 14 || x == 27 && y == 14) continue;
                    if (x == PosX && y == PosY) continue;
                    if (x != PosX && y != PosY) continue;

                    var targetPoint = new Tuple<int, int>(x, y);

                    if (map[y, x] == (int)Figures.EmptySpace && !visited.Contains(targetPoint) || map[y, x] == (int)Figures.Eat && !visited.Contains(targetPoint))
                    {
                        if (x == PosX + 1) Dir = 1;
                        if (x == PosX - 1) Dir = 2;
                        if (y == PosY + 1) Dir = 3;
                        if (y == PosY - 1) Dir = 4;

                        return;
                    }
                }
            }
            visited.Clear();
        }

        public override void Update()
        {
            switch (Dir)
            {
                case 1:
                    if (PosX + 1 == 27 && PosY == 14)
                    {
                        Dir = 2;
                        break;
                    }
                    else if (map[PosY, PosX + 1] == (int)Figures.EmptySpace || map[PosY, PosX + 1] == (int)Figures.Eat)
                    {
                        ClearTheTrack();
                        PosX++;
                        FindDir();
                    }
                    break;
                case 2:
                    if (PosX - 1 == 0 && PosY == 14)
                    {
                        Dir = 1;
                        break;
                    }
                    else if(map[PosY, PosX - 1] == (int)Figures.EmptySpace || map[PosY, PosX - 1] == (int)Figures.Eat)
                    {
                        ClearTheTrack();
                        PosX--;
                        FindDir();
                    }
                    break;
                case 3:
                    if (map[PosY + 1, PosX] == (int)Figures.EmptySpace || map[PosY + 1, PosX] == (int)Figures.Eat)
                    {
                        ClearTheTrack();
                        PosY++;
                        FindDir();
                    }
                    else 
                        Dir = 4;
                    break;
                case 4:
                    if (map[PosY - 1, PosX] == (int)Figures.EmptySpace || map[PosY - 1, PosX] == (int)Figures.Eat)
                    {
                        ClearTheTrack();
                        PosY--;
                        FindDir();
                    }
                    else 
                        Dir = 3;
                    break;
                default:
                    break;
            }
        }
    }

    class Green : Ghost
    {
        public Green(ref int[,] _map) : base(ref _map)
        {
            PosX = 13; PosY = 14;
        }
        public override void Draw()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            base.Draw();
        }

        private void FindDir()
        {
            for (int x = PosX + 1; x >= PosX - 1; x--)
            {
                for (int y = PosY + 1; y >= PosY - 1; y--)
                {
                    if (x == 0 && y == 14 || x == 27 && y == 14) continue;
                    if (x == PosX && y == PosY) continue;
                    if (x != PosX && y != PosY) continue;

                    var targetPoint = new Tuple<int, int>(x, y);

                    if (map[y, x] == (int)Figures.EmptySpace && !visited.Contains(targetPoint) || map[y, x] == (int)Figures.Eat && !visited.Contains(targetPoint))
                    {
                        if (x == PosX + 1) Dir = 1;
                        if (x == PosX - 1) Dir = 2;
                        if (y == PosY + 1) Dir = 3;
                        if (y == PosY - 1) Dir = 4;

                        return;
                    }
                }
            }
            visited.Clear();
        }

        public override void Update()
        {
            switch (Dir)
            {
                case 1:
                    if (map[PosY, PosX + 1] == (int)Figures.EmptySpace || map[PosY, PosX + 1] == (int)Figures.Eat)
                    {
                        ClearTheTrack();
                        PosX++;
                    }
                    else 
                        FindDir();
                    break;
                case 2:
                    if (map[PosY, PosX - 1] == (int)Figures.EmptySpace || map[PosY, PosX - 1] == (int)Figures.Eat)
                    {
                        ClearTheTrack();
                        PosX--;
                    }
                    else 
                        FindDir();
                    break;
                case 3:
                    if (map[PosY + 1, PosX] == (int)Figures.EmptySpace || map[PosY + 1, PosX] == (int)Figures.Eat)
                    {
                        ClearTheTrack();
                        PosY++;
                    }
                    else 
                        FindDir();
                    break;
                case 4:
                    if (map[PosY - 1, PosX] == (int)Figures.EmptySpace || map[PosY - 1, PosX] == (int)Figures.Eat)
                    {
                        ClearTheTrack();
                        PosY--;
                    }
                    else 
                        FindDir();
                    break;
                default:
                    break;
            }
        }
    }

    class Magenta : Ghost
    {
        public Magenta(ref int[,] _map) : base(ref _map)
        {
            PosX = 14; PosY = 14;
        }
        public override void Draw()
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            base.Draw();
        }

        private void FindDir()
        {
            for (int x = PosX + 1; x >= PosX - 1; x--)
            {
                for (int y = PosY + 1; y >= PosY - 1; y--)
                {
                    if (x == 0 && y == 14 || x == 27 && y == 14) continue;
                    if (x == PosX && y == PosY) continue;
                    if (x != PosX && y != PosY) continue;

                    var targetPoint = new Tuple<int, int>(x, y);

                    if (map[y, x] == (int)Figures.EmptySpace && !visited.Contains(targetPoint) || map[y, x] == (int)Figures.Eat && !visited.Contains(targetPoint))
                    {
                        if (x == PosX + 1) Dir = 1;
                        if (x == PosX - 1) Dir = 2;
                        if (y == PosY + 1) Dir = 3;
                        if (y == PosY - 1) Dir = 4;

                        return;
                    }
                }
            }
            visited.Clear();
        }

        public override void Update()
        {
            switch (Dir)
            {
                case 1:
                    if (PosX + 1 == 27 && PosY == 14)
                    {
                        Dir = 2;
                        break;
                    }
                    else if (map[PosY, PosX + 1] == (int)Figures.EmptySpace || map[PosY, PosX + 1] == (int)Figures.Eat)
                    {
                        ClearTheTrack();
                        PosX++;
                        FindDir();
                    }
                    break;
                case 2:
                    if (PosX - 1 == 0 && PosY == 14)
                    {
                        Dir = 1;
                        break;
                    }
                    else if (map[PosY, PosX - 1] == (int)Figures.EmptySpace || map[PosY, PosX - 1] == (int)Figures.Eat)
                    {
                        ClearTheTrack();
                        PosX--;
                        FindDir();
                    }
                    break;
                case 3:
                    if (map[PosY + 1, PosX] == (int)Figures.EmptySpace || map[PosY + 1, PosX] == (int)Figures.Eat)
                    {
                        ClearTheTrack();
                        PosY++;
                        FindDir();
                    }
                    else 
                        Dir = 4;
                    break;
                case 4:
                    if (map[PosY - 1, PosX] == (int)Figures.EmptySpace || map[PosY - 1, PosX] == (int)Figures.Eat)
                    {
                        ClearTheTrack();
                        PosY--;
                        FindDir();
                    }
                    else 
                        Dir = 3;
                    break;
                default:
                    break;
            }
        }
    }
}
