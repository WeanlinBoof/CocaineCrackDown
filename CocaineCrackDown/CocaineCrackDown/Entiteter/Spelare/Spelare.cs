﻿using System;
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
        public bool Lokal { get; set; }
        public SpelarData LokalSpelarData { get; set; }

        public SpelarData NySpelarData { get; set; }
        public SpelarData GammalSpelarData{ get; set; }
        public bool LokalSpelare { get; set; }
        public Vector2 Spawn;
        public TiledMap Map;
        public Spelare(string namn,bool lokal) {
            Lokal = lokal;
            Namn = namn;
            inmatningsHanterare = new InmatningsHanterare();
            Core.RegisterGlobalManager(inmatningsHanterare);
            if(Lokal) {
                rörelseKomponent = new RörelseKomponent( RörelseHastighet,this,inmatningsHanterare);
                atlasAnimationsKomponent = new AtlasAnimationKomponent(inmatningsHanterare);
            }
            else {
                rörelseKomponent = new RörelseKomponent( RörelseHastighet,this);
                atlasAnimationsKomponent = new AtlasAnimationKomponent();
            }

            followCamera = new FollowCamera(this);
            
        }
        public override void OnAddedToScene() {
            Name = Namn;
            Map = (TiledMap)Scene.FindEntity("testnr1");
            //bewrode på vilken spelare så spawnar den på respective plats           
            if(Namn == "randy") {
                Spawn = new Vector2(Map.SpawnSpelareTvå.X , Map.SpawnSpelareTvå.Y);
            }
            if(Namn == "doug") {
                Spawn = new Vector2(Map.SpawnSpelareEtt.X , Map.SpawnSpelareEtt.Y);
            }
            Parent = Map.Transform;
            Position = Spawn;

            if(Lokal) {
                AddComponent(atlasAnimationsKomponent);
                AddComponent(rörelseKomponent);
                AddComponent(followCamera);
            }
 



            
        }

        public override void OnRemovedFromScene() {
            base.OnRemovedFromScene();
        }

        public override void Update() {
            base.Update();
            if(Lokal) {
              Vector2 moveDir = new Vector2(inmatningsHanterare.RörelseAxelX.Value , inmatningsHanterare.RörelseAxelY.Value);
               AtlasAnimationsKomponentUppdatera(moveDir);
                RörelseKomponentUppdatera(moveDir);

            }
            if(!Lokal) {
               //Vector2 moveDir = new Vector2(inmatningsHanterare.RörelseAxelX.Value , inmatningsHanterare.RörelseAxelY.Value);
               //AtlasAnimationsKomponentUppdatera(moveDir);
               //RörelseKomponentUppdatera(moveDir);

            }

        }

        private void RörelseKomponentUppdatera(Vector2 moveDir) {
            if(moveDir != Vector2.Zero) {
                                          
                Vector2 movement = moveDir * RörelseHastighet * Time.DeltaTime;

                rörelseKomponent.Röraren.BeräknaRörelse(ref movement , out CollisionResult res);
                rörelseKomponent.V2Pixel.Update(ref movement);
                rörelseKomponent.Röraren.TillämpaRörelsen(movement);
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
