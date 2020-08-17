using Nez;

using System;

namespace CocaineCrackDown.Komponenter {

    public class FörsvarKomponent : Component {

        private float försvar;
        public const float basFörsvar = 40;
        
        public float Försvar {
            get => försvar;
            set {
                float klämnd = Mathf.Clamp(value, basFörsvar, 999);
                if (försvar != klämnd) {
                    NärFörsvarÄndras?.Invoke(klämnd);
                }
                försvar = klämnd;
            }
        }

        public event NärFörsvarÄndrasHanterare NärFörsvarÄndras;
        public delegate void NärFörsvarÄndrasHanterare(float NytVärde);
    }

}
