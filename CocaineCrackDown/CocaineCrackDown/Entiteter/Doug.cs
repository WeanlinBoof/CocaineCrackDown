using Nez;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nez.Sprites;
using Microsoft.Xna.Framework.Input;

namespace CocaineCrackDown.Entiteter {
    public class Doug : Entitet, IUpdatable, ITriggerListener {
        private SubpixelVector2 SubPixelVecTvå = new SubpixelVector2();

        private VirtualButton attackknapp;

        private VirtualIntegerAxis XAxisknappar;

        private VirtualIntegerAxis YAxisknappar;

        private string animation = "doug-stilla";

        private Riktning DougRiktning;

        private bool Attackerar;
        public float RörelseHastighet;

        public Doug() {
        }
        public override void OnAddedToEntity() {
            base.OnAddedToEntity();
            Atlas = Entity.Scene.Content.LoadSpriteAtlas("Content/doug.atlas");
            EntitetTextur = Entity.Scene.Content.LoadTexture("Content/doug.png");
            BoxKollision = Entity.AddComponent(new BoxCollider(-20, -31, 40, 63));
            Förflytare = Entity.AddComponent(new Mover());
            Animerare = Entity.AddComponent<SpriteAnimator>();
            RörelseHastighet = 100f;
            Animerare.AddAnimationsFromAtlas(Atlas);
            SkapaInmatning();

        }
        private void SkapaInmatning() {

            // Inmatning för attack
            attackknapp = new VirtualButton();
            attackknapp.Nodes.Add(new VirtualButton.KeyboardKey(Keys.Z));
            attackknapp.Nodes.Add(new VirtualButton.GamePadButton(0, Buttons.A));

            // Horizontel Inmatning
            XAxisknappar = new VirtualIntegerAxis();
            XAxisknappar.Nodes.Add(new VirtualAxis.GamePadDpadLeftRight());
            XAxisknappar.Nodes.Add(new VirtualAxis.GamePadLeftStickX());
            XAxisknappar.Nodes.Add(new VirtualAxis.KeyboardKeys(VirtualInput.OverlapBehavior.TakeNewer, Keys.Left, Keys.Right));

            // Vertikal Inmatning
            YAxisknappar = new VirtualIntegerAxis();
            YAxisknappar.Nodes.Add(new VirtualAxis.GamePadDpadUpDown());
            YAxisknappar.Nodes.Add(new VirtualAxis.GamePadLeftStickY());
            YAxisknappar.Nodes.Add(new VirtualAxis.KeyboardKeys(VirtualInput.OverlapBehavior.TakeNewer, Keys.Up, Keys.Down));

        }
        public void Update() {

            Vector2 moveDir = new Vector2(XAxisknappar.Value, YAxisknappar.Value);


            if(moveDir.Y < 0 || moveDir.Y > 0) {
                if (animation != "doug-gång") {
                    animation = "doug-gång";
                }
            }
            if (moveDir.X < 0) {
                DougRiktning = Riktning.vänster;
                if (animation != "doug-gång") {
                    animation = "doug-gång";
                }
            }
            if (moveDir.X > 0) {
                DougRiktning = Riktning.höger;
                if (animation != "doug-gång") {
                    animation = "doug-gång";
                }
            }
            if (moveDir.X == 0 && moveDir.Y == 0) {
                if (animation != "doug-stilla") {
                    animation = "doug-stilla";
                }
            }
            if (DougRiktning == Riktning.höger) {
                Animerare.FlipX = false;
            }
            if (DougRiktning == Riktning.vänster) {
                Animerare.FlipX = true;
            }
            if (moveDir != Vector2.Zero)
            {

                Vector2 movement = moveDir * RörelseHastighet * Time.UnscaledDeltaTime;

                Förflytare.CalculateMovement(ref movement, out CollisionResult res);
                SubPixelVecTvå.Update(ref movement);
                Förflytare.ApplyMovement(movement);
            }
            if (!Animerare.IsAnimationActive(animation))
            {
                Animerare.Play(animation);
            }
            else
            {
                Animerare.UnPause();
            }
        }
        public void OnTriggerEnter(Collider other, Collider local) {

        }
        public void OnTriggerExit(Collider other, Collider local) {

        }
    }
}
