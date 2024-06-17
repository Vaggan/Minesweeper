namespace Minesweeper
{
    internal class Location
    {
        internal int X { get; }
        internal int Y { get; }

        internal Location() : this(5, 5) { }

        internal Location(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public override string ToString()
        {
            return String.Format("{0},{1}", X, Y);
        }

        internal Location East => new Location(X + 1, Y);

        internal Location West => new Location(X - 1, Y);

        internal Location South => new Location(X, Y - 1);

        internal Location North => new Location(X, Y + 1);

        internal Location Northeast => new Location(X + 1, Y + 1);

        internal Location Southeast => new Location(X + 1, Y - 1);

        internal Location Northwest => new Location(X - 1, Y + 1);

        internal Location Southwest => new Location(X - 1, Y - 1);
    }

}