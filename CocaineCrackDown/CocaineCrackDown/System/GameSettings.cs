using System;

using CocaineCrackDown.System.SpelLäggeHanterare;

namespace CocaineCrackDown.System {
    public class GameSettings {
        public static GameSettings Default = new GameSettings {
            SpelLäge = SpelLägen.HUB,
            Karta = "testnr1",
            MaxPoäng = 3,
        };
        public CoopLägen CoopLäge { get; set; }
        public SpelLägen SpelLäge { get; set; }
        public string Karta { get; set; }
        public int MaxPoäng { get; set; }
        public bool VänligEld => GetVänligEld();
        private bool GetVänligEld() {
            return CoopLäge == CoopLägen.FrendFire;
        }

        public float SkadaGångrare { get; set; } = 1.0f;
        public float SwepadGångrare { get; set; } = 1.0f;
        public float SwepadLuftburenGångrare { get; set; } = 1.0f;
        public float UthålighetGångrare { get; set; } = 1.0f;
    }
}
