namespace Minesweeper
{
    internal class Field
    {
        internal Field(Location location)
        {
            Location = location;
        }

        internal int NeighbouringBombs { get; set; } = 0;
        internal bool IsVisited { get; set; } = false;
        internal bool IsBomb { get; set; } = false;
        internal Location Location { get; }
        internal string Value
        {
            get
            {
                if (IsVisited && IsBomb)
                    return "X";
                if (IsVisited && NeighbouringBombs > 0)
                    return NeighbouringBombs.ToString();
                if (IsVisited && NeighbouringBombs == 0)
                    return " ";

                return "?";
            }
        }

    }
}