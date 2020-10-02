using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

using CocaineCrackDown.Client.Managers;
using CocaineCrackDown.Komponenter;

using Microsoft.Xna.Framework;

using Nez;
using Nez.Sprites;
using Nez.Tiled;

namespace CocaineCrackDown.Entiteter {
    public class Spelare : Entity {
        public float RörelseHastighet { get; set; } = 100f;
        protected string Namn { get; set; }
        public double SenasteUpdateringsTid { get; set; }
        public Scene Scen { get; set; }
        public InmatningsHanterare inmatningsHanterare;
        public RörelseKomponent rörelseKomponent;
        public AtlasAnimationKomponent atlasAnimationsKomponent;
        public FollowCamera followCamera;
        public SpelarData LokalSpelarData { get; set; }

        public SpelarData NySpelarData { get; set; }
        public SpelarData GammalSpelarData{ get; set; }
        public bool LokalSpelare { get; set; }
        public Spelare(string namn) {
            Namn = namn;
            inmatningsHanterare = new InmatningsHanterare();
            rörelseKomponent = new RörelseKomponent(inmatningsHanterare , RörelseHastighet);
            atlasAnimationsKomponent = new AtlasAnimationKomponent(inmatningsHanterare);
            followCamera = new FollowCamera(this);
            LokalSpelare = true;
            Core.RegisterGlobalManager(inmatningsHanterare);
        }
        public Spelare(SpelarData spelarData, string namn){
            NySpelarData = spelarData;
            Namn = namn;
            rörelseKomponent = new RörelseKomponent(inmatningsHanterare , RörelseHastighet);
            atlasAnimationsKomponent = new AtlasAnimationKomponent(inmatningsHanterare);
            LokalSpelare = false;
        }

        public override void OnAddedToScene() {
            Name = Namn;
            if(LokalSpelare) {
                Position = new Vector2(Scene.SceneRenderTargetSize.X / 2 , Scene.SceneRenderTargetSize.Y / 2);
                AddComponent(rörelseKomponent);
                AddComponent(atlasAnimationsKomponent);
                AddComponent(followCamera);
 
            }
            if(!LokalSpelare) {
                Position = new Vector2(Scene.SceneRenderTargetSize.X / 3 , Scene.SceneRenderTargetSize.Y / 3);
                AddComponent(rörelseKomponent);
                AddComponent(atlasAnimationsKomponent);
                GammalSpelarData = NySpelarData;
            }
           Entity Map = Scene.FindEntity("testnr1");
           Parent = Map.Transform;
            
        }

        public override void OnRemovedFromScene() {
            base.OnRemovedFromScene();
        }

        public override void Update() {
            base.Update();
            if(NySpelarData != SpelarData.SpelarDataNull && NySpelarData != GammalSpelarData) {
                Vector2 moveDir = new Vector2(NySpelarData.X , NySpelarData.Y);
                AtlasAnimationsKomponentUppdatera(moveDir);
                RörelseKomponentUppdatera(moveDir);
                
            }
            if(LokalSpelare) {
                Vector2 moveDir = new Vector2(inmatningsHanterare.RörelseAxelX.Value , inmatningsHanterare.RörelseAxelY.Value);
                AtlasAnimationsKomponentUppdatera(moveDir);
                RörelseKomponentUppdatera(moveDir);
            }


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
            if(LokalSpelare && atlasAnimationsKomponent.Inmatnings.AttackKnapp.IsPressed) {
                atlasAnimationsKomponent.Attackerar = true;
            }
            if(!LokalSpelare && NySpelarData.Attack) {
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
}
