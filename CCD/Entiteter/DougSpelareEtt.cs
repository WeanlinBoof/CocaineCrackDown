using CocaineCrackDown.Scener;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Nez;
using Nez.AI.FSM;
using Nez.Sprites;
using Nez.Timers;

using System;

namespace CocaineCrackDown.Entiteter {

    public class DougSpelareEtt : Entitet, IUpdatable {

        public DougSpelareEtt(string namn = "doug", EntitetRelation entitetRelation = EntitetRelation.Hjälte) : base(namn, entitetRelation) {
            ///this.other = collider;
        }


        private SubpixelVector2 SubPixelVecTvå = new SubpixelVector2();

        private VirtualButton attackknapp;

        private VirtualIntegerAxis XAxisknappar;

        private VirtualIntegerAxis YAxisknappar;

        //private UnscaledDeltaTime UnscaledDeltaTime;

        private string animation = "doug-stilla";

        private Riktning DougRiktning;

        private bool Attack;


        public BoxCollider DougBoxKollision;

        private BoxCollider HejdukBoxKollision;

        public override void OnAddedToEntity() {
            SpriteAtlas AtlasTextur = Entity.Scene.Content.LoadSpriteAtlas("Content/doug.atlas");
            Texture2D Textur = Entity.Scene.Content.LoadTexture(TexturPlats);
            DougBoxKollision = Entity.AddComponent(new BoxCollider(-20, -31, 40, 63));
            Röraren = Entity.AddComponent(new Mover());
            Animerare = Entity.AddComponent<SpriteAnimator>();

            Animerare.AddAnimationsFromAtlas(AtlasTextur);
            HejdukBoxKollision = Entity.Scene.FindEntity("hejduk").GetComponent<BoxCollider>();

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



            // handle movement and animations
            Vector2 moveDir = new Vector2(XAxisknappar.Value, YAxisknappar.Value);

            if (attackknapp.IsPressed) {
                Attack = true;
            }

            if(Attack == true){
                animation = "doug-lättattack";
                AttackTimer += Time.UnscaledDeltaTime;

                if (AttackTimer >= 0.3f) {
                        Attack = false;
                        AttackTimer = AttackTimerNollstälare;
                }
            }

            if (moveDir.Y < 0 || moveDir.Y > 0) {
                if (animation != "doug-gång") {
                    animation = "doug-gång";
                }
            }
            if (moveDir.X < 0 ) {
                DougRiktning = Riktning.vänster;
                if (animation != "doug-gång") {
                    animation = "doug-gång";
                }
            }
            if (moveDir.X > 0 ) {
                DougRiktning = Riktning.höger;
                if (animation != "doug-gång") {
                    animation = "doug-gång";
                }
            }
            if(moveDir.X == 0 && moveDir.Y == 0 && Attack == false) {
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

            //har bara fixat för doug btw vet exaxt hur man ska fixa bara inte årkat
            bool Hejdkubool = DougBoxKollision.Overlaps(HejdukBoxKollision);
            if (Hejdkubool) {
                Console.WriteLine("doug och hejduk");

            }

            if (moveDir != Vector2.Zero) {

                Rörelse = moveDir * RörelseHastighet * Time.UnscaledDeltaTime;
                //Röraren.CalculateMovement(ref Rörelse, out CollisionResult res);
                SubPixelVecTvå.Update(ref Rörelse);
                Röraren.ApplyMovement(Rörelse);
            }
            if (!Animerare.IsAnimationActive(animation)) {
                Animerare.Play(animation);
            }
            else {
                Animerare.UnPause();
            }


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
