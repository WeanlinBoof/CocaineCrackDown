using Nez;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Nez.Sprites;

namespace CocaineCrackDown.Entiteter {

    public class Entitet : Component{

        protected Mover Förflytare;

        protected Texture2D EntitetTextur;

        protected BoxCollider BoxKollision;

        protected Vector2 EntitetPostition;

        protected SpriteAtlas Atlas;

        protected SpriteAnimator Animerare;

        protected SpriteAnimator.LoopMode AnimationUppspelningsTyp;

        //public virtual bool KanTaSkada => true;

        public Entitet() {


        }



    }
}
