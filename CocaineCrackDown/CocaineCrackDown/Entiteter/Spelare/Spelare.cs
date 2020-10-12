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
        public Vector2 Spawn;
        public TiledMap Map;
        public Spelare(string namn) {
            Namn = namn;
            inmatningsHanterare = new InmatningsHanterare();
            rörelseKomponent = new RörelseKomponent(inmatningsHanterare , RörelseHastighet);
            atlasAnimationsKomponent = new AtlasAnimationKomponent(inmatningsHanterare);
            followCamera = new FollowCamera(this);
            
            Core.RegisterGlobalManager(inmatningsHanterare);
        }
        public override void OnAddedToScene() {
            Name = Namn;
            Map = (TiledMap)Scene.FindEntity("testnr1");
            //bewrode på vilken spelare så spawnar den på respective plats 
            Spawn = Name switch {
                "doug" => new Vector2(Map.SpawnSpelareEtt.X , Map.SpawnSpelareEtt.Y),
                "randy" => new Vector2(Map.SpawnSpelareTvå.X , Map.SpawnSpelareTvå.Y),
                _ => new Vector2(Map.Position.X , Map.Position.Y),
            };
            Parent = Map.Transform;
            Position = Spawn;
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
