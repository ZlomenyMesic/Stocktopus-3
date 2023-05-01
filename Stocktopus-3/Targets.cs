//
// Stocktopus 3 chess engine
//      by ZlomenyMesic 
//

namespace Stocktopus_3 {
    internal static class Targets {
        // chess speaks for itself

        private static ulong GetPawnPushes(ulong pawn, Board board, Color color) {
            ulong singleStepPushTargets = color == Color.White
                ? Compass.North(pawn) & board.empty
                : Compass.South(pawn) & board.empty;

            ulong doubleStepPushTargets = color == Color.White
                ? Compass.North(singleStepPushTargets) & board.empty & 0x000000FF00000000
                : Compass.South(singleStepPushTargets) & board.empty & 0x00000000FF000000;

            return singleStepPushTargets | doubleStepPushTargets;
        }

        private static ulong GetPawnAttacks(ulong pawn, Board board, Color color) {
            ulong westAttacks = color == Color.White
                ? Compass.NorthWest(pawn)
                : Compass.SouthWest(pawn);

            ulong eastAttacks = color == Color.White
                ? Compass.NorthEast(pawn)
                : Compass.SouthEast(pawn);

            return (westAttacks | eastAttacks) & board.occupied[color == Color.White ? 1 : 0];
        }

        internal static ulong GetPawnTargets(ulong pawn, Board board, Color color) {
            return GetPawnPushes(pawn, board, color) | GetPawnAttacks(pawn, board, color);
        }

        internal static ulong GetKnightTargets(ulong knight, Board board, Color color) {
            ulong east = Compass.East(knight);
            ulong west = Compass.West(knight);
            ulong targets = ((east | west) << 16) | ((east | west) >> 16);

            east = Compass.East(east);
            west = Compass.West(west);
            targets |= Compass.South(east | west) | Compass.North(east | west);

            return targets & (board.occupied[color == Color.White ? 1 : 0] | board.empty);
        }

        internal static ulong GetBishopTargets(ulong bishop, Board board, Color color) {
            ulong occupied = ~board.empty, targets = 0;
            int square = Bitboard.BitScanForward(bishop);

            int diagonal = 7 + (square >> 3) - (square & 7);
            int occupancy = (int)((occupied & Constants.A1H8DiagonalMask[diagonal]) * Constants.A1H8DiagonalMagic[diagonal] >> 56);
            targets |= LookupTables.A1H8DiagonalAttacks[square][(occupancy >> 1) & 63];

            diagonal = (square >> 3) + (square & 7);
            occupancy = (int)((occupied & Constants.H1A8DiagonalMask[diagonal]) * Constants.H1A8DiagonalMagic[diagonal] >> 56);
            targets |= LookupTables.H1A8DiagonalAttacks[square][(occupancy >> 1) & 63];

            return targets & (board.occupied[color == Color.White ? 1 : 0] | board.empty);
        }

        internal static ulong GetRookTargets(ulong rook, Board board, Color color) {
            ulong occupied = ~board.empty, targets = 0;
            int square = Bitboard.BitScanForward(rook);

            int occupancy = (int)((occupied & Constants.SixBitRankMask[square >> 3]) >> (8 * (square >> 3)));
            targets |= LookupTables.RankAttacks[square][(occupancy >> 1) & 63];

            occupancy = (int)((occupied & Constants.SixBitFileMask[square & 7]) * Constants.FileMagic[square & 7] >> 56);
            targets |= LookupTables.FileAttacks[square][(occupancy >> 1) & 63];

            return targets & (board.occupied[color == Color.White ? 1 : 0] | board.empty);
        }

        internal static ulong GetKingTargets(ulong king, Board board, Color color) {
            ulong targets = LookupTables.KingAttacks[Bitboard.BitScanForward(king)];
            return targets & (board.occupied[color == Color.White ? 1 : 0] | board.empty);
        }
    }
}
