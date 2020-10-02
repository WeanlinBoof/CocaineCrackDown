
using Microsoft.Xna.Framework;

namespace CocaineCrackDown.Entiteter {
    public enum NärstridsVapenTyp {
        Hold, Swing
    }
    public class NärstridsVapenParameters : VapenParameters {
        public NärstridsVapenTyp MeleeType { get; set; } = NärstridsVapenTyp.Swing;
        public NärstridsVapnenAnimationer Sprite { get; set; }
        public float Damage { get; set; } = 10f;
        public float AerialKnockback { get; set; } = 0f;
        public float Knockback { get; set; } = 1.25f;
        public bool Flip { get; set; } = true;
        public Vector2 HitboxOffset { get; set; } = new Vector2(10 , 0);
        public Vector2 HitboxSize { get; set; } = new Vector2(10 , 0);
    }
}
