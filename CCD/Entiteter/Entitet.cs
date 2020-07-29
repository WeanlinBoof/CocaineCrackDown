using Nez;
using Nez.Sprites;

namespace CocaineCrackDown.Entiteter {
    public class Entitet : Component {
        protected const float AttackTimerNollstälare = 0f;
        protected string Namn { get; private set; }
        protected string TexturPlats { get; private set; }

        protected SpriteAtlas Atlas;

        protected SpriteAnimator Animerare;

        protected Mover Röraren;

        protected BoxCollider Kollision;


        protected float RörelseHastighet = 100f;

        public Entitet(string namn) {
            Namn = namn;
            TexturPlats = $"Content/{Namn}.png";

        }
    }
}
