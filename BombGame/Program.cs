using BombGame.Model;
using BombGame.Persistence;

namespace BombGame
{
    internal class Program
    {
        static void Main(string[] args)
        {
            BombModel m = new BombModel(new TextFilePersistence());

            m.LoadGame("Input\\inp.txt");
            //m.MoveEnemy(1, 0);
            m.ReLoad();
            Console.WriteLine();
            m.ReLoad();
            Console.WriteLine();
            m.ReLoad();
        }
    }
}