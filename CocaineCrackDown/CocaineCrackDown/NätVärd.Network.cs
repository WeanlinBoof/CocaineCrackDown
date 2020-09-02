using System;
using System.Threading;

using Lidgren.Network;

using Microsoft.Xna.Framework;

namespace CocaineCrackDown {

    public partial class NätVärd {

        /// <summary>
        /// A Basics Network class
        /// </summary>
        private static class Network {

            /// <summary>
            /// the Server
            /// </summary>
            public static NetServer Server;

            /// <summary>
            /// the Server config
            /// </summary>
            public static NetPeerConfiguration Config;

            /// <summary>
            /// the incoming messages that server can read from clients
            /// </summary>
            private static NetIncomingMessage Inkomande;

            /// <summary>
            /// the outgoing messages that clients can receive and read
            /// </summary>
            public static NetOutgoingMessage UtGående;

            /// <summary>
            /// below for explanation...
            /// </summary>
            private static readonly bool SpelarUppfriskning;

            private static Vector2 vector;


            public static void Update() {
                NetIncomingMessage InkomandeMeddelande;
                while((InkomandeMeddelande = Server.ReadMessage()) != null) {
                    switch(InkomandeMeddelande.MessageType) {

                        case NetIncomingMessageType.ConnectionLatencyUpdated:

                        case NetIncomingMessageType.UnconnectedData:

                        case NetIncomingMessageType.NatIntroductionSuccess:

                        case NetIncomingMessageType.ConnectionApproval:

                        case NetIncomingMessageType.DiscoveryRequest:

                        case NetIncomingMessageType.DiscoveryResponse:

                        case NetIncomingMessageType.StatusChanged:

                        case NetIncomingMessageType.Receipt:

                        case NetIncomingMessageType.DebugMessage:
                        case NetIncomingMessageType.WarningMessage:
                        case NetIncomingMessageType.ErrorMessage:
                            Console.WriteLine(InkomandeMeddelande.ReadString());
                            break;
                        case NetIncomingMessageType.Data:
                            Console.WriteLine(InkomandeMeddelande.ReadVector2());
                            break;
                        default:
                            Console.WriteLine("Unhandled type: " + InkomandeMeddelande.MessageType);
                            break;
                    }
                    Server.Recycle(InkomandeMeddelande);
                }
              
                
            }
        }
    }
}