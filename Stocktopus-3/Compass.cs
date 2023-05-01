using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stocktopus_3 {
    internal class Compass {
        internal static ulong North(ulong bitboard) => bitboard >> 8;
        internal static ulong South(ulong bitboard) => bitboard << 8;
        internal static ulong West(ulong bitboard) => bitboard >> 1 & 0x7F7F7F7F7F7F7F7F;
        internal static ulong East(ulong bitboard) => bitboard << 1 & 0xFEFEFEFEFEFEFEFE;

        internal static ulong SouthEast(ulong bitboard) => bitboard << 9 & 0xFEFEFEFEFEFEFEFE;
        internal static ulong SouthWest(ulong bitboard) => bitboard << 7 & 0x7F7F7F7F7F7F7F7F;
        internal static ulong NorthEast(ulong bitboard) => bitboard >> 7 & 0xFEFEFEFEFEFEFEFE;
        internal static ulong NorthWest(ulong bitboard) => bitboard >> 9 & 0x7F7F7F7F7F7F7F7F;
    }
}
