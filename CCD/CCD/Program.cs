using System;

namespace CCD {
    public static class Program {
        [STAThread]
        static void Main() {
            using Spel CCD = new Spel();
            CCD.Run();
        }
    }
}
