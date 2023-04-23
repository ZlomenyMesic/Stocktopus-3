using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stocktopus_3 {
    internal class Board {
        internal ulong[,] bitboards = new ulong[2,6];
        internal Piece[] mailbox = new Piece[64];

        internal byte castlingFlags;
        internal byte enPassantSquare;

        internal Color sideToMove;

        internal Board() {
            Array.Fill(mailbox, new Piece(PieceType.None, Color.None));
        }

        internal static void PerformMove(Board board, Move move) {

        }

        internal static void Print(Board board) {
            for (int i = 0; i < 64; i++) {
                char square = '-';
                switch (board.mailbox[i].pieceType) {
                    case PieceType.Pawn: square = board.mailbox[i].color == Color.White ? 'P' : 'p'; break;
                    case PieceType.Knight: square = board.mailbox[i].color == Color.White ? 'N' : 'n'; break;
                    case PieceType.Bishop: square = board.mailbox[i].color == Color.White ? 'B' : 'b'; break;
                    case PieceType.Rook: square = board.mailbox[i].color == Color.White ? 'R' : 'r'; break;
                    case PieceType.Queen: square = board.mailbox[i].color == Color.White ? 'Q' : 'q'; break;
                    case PieceType.King: square = board.mailbox[i].color == Color.White ? 'K' : 'k'; break;
                }
                Console.Write($"{square} ");
                if ((i + 1) % 8 == 0 && i != 0) Console.WriteLine();
            }
        }
    }
}
