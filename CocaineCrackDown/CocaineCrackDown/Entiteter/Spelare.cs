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
        public Spelare(string namn) {
            Namn = namn;
        }
        public bool Lokal { get; set; }
        public override void OnAddedToScene() {
            Name = Namn;
            inmatningsHanterare = new InmatningsHanterare();
            Core.RegisterGlobalManager(inmatningsHanterare);
            AddComponent(new RörelseKomponent(inmatningsHanterare , RörelseHastighet));
            AddComponent(new AtlasAnimationKomponent(inmatningsHanterare));
            
            //AddComponent<>()
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
