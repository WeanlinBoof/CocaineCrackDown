
using Lidgren.Network;

namespace CocaineCrackDown.Hanterare.Nät.Meddelande {

    public interface ISpelMeddelande {
        SpelMeddelandeTyper MeddelandeTyp { get; }

        void AvKoda(NetIncomingMessage InkomandeMeddelande);

        void Encode(NetOutgoingMessage UtMeddelande);
    }
}