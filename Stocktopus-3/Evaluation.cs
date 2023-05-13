//
// Stocktopus 3 chess engine
//      by ZlomenyMesic 
//

namespace Stocktopus_3 {
    internal static class Evaluation {
        internal static int Evaluate(Board board) {
            // used to evaluate a position, see GetTableValue() for more info
            int eval = 0;
            for (int i = 0; i < 64; i++) {
                if (board.mailbox[i].pieceType != PieceType.None)
                    eval += GetTableValue(board.mailbox[i], i, board.numberOfPieces) * (board.mailbox[i].color == Core.engineColor ? 1 : -1);
            }
            return eval;
        }

        private static int GetTableValue(Piece piece, int pos, int numberOfPieces) {
            // this method uses the value tables in Constants.cs, and is used to evaluate a piece position
            // there are two tables - midgame and endgame, this is important, because the pieces should be
            // in different positions as the game progresses (e.g. a king in the midgame should be in the corner,
            // but in the endgame in the center)

            pos = piece.color == Color.White ? pos + 1 : 64 - pos;
            int mgValue = Constants.MidgameTables[((int)piece.pieceType * 64) - pos];
            int egValue = Constants.EndgameTables[((int)piece.pieceType * 64) - pos];

            // TODO: Gradual evaluation

            //return numberOfPieces > 24 ? mgValue : ((mgValue - egValue) / 12) * (24 - numberOfPieces);

            return numberOfPieces > 16 ? mgValue : egValue;
        }

        internal static int BasicEval(Board board) {
            // a simpler version of the Evaluate() method
            // this version only evaluates with the simple piece values:
            // pawn = 1, knight/bishop = 3, rook = 5, queen = 9

            int eval = 0;
            for (int i = 0; i < 64; i++)
                if (board.mailbox[i].pieceType != PieceType.None)
                    eval += Constants.pieceValues[(int)board.mailbox[i].pieceType - 1] * (board.mailbox[i].color == Core.engineColor ? 1 : -1);
            return eval;
        }

        internal static Board[] SortBoardChildren(Board[] children, Color color) {
            // this method is used to speed up the alpha-beta pruning during the search.
            // it takes an array of subsequent nodes of a position, and sorts them based
            // on their very basic evaluation.
            (Board, int)[] toSort = new (Board, int)[children.Length];

            for (int i = 0; i < children.Length; i++) {
                int eval = BasicEval(children[i]);
                toSort[i] = (children[i], eval);
            }

            // a simple bubble sort
            while (true) {
                bool wereSortsMade = false;
                for (int i = 0; i < toSort.Length - 1; i++) {
                    if ((color == Core.engineColor && toSort[i].Item2 < toSort[i + 1].Item2) || (color != Core.engineColor && toSort[i].Item2 > toSort[i + 1].Item2)) {
                        (toSort[i], toSort[i + 1]) = (toSort[i + 1], toSort[i]);
                        wereSortsMade = true;
                    }
                }
                if (!wereSortsMade) break;
            }

            Board[] sorted = new Board[children.Length];
            for (int i = 0; i < children.Length; i++)
                sorted[i] = toSort[i].Item1;

            return sorted;
        }
    }
}