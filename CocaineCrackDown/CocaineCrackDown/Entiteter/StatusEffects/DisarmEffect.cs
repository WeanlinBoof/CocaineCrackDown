using CocaineCrackDown.Entiteter.osorterat.Players;

namespace CocaineCrackDown.Entiteter.osorterat.StatusEffects {
    public class DisarmEffect : StatusEffect {
        public DisarmEffect(float durationSeconds) : base(durationSeconds) { }

        protected override void Effect(Player player) {
            // nope
        }

        protected override void OnEffectAdded(Player player) {
            player.Disarmed = true;
        }

        protected override void OnEffectRemoved(Player player) {
            player.Disarmed = false;
        }
    }
}
