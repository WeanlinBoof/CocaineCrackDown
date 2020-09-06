
using CocaineCrackDown.Entiteter;

using Lidgren.Network;

using Microsoft.Xna.Framework;

namespace CocaineCrackDown.Hanterare.Nät.Meddelande {
    public class UppdateraSpelareStatusMeddelande : ISpelMeddelande {
        public UppdateraSpelareStatusMeddelande(NetIncomingMessage InkomandeMeddelande) {
            AvKoda(InkomandeMeddelande);
        }
        public UppdateraSpelareStatusMeddelande() {
        }
        public UppdateraSpelareStatusMeddelande(Spelare Spelare) {
            ID = Spelare.Id;
            Position = Spelare.Position;
            Rörelse = Spelare.Rörelse;
            MeddelandesTid = NetTime.Now;
        }

        public uint ID { get; set; }

        public double MeddelandesTid { get; set; }

        public SpelMeddelandeTyper MeddelandeTyp => SpelMeddelandeTyper.UppdateraSpelarStatus;

        public Vector2 Position { get; set; }

        public Vector2 Rörelse { get; set; }
        public void AvKoda(NetIncomingMessage InkomandeMeddelande) {
            ID = InkomandeMeddelande.ReadUInt32();
            MeddelandesTid = InkomandeMeddelande.ReadDouble();
            Position = InkomandeMeddelande.ReadVector2();
            Rörelse = InkomandeMeddelande.ReadVector2();
        }
        public void Encode(NetOutgoingMessage UtMeddelande) {
            UtMeddelande.Write(ID);
            UtMeddelande.Write(MeddelandesTid);
            UtMeddelande.Write(Position);
            UtMeddelande.Write(Rörelse);
        }
    }
}