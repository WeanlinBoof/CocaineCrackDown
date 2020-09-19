
using CocaineCrackDown;
using CocaineCrackDown.Entiteter;

using Lidgren.Network;

using Microsoft.Xna.Framework;

namespace CocaineCrackDown.Nätverk.Meddelande {
    public class UppdateraSpelareStatusMeddelande : ISpelMeddelande {
        public UppdateraSpelareStatusMeddelande(NetIncomingMessage InkomandeMeddelande) {
            AvKoda(InkomandeMeddelande);
        }
        public UppdateraSpelareStatusMeddelande() {
        }
        public UppdateraSpelareStatusMeddelande(SpelarData Spelare) {
            ID = Spelare.ID;
            Användarnamn = Spelare.Användarnamn;
            Position = new Vector2(Spelare.X , Spelare.Y);
            MeddelandesTid = NetTime.Now;
        }
        public ulong ID { get; set; }       
        public string Användarnamn { get; set; }

        public Vector2 Position { get; set; }

        public double MeddelandesTid { get; set; }

        public SpelMeddelandeTyper MeddelandeTyp => SpelMeddelandeTyper.UppdateraSpelarStatus;


        public void AvKoda(NetIncomingMessage InkomandeMeddelande) {
            ID = InkomandeMeddelande.ReadUInt64();
            MeddelandesTid = InkomandeMeddelande.ReadDouble();
            Position = InkomandeMeddelande.ReadVector2();
        }
        public void Encode(NetOutgoingMessage UtMeddelande) {
            UtMeddelande.Write(ID);
            UtMeddelande.Write(Användarnamn);
            UtMeddelande.Write(Position);
            UtMeddelande.Write(MeddelandesTid);
        }
    }
}