using System;
using System.Collections.Generic;
using System.Text;

using Lidgren.Network;

namespace CocaineCrackDown {
    public static class LidNetExt {
        public static void Write(this NetBuffer meddelande , SpelarData spelardata) {
            meddelande.Write(spelardata.ID);
            meddelande.Write(spelardata.Namn);
            meddelande.Write(spelardata.X);
            meddelande.Write(spelardata.Y);
            meddelande.Write(spelardata.Attack);
        }
        public static SpelarData ReadSpelarData(this NetBuffer meddelande) {
            SpelarData retval;
            retval.ID = meddelande.ReadUInt64();
            retval.Namn = meddelande.ReadString();
            retval.X = meddelande.ReadSingle();
            retval.Y = meddelande.ReadSingle();
            retval.Attack = meddelande.ReadBoolean();
            return retval;
        }
    }
}