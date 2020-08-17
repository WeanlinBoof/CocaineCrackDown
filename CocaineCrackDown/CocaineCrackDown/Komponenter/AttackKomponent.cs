using Nez;

using System;

namespace CocaineCrackDown.Komponenter {
    
    public class AttackKomponent : Component {
        private float attack;
        private const float basAttack = 40;

        public float Attack {
            get => attack;
            set {
                float klämnd = Mathf.Clamp(value, basAttack, 999);
                if (attack != klämnd) {
                    NärAttackÄndras?.Invoke(klämnd);
                }
                attack = klämnd;
            }
        }
        public event NärAttackÄndrasHanterare NärAttackÄndras;
        public delegate void NärAttackÄndrasHanterare(float NytVärde);
    }


}