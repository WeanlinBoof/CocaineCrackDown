using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Nez;
using Nez.AI.FSM;
using Nez.Sprites;
using Nez.Timers;
using CocaineCrackDown.Scener;
using FarseerPhysics.Collision;

namespace CocaineCrackDown.Entiteter {

    public class Hejduk : Entitet, IUpdatable {

        public Hejduk(string namn = "doug", EntitetRelation entitetRelation = EntitetRelation.Hjälte) : base(namn, entitetRelation) {
        }

        private SubpixelVector2 SubPixelVecTvå = new SubpixelVector2();

        private VirtualButton attackknapp;

        private VirtualIntegerAxis XAxisknappar;

        private VirtualIntegerAxis YAxisknappar;

        private string animation = "doug-stilla";

        private Collider other;
        private CollisionResult result;

        public BoxCollider BoxKollision;

        public override void OnAddedToEntity() {

            SpriteAtlas AtlasTextur = Entity.Scene.Content.LoadSpriteAtlas("Content/doug.atlas");
            Texture2D Textur = Entity.Scene.Content.LoadTexture(TexturPlats);

            BoxKollision = Entity.AddComponent(new BoxCollider(-20, -31, 40, 63));
            Röraren = Entity.AddComponent(new Mover());
            Animerare = Entity.AddComponent<SpriteAnimator>();

            Animerare.AddAnimationsFromAtlas(AtlasTextur);
            SkapaInmatning();

        }
        /// <summary>
        /// //////////////////////////////Fixa Ai Fixa Ai Fixa Ai Fixa Ai Fixa Ai Fixa Ai Fixa Ai Fixa Ai Fixa Ai
        /// </summary>
        private void SkapaInmatning() {

            //Fixa Ai  Inmatning för attack
            attackknapp = new VirtualButton();
            attackknapp.Nodes.Add(new VirtualButton.KeyboardKey(Keys.G));

            //Fixa Ai  Horizontel Inmatning
            XAxisknappar = new VirtualIntegerAxis();
            XAxisknappar.Nodes.Add(new VirtualAxis.KeyboardKeys(VirtualInput.OverlapBehavior.TakeNewer, Keys.B, Keys.M));

            // Fixa Ai Vertikal Inmatning
            YAxisknappar = new VirtualIntegerAxis();
            YAxisknappar.Nodes.Add(new VirtualAxis.KeyboardKeys(VirtualInput.OverlapBehavior.TakeNewer, Keys.H, Keys.N));
        }
        public void Update() {
            Vector2 moveDir = new Vector2(XAxisknappar.Value, YAxisknappar.Value);


            if (moveDir != Vector2.Zero) {

                Rörelse = moveDir * RörelseHastighet * Time.UnscaledDeltaTime;

                Röraren.CalculateMovement(ref Rörelse, out CollisionResult res);
                SubPixelVecTvå.Update(ref Rörelse);
                Röraren.ApplyMovement(Rörelse);
            }

            else {
                animation = "doug-stilla";
            }

            if (!Animerare.IsAnimationActive(animation)) {
                Animerare.Play(animation);
            }
            else {
                Animerare.UnPause();
            }

            //StandardScen standardScen = Entity.Scene as StandardScen;
        }
    }
}
