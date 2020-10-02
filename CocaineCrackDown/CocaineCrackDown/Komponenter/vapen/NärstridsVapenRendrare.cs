using CocaineCrackDown.Entiteter;
using CocaineCrackDown.Komponenter;

using Nez;
using Nez.Sprites;


namespace CocaineCrackDown.Komponenter {

    public class NärstridsVapenRendrare : VapenRendrare {

        private NärstridsVapenParameters parametrar;

        private NärstridsVapen NärstridsVapen;

        private float renderOffset;

        public AtlasAnimationKomponent<NärstridsVapnenAnimationer> atlasAnimationsKomponent;

        public NärstridsVapenRendrare(NärstridsVapen melee , Spelare spelare) : base(spelare) {
            NärstridsVapen = melee;
            parametrar = melee.Parameters as NärstridsVapenParameters;
            renderOffset = NärstridsVapen.Parameters.RenderOffset;
        }

        public override void OnAddedToEntity() {
            base.OnAddedToEntity();
            Animerare = Entity.GetComponent<SpriteAnimator>();
            Entity.SetScale(NärstridsVapen.Parameters.Skala);
            SetUpdateOrder(1);

            atlasAnimationsKomponent = Entity.AddComponent(new AtlasAnimationKomponent<NärstridsVapnenAnimationer>());

            atlasAnimationsKomponent.animation = $"{Entity.Name}-{NärstridsVapnenAnimationer.utrustadestilla}";
        }

        public override void Attack() {
            if(Spelare.atlasAnimationsKomponent.Attackerar && Enabled) {
                atlasAnimationsKomponent.animation = $"{Entity.Name}-{NärstridsVapnenAnimationer.utrustadelättattack}";
            }
        }

        public override void Update() {
            atlasAnimationsKomponent.Animerare.FlipX = Spelare.atlasAnimationsKomponent.Animerare.FlipX;
            if(!atlasAnimationsKomponent.Animerare.IsAnimationActive(atlasAnimationsKomponent.animation)) {
                atlasAnimationsKomponent.Animerare.Play(atlasAnimationsKomponent.animation);
            }
            else {
                atlasAnimationsKomponent.Animerare.UnPause();
            }
        }
    }
}