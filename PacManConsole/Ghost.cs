﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace PacManConsole
{
    abstract class Ghost
    {
        protected List<Tuple<int, int>> visited;
        protected static int[,] map;
        protected LeeAlgorithm li;
        protected int AttackPower;
        protected bool SaveTheSkin;

        protected char Icon { get; set; }
        public int PosX { get; set; }
        public int PosY { get; set; }
        protected int Dir { get; set; }

        public Ghost(ref int[,] _map)
        {
            Icon = 'W';
            Dir = 4;
            visited = new List<Tuple<int, int>>() { new Tuple<int, int>(PosX, PosY) };
            map = _map;
            li = null;
            SaveTheSkin = false;
        }

        public virtual void Draw()
        {
            visited.Add(new Tuple<int, int>(PosX, PosY));
            Console.SetCursorPosition(PosX, PosY);
            Console.Write(Icon);
        }

        protected void ClearTheTrack()
        {
            Console.SetCursorPosition(PosX, PosY);
            if (map[Console.CursorTop, Console.CursorLeft] == (int)Figures.Eat)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write(Convert.ToChar(0x2219));
            }
            else if(map[Console.CursorTop, Console.CursorLeft] == (int)Figures.Bonus)
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.Write(Convert.ToChar(0xA4));
            }
            else 
                Console.Write(" ");
        }

        protected bool FindPathToPacMan(ref int pacManPosX, ref int pacManPosY)
        {
            int[,] map_1 = new int[map.GetLength(0), map.GetLength(1)];
            Array.Copy(map, map_1, map.Length);

            if (SaveTheSkin)
            {
                map_1[13, 13] = (int)Figures.Destination;
                map_1[PosY, PosX] = (int)Figures.StartPosition;
                Icon = 'M';
                AttackPower = 100;
            }
            else
            {
                map_1[pacManPosY, pacManPosX] = (int)Figures.Destination;
                map_1[PosY, PosX] = (int)Figures.StartPosition;
            }

            li = new LeeAlgorithm(map_1);

            if (li.PathFound != null && li.LengthPath <= AttackPower)
            {
                visited.Clear();
                ClearTheTrack();
                PosY = li.PathFound.Item1;
                PosX = li.PathFound.Item2;
                return true;
            }
            return false;
        }

        protected bool ChangeDir(ref int x, ref int y)
        {
            var targetPoint = new Tuple<int, int>(x, y);
            
            if (Global.checkItemMap.Contains(map[y, x]) && !visited.Contains(targetPoint))
            {
                if (x == PosX + 1) Dir = 1;
                if (x == PosX - 1) Dir = 2;
                if (y == PosY + 1) Dir = 3;
                if (y == PosY - 1) Dir = 4;

                return true;
            }
            return false;
        }

        public virtual void Update(int pacManPosX, int pacManPosY, bool pacManAtack)
        {
            switch (Dir)
            {
                case 1:
                    if (PosX + 1 == 27 && PosY == 14)
                    {
                        Dir = 2;
                    }
                    else if (Global.checkItemMap.Contains(map[PosY, PosX + 1]))
                    {
                        ClearTheTrack();
                        PosX++;
                    }
                    else
                        FindDir();
                    break;
                case 2:
                    if (PosX - 1 == 0 && PosY == 14)
                    {
                        Dir = 1;
                    }
                    else if (Global.checkItemMap.Contains(map[PosY, PosX - 1]))
                    {
                        ClearTheTrack();
                        PosX--;
                    }
                    else
                        FindDir();
                    break;
                case 3:
                    if (Global.checkItemMap.Contains(map[PosY + 1, PosX]))
                    {
                        ClearTheTrack();
                        PosY++;
                    }
                    else
                        FindDir();
                    break;
                case 4:
                    if (Global.checkItemMap.Contains(map[PosY - 1, PosX]))
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

        public abstract void FindDir();
        public abstract void ResetAttack();
    }

    class Red : Ghost
    {
        public Red(ref int [,] _map) : base(ref _map)
        {
            PosX = 13; PosY = 13;
            AttackPower = 20;
        }

        public override void Draw()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            base.Draw();
        }

        public override void FindDir()
        {
            for (int x = PosX - 1; x <= PosX + 1; x++)
            {
                for (int y = PosY + 1; y >= PosY - 1; y--)
                {
                    if (x == 0 && y == 14 || x == 27 && y == 14) continue;
                    if (x == PosX && y == PosY) continue;
                    if (x != PosX && y != PosY) continue;

                    if (ChangeDir(ref x, ref y))
                        return;
                }
            }
            visited.Clear();
        }

        public override void ResetAttack()
        {
            SaveTheSkin = false;
            Icon = 'W';
            AttackPower = 20;
        }

        public override void Update(int pacManPosX, int pacManPosY, bool pacManAtack)
        {
            SaveTheSkin = pacManAtack;

            if (FindPathToPacMan(ref pacManPosX, ref pacManPosY)) 
                return;

            base.Update(pacManPosX, pacManPosY, pacManAtack);
        }
    }

    class Blue : Ghost
    {
        public Blue(ref int[,] _map) : base(ref _map)
        {
            PosX = 14; PosY = 13;
            AttackPower = 18;
        }

        public override void Draw()
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            base.Draw();
        }

        public override void FindDir()
        {
            for (int x = PosX + 1; x >= PosX - 1; x--)
            {
                for (int y = PosY - 1; y <= PosY + 1; y++)
                {
                    if (x == 0 && y == 14 || x == 27 && y == 14) continue;
                    if (x == PosX && y == PosY) continue;
                    if (x != PosX && y != PosY) continue;

                    if (ChangeDir(ref x, ref y))
                        return;
                }
            }
            visited.Clear();
        }

        public override void ResetAttack()
        {
            SaveTheSkin = false;
            Icon = 'W';
            AttackPower = 18;
        }

        public override void Update(int pacManPosX, int pacManPosY, bool pacManAtack)
        {
            SaveTheSkin = pacManAtack;

            if (FindPathToPacMan(ref pacManPosX, ref pacManPosY))
                return;

            switch (Dir)
            {
                case 1:
                    if (PosX + 1 == 27 && PosY == 14)
                    {
                        Dir = 2;
                    }
                    else if (Global.checkItemMap.Contains(map[PosY, PosX + 1]))
                    {
                        ClearTheTrack();
                        PosX++;
                        FindDir();
                    }
                    else
                        Dir = 2;
                    break;
                case 2:
                    if (PosX - 1 == 0 && PosY == 14)
                    {
                        Dir = 1;
                    }
                    else if (Global.checkItemMap.Contains(map[PosY, PosX - 1]))
                    {
                        ClearTheTrack();
                        PosX--;
                        FindDir();
                    }
                    else
                        Dir = 1;
                    break;
                case 3:
                    if (Global.checkItemMap.Contains(map[PosY + 1, PosX]))
                    {
                        ClearTheTrack();
                        PosY++;
                        FindDir();
                    }
                    else 
                        Dir = 4;
                    break;
                case 4:
                    if (Global.checkItemMap.Contains(map[PosY - 1, PosX]))
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
            AttackPower = 10;
        }

        public override void Draw()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            base.Draw();
        }

        public override void FindDir()
        {
            for (int x = PosX + 1; x >= PosX - 1; x--)
            {
                for (int y = PosY + 1; y >= PosY - 1; y--)
                {
                    if (x == 0 && y == 14 || x == 27 && y == 14) continue;
                    if (x == PosX && y == PosY) continue;
                    if (x != PosX && y != PosY) continue;

                    if (ChangeDir(ref x, ref y))
                        return;
                }
            }
            visited.Clear();
        }

        public override void ResetAttack()
        {
            SaveTheSkin = false;
            Icon = 'W';
            AttackPower = 10;
        }

        public override void Update(int pacManPosX, int pacManPosY, bool pacManAtack)
        {
            SaveTheSkin = pacManAtack;

            if (FindPathToPacMan(ref pacManPosX, ref pacManPosY))
                return;

            base.Update(pacManPosX, pacManPosY, pacManAtack);
        }
    }

    class Magenta : Ghost
    {
        public Magenta(ref int[,] _map) : base(ref _map)
        {
            PosX = 14; PosY = 14;
            AttackPower = 8;
        }

        public override void Draw()
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            base.Draw();
        }

        public override void FindDir()
        {
            for (int x = PosX + 1; x >= PosX - 1; x--)
            {
                for (int y = PosY + 1; y >= PosY - 1; y--)
                {
                    if (x == 0 && y == 14 || x == 27 && y == 14) continue;
                    if (x == PosX && y == PosY) continue;
                    if (x != PosX && y != PosY) continue;

                    if (ChangeDir(ref x, ref y))
                        return;
                }
            }
            visited.Clear();
        }

        public override void ResetAttack()
        {
            SaveTheSkin = false;
            Icon = 'W';
            AttackPower = 8;
        }

        public override void Update(int pacManPosX, int pacManPosY, bool pacManAtack)
        {
            SaveTheSkin = pacManAtack;

            if (FindPathToPacMan(ref pacManPosX, ref pacManPosY))
                return;

            switch (Dir)
            {
                case 1:
                    if (PosX + 1 == 27 && PosY == 14)
                    {
                        Dir = 2;
                    }
                    else if (Global.checkItemMap.Contains(map[PosY, PosX + 1]))
                    {
                        ClearTheTrack();
                        PosX++;
                        FindDir();
                    }
                    else
                        Dir = 2;
                    break;
                case 2:
                    if (PosX - 1 == 0 && PosY == 14)
                    {
                        Dir = 1;
                    }
                    else if (Global.checkItemMap.Contains(map[PosY, PosX - 1]))
                    {
                        ClearTheTrack();
                        PosX--;
                        FindDir();
                    }
                    else
                        Dir = 1;
                    break;
                case 3:
                    if (Global.checkItemMap.Contains(map[PosY + 1, PosX]))
                    {
                        ClearTheTrack();
                        PosY++;
                        FindDir();
                    }
                    else 
                        Dir = 4;
                    break;
                case 4:
                    if (Global.checkItemMap.Contains(map[PosY - 1, PosX]))
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
