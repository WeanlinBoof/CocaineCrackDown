
using CocaineCrackDown.Scener;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Nez;
using Nez.Sprites;

namespace CocaineCrackDown.Entiteter {

    public class RandySpelareTvå : Entitet, IUpdatable {

        public RandySpelareTvå(string namn = "doug") : base(namn) {
        }

        private SubpixelVector2 SubPixelVecTvå = new SubpixelVector2();

        private VirtualButton attackknapp;

        private VirtualIntegerAxis XAxisknappar;

        private VirtualIntegerAxis YAxisknappar;

        public override void OnAddedToEntity() {
            SpriteAtlas AtlasTextur = Entity.Scene.Content.LoadSpriteAtlas("Content/doug.atlas");
            Texture2D Textur = Entity.Scene.Content.LoadTexture(TexturPlats);

            Röraren = Entity.AddComponent(new Mover());
            Animerare = Entity.AddComponent<SpriteAnimator>();

            Animerare.AddAnimationsFromAtlas(AtlasTextur);

            SkapaInmatning();
        }

        private void SkapaInmatning() {

            // Inmatning för attack
            attackknapp = new VirtualButton();
            attackknapp.Nodes.Add(new VirtualButton.KeyboardKey(Keys.J));
            attackknapp.Nodes.Add(new VirtualButton.GamePadButton(0, Buttons.A));

            // Horizontel Inmatning
            XAxisknappar = new VirtualIntegerAxis();
            XAxisknappar.Nodes.Add(new VirtualAxis.GamePadDpadLeftRight());
            XAxisknappar.Nodes.Add(new VirtualAxis.GamePadLeftStickX());
            XAxisknappar.Nodes.Add(new VirtualAxis.KeyboardKeys(VirtualInput.OverlapBehavior.TakeNewer, Keys.A, Keys.D));

            // Vertikal Inmatning
            YAxisknappar = new VirtualIntegerAxis();
            YAxisknappar.Nodes.Add(new VirtualAxis.GamePadDpadUpDown());
            YAxisknappar.Nodes.Add(new VirtualAxis.GamePadLeftStickY());
            YAxisknappar.Nodes.Add(new VirtualAxis.KeyboardKeys(VirtualInput.OverlapBehavior.TakeNewer, Keys.W, Keys.S));
        }

        public void Update() {

            // handle movement and animations
            Vector2 moveDir = new Vector2(XAxisknappar.Value, YAxisknappar.Value);
            string animation;

            if (moveDir.X < 0 || moveDir.Y < 0|| moveDir.X > 0 || moveDir.Y > 0) {
                animation = "doug-gång";
            }
            else {
                animation = "doug-stilla";
            }

            if (moveDir != Vector2.Zero) {
                if (!Animerare.IsAnimationActive(animation)) {
                    Animerare.Play(animation);
                }
                else {
                    Animerare.UnPause();
                }

                Vector2 movement = moveDir * RörelseHastighet * Time.DeltaTime;

                Röraren.CalculateMovement(ref movement, out CollisionResult res);
                SubPixelVecTvå.Update(ref movement);
                Röraren.ApplyMovement(movement);
            }
            else {
                Animerare.Pause();
            }

            StandardScen ScenEtt = Entity.Scene as StandardScen;
        }
    }
}

/*//textur
SpriteAtlas dougAtlas = Content.LoadSpriteAtlas("Content/doug.atlas");
Entity doug = CreateEntity("dougtest", new Vector2(320, 180));
SpriteAnimator animator = doug.AddComponent<SpriteAnimator>();

//animator
animator.AddAnimationsFromAtlas(dougAtlas);
            animator.Play("doug-gång");

            //animator.Stop();
            //animator.Play("doug-stilla");
*/
