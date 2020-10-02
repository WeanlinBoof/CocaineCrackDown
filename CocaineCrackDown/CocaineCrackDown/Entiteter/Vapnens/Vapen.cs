
using CocaineCrackDown.Verktyg;

namespace CocaineCrackDown.Entiteter {

    public abstract class Vapen : SpelObjekt {

        private readonly Spelare Spelare;

        public abstract CollectibleMetadata Metadata { get; }

        public abstract VapenParameters Parameters { get; }

        public Cooldown Cooldown { get; set; }

        public string Namn => Parameters.Namn;

        protected Vapen(Spelare spelare) : base(0 , 0) {
            Name = Namn;
            Spelare = spelare;
        }

        public override void VidDespawn() {
        }

        public override void VidSpawn() {
            Cooldown.Start();
            UpdateOrder = 1;
        }

        public override void Uppdatera() {
            Cooldown.Update();
        }

        public void Förinta() {
            RemoveAllComponents();
            SetEnabled(false);
            Destroy();
        }

        public virtual void VäxlaSpringLäge(bool Springer) {
        }

        public virtual void Attack() {
        }
    }
}