using CocaineCrackDown.Hanterare;
using CocaineCrackDown.Nätverk.Meddelande;

using Lidgren.Network;

namespace CocaineCrackDown.Nätverk {
    public interface INätverkHanterare {
        public SpelarHanterare SpelarHanterare { get; set; }
        public NetIncomingMessage InkommandeMeddelade { get; set; }
        void Anslut(string ip);
        NetOutgoingMessage SkapaMeddelande();
        void Frånkoppla();
        NetIncomingMessage LäsMeddelande();
        void Återvin(NetIncomingMessage InkomandeMeddelande);
        void SickaMeddelande(ISpelMeddelande SpelMeddelande);
    }
}
