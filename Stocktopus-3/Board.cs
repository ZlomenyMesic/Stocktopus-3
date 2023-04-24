//
// Stocktopus 3 chess engine
//      by ZlomenyMesic 
//

using System.Drawing;
using System.Transactions;

namespace Stocktopus_3 {
    internal class Board {
        internal ulong[,] bitboards = new ulong[2, 6];
        internal ulong[] occupied = new ulong[2];
        internal ulong empty = 0;

        internal Piece[] mailbox = new Piece[64];

        internal byte castlingFlags;
        internal byte enPassantSquare;

        internal Color sideToMove;

        internal Board() {
            Array.Fill(mailbox, new Piece(PieceType.None, Color.None));
        }

        internal static void PerformMove(Board board, Move move) {
            Color color = board.mailbox[move.start].color;

            // update the mailbox
            board.mailbox[move.end] = board.mailbox[move.start];
            board.mailbox[move.start] = new Piece(PieceType.None, Color.None);

            // update the en passant square
            if (move.piece == PieceType.Pawn && ((color == Color.White && move.start >= 48 && move.start <= 55 && move.end >= 32 && move.end <= 39) 
                || (color == Color.Black && move.start <= 15 && move.start >= 8 && move.end >= 24 && move.end <= 31)))
                board.enPassantSquare = (byte)move.end;
            else board.enPassantSquare = 0;

            ulong fromToBB = Constants.SquareMask[move.start] | Constants.SquareMask[move.end];
            board.bitboards[(int)board.mailbox[move.end].color, (int)move.piece] ^= fromToBB;
            board.occupied[(int)board.mailbox[move.end].color] ^= fromToBB;
            board.empty ^= fromToBB;

            if (move.capture != PieceType.None) {
                if (move.promotion == PieceType.Pawn) {
                    // en passant
                } else {
                    // other capture
                }
            }
        }

        internal static void UpdateBitboards(Board board) {
            // this is a really slow way of updating the bitboards, so it's used only when setting a new position.
            // first clear the bitboards
            board.occupied[0] = 0;
            board.occupied[1] = 0;
            for (int i = 0; i < 6; i++) {
                board.bitboards[0, i] = 0;
                board.bitboards[1, i] = 0;
            }
            // add pieces from the mailbox
            for (int i = 0; i < 64; i++) {
                if (board.mailbox[i].pieceType != PieceType.None) {
                    board.bitboards[(int)board.mailbox[i].color, (int)board.mailbox[i].pieceType - 1] |= Constants.SquareMask[i];
                    board.occupied[(int)board.mailbox[i].color] |= Constants.SquareMask[i];
                }
            }
            // update empty squares
            board.empty = board.occupied[0] | board.occupied[1];
        }

        internal static void Print(Board board) {
            for (int i = 0; i < 64; i++) {
                char square = board.mailbox[i].pieceType != PieceType.None 
                    ? Constants.PIECES[(int)board.mailbox[i].pieceType - 1] : '-';

                Console.Write($"{(board.mailbox[i].color == Color.Black ? square : char.ToUpper(square))} ");
                if ((i + 1) % 8 == 0 && i != 0) Console.WriteLine();
            }
        }
    }
}
