
using Microsoft.Xna.Framework;

using Nez;
using Nez.Sprites;


namespace CocaineCrackDown.Komponenter {
    public class Entitet : Component {

        protected float AttackTimer = 0f;

        protected const float AttackTimerNollstälare = 0f;
        protected string Namn { get; set; }
        protected string TexturPlats { get; private set; }

        // Can take damage

        public virtual bool KanTaSkada => true;

        public bool IsLocked { get; set; }

        public bool IsBusy { get; set; }

        public bool ForcedGround { get; set; }

        protected SpriteAtlas Atlas;

        protected SpriteAnimator Animerare;

        protected SpriteAnimator.LoopMode AnimationUppspelningsTyp;

        protected Mover Röraren;

        protected StatusVärdeKomponent StatusVärde;

        public BoxCollider BoxKollision;

        protected Vector2 Rörelse;
        public Entitet(string namn) {
            Namn = namn;
            TexturPlats = $"Content/{Namn}.png";
        }

        public bool ÄrRörelseTilgänglig() {
            return !IsBusy && !IsLocked;
        }
    }
}
