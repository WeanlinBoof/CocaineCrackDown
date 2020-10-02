using System;

using CocaineCrackDown.Entiteter;
using CocaineCrackDown.Entiteter.Gestalter;


namespace CocaineCrackDown.System {
    public class PlayerMetadata : IComparable<PlayerMetadata>
    {
        public int LagIndex { get; set; }
        public int Poäng { get; set; }
        public int SpelarIndex { get; set; }
        public GestaltParameter Gestalt { get; set; }
        public bool Initialiserad { get; set; }

        public VapenParameters Vapnet { get; set; }

        public PlayerMetadata() {
            Poäng = 0;
            Gestalt = Gestalter.Get("Doug");
        }

        public int CompareTo(PlayerMetadata ananSpelare) {
            return Poäng.CompareTo(ananSpelare.Poäng);
        }
    }

}
