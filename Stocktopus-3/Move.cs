//
// Stocktopus 3 chess engine
//      by ZlomenyMesic 
//

namespace Stocktopus_3 {
    internal class Move {
        internal int start;
        internal int end;

        // override promotion with PieceType.Pawn for en passant or with PieceType.King for castling

        internal PieceType piece;
        internal PieceType capture;
        internal PieceType promotion;

        internal Move(int start, int end, PieceType piece, PieceType capture, PieceType promotion) {
            this.start = start;
            this.end = end;

            this.piece = piece;
            this.capture = capture;
            this.promotion = promotion;
        }

        internal static bool IsCorrectFormat(string str) {
            // checks if the move from the user input makes sense
            // caution: don't try to understand this mess
            return str.Length >= 4 && Constants.FILES.Contains(str[0]) && char.IsDigit(str[1]) && Constants.FILES.Contains(str[2]) && char.IsDigit(str[3])
                && (str.Length == 4 || (str.Length == 5 && Constants.PIECES.Contains(str[4])));
        }

        internal static string ToString(Move move) {
            // converts a move back to a string, see the method below for more information
            string start = Constants.FILES[move.start % 8] + (8 - ((move.start - (move.start % 8)) / 8)).ToString();
            string end = Constants.FILES[move.end % 8] + (8 - ((move.end - (move.end % 8)) / 8)).ToString();
            string promotion = "";
            if (move.promotion != PieceType.None && move.promotion != PieceType.Pawn && move.promotion != PieceType.King)
                promotion = Constants.PIECES[(int)move.promotion - 1].ToString();

            return $"{start}{end}{promotion}";
        }

        internal static Move ToMove(Board board, string str) {
            // converts a string to a Move
            // The move in the string is stored using a form of Long Algebraic Notation (LAN),
            // which is used by UCI. There is no information about the piece moved, only the starting square
            // and the destination (e.g. "e2e4"), or an additional character for the promotion (e.g. "e7e8q").
            int start = ((8 - (str[1] - '0')) * 8) + Constants.FILES.IndexOf(str[0]);
            int end = ((8 - (str[3] - '0')) * 8) + Constants.FILES.IndexOf(str[2]);

            PieceType piece = board.mailbox[start].pieceType;
            PieceType capture = board.mailbox[end].pieceType;
            PieceType promotion = str.Length == 5 ? (PieceType)(Constants.PIECES.IndexOf(str[4]) + 1) : PieceType.None;

            // override promotion if castling or en passant
            if (piece == PieceType.King && (str == "e8c8" || str == "e8g8" || str == "e1c1" || str == "e1g1")) promotion = PieceType.King;
            if (piece == PieceType.Pawn && capture == PieceType.None && str[0] != str[2]) promotion = PieceType.Pawn;

            return new Move(start, end, piece, capture, promotion);
        }
    }
}
