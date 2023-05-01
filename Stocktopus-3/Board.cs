//
// Stocktopus 3 chess engine
//      by ZlomenyMesic 
//

using System.Transactions;

namespace Stocktopus_3 {
    internal class Board {
        internal ulong[,] bitboards = new ulong[2, 6];
        internal ulong[] occupied = new ulong[2];
        internal ulong empty = 0;

        internal Piece[] mailbox = new Piece[64];

        internal byte castlingFlags = 0x0F;
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



            // position startpos moves g2g3 c7c5 f2f3 g7g5 h2h3 g8f6 g3g4 f8h6 b1a3 f6d5 a3b1 d8a5 e2e3 e8d8 e3e4
            board.bitboards[(int)color, (int)move.piece - 1] ^= fromToBB; // FIXME




            board.occupied[(int)color] ^= fromToBB;
            //board.empty |= fromToBB;

            if (move.capture != PieceType.None) {
                // normal captures
                board.bitboards[(int)opposite, (int)move.capture - 1] ^= Constants.SquareMask[move.end];
                board.occupied[(int)opposite] ^= Constants.SquareMask[move.end];
                board.empty ^= Constants.SquareMask[move.start];
            } else board.empty ^= fromToBB;

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

            else if (move.promotion == PieceType.King) {
                // castling
                if (move.end == 2) PerformMove(board, new Move(0, 3, PieceType.Rook, 0, 0));
                else if (move.end == 6) PerformMove(board, new Move(7, 5, PieceType.Rook, 0, 0));
                else if (move.end == 58) PerformMove(board, new Move(56, 59, PieceType.Rook, 0, 0));
                else if (move.end == 62) PerformMove(board, new Move(63, 61, PieceType.Rook, 0, 0));
            }

            else if (move.promotion != PieceType.None) {
                // promotions
                board.mailbox[move.end].pieceType = move.promotion;
                board.bitboards[(int)color, 0] ^= Constants.SquareMask[move.end];
                board.bitboards[(int)color, (int)move.promotion - 1] ^= Constants.SquareMask[move.end];
            }
        }

        internal static void UpdateBitboards(Board board) {
            // this is a very slow way of updating the bitboards, so it's only used when setting a new position
            board.empty = 0;
            board.occupied[0] = 0;
            board.occupied[1] = 0;
            for (int i = 0; i < 6; i++) {
                board.bitboards[0, i] = 0;
                board.bitboards[1, i] = 0;
            }
            for (int i = 0; i < 64; i++) {
                if (board.mailbox[i].pieceType != PieceType.None) {
                    board.bitboards[(int)board.mailbox[i].color, (int)board.mailbox[i].pieceType - 1] |= Constants.SquareMask[i];
                    board.occupied[(int)board.mailbox[i].color] |= Constants.SquareMask[i];
                }
            }
            board.empty = ~(board.occupied[0] | board.occupied[1]);
        }

        internal static bool IsMoveLegal(Board board, Move move) {
            Board temp = Clone(board);
            PerformMove(temp, move);
            return !IsCheck(temp, temp.mailbox[move.end].color);
        }

        internal static bool IsCheck(Board board, Color kingColor) {
            // instead of generating all possible opponent's moves, it's faster to generate moves from the king
            Move[] moves = new Move[64];
            int i = 0, j = 0;

            Movegen.GetKingMoves(board, kingColor, moves, ref i);
            for (int k = j; k < i; k++, j++)
                if (moves[k].capture == PieceType.King || moves[k].capture == PieceType.Queen) return true;

            Movegen.GetPawnMoves(board, kingColor, moves, ref i, true);
            for (int k = j; k < i; k++, j++)
                if (moves[k].capture == PieceType.Pawn) return true;

            Movegen.GetKnightMoves(board, kingColor, moves, ref i, true);
            for (int k = j; k < i; k++, j++)
                if (moves[k].capture == PieceType.Knight) return true;

            Movegen.GetRookMoves(board, kingColor, moves, ref i, true);
            for (int k = j; k < i; k++, j++)
                if (moves[k].capture == PieceType.Rook || moves[k].capture == PieceType.Queen) return true;

            Movegen.GetBishopMoves(board, kingColor, moves, ref i, true);
            for (int k = j; k < i; k++, j++)
                if (moves[k].capture == PieceType.Bishop || moves[k].capture == PieceType.Queen) return true;

            return false;
        }

        internal static Board Clone(Board board) {
            Board temp = new();
            for (int k = 0; k < 6; k++) {
                temp.bitboards[0, k] = board.bitboards[0, k];
                temp.bitboards[1, k] = board.bitboards[1, k];
            }

            for (int k = 0; k < 64; k++)
                temp.mailbox[k] = new Piece(board.mailbox[k].pieceType, board.mailbox[k].color);

            temp.empty = board.empty;
            temp.occupied[0] = board.occupied[0];
            temp.occupied[1] = board.occupied[1];

            temp.castlingFlags = board.castlingFlags;
            temp.enPassantSquare = board.enPassantSquare;

            return temp;
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
