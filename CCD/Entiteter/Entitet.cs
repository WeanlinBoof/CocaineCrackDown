using Microsoft.Xna.Framework;

using Nez;
using Nez.Sprites;

namespace CocaineCrackDown.Entiteter {

    public enum Riktning {

        höger,

        vänster,

        upp,

        ner,
    }
    public enum EntitetRelation {
        Skurk,
        Hjälte,
        Civil,
        ETC,

    }
    public class Entitet : Component {
        public EntitetRelation EntitetRelationen { get; set; }

        protected float AttackTimer = 0f;

        protected const float AttackTimerNollstälare = 0f;
        protected string Namn { get; private set; }
        protected string TexturPlats { get; private set; }

        protected SpriteAtlas Atlas;

        protected SpriteAnimator Animerare;

        protected SpriteAnimator.LoopMode AnimationUppspelningsTyp;

        protected Mover Röraren;

        //public BoxCollider BoxKollision;

        protected float RörelseHastighet = 100f;

        protected Vector2 Rörelse;

        public Collider other;
        public CollisionResult result;

        public Entitet(string namn,EntitetRelation entitetRelation ) {
            EntitetRelationen = entitetRelation;
            Namn = namn;
            TexturPlats = $"Content/{Namn}.png";

        }
    }
}
