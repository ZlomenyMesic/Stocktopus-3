//
// Stocktopus 3 chess engine
//      by ZlomenyMesic
//

namespace Stocktopus_3 {
    internal static class Openings {
        private static string[][] book = new string[300][];
        internal static bool CheckForBookMove(out string bookmove) {
            bookmove = "";
            List<string> possible = new();
            for (int i = 0; i < book.Length; i++) {
                if (book[i] != null && book[i][0] == Board.ConvertToBookFormat(Core.board)) {
                    possible.Add(book[i][1]);
                }
            }
            if (possible.Count == 0) return false;
            bookmove = possible[new Random().Next(0, possible.Count)];
            return true;
        }

        internal static void Extract() {
            var lines = File.ReadAllLines("book.txt");
            int count = 0;

            for (int i = 0; i < lines.Length; i++) {
                if (!lines[i].StartsWith("//")) {
                    var split = lines[i].Split(' ');
                    book[count++] = new string[] { split[0], split[1] };
                }
            }
        }
    }
}
