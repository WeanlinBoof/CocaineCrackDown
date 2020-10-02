using System;

using CocaineCrackDown.Entiteter;
using CocaineCrackDown.Hanterare;
using CocaineCrackDown.Nätverk.Meddelande;

using Lidgren.Network;

using Nez;

namespace CocaineCrackDown.Nätverk {

    public class ServerNätverkHanterare : GlobalManager, INätverkHanterare {
        public NetServer NetPeer { get; set; }
        public SpelarHanterare SpelarHanterare { get; set; }
        public NetIncomingMessage InkommandeMeddelade { get; set; }
        public SpelarData InkomandeData { get; set; }
        public SpelarData UtData { get; set; }

        public ServerNätverkHanterare() {
            NetPeerConfiguration Konfig = new NetPeerConfiguration(StandigaVarden.SPELNAMN) {
                Port = StandigaVarden.PORTEN ,
                EnableUPnP = true
            };
            Konfig.EnableMessageType(NetIncomingMessageType.Data);
            Konfig.EnableMessageType(NetIncomingMessageType.ErrorMessage);
            Konfig.EnableMessageType(NetIncomingMessageType.Error);
            Konfig.EnableMessageType(NetIncomingMessageType.DebugMessage);
            Konfig.EnableMessageType(NetIncomingMessageType.ConnectionApproval);

            NetPeer = new NetServer(Konfig);
        }

        public void Anslut(string ip = "localhost") {
            NetPeer.Start();
            NetPeer.UPnP.ForwardPort(StandigaVarden.PORTEN , "CCD Port");
        }

        public NetOutgoingMessage SkapaMeddelande() {
            return NetPeer.CreateMessage();
        }

        public void Frånkoppla() {
            NetPeer.Shutdown("Ses!");
        }

        public NetIncomingMessage LäsMeddelande() {
            return NetPeer.ReadMessage();
        }

        public void Återvin(NetIncomingMessage InkomandeMeddelande) {
            NetPeer.Recycle(InkomandeMeddelande);
        }

        public void SkickaMeddelande(SpelarData spelarData) {
            UtData = spelarData;
            NetOutgoingMessage UtMeddelande = NetPeer.CreateMessage();
            UtMeddelande.Write(UtData);
            NetPeer.SendToAll(UtMeddelande , NetDeliveryMethod.UnreliableSequenced);
        }

        public override void Update() {
            base.Update();
            while((InkommandeMeddelade = LäsMeddelande()) != null) {
                ProcessNetworkMessages(InkommandeMeddelade);
            }
        }

        public void ProcessNetworkMessages(NetIncomingMessage InkommandeMeddelade) {
            if(InkommandeMeddelade.MessageType == NetIncomingMessageType.VerboseDebugMessage || InkommandeMeddelade.MessageType == NetIncomingMessageType.DebugMessage || InkommandeMeddelade.MessageType == NetIncomingMessageType.WarningMessage || InkommandeMeddelade.MessageType == NetIncomingMessageType.ErrorMessage) {
                Console.WriteLine(InkommandeMeddelade.ReadString());
            }
            else if(InkommandeMeddelade.MessageType == NetIncomingMessageType.StatusChanged) {
                switch((NetConnectionStatus)InkommandeMeddelade.ReadByte()) {
                    case NetConnectionStatus.Connected:
                        Console.WriteLine("{0} Connected" , InkommandeMeddelade.SenderEndPoint);
                        break;

                    case NetConnectionStatus.Disconnected:
                        Console.WriteLine("{0} Disconnected" , InkommandeMeddelade.SenderEndPoint);
                        break;

                    case NetConnectionStatus.RespondedAwaitingApproval:
                        NetOutgoingMessage hailMessage = SkapaMeddelande();
                        new UppdateraSpelareStatusMeddelande(SpelarHanterare.AddPlayer(this , false)).Encode(hailMessage);
                        InkommandeMeddelade.SenderConnection.Approve(hailMessage);
                        break;
                }
            }
            else if(InkommandeMeddelade.MessageType == NetIncomingMessageType.Data) {
                MottaMeddelande(InkommandeMeddelade.ReadSpelarData());
                
            }
            Återvin(InkommandeMeddelade);
        }

        public void MottaMeddelande(SpelarData spelarData) {
            InkomandeData = spelarData;
        }
    }
}