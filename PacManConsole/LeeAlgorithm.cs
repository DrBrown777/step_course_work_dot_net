using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacManConsole
{
    class LeeAlgorithm
    {
        public int[,] ArrayGraph { get; private set; }
        public List<Tuple<int, int>> Path { get; private set; }
        public int Width { get; private set; }
        public int Heidth { get; private set; }
        public Tuple<int, int> PathFound { get; private set; }
        public int LengthPath { get { return Path.Count; } }

        private int _step;
        private bool _finishingCellMarked;
        private int _finishPointI;
        private int _finishPointJ;

        public LeeAlgorithm(int[,] array)
        {
            ArrayGraph = array;
            Width = ArrayGraph.GetLength(0);
            Heidth = ArrayGraph.GetLength(1);
            int startX;
            int startY;
            FindStartCell(out startX, out startY);
            SetStarCell(startX, startY);
            PathFound = PathSearch();

        }

        private void FindStartCell(out int startX, out int startY)
        {
            int w = Width;
            int h = Heidth;

            for (int i = 0; i < w; i++)
            {
                for (int j = 0; j < h; j++)
                {
                    if (ArrayGraph[i, j] == (int)Figures.StartPosition)
                    {
                        startX = i;
                        startY = j;
                        return;
                    }
                }
            }
            throw new AggregateException("Нет начальной точки");
        }

        private void SetStarCell(int startX, int startY)
        {
            if (startX > this.ArrayGraph.GetLength(0) || startX < 0)
                throw new ArgumentException("Неправильная координата x");
            if (startY > this.ArrayGraph.GetLength(1) || startY < 0)
                throw new ArgumentException("Неправильная координата y");
            _step = 0;
            ArrayGraph[startX, startY] = _step;
        }

        private Tuple<int, int> PathSearch()
        {
            if (WavePropagation())
                if (RestorePath())
                    return Path.ElementAt(Path.Count() - 2);
            return null;
        }

        private bool WavePropagation()
        {
            int w = Width;
            int h = Heidth;

            bool finished = false;
            do
            {
                for (int i = 0; i < w; i++)
                {
                    for (int j = 0; j < h; j++)
                    {
                        if (ArrayGraph[i, j] == _step)
                        {
                            if (i != w - 1)
                                if (ArrayGraph[i + 1, j] == (int)Figures.EmptySpace || ArrayGraph[i + 1, j] == (int)Figures.Eat) ArrayGraph[i + 1, j] = _step + 1;
                            if (j != h - 1)
                                if (ArrayGraph[i, j + 1] == (int)Figures.EmptySpace || ArrayGraph[i, j + 1] == (int)Figures.Eat) ArrayGraph[i, j + 1] = _step + 1;
                            if (i != 0)
                                if (ArrayGraph[i - 1, j] == (int)Figures.EmptySpace || ArrayGraph[i - 1, j] == (int)Figures.Eat) ArrayGraph[i - 1, j] = _step + 1;
                            if (j != 0)
                                if (ArrayGraph[i, j - 1] == (int)Figures.EmptySpace || ArrayGraph[i - 1, j] == (int)Figures.Eat) ArrayGraph[i, j - 1] = _step + 1;

                            if (i < w - 1)
                                if (ArrayGraph[i + 1, j] == (int)Figures.Destination)
                                {
                                    _finishPointI = i + 1;
                                    _finishPointJ = j;
                                    finished = true;
                                }
                            if (j < h - 1)
                                if (ArrayGraph[i, j + 1] == (int)Figures.Destination)
                                {
                                    _finishPointI = i;
                                    _finishPointJ = j + 1;
                                    finished = true;
                                }
                            if (i > 0)
                                if (ArrayGraph[i - 1, j] == (int)Figures.Destination)
                                {
                                    _finishPointI = i - 1;
                                    _finishPointJ = j;
                                    finished = true;
                                }
                            if (j > 0)
                                if (ArrayGraph[i, j - 1] == (int)Figures.Destination)
                                {
                                    _finishPointI = i;
                                    _finishPointJ = j - 1;
                                    finished = true;
                                }
                        }

                    }
                }
                _step++;
            } while (!finished && _step < w * h);
            _finishingCellMarked = finished;
            return finished;
        }

        private bool RestorePath()
        {
            if (!_finishingCellMarked)
                return false;

            int w = Width;
            int h = Heidth;
            int i = _finishPointI;
            int j = _finishPointJ;
            Path = new List<Tuple<int, int>>();
            AddToPath(i, j);

            do
            {
                if (i < w - 1)
                    if (ArrayGraph[i + 1, j] == _step - 1)
                    {
                        AddToPath(++i, j);
                    }
                if (j < h - 1)
                    if (ArrayGraph[i, j + 1] == _step - 1)
                    {
                        AddToPath(i, ++j);
                    }
                if (i > 0)
                    if (ArrayGraph[i - 1, j] == _step - 1)
                    {
                        AddToPath(--i, j);
                    }
                if (j > 0)
                    if (ArrayGraph[i, j - 1] == _step - 1)
                    {
                        AddToPath(i, --j);
                    }
                _step--;
            } while (_step != 0);
            return true;
        }

        private void AddToPath(int x, int y)
        {
            Path.Add(new Tuple<int, int>(x, y));
        }
    }
}
