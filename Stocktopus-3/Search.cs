//
// Stocktopus 3 chess engine
//      by ZlomenyMesic
//

namespace Stocktopus_3 {
    internal static class Search {
        internal static int depth = 4;
        internal static int nodes = 0;
        internal static int transpositions = 0;

        internal static Move SearchBestMove(Board board) {
            // this starts the search
            Move[] moves = Movegen.GetLegalMoves(board, Core.engineColor);
            Move[] bestMoves = new Move[64];
            int bestEval = int.MinValue;
            int moveCount = 0;

            for (int i = 0; i < moves.Length; i++) {
                Board temp = Board.Clone(board);
                Board.PerformMove(temp, moves[i]);

                int eval = Negamax(temp, depth - 1, Core.playerColor, 0, 0);
                if (eval > bestEval) {
                    moveCount = 0;
                    bestEval = eval;
                    bestMoves[moveCount++] = moves[i];
                } else if (eval == bestEval) bestMoves[moveCount++] = moves[i];

                Console.WriteLine($"{Move.ToString(moves[i])} {eval}");
            }

            return bestMoves[new Random().Next(0, moveCount)];
        }

        private static int Negamax(Board board, int depth, Color color, int alpha, int beta) {
            if (depth == 0 || IsDraw(board)) {
                // TODO: SAVE TO TRANSPOSITION TABLE
                nodes++;
                return 0;
                return Evaluation.Evaluate(board);
            }

            int eval = int.MinValue;
            Board[] children = Board.GetChildren(board, color);

            // checkmate
            if (!Board.IsCheck(board, color) && children.Length == 0) return depth * 1000 * (color == Core.engineColor ? -1 : 1);
            // stalemate
            //else if (children.Length == 0) return 420 * (color == Core.engineColor ? 1 : -1);

            for (int i = 0; i < children.Length; i++) {
                eval = Math.Max(eval, -Negamax(children[i], depth - 1, (color == Color.White ? Color.Black : Color.White), -beta, -alpha));
                //alpha = Math.Max(eval, alpha);
                //if (alpha >= beta) break;
            }

            return eval;
        }

        private static bool IsDraw(Board board) {
            return false;
        }
    }
}
