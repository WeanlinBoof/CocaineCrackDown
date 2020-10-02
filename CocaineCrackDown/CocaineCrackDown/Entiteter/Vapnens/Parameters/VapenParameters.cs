

namespace CocaineCrackDown.Entiteter {
    public class VapenParameters {
        public string Namn { get; set; }

        public float AttackHastighet { get; set; } = 0.1f;

        public float RenderOffset { get; set; }
        public bool RoterarMedSpelare { get; set; } = true;
        public bool AlltidOvanförSpelare { get; set; } = false;
        public bool FlipXMedSpelare { get; set; } = false;
        public bool FlipYMedSpelare { get; set; } = true;
        public float Skala { get; set; } = 0.6f;

        public float ChansAttTappa { get; set; }
        public Ovanlighet Ovanlighet { get; set; }
        public VapenTyp Typ { get; set; }
    }
}
