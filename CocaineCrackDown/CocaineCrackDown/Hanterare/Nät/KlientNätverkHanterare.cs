
using CocaineCrackDown.Hanterare.Nät.Meddelande;

using Lidgren.Network;

using Nez;

using Random = Nez.Random;

namespace CocaineCrackDown.Hanterare.Nät {
    public class KlientNätverkHanterare : GlobalManager, INätverkHanterare {

        private readonly NetClient Klient;

        public KlientNätverkHanterare() {
            NetPeerConfiguration Konfig = new NetPeerConfiguration(StandigaVarden.SPELNAMN);

            Konfig.EnableMessageType(NetIncomingMessageType.WarningMessage);
            Konfig.EnableMessageType(NetIncomingMessageType.VerboseDebugMessage);
            Konfig.EnableMessageType(NetIncomingMessageType.ErrorMessage);
            Konfig.EnableMessageType(NetIncomingMessageType.Error);
            Konfig.EnableMessageType(NetIncomingMessageType.DebugMessage);
            Konfig.EnableMessageType(NetIncomingMessageType.ConnectionApproval);

            Klient = new NetClient(Konfig);
        }
        /// <summary>
        /// Klient Anslut
        /// </summary>
        /// <param name="ip"></param>
        public void Anslut(string ip) {
            Klient.Start();
            Klient.Connect(ip , StandigaVarden.PORTEN);
        }
        /// <summary>
        /// Skapa Ut meddelande
        /// </summary>
        public NetOutgoingMessage SkapaMeddelande() {
            return Klient.CreateMessage();
        }
        /// <summary>
        /// Koppla Ifrån
        /// </summary>
        public void Frånkoppla() {
            Klient.Disconnect("Ses Senare");
        }
        /// <summary>
        /// Läs Inkomande Medelande
        /// </summary>
        public NetIncomingMessage LäsMeddelande() {
            return Klient.ReadMessage();
        }
        /// <summary>
        /// ÅterVinging Av Kommande Meddelande
        /// </summary>
        /// <param name="InkomandeMeddelande">The im.</param>
        public void Återvin(NetIncomingMessage InkomandeMeddelande) {
            Klient.Recycle(InkomandeMeddelande);
        }
        /// <summary>
        /// Sicka Medelande Ut
        /// </summary>
        /// <param name="SpelMeddelande">The game message.</param>
        public void SickaMeddelande(ISpelMeddelande SpelMeddelande) {
            NetOutgoingMessage UtMeddelande = Klient.CreateMessage();
            UtMeddelande.Write((byte)SpelMeddelande.MeddelandeTyp);
            SpelMeddelande.Encode(UtMeddelande);

            Klient.SendMessage(UtMeddelande , NetDeliveryMethod.ReliableUnordered);
        }

 
    }
}
