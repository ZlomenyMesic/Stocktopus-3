//
// Stocktopus 3 chess engine
//      by ZlomenyMesic 
//

namespace Stocktopus_3 {
    internal static class Evaluation {
        internal static int Evaluate(Board board) {
            int eval = 0;
            for (int i = 0; i < 64; i++) {
                if (board.mailbox[i].pieceType != PieceType.None)
                    eval += Constants.pieceValues[(int)board.mailbox[i].pieceType - 1] * (board.mailbox[i].color == Core.engineColor ? 1 : -1);
            }
            return eval;
        }
    }
}
