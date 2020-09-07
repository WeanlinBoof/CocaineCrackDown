
using System;

using CocaineCrackDown.Hanterare.Nät.Meddelande;

using Lidgren.Network;

using Nez;
using Random = Nez.Random;
namespace CocaineCrackDown.Hanterare.Nät {
    public class ServerNätverkHanterare : GlobalManager, INätverkHanterare {

        public static NetServer Server;

        public ServerNätverkHanterare() {
            NetPeerConfiguration Konfig = new NetPeerConfiguration(StandigaVarden.SPELNAMN) {
                Port = Convert.ToInt32(StandigaVarden.PORTEN) ,
                EnableUPnP = true ,
                UseMessageRecycling = true
            };
            Konfig.EnableMessageType(NetIncomingMessageType.Data);
            Konfig.EnableMessageType(NetIncomingMessageType.ErrorMessage);
            Konfig.EnableMessageType(NetIncomingMessageType.Error);
            Konfig.EnableMessageType(NetIncomingMessageType.DebugMessage);
            Konfig.EnableMessageType(NetIncomingMessageType.ConnectionApproval);

            Server = new NetServer(Konfig);
        }
        public void Anslut(string ip = "localhost") {
            Server.Start();
            Server.UPnP.ForwardPort(StandigaVarden.PORTEN , "CCD Port");
        }
        public NetOutgoingMessage SkapaMeddelande() {
            return Server.CreateMessage();
        }
        public void Frånkoppla() {
            Server.Shutdown("Ses!");
        }
        public NetIncomingMessage LäsMeddelande() {
            return Server.ReadMessage();
        }
        public void Återvin(NetIncomingMessage InkomandeMeddelande) {
            Server.Recycle(InkomandeMeddelande);
        }
        public void SickaMeddelande(ISpelMeddelande SpelMeddelande) {
            NetOutgoingMessage UtMeddelande = Server.CreateMessage();
            UtMeddelande.Write((byte)SpelMeddelande.MeddelandeTyp);
            SpelMeddelande.Encode(UtMeddelande);

            Server.SendToAll(UtMeddelande , NetDeliveryMethod.UnreliableSequenced);
        }
        
    }
}
