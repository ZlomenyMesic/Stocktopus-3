//
// Stocktopus 3 chess engine
//      by ZlomenyMesic 
//

namespace Stocktopus_3 {
    class UCI {
        static void Main(string[] args) {
            for (; ; ) {
                // Dereference of a possibly null reference.
                // literally nobody asked
                string[] tokens = Console.ReadLine().Split(" ");
                RunCommand(tokens);
            }
        }

        static void RunCommand(string[] tokens) {
            switch (tokens[0]) {
                case "uci": Console.WriteLine(Constants.ENGINE_INFO); break;
                case "isready": Console.WriteLine("readyok"); break;
                case "position": Core.SetPosition(tokens); break;
                case "go": Console.WriteLine("bestmove e7e5"); break;
                case "setoption": Core.SetOption(tokens); break;
                default: Console.WriteLine($"unknown command: {tokens[0]}"); break;
            }
        }
    }
}