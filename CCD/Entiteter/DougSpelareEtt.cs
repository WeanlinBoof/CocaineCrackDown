﻿using CocaineCrackDown.Scener;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Nez;
using Nez.AI.FSM;
using Nez.Sprites;
using Nez.Timers;


namespace CocaineCrackDown.Entiteter {

    public enum Riktning {

        höger,

        vänster,

        upp,

        ner,
    }

    public class DougSpelareEtt : Entitet, IUpdatable {

        public DougSpelareEtt(string namn = "doug") : base(namn) {
        }

        private SubpixelVector2 SubPixelVecTvå = new SubpixelVector2();

        private VirtualButton attackknapp;

        private VirtualIntegerAxis XAxisknappar;

        private VirtualIntegerAxis YAxisknappar;

        //private UnscaledDeltaTime UnscaledDeltaTime;

        private string animation = "doug-stilla";

        private Riktning DougRiktning;

        private bool Attack;

        private float AttackTimer = 0f;

        public override void OnAddedToEntity() {
            SpriteAtlas AtlasTextur = Entity.Scene.Content.LoadSpriteAtlas("Content/doug.atlas");
            Texture2D Textur = Entity.Scene.Content.LoadTexture(TexturPlats);

            Kollision = Entity.AddComponent(new BoxCollider(-20, -31, 40, 63));
            Röraren = Entity.AddComponent(new Mover());
            Animerare = Entity.AddComponent<SpriteAnimator>();

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

        public void Update() {

            // handle movement and animations
            Vector2 moveDir = new Vector2(XAxisknappar.Value, YAxisknappar.Value);

            if (attackknapp.IsPressed) {
                    animation = "doug-lättattack";



                if (AttackTimer >= 0.6f) {
                        Attack = false;
                        AttackTimer = AttackTimerNollstälare;
                }
            }
            //animation = "doug-stilla";


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

            if (DougRiktning == Riktning.höger) {
                Animerare.FlipX = false;
            }
            if (DougRiktning == Riktning.vänster) {
                Animerare.FlipX = true;
            }
            /*            else {
                            Animerare.Play("doug-stilla", SpriteAnimator.LoopMode.Loop);
                        }*/





            if (moveDir != Vector2.Zero) {
                if (!Animerare.IsAnimationActive(animation)) {
                    Animerare.Play(animation);
                }
                else {
                    Animerare.UnPause();
                }

                Vector2 movement = moveDir * RörelseHastighet * Time.UnscaledDeltaTime;

                Röraren.CalculateMovement(ref movement, out CollisionResult res);
                SubPixelVecTvå.Update(ref movement);
                Röraren.ApplyMovement(movement);
            }
            if (Input.IsKeyDown(Keys.H) == false) {
                if (!Animerare.IsAnimationActive(animation)) {
                    Animerare.Play(animation);
                }
                else {
                    Animerare.UnPause();
                }
            } 
            else {

                Animerare.Pause();
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