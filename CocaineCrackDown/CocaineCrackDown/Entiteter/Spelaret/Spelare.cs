using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

using CocaineCrackDown.Client.Managers;
using CocaineCrackDown.Entiteter.Gestalter;
using CocaineCrackDown.Komponenter;
using CocaineCrackDown.System;

using Microsoft.Xna.Framework;

using Nez;
using Nez.Sprites;
using Nez.Tiled;

using Riktning = CocaineCrackDown.Komponenter.Riktning;

namespace CocaineCrackDown.Entiteter {
    public class Spelare : Entity {
        protected SpelarKollitionsHanterare SpelarKollitionsHanterare;
        protected CameraTracker KamraSpåre;
        protected SpelarInventory Inventory;
        protected Collider NärhetBox;
        protected Collider StandarBox;
        protected BloodEngine Blod;
        protected SpelSystem SpelSystem;
        protected List<Entity> EntiteterINärhet;
        public SpelarRörlighetsTillstånd SpelareRörlighetsTillstånd { get; set; }
        public SpelarTillstånd SpelareTillstånd { get; set; }
        public GestaltParameter Parameter { get; set; }
        public float RörelseHastighet { get; set; } = 100f;
        protected string Namn { get; set; }
        public double SenasteUpdateringsTid { get; set; }
        public Scene Scen { get; set; }
        public Riktning Riktning;
        public SpelarRörlighetsTillstånd spelarRörlighetsTillstånd;
        protected InmatningsHanterare inmatningsHanterare;
        protected RörelseKomponent rörelseKomponent;
        public AtlasAnimationKomponent<Animationer> atlasAnimationsKomponent;
        protected FollowCamera followCamera;
        public Spelare(string namn) {
            Namn = namn;
            inmatningsHanterare = new InmatningsHanterare();
            rörelseKomponent = new RörelseKomponent(inmatningsHanterare , RörelseHastighet);
            atlasAnimationsKomponent = new AtlasAnimationKomponent<Animationer>(inmatningsHanterare);
            followCamera = new FollowCamera(this);
            EntiteterINärhet = new List<Entity>();
            Core.RegisterGlobalManager(inmatningsHanterare);
        }
        public override void OnAddedToScene() {
            Name = Namn;
            Position = new Vector2(Scene.SceneRenderTargetSize.X / 2 , Scene.SceneRenderTargetSize.Y / 2);
            AddComponent(rörelseKomponent);
            AddComponent(atlasAnimationsKomponent);
            AddComponent(followCamera);
            Entity Map = Scene.FindEntity("testnr1");
            Parent = Map.Transform;
        }
        public void Damage(NärstridsVapen närstridsVapen) {
            throw new NotImplementedException();
        }
        public void EquipWeapon(VapenParameters vapen , CollectibleMetadata metadata) {
            throw new NotImplementedException();
        }

        public override void VidDespawn() {
            throw new NotImplementedException();
        }

        public override void VidSpawn() {
            throw new NotImplementedException();
        }

        public override void Uppdatera() {
            Vector2 moveDir = new Vector2(inmatningsHanterare.RörelseAxelX.Value , inmatningsHanterare.RörelseAxelY.Value);
            AtlasAnimationsKomponentUppdatera(moveDir);
            RörelseKomponentUppdatera(moveDir);
        }
        private void AtlasAnimationsKomponentUppdatera(Vector2 moveDir) {

            if(atlasAnimationsKomponent.Inmatnings.AttackKnapp.IsPressed) {
                atlasAnimationsKomponent.Attackerar = true;
            }
            atlasAnimationsKomponent.AttackBox.Enabled = atlasAnimationsKomponent.Attackerar;
            if(atlasAnimationsKomponent.Attackerar) {
                atlasAnimationsKomponent.animation = $"{Name}-{Animationer.lättattack}";
                atlasAnimationsKomponent.AttackTimer += Time.UnscaledDeltaTime;
                if(atlasAnimationsKomponent.AttackTimer >= 0.3f) {
                    atlasAnimationsKomponent.Attackerar = false;
                    atlasAnimationsKomponent.AttackTimer = AtlasAnimationKomponent<Animationer>.AttackTimerNollstälare;
                }
            }

            if(moveDir.Y < 0 || moveDir.Y > 0) {
                if(atlasAnimationsKomponent.animation != $"{Name}-{Animationer.gång}") {
                    atlasAnimationsKomponent.animation = $"{Name}-{Animationer.gång}";
                }
            }
            if(moveDir.X < 0) {
                atlasAnimationsKomponent.Riktning = Riktning.vänster;
                if(atlasAnimationsKomponent.animation != $"{Name}-{Animationer.gång}") {
                    atlasAnimationsKomponent.animation = $"{Name}-{Animationer.gång}";
                }
            }
            if(moveDir.X > 0) {
                atlasAnimationsKomponent.Riktning = Riktning.höger;
                if(atlasAnimationsKomponent.animation != $"{Name}-{Animationer.gång}") {
                    atlasAnimationsKomponent.animation = $"{Name}-{Animationer.gång}";
                }
            }
            if(moveDir.X == 0 && moveDir.Y == 0 && !atlasAnimationsKomponent.Attackerar && atlasAnimationsKomponent.animation != $"{Name}-{Animationer.stilla}") {
                atlasAnimationsKomponent.animation = $"{Name}-{Animationer.stilla}";
            }
            if(atlasAnimationsKomponent.Riktning == Riktning.höger) {
                Riktning = atlasAnimationsKomponent.Riktning;
                atlasAnimationsKomponent.Animerare.FlipX = false;
                //fixa brugh
                atlasAnimationsKomponent.AttackBox.SetLocalOffset(new Vector2(5 , -15));
            }
            if(atlasAnimationsKomponent.Riktning == Riktning.vänster) {
                Riktning = atlasAnimationsKomponent.Riktning;
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
    
        private void RörelseKomponentUppdatera(Vector2 moveDir) {
            if(moveDir != Vector2.Zero) {

                Vector2 movement = moveDir * RörelseHastighet * Time.DeltaTime;

                rörelseKomponent.Röraren.CalculateMovement(ref movement , out CollisionResult res);
                rörelseKomponent.V2Pixel.Update(ref movement);
                rörelseKomponent.Röraren.ApplyMovement(movement);
            }
        }
    }
}
