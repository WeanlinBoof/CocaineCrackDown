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
        public const float AttackTimerNollstälare = 0f;
        public SpriteAtlas Atlas;
        //lägg till mer sen :)
        public BoxCollider StandardBox;
        public BoxCollider AttackBox;
        public AtlasAnimationKomponent anitmation;
        public  SpriteAnimator Animerare;

        protected SpriteAnimator.LoopMode AnimationUppspelningsTyp;
        public bool Attackerar;
        public string animation = "doug-stilla";
        public Riktning DougRiktning;
        public float AttackTimer;

        public AtlasAnimationKomponent(InmatningsHanterare inmatnings) {
            Inmatnings = inmatnings;
        }
        public override void OnAddedToEntity() {
            Atlas = Entity.Scene.Content.LoadSpriteAtlas("Content/doug.atlas");
            Entity.Scene.Content.LoadTexture("Content/doug.png");
            Animerare = Entity.AddComponent<SpriteAnimator>();
            Animerare.SetRenderLayer(1);
            StandardBox = Entity.AddComponent(new BoxCollider(-20 , -31 , 40 , 63));
            AttackBox = Entity.AddComponent(new BoxCollider(-20 , -31 , 50 , 31));
            AttackBox.IsTrigger = true;
            Animerare.AddAnimationsFromAtlas(Atlas);
        }
        public void Update() {
            Vector2 moveDir = new Vector2(Inmatnings.RörelseAxelX.Value , Inmatnings.RörelseAxelY.Value);

            if(Inmatnings.AttackKnapp.IsPressed) {
                Attackerar = true;
            }
            AttackBox.Enabled = Attackerar;
            if(Attackerar) {
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
                //fixa brugh
                AttackBox.SetLocalOffset(new Vector2(-20 ,-31));

            }
            if(DougRiktning == Riktning.vänster) {
                Animerare.FlipX = true;
                //fixa bruhg
                AttackBox.SetLocalOffset(new Vector2(-4*0 , -31));
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
