﻿//
// Stocktopus 3 chess engine
//      by ZlomenyMesic 
//

namespace Stocktopus_3 {
    enum Color {
        White = 0,
        Black = 1,
        None = 2
    }

    enum PieceType {
        None = 0,
        Pawn = 1,
        Knight = 2,
        Bishop = 3,
        Rook = 4,
        Queen = 5,
        King = 6
    }

    internal static class Constants {
        internal const string ENGINE_INFO = "id name Stocktopus-3\nid author ZlomenyMesic\nuciok";
        internal const string STARTPOS_FEN = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
        internal const string FILES = "abcdefgh";
        internal const string PIECES = "pnbrqk";

        internal const ulong OO_WHITE_MASK = 0x6000000000000000;
        internal const ulong OOO_WHITE_MASK = 0x0E00000000000000;
        internal const ulong OO_BLACK_MASK = 0x0000000000000060;
        internal const ulong OOO_BLACK_MASK = 0x000000000000000E;

        internal const ulong DeBrujinValue = 0x07EDD5E59A4E28C2;

        internal static readonly int[] DeBrujinTable =
        {
            63,  0, 58,  1, 59, 47, 53,  2,
            60, 39, 48, 27, 54, 33, 42,  3,
            61, 51, 37, 40, 49, 18, 28, 20,
            55, 30, 34, 11, 43, 14, 22,  4,
            62, 57, 46, 52, 38, 26, 32, 41,
            50, 36, 17, 19, 29, 10, 13, 21,
            56, 45, 25, 31, 35, 16,  9, 12,
            44, 24, 15,  8, 23,  7,  6,  5
        };

        internal static readonly ulong[] SixBitRankMask = new ulong[]{
            0x000000000000007E, 0x0000000000007E00, 0x00000000007E0000,
            0x000000007E000000, 0x0000007E00000000, 0x00007E0000000000,
            0x007E000000000000, 0x7E00000000000000
        };

        internal static readonly ulong[] SixBitFileMask = new ulong[]{
            0x0001010101010100, 0x0002020202020200, 0x0004040404040400,
            0x0008080808080800, 0x0010101010101000, 0x0020202020202000,
            0x0040404040404000, 0x0080808080808000
        };

        internal static readonly ulong[] FileMagic = new ulong[]{
            0x8040201008040200, 0x4020100804020100, 0x2010080402010080, 0x1008040201008040,
            0x0804020100804020, 0x0402010080402010, 0x0201008040201008, 0x0100804020100804
        };

        internal static readonly ulong[] A1H8DiagonalMagic = new ulong[]{
            0x0000000000000000, 0x0000000000000000, 0x0808080000000000, 0x1010101000000000,
            0x2020202020000000, 0x4040404040400000, 0x8080808080808000, 0x0101010101010100,
            0x0101010101010100, 0x0101010101010100, 0x0101010101010100, 0x0101010101010100,
            0x0101010101010100, 0x0000000000000000, 0x0000000000000000
        };

        internal static readonly ulong[] H1A8DiagonalMagic = new ulong[]{
            0x0000000000000000, 0x0000000000000000, 0x0101010101010100, 0x0101010101010100,
            0x0101010101010100, 0x0101010101010100, 0x0101010101010100, 0x0101010101010100,
            0x0080808080808080, 0x0040404040404040, 0x0020202020202020, 0x0010101010101010,
            0x0008080808080808, 0x0000000000000000, 0x0000000000000000
        };

        internal static readonly ulong[] A1H8DiagonalMask = new ulong[]{
            0x0000000000000080, 0x0000000000008040, 0x0000000000804020, 0x0000000080402010,
            0x0000008040201008, 0x0000804020100804, 0x0080402010080402, 0x8040201008040201,
            0x4020100804020100, 0x2010080402010000, 0x1008040201000000, 0x0804020100000000,
            0x0402010000000000, 0x0201000000000000, 0x0100000000000000
        };

        internal static readonly ulong[] H1A8DiagonalMask = new ulong[]{
            0x0000000000000001, 0x0000000000000102, 0x0000000000010204, 0x0000000001020408,
            0x0000000102040810, 0x0000010204081020, 0x0001020408102040, 0x0102040810204080,
            0x0204081020408000, 0x0408102040800000, 0x0810204080000000, 0x1020408000000000,
            0x2040800000000000, 0x4080000000000000, 0x8000000000000000
        };

        internal static ulong[] SquareMask = new ulong[] {
            0x0000000000000001,
            0x0000000000000002,
            0x0000000000000004,
            0x0000000000000008,
            0x0000000000000010,
            0x0000000000000020,
            0x0000000000000040,
            0x0000000000000080,

            0x0000000000000100,
            0x0000000000000200,
            0x0000000000000400,
            0x0000000000000800,
            0x0000000000001000,
            0x0000000000002000,
            0x0000000000004000,
            0x0000000000008000,

            0x0000000000010000,
            0x0000000000020000,
            0x0000000000040000,
            0x0000000000080000,
            0x0000000000100000,
            0x0000000000200000,
            0x0000000000400000,
            0x0000000000800000,

            0x0000000001000000,
            0x0000000002000000,
            0x0000000004000000,
            0x0000000008000000,
            0x0000000010000000,
            0x0000000020000000,
            0x0000000040000000,
            0x0000000080000000,

            0x0000000100000000,
            0x0000000200000000,
            0x0000000400000000,
            0x0000000800000000,
            0x0000001000000000,
            0x0000002000000000,
            0x0000004000000000,
            0x0000008000000000,

            0x0000010000000000,
            0x0000020000000000,
            0x0000040000000000,
            0x0000080000000000,
            0x0000100000000000,
            0x0000200000000000,
            0x0000400000000000,
            0x0000800000000000,

            0x0001000000000000,
            0x0002000000000000,
            0x0004000000000000,
            0x0008000000000000,
            0x0010000000000000,
            0x0020000000000000,
            0x0040000000000000,
            0x0080000000000000,

            0x0100000000000000,
            0x0200000000000000,
            0x0400000000000000,
            0x0800000000000000,
            0x1000000000000000,
            0x2000000000000000,
            0x4000000000000000,
            0x8000000000000000
        };
    }
}
