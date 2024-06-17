using System.Text.RegularExpressions;

namespace Minesweeper;

internal class Minefield
{
    private const int DEFAULT_SiZE = 5;

    internal Dictionary<string, Field> Fields { get; set; }
    internal Location FieldDimensions { get; set; }

    internal Minefield() : this(DEFAULT_SiZE, DEFAULT_SiZE) { }

    internal Minefield(int x, int y)
    {
        this.FieldDimensions = new Location(x, y);
        this.Fields = new Dictionary<string, Field>();
    }

    internal Dictionary<string, Field> GetBombs()
    {
        return Fields
            .Where(f => f.Value.IsBomb)
            .ToDictionary(f => f.Key, f => f.Value);
    }

    internal void MakeMove(string input)
    {
        var move = ParseInput(input);
        if (move == null)
        {
            Console.WriteLine("Incorrect input, try again.");
            return;
        }

        var field = GetFieldByLocation(move);
        if (field != null)
        {
            if (field.IsBomb)
                throw new GameOverException();

            field.IsVisited = true;
            if (field.Value.Equals(" "))
                ShowNeighbours(move);
        }
    }

    private void ShowNeighbours(Location move)
    {
        var neighbours = new List<Field>();
        AddIfNotNull(move.East, neighbours);
        AddIfNotNull(move.West, neighbours);
        AddIfNotNull(move.South, neighbours);
        AddIfNotNull(move.North, neighbours);
        AddIfNotNull(move.Northeast, neighbours);
        AddIfNotNull(move.Southeast, neighbours);
        AddIfNotNull(move.Northwest, neighbours);
        AddIfNotNull(move.Southwest, neighbours);

        neighbours.FindAll(x => x != null && !x.IsVisited).ForEach(n =>
        {
            n.IsVisited = !n.IsBomb;
            if (n.Value.Equals(" "))
                ShowNeighbours(n.Location);
        });
    }

    private void AddIfNotNull(Location location, List<Field> list)
    {
        var field = GetFieldByLocation(location);
        if (field != null)
            list.Add(field);
    }

    internal bool OnlyBomsLeft()
    {
        return GetBombs().Count == Fields.Where(f => f.Value.IsVisited == false).Count();
    }

    internal Location? ParseInput(string input)
    {
        var rg = new Regex(@"^\d \d$");
        if (rg.IsMatch(input))
        {
            var coordinates = input.Split(" ");
            var location = new Location(Int32.Parse(coordinates[0]), Int32.Parse(coordinates[1]));
            if (IsDimensionsOk(location))
                return location;
        }

        return null;
    }

    internal void SetBomb(Location location)
    {
        var isBombAdded = AddBombTo(location);
        if (!isBombAdded)
            return;

        AddNeighbourBomdTo(location.East);
        AddNeighbourBomdTo(location.West);
        AddNeighbourBomdTo(location.South);
        AddNeighbourBomdTo(location.North);
        AddNeighbourBomdTo(location.Northeast);
        AddNeighbourBomdTo(location.Southeast);
        AddNeighbourBomdTo(location.Northwest);
        AddNeighbourBomdTo(location.Southwest);
    }

    private bool AddBombTo(Location location)
    {
        var bombField = GetFieldByLocation(location);
        if (bombField == null)
            return false;

        bombField.IsBomb = true;
        return true;
    }

    private void AddNeighbourBomdTo(Location location)
    {
        var field = GetFieldByLocation(location);
        if (field != null)
            field.NeighbouringBombs++;
    }

    private Field? GetFieldByLocation(Location location)
    {
        if (!IsDimensionsOk(location))
            return null;

        if (!Fields.ContainsKey(location.ToString()))
            Fields.Add(location.ToString(), new Field(location));

        return Fields[location.ToString()];
    }

    internal bool IsDimensionsOk(Location location)
    {
        return location.X >= 0 &&
               location.X < FieldDimensions.X &&
               location.Y >= 0 &&
               location.Y < FieldDimensions.Y;
    }

    public override string ToString()
    {
        var rows = new List<string>();
        for (int y = 0; y < FieldDimensions.Y; y++)
        {
            var row = "";
            for (int x = 0; x < FieldDimensions.X; x++)
            {
                var field = GetFieldByLocation(new Location(x, y));
                row += field?.Value ?? "?";
            }
            rows.Add(row);
        }

        string multilineString = $@"
              01234
            4|{rows[4]}
            3|{rows[3]}
            2|{rows[2]}
            1|{rows[1]}
            0|{rows[0]}
            ";

        return multilineString;
    }
}