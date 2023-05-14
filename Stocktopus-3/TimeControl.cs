//
// Stocktopus 3 chess engine
//      by ZlomenyMesic
//

namespace Stocktopus_3 {
    internal static class TimeControl {
        internal static long GetAvailableTime(string[] tokens) {
            long wtime = 0, btime = 0;
            int movestogo = 20, tokc = tokens.Length;

            for (int i = 1; i < tokens.Length; i++) {
                if (tokens[i] == "infinite") {
                    // infinite time
                    wtime = btime = long.MaxValue;
                } else if (tokens[i] == "wtime" && i != tokc - 1) {
                    try {
                        wtime += long.Parse(tokens[++i]);
                    } catch { Console.WriteLine($"invalid white's time: {tokens[i]}"); }
                } else if (tokens[i] == "btime" && i != tokc - 1) {
                    try {
                        btime += long.Parse(tokens[++i]);
                    } catch { Console.WriteLine($"invalid black's time: {tokens[i]}"); }
                } else if (tokens[i] == "winc" && i != tokc - 1) {
                    try {
                        wtime += long.Parse(tokens[++i]);
                    } catch { Console.WriteLine($"invalid white's incrementation: {tokens[i]}"); }
                } else if (tokens[i] == "binc" && i != tokc - 1) {
                    try {
                        btime += long.Parse(tokens[++i]);
                    } catch { Console.WriteLine($"invalid black's incrementation: {tokens[i]}"); }
                } else if (tokens[i] == "movestogo" && i != tokc - 1) {
                    try {
                        movestogo = int.Parse(tokens[++i]);
                    } catch { Console.WriteLine($"invalid number of moves to the next time control: {tokens[i]}"); }
                }
            }

            if (wtime == 0) wtime = 600000;
            if (btime == 0) btime = 600000;

            return CalculateTime(Core.engineColor == Color.White ? wtime : btime, movestogo);
        }

        private static long CalculateTime(long time, int movestogo) {
            // TODO: better time calculation
            return time / movestogo;
        }
    }
}
