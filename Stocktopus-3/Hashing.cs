//
// Stocktopus 3 chess engine
//      by ZlomenyMesic 
//

namespace Stocktopus_3 {
    internal static class Hashing {
        internal static readonly ulong[,] zobrist = new ulong[64, 12];

        internal static void InitZobrist() {
            for (int i = 0; i < 64; i++)
                for (int j = 0; j < 12; j++)
                    zobrist[i, j] = RandomUInt64();
        }

        internal static ulong GetBoardHash(Board board) {
            ulong hash = 0;

            for (int i = 0; i < 64; i++)
                if (board.mailbox[i].pieceType != PieceType.None)
                    hash ^= zobrist[i, (int)board.mailbox[i].pieceType + (board.mailbox[i].color == Color.White ? -1 : 5)];

            return hash ^ board.enPassantSquare;
        }

        private static ulong RandomUInt64() {
            var buffer = new byte[sizeof(ulong)];
            new Random().NextBytes(buffer);
            return BitConverter.ToUInt64(buffer, 0);
        }
    }
}
