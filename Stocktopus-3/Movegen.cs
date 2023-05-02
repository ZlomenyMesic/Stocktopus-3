//
// Stocktopus 3 chess engine
//      by ZlomenyMesic 
//

namespace Stocktopus_3 {
    internal static class Movegen {
        internal static void GetPseudoLegalMoves(Board board, Color color, Move[] moves, ref int i) {
            GetPawnMoves(board, color, moves, ref i);
            GetKnightMoves(board, color, moves, ref i);
            GetBishopMoves(board, color, moves, ref i);
            GetRookMoves(board, color, moves, ref i);
            GetQueenMoves(board, color, moves, ref i);
            GetKingMoves(board, color, moves, ref i);
            GetCastlingMoves(board, color, moves, ref i);
        }

        internal static Move[] GetLegalMoves(Board board, Color color) {
            // the theoretical number of possible moves in a single position is higher, but probably unreachable
            Move[] pseudoLegal = new Move[200];
            Move[] legal = new Move[200];

            int pseudoLegalCount = 0, legalCount = 0;
            GetPseudoLegalMoves(board, color, pseudoLegal, ref pseudoLegalCount);

            // legality check
            for (int i = 0; i < pseudoLegalCount; i++) {
                if (Board.IsMoveLegal(board, pseudoLegal[i])) legal[legalCount++] = pseudoLegal[i];
            }

            // copy the legal moves to a new array to avoid empty moves
            Move[] finalLegal = new Move[legalCount];
            for (int i = 0; i < legalCount; i++) {
                finalLegal[i] = legal[i];
            }

            return finalLegal;
        }

        internal static void GetCastlingMoves(Board board, Color color, Move[] moves, ref int i) {
            if (color == Color.White) {
                if (board.castling[0] == true && (~board.empty & Constants.OO_WHITE_MASK) == 0)
                    moves[i++] = new Move(60, 62, PieceType.King, 0, PieceType.King);
                if (board.castling[1] == true && (~board.empty & Constants.OOO_WHITE_MASK) == 0)
                    moves[i++] = new Move(60, 58, PieceType.King, 0, PieceType.King);
            } else {
                if (board.castling[2] == true && (~board.empty & Constants.OO_BLACK_MASK) == 0)
                    moves[i++] = new Move(4, 6, PieceType.King, 0, PieceType.King);
                if (board.castling[3] == true && (~board.empty & Constants.OOO_BLACK_MASK) == 0)
                    moves[i++] = new Move(4, 2, PieceType.King, 0, PieceType.King);
            }
        }

        // this pattern applies to all following methods:
        // First a copy of the piece bitboard is created. Then the bits are removed one by one from the copy
        // until it's empty. Each of the removed bits represent a piece, and for every piece a bitboard of
        // targets is generated (which contains all possible move destinations for that piece). Then, the bits
        // are removed from the targets bitboard and for each bit a move is generated.

        internal static void GetPawnMoves(Board board, Color color, Move[] moves, ref int i, bool kingCheck = false) {
            ulong targets; int start; int end;
            ulong pawns = board.bitboards[(int)color, kingCheck ? 5 : 0];

            while (pawns != 0) {
                start = Bitboard.BitScanForwardReset(ref pawns);
                targets = Targets.GetPawnTargets(Constants.SquareMask[start], board, color);

                if (kingCheck == false && board.enPassantSquare != 0) {
                    int difference = ((start - (start % 8)) / 8) - ((board.enPassantSquare - (board.enPassantSquare % 8)) / 8);
                    if (difference == 0 && start + 1 == board.enPassantSquare)
                        moves[i++] = new Move(start, start + (color == Color.White ? -7 : 9), PieceType.Pawn, PieceType.None, PieceType.Pawn);
                    else if (difference == 0 && start - 1 == board.enPassantSquare)
                        moves[i++] = new Move(start, start + (color == Color.White ? -9 : 7), PieceType.Pawn, PieceType.None, PieceType.Pawn);
                }

                while (targets != 0) {
                    end = (byte)Bitboard.BitScanForwardReset(ref targets);

                    if ((color == Color.White && end < 8) || (color == Color.Black && end > 55)) {
                        // promotions
                        moves[i++] = new Move(start, end, PieceType.Pawn, board.mailbox[end].pieceType, PieceType.Knight);
                        moves[i++] = new Move(start, end, PieceType.Pawn, board.mailbox[end].pieceType, PieceType.Bishop);
                        moves[i++] = new Move(start, end, PieceType.Pawn, board.mailbox[end].pieceType, PieceType.Rook);
                        moves[i++] = new Move(start, end, PieceType.Pawn, board.mailbox[end].pieceType, PieceType.Queen);
                    } else {
                        // other moves
                        moves[i++] = new Move(start, end, PieceType.Pawn, board.mailbox[end].pieceType, 0);
                    }
                }
            }
        }

        internal static void GetKnightMoves(Board board, Color color, Move[] moves, ref int i, bool kingCheck = false) {
            ulong targets; byte start; byte end;
            ulong knights = board.bitboards[(int)color, kingCheck ? 5 : 1];

            while (knights != 0) {
                start = (byte)Bitboard.BitScanForwardReset(ref knights);
                targets = Targets.GetKnightTargets(Constants.SquareMask[start], board, color);

                while (targets != 0) {
                    end = (byte)Bitboard.BitScanForwardReset(ref targets);
                    moves[i++] = new Move(start, end, PieceType.Knight, board.mailbox[end].pieceType, 0);
                }
            }
        }

        internal static void GetBishopMoves(Board board, Color color, Move[] moves, ref int i, bool kingCheck = false) {
            ulong targets; byte start; byte end;
            ulong bishops = board.bitboards[(int)color, kingCheck ? 5 : 2];

            while (bishops != 0) {
                start = (byte)Bitboard.BitScanForwardReset(ref bishops);
                targets = Targets.GetBishopTargets(Constants.SquareMask[start], board, color);

                while (targets != 0) {
                    end = (byte)Bitboard.BitScanForwardReset(ref targets);
                    moves[i++] = new Move(start, end, PieceType.Bishop, board.mailbox[end].pieceType, 0);
                }
            }
        }

        internal static void GetRookMoves(Board board, Color color, Move[] moves, ref int i, bool kingCheck = false) {
            ulong targets; byte start; byte end;
            ulong rooks = board.bitboards[(int)color, kingCheck ? 5 : 3];

            while (rooks != 0) {
                start = (byte)Bitboard.BitScanForwardReset(ref rooks);
                targets = Targets.GetRookTargets(Constants.SquareMask[start], board, color);

                while (targets != 0) {
                    end = (byte)Bitboard.BitScanForwardReset(ref targets);
                    moves[i++] = new Move(start, end, PieceType.Rook, board.mailbox[end].pieceType, 0);
                }
            }
        }

        internal static void GetQueenMoves(Board board, Color color, Move[] moves, ref int i) {
            ulong targets; byte start; byte end;
            ulong queens = board.bitboards[(int)color, 4];

            while (queens != 0) {
                start = (byte)Bitboard.BitScanForwardReset(ref queens);
                targets = Targets.GetRookTargets(Constants.SquareMask[start], board, color) | Targets.GetBishopTargets(Constants.SquareMask[start], board, color);

                while (targets != 0) {
                    end = (byte)Bitboard.BitScanForwardReset(ref targets);
                    moves[i++] = new Move(start, end, PieceType.Queen, board.mailbox[end].pieceType, 0);
                }
            }
        }

        internal static void GetKingMoves(Board board, Color color, Move[] moves, ref int i) {
            ulong targets; byte start; byte end;
            ulong king = board.bitboards[(int)color, 5];

            while (king != 0) {
                start = (byte)Bitboard.BitScanForwardReset(ref king);
                targets = Targets.GetKingTargets(Constants.SquareMask[start], board, color);

                while (targets != 0) {
                    end = (byte)Bitboard.BitScanForwardReset(ref targets);
                    moves[i++] = new Move(start, end, PieceType.King, board.mailbox[end].pieceType, 0);
                }
            }
        }
    }
}
