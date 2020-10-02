
using Microsoft.Xna.Framework;

using Nez;

namespace CocaineCrackDown.Entiteter {

    public abstract class SpelObjekt : Entity, ISpelObjekt {

        protected bool Spawnad { get; private set; }

        private Vector2 SpawnPosition { get; set; }

        public Vector2 Hastighet { get; set; }

        public Vector2 Storlek { get; set; }

        protected SpelObjekt(float x, float y) {
            SpawnPosition = new Vector2(x, y);
        }

        public abstract void VidDespawn();

        public abstract void VidSpawn();
        public abstract void Uppdatera();

        public override void OnAddedToScene() {
            Position = SpawnPosition;
            if(!Spawnad) {
                VidSpawn();
                Spawnad = true;
            }
        }

        public override void OnRemovedFromScene() {
            base.OnRemovedFromScene();
            VidDespawn();
        }

        public override void Update() {
            base.Update();
            Uppdatera();
        }

    }
}