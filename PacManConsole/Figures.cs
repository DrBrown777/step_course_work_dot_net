namespace PacManConsole
{
    public enum Figures
    {
        StartPosition = 0,
        EmptySpace = -1,
        Destination = -2,
        Path = -3,
        Barrier = -4,
        Eat = -5,
        Bonus = -6
    }

    public static class Global
    {
        public static readonly int[] checkItemMap = new int[] { (int)Figures.EmptySpace, (int)Figures.Eat, (int)Figures.Bonus };
    }
}