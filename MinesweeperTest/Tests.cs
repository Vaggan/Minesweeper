namespace MinesweeperTest;

using Minesweeper;

[TestClass]
public class Tests
{
    private Minefield minefield = new Minefield();
    private Location bomb1 = new Location(0, 0);
    private Location bomb2 = new Location(1, 1);
    private Location bomb3 = new Location(1, 4);
    private Location bomb4 = new Location(4, 2);

    private const string BOMB_LOCATION = "1 1";
    private const string NOT_BOMB_STRING = "2 2";
    private readonly Location NOT_BOMB_LOCATION = new Location(2, 2);

    [TestInitialize]
    public void Setup()
    {
        minefield = new Minefield();
        minefield.SetBomb(bomb1);
        minefield.SetBomb(bomb2);
        minefield.SetBomb(bomb3);
        minefield.SetBomb(bomb4);
    }

    [TestMethod]
    public void TestInitialDimensions()
    {
        Assert.IsNotNull(minefield.FieldDimensions);
        Assert.AreEqual(5, minefield.FieldDimensions.X, "Default X dimension is 5");
        Assert.AreEqual(5, minefield.FieldDimensions.Y, "Default Y dimension is 5");
    }

    [TestMethod]
    public void TestCustomDimensions()
    {
        minefield = new Minefield(7, 7);
        Assert.IsNotNull(minefield.FieldDimensions);
        Assert.AreEqual(7, minefield.FieldDimensions.X, "Default X dimension is 5");
        Assert.AreEqual(7, minefield.FieldDimensions.Y, "Default Y dimension is 5");
    }

    [TestMethod]
    public void TestSetBomb()
    {
        Dictionary<string, Field> bombs = minefield.GetBombs();

        Assert.AreEqual(4, bombs.Count, "There should be 4 bombs as default");
        Assert.IsTrue(bombs.ContainsKey(bomb1.ToString()));
        Assert.IsTrue(bombs.ContainsKey(bomb2.ToString()));
        Assert.IsTrue(bombs.ContainsKey(bomb3.ToString()));
        Assert.IsTrue(bombs.ContainsKey(bomb4.ToString()));
    }

    [TestMethod]
    public void TestSetBombAndNeighbourFields()
    {
        const int BOMB_LOCATION = 3;
        minefield = new Minefield();

        var bomb = new Location(BOMB_LOCATION, BOMB_LOCATION);
        minefield.SetBomb(bomb);

        Dictionary<string, Field> bombs = minefield.GetBombs();

        Assert.AreEqual(1, bombs.Count, "There should be 1 bombs");
        Assert.IsTrue(bombs.ContainsKey(bomb.ToString()));

        Dictionary<string, Field> fields = minefield.Fields;
        Location NorthWest = bomb.Northwest;
        Assert.AreEqual(1, fields[NorthWest.ToString()].NeighbouringBombs, "North west firld should have one neighbour");
    }

    [TestMethod]
    public void TestSetBombAndNeighbourFieldsWithTwoBombs()
    {
        const int BOMB_LOCATION_1 = 3;
        const int BOMB_LOCATION_2 = 4;
        minefield = new Minefield();

        var bomb1 = new Location(BOMB_LOCATION_1, BOMB_LOCATION_1);
        var bomb2 = new Location(BOMB_LOCATION_2, BOMB_LOCATION_2);
        minefield.SetBomb(bomb1);
        minefield.SetBomb(bomb2);

        Dictionary<string, Field> bombs = minefield.GetBombs();

        Assert.AreEqual(2, bombs.Count, "There should be 2 bombs");
        Assert.IsTrue(bombs.ContainsKey(bomb1.ToString()));
        Assert.IsTrue(bombs.ContainsKey(bomb2.ToString()));

        Dictionary<string, Field> fields = minefield.Fields;
        Location North = bomb1.North;
        Assert.AreEqual(2, fields[North.ToString()].NeighbouringBombs, "North firld should have two neighbour bombs");
    }

    [TestMethod]
    public void TestSetBombNoNeighbourOutsideGrid()
    {
        const int BOMB_LOCATION = 4;
        minefield = new Minefield();

        var bomb = new Location(BOMB_LOCATION, BOMB_LOCATION);
        minefield.SetBomb(bomb);

        Dictionary<string, Field> bombs = minefield.GetBombs();
        Assert.AreEqual(1, bombs.Count, "There should be 1 bombs");
        Assert.IsTrue(bombs.ContainsKey(bomb.ToString()));

        Dictionary<string, Field> fields = minefield.Fields;
        Assert.AreEqual(4, fields.Count, "There should not be any fields outside the grid");
    }

    [TestMethod]
    public void TestFieldValue()
    {
        var location = new Location(0, 0);

        Field field1 = new Field(location);
        Assert.AreEqual("?", field1.Value, "A new field should always show '?'");

        Field field2 = new Field(location) { IsBomb = true };
        Assert.AreEqual("?", field2.Value, "A new field should always show '?' even if it has a bomb");

        Field field3 = new Field(location) { NeighbouringBombs = 1 };
        Assert.AreEqual("?", field3.Value, "A new field should always show '?' even if it has neighbouting bombs");

        Field field4 = new Field(location) { IsBomb = true, NeighbouringBombs = 1 };
        Assert.AreEqual("?", field4.Value, "A new field should always show '?' even if it has a bomb and has neighbouting bombs");

        Field field5 = new Field(location) { IsVisited = true };
        Assert.AreEqual(" ", field5.Value, "A new field should always show '{empty}' if it is visited");

        Field field6 = new Field(location) { IsVisited = true, IsBomb = true };
        Assert.AreEqual("X", field6.Value, "A visited bomb field should always show 'X'");

        Field field7 = new Field(location) { IsVisited = true, NeighbouringBombs = 1 };
        Assert.AreEqual("1", field7.Value, "A visitedd field that has has neighbouting bombs should show correct neighbours");

        Field field8 = new Field(location) { IsVisited = true, NeighbouringBombs = 4 };
        Assert.AreEqual("4", field8.Value, "A visitedd field that has has neighbouting bombs should show correct neighbours");

        Field field9 = new Field(location) { IsVisited = true, IsBomb = true, NeighbouringBombs = 1 };
        Assert.AreEqual("X", field9.Value, "A field that has a bomb and has neighbouting bombs shold shov 'X'");
    }

    [TestMethod]
    public void TestMakeMove()
    {
        minefield.MakeMove(NOT_BOMB_STRING);

        var field = minefield.Fields[NOT_BOMB_LOCATION.ToString()];
        Assert.IsTrue(field.IsVisited);
    }

    [TestMethod]
    public void TestMakeMoveOnABomb()
    {
        Assert.ThrowsException<GameOverException>(() => minefield.MakeMove(BOMB_LOCATION));
    }

    [TestMethod]
    public void TestIsDimensionsOk()
    {
        var location1 = new Location(1, 1);
        Assert.IsTrue(minefield.IsDimensionsOk(location1), "Location should be inside minefield dimensions");

        var location2 = new Location(5, 5);
        Assert.IsFalse(minefield.IsDimensionsOk(location2), "Location should be inside minefield dimensions");
    }

    [TestMethod]
    public void TestParseInput()
    {
        var location1 = minefield.ParseInput("1 1");
        Assert.IsNotNull(location1, "Location should be inside minefield dimensions");

        var location2 = minefield.ParseInput("11");
        Assert.IsNull(location2, "Input must be in format \"x y\"");

        var location3 = minefield.ParseInput("1,1");
        Assert.IsNull(location3, "Input must be in format \"x y\"");

        var location4 = minefield.ParseInput("5 9");
        Assert.IsNull(location4, "Input must be in format \"x y\"");
    }
}
