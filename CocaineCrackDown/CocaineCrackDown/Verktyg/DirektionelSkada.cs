using CocaineCrackDown.Entiteter;
using Microsoft.Xna.Framework;

namespace CocaineCrackDown.Verktyg {
    public class DirektionelSkada {
        public float Damage { get; set; }
        public float Knockback { get; set; }
        public Vector2 Direction { get; set; }
        public Spelare SourceOfDamage { get; set; }
        public bool CanHitSelf { get; set; }
        public float AerialKnockback { get; set; }
    }
}
