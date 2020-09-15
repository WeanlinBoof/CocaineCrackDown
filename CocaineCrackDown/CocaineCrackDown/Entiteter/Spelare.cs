using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

using CocaineCrackDown.Client.Managers;
using CocaineCrackDown.Komponenter;

using Microsoft.Xna.Framework;

using Nez;
using Nez.Sprites;

namespace CocaineCrackDown.Entiteter {
    public class Spelare : Entity {
        public float RörelseHastighet { get; set; } = 100f;
        protected string Namn { get; set; }
        public double SenasteUpdateringsTid { get; set; }
        public Scene Scen { get; set; }
        protected InmatningsHanterare inmatningsHanterare;
        protected RörelseKomponent rörelseKomponent;
        protected AtlasAnimationKomponent atlasAnimationsKomponent;
        protected FollowCamera followCamera;
        public Spelare(string namn) {
            Namn = namn;
            inmatningsHanterare = new InmatningsHanterare();
            rörelseKomponent = new RörelseKomponent(inmatningsHanterare , RörelseHastighet);
            atlasAnimationsKomponent = new AtlasAnimationKomponent(inmatningsHanterare);
            followCamera = new FollowCamera(this);
            Core.RegisterGlobalManager(inmatningsHanterare);
        }
        public bool Lokal { get; set; }
        public override void OnAddedToScene() {
            Name = Namn;
            Position = new Vector2(Scene.SceneRenderTargetSize.X / 2 , Scene.SceneRenderTargetSize.Y / 2);
            //AddComponent(new KollisionsKomponent());
            AddComponent(rörelseKomponent);
            AddComponent(atlasAnimationsKomponent);
            AddComponent(followCamera);
        }

        public override void OnRemovedFromScene() {
            base.OnRemovedFromScene();
        }

        public override void Update() {
            base.Update();
            Vector2 moveDir = new Vector2(inmatningsHanterare.RörelseAxelX.Value , inmatningsHanterare.RörelseAxelY.Value);
            AtlasAnimationsKomponentUppdatera(moveDir);
            RörelseKomponentUppdatera(moveDir);
        }

        private void RörelseKomponentUppdatera(Vector2 moveDir) {
            if(moveDir != Vector2.Zero) {

                Vector2 movement = moveDir * RörelseHastighet * Time.DeltaTime;

                rörelseKomponent.Röraren.CalculateMovement(ref movement , out CollisionResult res);
                rörelseKomponent.V2Pixel.Update(ref movement);
                rörelseKomponent.Röraren.ApplyMovement(movement);
            }
        }

        private void AtlasAnimationsKomponentUppdatera(Vector2 moveDir) {

            if(atlasAnimationsKomponent.Inmatnings.AttackKnapp.IsPressed) {
                atlasAnimationsKomponent.Attackerar = true;
            }
            atlasAnimationsKomponent.AttackBox.Enabled = atlasAnimationsKomponent.Attackerar;
            if(atlasAnimationsKomponent.Attackerar) {
                atlasAnimationsKomponent.animation = $"{Name}-lättattack";
                atlasAnimationsKomponent.AttackTimer += Time.UnscaledDeltaTime;
                if(atlasAnimationsKomponent.AttackTimer >= 0.3f) {
                    atlasAnimationsKomponent.Attackerar = false;
                    atlasAnimationsKomponent.AttackTimer = AtlasAnimationKomponent.AttackTimerNollstälare;
                }
            }

            if(moveDir.Y < 0 || moveDir.Y > 0) {
                if(atlasAnimationsKomponent.animation != $"{Name}-gång") {
                    atlasAnimationsKomponent.animation = $"{Name}-gång";
                }
            }
            if(moveDir.X < 0) {
                atlasAnimationsKomponent.DougRiktning = Riktning.vänster;
                if(atlasAnimationsKomponent.animation != $"{Name}-gång") {
                    atlasAnimationsKomponent.animation = $"{Name}-gång";
                }
            }
            if(moveDir.X > 0) {
                atlasAnimationsKomponent.DougRiktning = Riktning.höger;
                if(atlasAnimationsKomponent.animation != $"{Name}-gång") {
                    atlasAnimationsKomponent.animation = $"{Name}-gång";
                }
            }
            if(moveDir.X == 0 && moveDir.Y == 0 && atlasAnimationsKomponent.Attackerar == false) {
                if(atlasAnimationsKomponent.animation != $"{Name}-stilla") {
                    atlasAnimationsKomponent.animation = $"{Name}-stilla";
                }
            }
            if(atlasAnimationsKomponent.DougRiktning == Riktning.höger) {
                atlasAnimationsKomponent.Animerare.FlipX = false;
                //fixa brugh
                atlasAnimationsKomponent.AttackBox.SetLocalOffset(new Vector2(5 , -15));

            }
            if(atlasAnimationsKomponent.DougRiktning == Riktning.vänster) {
                atlasAnimationsKomponent.Animerare.FlipX = true;
                //fixa bruhg
                atlasAnimationsKomponent.AttackBox.SetLocalOffset(new Vector2(-5, -15));
            }

            if(!atlasAnimationsKomponent.Animerare.IsAnimationActive(atlasAnimationsKomponent.animation)) {
                atlasAnimationsKomponent.Animerare.Play(atlasAnimationsKomponent.animation);
            }
            else {
                atlasAnimationsKomponent.Animerare.UnPause();
            }
        }
    }
    public class Doug : Spelare {
        public Doug(string namn = "doug" , bool lokal = false) : base(namn) {
            Lokal = lokal;
        }
    }

    public class Randy : Spelare {
        public Randy(string namn = "randy", bool lokal = false) : base(namn) {
            Lokal = lokal;
        }
    }
}
