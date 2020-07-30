using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Nez;
using Nez.AI.FSM;
using Nez.Sprites;
using Nez.Timers;


namespace CocaineCrackDown.Entiteter {

    public class Hejduk : Entitet, IUpdatable {

        public Hejduk(string namn = "doug") : base(namn) {
        }

        private SubpixelVector2 SubPixelVecTvå = new SubpixelVector2();


        private string animation = "doug-stilla";


        public override void OnAddedToEntity() {
            SpriteAtlas AtlasTextur = Entity.Scene.Content.LoadSpriteAtlas("Content/doug.atlas");
            Texture2D Textur = Entity.Scene.Content.LoadTexture(TexturPlats);

            Kollision = Entity.AddComponent(new BoxCollider(-20, -31, 40, 63));
            Röraren = Entity.AddComponent(new Mover());
            Animerare = Entity.AddComponent<SpriteAnimator>();

            Animerare.AddAnimationsFromAtlas(AtlasTextur);
        }

        public void Update() {
            Animerare.Play(animation);

            //StandardScen standardScen = Entity.Scene as StandardScen;
        }
    }
}
