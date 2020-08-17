using Nez;

using System;

namespace CocaineCrackDown.Komponenter {
    
    public class RörelseKomponent : Component {
        private float rörelseHastighet;

        private const float basRörelseHastighet = 100;

        private const float maxRörelseHastighet = 1000;


        public float RörelseHastighet {
            get => rörelseHastighet;
            set {
                float klämnd = Mathf.Clamp(value, basRörelseHastighet, maxRörelseHastighet);
                if (rörelseHastighet != klämnd) {
                    NärHastighetÄndras?.Invoke(klämnd);
                }
                rörelseHastighet = klämnd;
            }
        }

        public event NärHastighetÄndrasHanterare NärHastighetÄndras;
        public delegate void NärHastighetÄndrasHanterare(float NytVärde);

    }
}