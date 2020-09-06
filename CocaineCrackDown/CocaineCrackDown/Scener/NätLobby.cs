using System;
using System.Text;

using CocaineCrackDown.Entiteter;
using CocaineCrackDown.Hanterare;
using CocaineCrackDown.Hanterare.Nät;
using CocaineCrackDown.Hanterare.Nät.Meddelande;

using Lidgren.Network;

using Microsoft.Xna.Framework.Input;

using Nez;

namespace CocaineCrackDown.Scener {

    public class NätLobby : GrundScen {
        private readonly INätverkHanterare NätHanterare;
        private SpelarHanterare Spelarhanterare;
        string bruh = "du är bog";
        public NätLobby(INätverkHanterare näthanterare) {
            NätHanterare = näthanterare;
        }
        private bool IsHost => NätHanterare is ServerNätverkHanterare;
        public override void Initialize() {
            base.Initialize();
            string AnvändarNamn = SlumpAnvändarNamn;
            Spelarhanterare = new SpelarHanterare(AnvändarNamn , IsHost);
            Spelarhanterare.SpelarStatusÄndrad += (sender , Args) => NätHanterare.SickaMeddelande(new UppdateraSpelareStatusMeddelande(Args.Player));
            LäggTillHost();

        }
        public override void OnStart() {
            base.OnStart();
        }
        public override void Update() {
            base.Update();
            KeyboardState state = Keyboard.GetState();
            if(state.IsKeyDown(Keys.R)) {
                NätHanterare.SkapaMeddelande().Write(bruh);
                NätHanterare.SickaMeddelande(new StringConsoleMeddelande(bruh));
            }
            ProcessNetworkMessages();
        }
        protected void LäggTillHost() {

            if(IsHost) {
                Spelarhanterare.AddPlayer(this, true);
            }
        }
        protected void Koppplafrån() {
            NätHanterare.Frånkoppla();
        }
        private void HandleUpdatePlayerStateMessage(NetIncomingMessage im) {
            UppdateraSpelareStatusMeddelande message = new UppdateraSpelareStatusMeddelande(im);

            float timeDelay = (float)(NetTime.Now - im.SenderConnection.GetLocalTime(message.MeddelandesTid));

            Spelare spelare = Spelarhanterare.GetPlayer(message.ID) ?? Spelarhanterare.AddPlayer(message.ID, this  , false);

            //message.Position;
            //message.Rörelse;
            if(spelare.SenasteUpdateringsTid < message.MeddelandesTid) {
                spelare.Position = message.Position += message.Rörelse * timeDelay;
                spelare.Rörelse = message.Rörelse;

                spelare.SenasteUpdateringsTid = message.MeddelandesTid;
            }
        }

        protected void ProcessNetworkMessages() {
            NetIncomingMessage InkomandeMeddelande;

            while((InkomandeMeddelande = NätHanterare.LäsMeddelande()) != null) {
                switch(InkomandeMeddelande.MessageType) {
                    case NetIncomingMessageType.VerboseDebugMessage:
                    case NetIncomingMessageType.DebugMessage:
                    case NetIncomingMessageType.WarningMessage:
                    case NetIncomingMessageType.ErrorMessage:
                        Console.WriteLine(InkomandeMeddelande.ReadString());
                        break;

                    case NetIncomingMessageType.StatusChanged:
                        switch((NetConnectionStatus)InkomandeMeddelande.ReadByte()) {
                            case NetConnectionStatus.Connected:
                                if(!IsHost) {
                                    UppdateraSpelareStatusMeddelande message = new UppdateraSpelareStatusMeddelande(InkomandeMeddelande.SenderConnection.RemoteHailMessage);
                                    Spelarhanterare.AddPlayer(message.ID ,this, true);
                                    Console.WriteLine("Connected to {0}" , InkomandeMeddelande.SenderEndPoint);
                                }
                                else {
                                    Console.WriteLine("{0} Connected" , InkomandeMeddelande.SenderEndPoint);
                                }
                                break;
                            case NetConnectionStatus.Disconnected:
                                Console.WriteLine(IsHost ? "{0} Disconnected" : "Disconnected from {0}" , InkomandeMeddelande.SenderEndPoint);
                                break;
                            case NetConnectionStatus.RespondedAwaitingApproval:
                                NetOutgoingMessage hailMessage = NätHanterare.SkapaMeddelande();
                                new UppdateraSpelareStatusMeddelande(Spelarhanterare.AddPlayer(this, false)).Encode(hailMessage);
                                InkomandeMeddelande.SenderConnection.Approve(hailMessage);
                                break;
                        }
                        break;
                    case NetIncomingMessageType.Data:
                        switch((SpelMeddelandeTyper)InkomandeMeddelande.ReadByte()) {
                            case SpelMeddelandeTyper.UppdateraSpelarStatus:
                                HandleUpdatePlayerStateMessage(InkomandeMeddelande);
                                break;
                            case SpelMeddelandeTyper.FiendeSpawnad:
                                //HandleEnemySpawnedMessage(im);
                                break;
                        }
                        break;
                }

                NätHanterare.Återvin(InkomandeMeddelande);
            }
        }

    }
}