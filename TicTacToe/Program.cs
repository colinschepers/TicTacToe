using Core;
using Core.Games;
using GameLogic;
using Players;

namespace UltimateTicTacToe
{
    /// <summary>
    /// A Console Application as user interface! XD
    /// TODO: Create GUI
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                var engine = new GameEngine(CreateGame());
                engine.Run();
            }
        }

        private static IGame CreateGame()
        {
            var state = new State();
            var players = new IPlayer[]
            {
                new MiniMaxPlayer(),
                new MCTSPlayer(0.4, 100)
            };
            return new Game(state, players);
        }
    }
}
