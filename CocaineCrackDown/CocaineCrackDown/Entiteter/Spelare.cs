using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

using Microsoft.Xna.Framework;

using Nez;
using Nez.Sprites;

namespace CocaineCrackDown.Entiteter {
    public class Spelare : Entity {
        public Vector2 Rörelse { get; set; }
        protected string Namn { get; set; }
        public double SenasteUpdateringsTid { get; set; }
        public Scene Scen { get; set; }

        public Spelare(string namn) {
            Namn = namn;
        }
        public Spelare(Scene scene, string namn) :this(namn) {
            Scen = scene;
        }
        public override void OnAddedToScene() {
            Name = Namn;
            //AddComponent()
            //AddComponent<>()
        }

        public override void OnRemovedFromScene() {
            base.OnRemovedFromScene();
        }

        public override void Update() {
            base.Update();
        }

    }
}
