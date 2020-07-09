using System;

// RÖR INGET HÄR OM DU INTE VET VAD DU GÖR
namespace CocaineCrackDown {

    internal static class Program {

        [STAThread]
        private static void Main() {
            using Spel CCD = new Spel();
            CCD.Run();
        }
    }
}