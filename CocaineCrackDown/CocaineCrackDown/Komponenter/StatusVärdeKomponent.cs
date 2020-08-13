using Nez;

using System;

namespace CocaineCrackDown.Komponenter {

    public class StatusVärdeKomponent : Component {

        private float försvar;

        private const float basFörsvar = 40;

        private float attack;

        private const float basAttack = 40;

        private float hälsoPoäng;

        private const float maxHälsoPoäng = 100;

        private float rörelseHastighet;

        private const float basRörelseHastighet = 100;

        private const float maxRörelseHastighet = 1000;

        private float energi;

        private const float maxEnergi = 100;

        public StatusVärdeKomponent() : this(100, 100, 40, 100, 40) { }
        public StatusVärdeKomponent(float StartHälsa = 100, float energi = 100, float attack = 40, float rörelsehastighet = 100, float försvar = 40) {
            hälsoPoäng = Math.Min(StartHälsa, maxHälsoPoäng);
            Försvar = försvar;
            Attack = attack;
            Energi = energi;
            RörelseHastighet = rörelsehastighet;
        }

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

        //event för när Hp ändras som skada eller healing
        public event NärHälsoPoängÄndrasHanterare NärHälsoPoängÄndras;

        public delegate void NärHälsoPoängÄndrasHanterare(float NytVärde);

        //event för hastighet
        public event NärHastighetÄndrasHanterare NärHastighetÄndras;

        public delegate void NärHastighetÄndrasHanterare(float NytVärde);

        //event för energi
        public event NärEnergiÄndrasHanterare NärEnergiÄndras;

        public delegate void NärEnergiÄndrasHanterare(float NytVärde);

        //event för attack
        public event NärAttackÄndrasHanterare NärAttackÄndras;

        public delegate void NärAttackÄndrasHanterare(float NytVärde);

        //event för Försvar
        public event NärFörsvarÄndrasHanterare NärFörsvarÄndras;

        public delegate void NärFörsvarÄndrasHanterare(float NytVärde);

        public void Skada(float SkadeMängd) {
            SkadeMängd -= basFörsvar;
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

        public void EnergiKonsumption(float EnergiKonsumptionMängd) {
            Energi -= Math.Max(0, EnergiKonsumptionMängd);
        }
    }
}