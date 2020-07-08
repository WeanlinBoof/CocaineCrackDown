using System;
// RÖR INGET HÄR OM DU INTE VET VAD DU GÖR
namespace CocaineCrackDown {
    public static class Program {
        [STAThread]
        static void Main() {
            using Spel spel = new Spel();
            spel.Run();
        }
    }
}
