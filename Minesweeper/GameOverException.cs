namespace Minesweeper
{
    public class GameOverException : Exception
    {
        public GameOverException() : base("Game over")
        {
        }
    }
}