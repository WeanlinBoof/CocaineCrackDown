using System;

namespace CocaineCrackDown {
    public static class Program {
        [STAThread]
        private static void Main() {
            using Spel CCD = new Spel();
            CCD.Run();
        }
    }
}
