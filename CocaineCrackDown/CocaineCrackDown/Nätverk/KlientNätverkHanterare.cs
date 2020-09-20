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
        public void SkickaMeddelande(ISpelMeddelande SpelMeddelande) {
            NetOutgoingMessage UtMeddelande = NetPeer.CreateMessage();
            UtMeddelande.Write((byte)SpelMeddelande.MeddelandeTyp);
            SpelMeddelande.Encode(UtMeddelande);

            NetPeer.SendMessage(UtMeddelande , NetDeliveryMethod.ReliableUnordered);
        }
        public void HanteraSpelarDataMeddelade(NetIncomingMessage im) {
            UppdateraSpelareStatusMeddelande message = new UppdateraSpelareStatusMeddelande(im);
            float timeDelay = (float)(NetTime.Now - im.SenderConnection.GetLocalTime(message.MeddelandesTid));
            Spelare spelare = SpelarHanterare.GetPlayer(message.ID) ?? SpelarHanterare.AddPlayer(message.ID , this , false);
            //message.Position;
            //message.Rörelse;
            if(spelare.SenasteUpdateringsTid < message.MeddelandesTid) {
                spelare.Position = message.Position;
                spelare.SenasteUpdateringsTid = message.MeddelandesTid;
            }
        }
        private void ProcessNetworkMessages(NetIncomingMessage InkommandeMeddelade) {
           
                if(InkommandeMeddelade.MessageType == NetIncomingMessageType.VerboseDebugMessage || InkommandeMeddelade.MessageType == NetIncomingMessageType.DebugMessage || InkommandeMeddelade.MessageType == NetIncomingMessageType.WarningMessage || InkommandeMeddelade.MessageType == NetIncomingMessageType.ErrorMessage) {
                    Console.WriteLine(InkommandeMeddelade.ReadString());
                }
                else if(InkommandeMeddelade.MessageType == NetIncomingMessageType.StatusChanged) {
                    switch((NetConnectionStatus)InkommandeMeddelade.ReadByte()) {
                        case NetConnectionStatus.Connected:
                            UppdateraSpelareStatusMeddelande message = new UppdateraSpelareStatusMeddelande(InkommandeMeddelade.SenderConnection.RemoteHailMessage);
                            SpelarHanterare.AddPlayer(message.ID , this , true);
                            Console.WriteLine("Connected to {0}" , InkommandeMeddelade.SenderEndPoint);                            break;
                        case NetConnectionStatus.Disconnected:
                            Console.WriteLine("Disconnected From {0} " , InkommandeMeddelade.SenderEndPoint);
                            break;
                        case NetConnectionStatus.RespondedAwaitingApproval:
                            NetOutgoingMessage hailMessage = SkapaMeddelande();
                            new UppdateraSpelareStatusMeddelande(SpelarHanterare.AddPlayer(this , false)).Encode(hailMessage);
                            InkommandeMeddelade.SenderConnection.Approve(hailMessage);
                            break;
                    }
                }
                else if(InkommandeMeddelade.MessageType == NetIncomingMessageType.Data) {
                    if((SpelMeddelandeTyper)InkommandeMeddelade.ReadByte() == SpelMeddelandeTyper.UppdateraSpelarStatus) {
                        HanteraSpelarDataMeddelade(InkommandeMeddelade);
                    }
                    else if((SpelMeddelandeTyper)InkommandeMeddelade.ReadByte() == SpelMeddelandeTyper.FiendeSpawnad) {
                    }
                    else if((SpelMeddelandeTyper)InkommandeMeddelade.ReadByte() == SpelMeddelandeTyper.FiendeStatus) {
                    }
                    else if((SpelMeddelandeTyper)InkommandeMeddelade.ReadByte() == SpelMeddelandeTyper.stringconsole) {
                    }
                }
                Återvin(InkommandeMeddelade);
            
        }


    }
}