using System;
using System.Collections.Generic;
using System.Text;

using CocaineCrackDown.Client.Managers;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Nez;
using Nez.Sprites;

namespace CocaineCrackDown.Komponenter {
    public class AtlasAnimationKomponent : Component, IUpdatable {
        public InmatningsHanterare Inmatnings;
        protected const float AttackTimerNollstälare = 0f;
        protected SpriteAtlas Atlas;

        protected SpriteAnimator Animerare;

        protected SpriteAnimator.LoopMode AnimationUppspelningsTyp;
        public bool Attackerar;
        private string animation = "doug-stilla";
        private Riktning DougRiktning;
        private float AttackTimer;

        public AtlasAnimationKomponent(InmatningsHanterare inmatnings) {
            Inmatnings = inmatnings;
        }
        public override void OnAddedToEntity() {
            Atlas = Entity.Scene.Content.LoadSpriteAtlas("Content/doug.atlas");
            Entity.Scene.Content.LoadTexture("Content/doug.png");
            Animerare = Entity.AddComponent<SpriteAnimator>();
            Animerare.AddAnimationsFromAtlas(Atlas);
        }
        public void Update() {
            Vector2 moveDir = new Vector2(Inmatnings.RörelseAxelX.Value , Inmatnings.RörelseAxelY.Value);

            if(Inmatnings.AttackKnapp.IsPressed) {
                Attackerar = true;
            }

            if(Attackerar == true) {
                animation = "doug-lättattack";
                AttackTimer += Time.UnscaledDeltaTime;

                if(AttackTimer >= 0.3f) {
                    Attackerar = false;
                    AttackTimer = AttackTimerNollstälare;
                }
            }

            if(moveDir.Y < 0 || moveDir.Y > 0) {
                if(animation != "doug-gång") {
                    animation = "doug-gång";
                }
            }
            if(moveDir.X < 0) {
                DougRiktning = Riktning.vänster;
                if(animation != "doug-gång") {
                    animation = "doug-gång";
                }
            }
            if(moveDir.X > 0) {
                DougRiktning = Riktning.höger;
                if(animation != "doug-gång") {
                    animation = "doug-gång";
                }
            }
            if(moveDir.X == 0 && moveDir.Y == 0 && Attackerar == false) {
                if(animation != "doug-stilla") {
                    animation = "doug-stilla";
                }
            }

            if(DougRiktning == Riktning.höger) {
                Animerare.FlipX = false;
            }
            if(DougRiktning == Riktning.vänster) {
                Animerare.FlipX = true;
            }

            if(!Animerare.IsAnimationActive(animation)) {
                Animerare.Play(animation);
            }
            else {
                Animerare.UnPause();
            }
        }
    }
}
