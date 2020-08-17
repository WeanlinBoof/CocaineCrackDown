using Nez;

using System;

namespace CocaineCrackDown.Komponenter {
    
    public class hpKomponent : Component {
        private float hälsoPoäng;
        private const float maxHälsoPoäng = 100;

        public float HälsoPoäng {
            get => hälsoPoäng;
            set {

                //tar värde, minsta värde möjligt och max hälsa för att kolla så att man inte går över 100hp eller under 0 hp
                float klämnd = Mathf.Clamp(value, 0, maxHälsoPoäng);
                if (hälsoPoäng != klämnd) {
                    NärHälsoPoängÄndras?.Invoke(klämnd);
                }

                hälsoPoäng = klämnd;
            }
        }

       public event NärHälsoPoängÄndrasHanterare NärHälsoPoängÄndras;
        public delegate void NärHälsoPoängÄndrasHanterare(float NytVärde);
        public void Skada(float SkadeMängd) {
            SkadeMängd -= FörsvarKomponent.basFörsvar;
            if (SkadeMängd >= 0) {
                HälsoPoäng -= SkadeMängd;
            }
            else {
                return;
            }
        }

        public void Helning(float HelningMängd) {
            HälsoPoäng += Math.Max(0, HelningMängd);
        }
    }
}