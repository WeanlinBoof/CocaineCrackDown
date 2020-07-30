using CocaineCrackDown.Scener;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Nez;
using Nez.AI.FSM;
using Nez.Sprites;
using Nez.Timers;


namespace CocaineCrackDown.Entiteter {

    public class RandySpelareTvå : Entitet, IUpdatable {

        public RandySpelareTvå(string namn = "randy", EntitetRelation entitetRelation = EntitetRelation.Hjälte) : base(namn, entitetRelation) {
        }

        private SubpixelVector2 SubPixelVecTvå = new SubpixelVector2();

        private VirtualButton attackknapp;

        private VirtualIntegerAxis XAxisknappar;

        private VirtualIntegerAxis YAxisknappar;

        //private UnscaledDeltaTime UnscaledDeltaTime;

        private string animation = "randy-stilla";

        private Riktning RandyRiktning;

        private bool Attack;


        public override void OnAddedToEntity() {
            SpriteAtlas AtlasTextur = Entity.Scene.Content.LoadSpriteAtlas("Content/randy.atlas");
            Texture2D Textur = Entity.Scene.Content.LoadTexture(TexturPlats);

            BoxKollision = Entity.AddComponent(new BoxCollider(-20, -31, 40, 63));
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

            if (attackknapp.IsPressed) {
                Attack = true;
            }

            if(Attack == true){
                animation = "randy-lättattack";
                AttackTimer += Time.UnscaledDeltaTime;

                if (AttackTimer >= 0.3f) {
                        Attack = false;
                        AttackTimer = AttackTimerNollstälare;
                }
            }
            //animation = "doug-stilla";


            if (moveDir.Y < 0 || moveDir.Y > 0) {
                if (animation != "randy-gång") {
                    animation = "randy-gång";
                }
            }
            if (moveDir.X < 0 ) {
                RandyRiktning = Riktning.vänster;
                if (animation != "randy-gång") {
                    animation = "randy-gång";
                }
            }
            if (moveDir.X > 0 ) {
                RandyRiktning = Riktning.höger;
                if (animation != "randy-gång") {
                    animation = "randy-gång";
                }
            }
            if(moveDir.X == 0 && moveDir.Y == 0 && Attack == false) {
                animation = "randy-stilla";
            }

            if (RandyRiktning == Riktning.höger) {
                Animerare.FlipX = false;
            }
            if (RandyRiktning == Riktning.vänster) {
                Animerare.FlipX = true;
            }
            /*            else {
                            Animerare.Play("doug-stilla", SpriteAnimator.LoopMode.Loop);
                        }*/





            if (moveDir != Vector2.Zero) {
                /**if (!Animerare.IsAnimationActive(animation)) {
                    Animerare.Play(animation);
                }
                else {
                    Animerare.UnPause();
                }**/

                Vector2 movement = moveDir * RörelseHastighet * Time.UnscaledDeltaTime;

                Röraren.CalculateMovement(ref movement, out CollisionResult res);
                SubPixelVecTvå.Update(ref movement);
                Röraren.ApplyMovement(movement);
            }
            if (!Animerare.IsAnimationActive(animation)) {
                Animerare.Play(animation);
            }
            else {
                Animerare.UnPause();
            }
            /*else {

                Animerare.Pause();
            }*/

            StandardScen standardScen = Entity.Scene as StandardScen;
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
