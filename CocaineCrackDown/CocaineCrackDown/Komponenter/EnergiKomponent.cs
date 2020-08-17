using Nez;

using System;

namespace CocaineCrackDown.Komponenter {
    
    public class EnergiKomponent : Component {
        private float energi;
        private const float maxEnergi = 100;
        public float Energi {
            get => energi;
            set {
                float klämnd = Mathf.Clamp(value, 0, maxEnergi);
                if (energi != klämnd) {
                    NärEnergiÄndras?.Invoke(klämnd);
                }
                energi = klämnd;
            }
        }

        public event NärEnergiÄndrasHanterare NärEnergiÄndras;
        public delegate void NärEnergiÄndrasHanterare(float NytVärde);

        public void EnergiKonsumption(float EnergiKonsumptionMängd) {
            Energi -= Math.Max(0, EnergiKonsumptionMängd);
        }
    }
}