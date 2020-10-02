using Lidgren.Network;

namespace CocaineCrackDown.Verktyg {

    public static class LidNetExt {

        public static void Write(this NetBuffer meddelande , SpelarData spelardata) {
            meddelande.Write(spelardata.ID);
            meddelande.Write(spelardata.Användarnamn);
            meddelande.Write(spelardata.X);
            meddelande.Write(spelardata.Y);
        }

        public static SpelarData ReadSpelarData(this NetBuffer meddelande) {
            SpelarData retval;
            retval.ID = meddelande.ReadUInt64();
            retval.Användarnamn = meddelande.ReadString();
            retval.X = meddelande.ReadSingle();
            retval.Y = meddelande.ReadSingle();
            return retval;
        }
    }
}