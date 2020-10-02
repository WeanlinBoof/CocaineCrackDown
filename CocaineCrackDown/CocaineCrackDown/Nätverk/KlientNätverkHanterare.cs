using System;

using CocaineCrackDown.Entiteter;
using CocaineCrackDown.Hanterare;
using CocaineCrackDown.Nätverk.Meddelande;

using Lidgren.Network;

using Nez;

using Random = Nez.Random;

namespace CocaineCrackDown.Nätverk {
    public class KlientNätverkHanterare : GlobalManager, INätverkHanterare {
        public NetClient NetPeer { get; set; }
        public SpelarHanterare SpelarHanterare { get; set; }
        public NetIncomingMessage InkommandeMeddelade { get; set; }
        public SpelarData InkomandeData { get; set; }
        public SpelarData UtData { get; set; }
        public KlientNätverkHanterare() {
            NetPeerConfiguration Konfig = new NetPeerConfiguration(StandigaVarden.SPELNAMN);

            Konfig.EnableMessageType(NetIncomingMessageType.WarningMessage);
            Konfig.EnableMessageType(NetIncomingMessageType.VerboseDebugMessage);
            Konfig.EnableMessageType(NetIncomingMessageType.ErrorMessage);
            Konfig.EnableMessageType(NetIncomingMessageType.Error);
            Konfig.EnableMessageType(NetIncomingMessageType.DebugMessage);
            Konfig.EnableMessageType(NetIncomingMessageType.ConnectionApproval);

            NetPeer = new NetClient(Konfig);
        }       
        public override void Update() {
            base.Update();
            while((InkommandeMeddelade = LäsMeddelande()) != null) {
            ProcessNetworkMessages(InkommandeMeddelade);
            }
            
        }
        /// <summary>
        /// Klient Anslut
        /// </summary>
        /// <param name="ip"></param>
        public void Anslut(string ip) {
            NetPeer.Start();
            NetPeer.Connect(ip , StandigaVarden.PORTEN);
        }
        /// <summary>
        /// Skapa Ut meddelande
        /// </summary>
        public NetOutgoingMessage SkapaMeddelande() {
            return NetPeer.CreateMessage();
        }
        /// <summary>
        /// Koppla Ifrån
        /// </summary>
        public void Frånkoppla() {
            NetPeer.Disconnect("Ses Senare");
        }
        /// <summary>
        /// Läs Inkomande Medelande
        /// </summary>
        public NetIncomingMessage LäsMeddelande() {
            return NetPeer.ReadMessage();
        }
        /// <summary>
        /// ÅterVinging Av Kommande Meddelande
        /// </summary>
        /// <param name="InkomandeMeddelande">The im.</param>
        public void Återvin(NetIncomingMessage InkomandeMeddelande) {
            NetPeer.Recycle(InkomandeMeddelande);
        }
        /// <summary>
        /// Sicka Medelande Ut
        /// </summary>
        /// <param name="SpelMeddelande">The game message.</param>
        public void SkickaMeddelande(SpelarData spelarData) {
            UtData = spelarData;
            NetOutgoingMessage UtMeddelande = NetPeer.CreateMessage();
            UtMeddelande.Write(UtData);
            NetPeer.SendMessage(UtMeddelande , NetDeliveryMethod.UnreliableSequenced);
        }
        private void ProcessNetworkMessages(NetIncomingMessage InkommandeMeddelade) {

            switch(InkommandeMeddelade.MessageType) {
                case NetIncomingMessageType.VerboseDebugMessage:
                case NetIncomingMessageType.DebugMessage:
                case NetIncomingMessageType.WarningMessage:
                case NetIncomingMessageType.ErrorMessage:
                    Console.WriteLine(InkommandeMeddelade.ReadString());
                    break;
                case NetIncomingMessageType.StatusChanged: {
                    switch((NetConnectionStatus)InkommandeMeddelade.ReadByte()) {
                        case NetConnectionStatus.Connected:
                            UppdateraSpelareStatusMeddelande message = new UppdateraSpelareStatusMeddelande(InkommandeMeddelade.SenderConnection.RemoteHailMessage);
                            SpelarHanterare.AddPlayer(message.ID , this , true);
                            Console.WriteLine("Connected to {0}" , InkommandeMeddelade.SenderEndPoint);
                            break;
                        case NetConnectionStatus.Disconnected:
                            Console.WriteLine("Disconnected From {0} " , InkommandeMeddelade.SenderEndPoint);
                            break;
                    }

                    break;
                }

                case NetIncomingMessageType.Data:
                    MottaMeddelande(InkommandeMeddelade.ReadSpelarData());
                    break;
            }
            Återvin(InkommandeMeddelade);
        }

        public void MottaMeddelande(SpelarData spelarData) {
            InkomandeData = spelarData;
        }
    }
}