//
// Stocktopus 3 chess engine
//      by ZlomenyMesic
//

namespace Stocktopus_3 {
    internal static class Transpositions {
        private static Dictionary<(ulong, int), int> table = new();

        internal static void Add(Board board, int depth, int eval) {
            table[(Hashing.GetBoardHash(board), depth)] = eval;
        }

        internal static bool TryRetrieveEval(Board board, int depth, out int eval) {
            return table.TryGetValue((Hashing.GetBoardHash(board), depth), out eval);
        }

        internal static void Clear() {
            table.Clear();
        }
    }
}
