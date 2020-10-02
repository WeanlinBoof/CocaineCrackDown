using Microsoft.Xna.Framework;

namespace CocaineCrackDown.Entiteter.Gestalter {
    public class GestaltParameter {
        public SpelarAtlas SpelarAtlas { get; set; }
        public string KaraktärNamn { get; set; }
        public float MaxLivsPoäng { get; set; } = 100;
        public float MaxUthållighet { get; set; } = 100;
        public float Hastighet { get; set; } = 100f;
        public Color Hudfärg { get; set; } = Color.Beige;
        public Color BlodFärg { get; set; } = Color.DarkRed;
    }
}
