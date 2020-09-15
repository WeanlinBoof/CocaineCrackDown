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
            Position = new Vector2(Scene.SceneRenderTargetSize.X/2, Scene.SceneRenderTargetSize.Y / 2);
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
