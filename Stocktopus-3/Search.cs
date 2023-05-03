//
// Stocktopus 3 chess engine
//      by ZlomenyMesic
//

namespace Stocktopus_3 {
    internal static class Search {
        internal static Move SearchBestMove(Board board) {
            // this will start the search
            return null;
        }

        private static int Negamax(Board board, int depth, Color color, int alpha, int beta) {
            if (depth == 0 || IsTerminalNode(board)) {
                // TODO: SAVE TO TRANSPOSITION TABLE
                return Evaluation.Evaluate(board) * (color == Core.engineColor ? 1 : -1);
            }

            int eval = int.MinValue;
            Board[] children = Board.GetChildren(board, color);
            for (int i = 0; i < children.Length; i++) {
                eval = Math.Max(eval, -Negamax(children[i], depth - 1, (color == Color.White ? Color.Black : Color.White), -beta, -alpha));
                alpha = Math.Max(eval, alpha);
                if (alpha >= beta) break;

            }

            return eval;
        }

        private static bool IsTerminalNode(Board board) {
            return false;
        }
    }
}
