
using Lidgren.Network;

namespace CocaineCrackDown.Nätverk.Meddelande {

    public interface ISpelMeddelande {
        SpelMeddelandeTyper MeddelandeTyp { get; }

        void AvKoda(NetIncomingMessage InkomandeMeddelande);

        void Encode(NetOutgoingMessage UtMeddelande);
    }
}