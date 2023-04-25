﻿//
// Stocktopus 3 chess engine
//      by ZlomenyMesic 
//

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
            Color opposite = color == Color.White ? Color.Black : Color.White;

            // update the mailbox
            board.mailbox[move.end] = board.mailbox[move.start];
            board.mailbox[move.start] = new Piece(PieceType.None, Color.None);

            // update the en passant square
            if (move.piece == PieceType.Pawn && ((color == Color.White && move.start >= 48 && move.start <= 55 && move.end >= 32 && move.end <= 39) 
                || (color == Color.Black && move.start <= 15 && move.start >= 8 && move.end >= 24 && move.end <= 31)))
                board.enPassantSquare = (byte)(move.end + color == Color.White ? 8 : -8);
            else board.enPassantSquare = 0;

            ulong fromToBB = Constants.SquareMask[move.start] | Constants.SquareMask[move.end];
            board.bitboards[(int)color, (int)move.piece - 1] ^= fromToBB;
            board.occupied[(int)color] ^= fromToBB;
            board.empty ^= fromToBB;

            if (move.capture != PieceType.None) {
                // normal captures
                board.bitboards[(int)opposite, (int)move.capture - 1] ^= Constants.SquareMask[move.end];
                board.occupied[(int)opposite] ^= Constants.SquareMask[move.end];
            }

            if (move.promotion == PieceType.Pawn) {
                // en passant
                ulong enPassantBB = color == Color.White
                        ? Compass.South(Constants.SquareMask[move.end])
                        : Compass.North(Constants.SquareMask[move.end]);

                board.mailbox[move.end + (color == Color.White ? 8 : -8)] = new Piece(PieceType.None, Color.None);

                board.bitboards[(int)opposite, 0] ^= enPassantBB;
                board.occupied[(int)opposite] ^= enPassantBB;
                board.empty ^= enPassantBB;
            }

            if (move.promotion == PieceType.King) {
                // castling
                if (move.end == 2) PerformMove(board, new Move(0, 3, PieceType.Rook, 0, 0));
                else if (move.end == 6) PerformMove(board, new Move(7, 5, PieceType.Rook, 0, 0));
                else if (move.end == 58) PerformMove(board, new Move(56, 59, PieceType.Rook, 0, 0));
                else if (move.end == 62) PerformMove(board, new Move(63, 61, PieceType.Rook, 0, 0));
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
