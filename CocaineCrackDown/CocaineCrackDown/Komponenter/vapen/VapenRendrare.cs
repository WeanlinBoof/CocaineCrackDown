using CocaineCrackDown.Entiteter;

using Microsoft.Xna.Framework;

using Nez;
using Nez.Sprites;

namespace CocaineCrackDown.Komponenter {

    public class VapenRendrare : Component, IUpdatable {

        private Vapen vapen;

        private float renderOffset;
        protected SpriteAnimator Animerare;
        public Spelare Spelare { get; set; }

        public bool SpringerSpelaren { get; set; }

        public VapenRendrare(Spelare spelare) {
            Spelare = spelare;
        }

        public void VäxlaSpringade(bool Springer) {
            SpringerSpelaren = Springer;
        }

        public virtual void Attack() {
        }

        public virtual void Update() {
        }
    }
}