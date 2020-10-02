using Nez;

namespace CocaineCrackDown.Verktyg {

    public class Cooldown {

        private readonly bool InteSkalad;

        private readonly float Längd;

        private float CoolDownTimer;

        public Cooldown(float duration , bool unscaled = false) {
            InteSkalad = unscaled;
            Längd = duration;
        }

        public void Start() {
            CoolDownTimer = Längd;
        }

        public bool IsReady() {
            return !IsOnCooldown();
        }

        public bool IsOnCooldown() {
            return CoolDownTimer > 0;
        }

        public void Reset() {
            CoolDownTimer = 0;
        }

        public float ElapsedNormalized() {
            return CoolDownTimer / Längd;
        }

        public void Update() {
            if(CoolDownTimer > 0) {
                CoolDownTimer -= InteSkalad ? Time.UnscaledDeltaTime : Time.DeltaTime;
            }
            else {
                CoolDownTimer = 0;
            }
        }
    }
}