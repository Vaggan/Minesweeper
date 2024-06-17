namespace Minesweeper;

class Minesweeper
{
    static void Main()
    {
        var game = new Minefield();

        //set the bombs...
        game.SetBomb(new Location(0, 0));
        game.SetBomb(new Location(0, 1));
        game.SetBomb(new Location(1, 1));
        game.SetBomb(new Location(1, 4));
        game.SetBomb(new Location(4, 2));

        //the mine field should look like this now:
        //  01234
        //4|1X1
        //3|11111
        //2|2211X
        //1|XX111
        //0|X31

        // Game code...
        const string EXIT_STRING = "quit";
        var input = "";

        Console.WriteLine("Welcome to minesweeper!");
        Console.WriteLine("Enter location like '1 2' (type 'quit' to stop playing).");

        Console.WriteLine(game.ToString());

        while (input != null && !input.Equals(EXIT_STRING))
        {
            Console.Write("Enter coordinates:");
            input = Console.ReadLine() ?? "";
            if (input.Equals(EXIT_STRING))
                break;

            try
            {
                if (input != null)
                    game.MakeMove(input);
            }
            catch (Exception)
            {
                Console.WriteLine("Game Over! You hit a bomb");
                break;
            }

            if (game.OnlyBomsLeft())
            {
                Console.WriteLine("Congratulations, you won!");
                break;
            }

            Console.WriteLine(game.ToString());
        }

        Console.WriteLine("Thanks for playing!");
    }
}
