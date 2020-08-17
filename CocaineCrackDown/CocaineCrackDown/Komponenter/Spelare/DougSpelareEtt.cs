using CocaineCrackDown.Scener;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Nez;
using Nez.AI.FSM;
using Nez.Sprites;
using Nez.Timers;


namespace CocaineCrackDown.Komponenter.Spelare {

    public class DougSpelareEtt : Entitet, IUpdatable {

        public DougSpelareEtt() : base("doug") {
        }

        private SubpixelVector2 SubPixelVecTvå = new SubpixelVector2();

        private VirtualButton attackknapp;

        private VirtualIntegerAxis XAxisknappar;

        private VirtualIntegerAxis YAxisknappar;

        //private UnscaledDeltaTime UnscaledDeltaTime;

        private string animation = "doug-stilla";

        private Riktning DougRiktning;

        private bool Attackerar;
        private float HälsoPoäng;
        public float RörelseHastighet;
        public override void OnAddedToEntity() {
            SpriteAtlas AtlasTextur = Entity.Scene.Content.LoadSpriteAtlas("Content/doug.atlas");
            Texture2D Textur = Entity.Scene.Content.LoadTexture(TexturPlats);
            BoxKollision = Entity.AddComponent(new BoxCollider(-20, -31, 40, 63));
            Röraren = Entity.AddComponent(new Mover());
            Animerare = Entity.AddComponent<SpriteAnimator>();
            //StatusVärde = Entity.AddComponent<StatusVärdeKomponent>();
            RörelseHastighet = 100f;
            Animerare.AddAnimationsFromAtlas(AtlasTextur);

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
       /* public bool DougÄrDöd() {
           HälsoPoäng = Entity.GetComponent<StatusVärdeKomponent>().HälsoPoäng;
            if (HälsoPoäng == 0) {
                return true;
            }
            else {
                return false;
            }
        }*/
        public void Update() {
            //DougÄrDöd();
            // handle movement and animations
            Vector2 moveDir = new Vector2(XAxisknappar.Value, YAxisknappar.Value);

            if (attackknapp.IsPressed) {
                Attackerar = true;
            }

            if (Attackerar == true) {
                animation = "doug-lättattack";
                AttackTimer += Time.UnscaledDeltaTime;

                if (AttackTimer >= 0.3f) {
                    Attackerar = false;
                    AttackTimer = AttackTimerNollstälare;
                }
            }

            if (moveDir.Y < 0 || moveDir.Y > 0) {
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
            if (moveDir.X == 0 && moveDir.Y == 0 && Attackerar == false) {
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




            if (moveDir != Vector2.Zero) {

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


            GrundScen standardScen = Entity.Scene as GrundScen;
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
