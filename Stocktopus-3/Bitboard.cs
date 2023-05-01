//
// Stocktopus 3 chess engine
//      by ZlomenyMesic 
//

namespace Stocktopus_3 {
    internal static class Bitboard {
        internal static bool IsBitSet(ulong bitboard, int index) {
            return (bitboard & ((ulong)1 << index)) != 0;
        }

        internal static int BitScanForward(ulong bitboard) {
            return Constants.DeBrujinTable[((ulong)((long)bitboard & -(long)bitboard) * Constants.DeBrujinValue) >> 58];
        }

        public static int BitScanForwardReset(ref ulong bitboard) {
            ulong bb = bitboard;
            bitboard &= bb - 1;
            return Constants.DeBrujinTable[((ulong)((long)bb & -(long)bb) * Constants.DeBrujinValue) >> 58];
        }
    }
}
