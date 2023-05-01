using System.Drawing;
using System;

namespace Stocktopus_3 {
    internal static class LookupTables {
        // Move generators in modern engines generate moves with lookup tables, which
        // are indexed by the square of the slider and a bitboard representing
        // occupied (impassable) squares that might block the movement of the piece.

        // To compress the huge set of possible occupancy bitboards into a reasonable
        // size, Magic Bitboards are often used. They involve multiplying the occupancy
        // bitboard by 'magic' numbers, which are chosen because they empirically
        // map the set of possible occupancy bitboards into a dense range.

        internal static readonly ulong[] KingAttacks = new ulong[64];

        internal static readonly ulong[][] RankAttacks = new ulong[64][];
        internal static readonly ulong[][] FileAttacks = new ulong[64][];
        internal static readonly ulong[][] A1H8DiagonalAttacks = new ulong[64][];
        internal static readonly ulong[][] H1A8DiagonalAttacks = new ulong[64][];

        internal static void Initialize() {
            InitKingAttacks();
            InitRankAttacks();
            InitFileAttacks();
            InitA1H8DiagonalAttacks();
            InitH1A8DiagonalAttacks();
        }

        private static void InitKingAttacks() {
            for (int i = 0; i < 64; i++) {
                ulong king = Constants.SquareMask[i];
                ulong attacks = Compass.East(king) | Compass.West(king);
                king |= attacks;
                attacks |= Compass.North(king) | Compass.South(king);
                KingAttacks[i] = attacks;
            }
        }

        private static void InitRankAttacks() {
            for (int i = 0; i < 64; i++) {
                RankAttacks[i] = new ulong[64];
            }

            for (int square = 0; square < 64; square++) {
                for (int occ = 0; occ < 64; occ++) {
                    ulong occupancy = (ulong)occ << 1;
                    ulong targets = 0;

                    int blocker = (square & 7) + 1;
                    while (blocker <= 7) {
                        targets |= Constants.SquareMask[blocker];
                        if (Bitboard.IsBitSet(occupancy, blocker)) break;
                        blocker++;
                    }

                    blocker = (square & 7) - 1;
                    while (blocker >= 0) {
                        targets |= Constants.SquareMask[blocker];
                        if (Bitboard.IsBitSet(occupancy, blocker)) break;
                        blocker--;
                    }

                    RankAttacks[square][occ] = targets << (8 * (square >> 3));
                }
            }
        }
        private static void InitFileAttacks() {
            for (int i = 0; i < 64; i++) {
                FileAttacks[i] = new ulong[64];
            }

            for (int sq = 0; sq < 64; sq++) {
                for (int occ = 0; occ < 64; occ++) {
                    ulong targets = 0;
                    ulong rankTargets = RankAttacks[7 - (sq / 8)][occ];

                    for (int bit = 0; bit < 8; bit++) {
                        if (Bitboard.IsBitSet(rankTargets, bit)) {
                            targets |= Constants.SquareMask[(sq & 7) + 8 * (7 - bit)];
                        }
                    }
                    FileAttacks[sq][occ] = targets;
                }
            }
        }

        private static void InitA1H8DiagonalAttacks() {
            for (int i = 0; i < 64; i++) {
                A1H8DiagonalAttacks[i] = new ulong[64];
            }

            for (int sq = 0; sq < 64; sq++) {
                for (int occ = 0; occ < 64; occ++) {
                    int diagonal = (sq >> 3) - (sq & 7);
                    ulong targets = 0;
                    ulong rankTargets = diagonal > 0 ? RankAttacks[sq % 8][occ] : RankAttacks[sq / 8][occ];

                    for (int bit = 0; bit < 8; bit++) {
                        int rank, file;

                        if (Bitboard.IsBitSet(rankTargets, bit)) {
                            if (diagonal >= 0) {
                                rank = diagonal + bit;
                                file = bit;
                            } else {
                                file = bit - diagonal;
                                rank = bit;
                            }

                            if ((file >= 0) && (file <= 7) && (rank >= 0) && (rank <= 7)) {
                                targets |= Constants.SquareMask[file + 8 * rank];
                            }
                        }
                    }

                    A1H8DiagonalAttacks[sq][occ] = targets;
                }
            }
        }

        private static void InitH1A8DiagonalAttacks() {
            for (int i = 0; i < 64; i++) {
                H1A8DiagonalAttacks[i] = new ulong[64];
            }

            for (int sq = 0; sq < 64; sq++) {
                for (int occ = 0; occ < 64; occ++) {
                    int diagonal = (sq >> 3) + (sq & 7);
                    ulong targets = 0;
                    ulong rankTargets = diagonal > 7 ? RankAttacks[7 - sq / 8][occ] : RankAttacks[sq % 8][occ];

                    for (int bit = 0; bit < 8; bit++) {
                        int rank; int file;

                        if (Bitboard.IsBitSet(rankTargets, bit)) {
                            if (diagonal >= 7) {
                                rank = 7 - bit;
                                file = (diagonal - 7) + bit;
                            } else {
                                rank = diagonal - bit;
                                file = bit;
                            }

                            if ((file >= 0) && (file <= 7) && (rank >= 0) && (rank <= 7)) {
                                targets |= Constants.SquareMask[file + 8 * rank];
                            }
                        }
                    }

                    H1A8DiagonalAttacks[sq][occ] = targets;
                }
            }
        }
    }
}
