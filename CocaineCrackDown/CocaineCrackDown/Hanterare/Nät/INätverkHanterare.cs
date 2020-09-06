
using CocaineCrackDown.Hanterare.Nät.Meddelande;

using Lidgren.Network;

namespace CocaineCrackDown.Hanterare.Nät {
    public interface INätverkHanterare {
        void Anslut(string ip);
        NetOutgoingMessage SkapaMeddelande();
        void Frånkoppla();
        NetIncomingMessage LäsMeddelande();
        void Återvin(NetIncomingMessage InkomandeMeddelande);
        void SickaMeddelande(ISpelMeddelande SpelMeddelande);
    }
}
