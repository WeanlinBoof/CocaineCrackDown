using System;
using System.Collections.Generic;
using System.Text;

using Lidgren.Network;

namespace CocaineCrackDown.Hanterare.Nät.Meddelande {
    class StringConsoleMeddelande : ISpelMeddelande {
        public StringConsoleMeddelande(NetIncomingMessage InkomandeMeddelande) {
            AvKoda(InkomandeMeddelande);
        }
        public StringConsoleMeddelande() {

        }
        public StringConsoleMeddelande(string text) {
            Text = text;
            MeddelandesTid = NetTime.Now;
        }
        public string Text { get; set; } 
        public SpelMeddelandeTyper MeddelandeTyp => SpelMeddelandeTyper.stringconsloe;

        public double MeddelandesTid { get; set; }
        public void AvKoda(NetIncomingMessage InkomandeMeddelande) {
            MeddelandesTid = InkomandeMeddelande.ReadDouble();
            Text = InkomandeMeddelande.ReadString();
        }
        public void Encode(NetOutgoingMessage UtMeddelande) {
            UtMeddelande.Write(MeddelandesTid);
            UtMeddelande.Write(Text);
        }
    }
}
