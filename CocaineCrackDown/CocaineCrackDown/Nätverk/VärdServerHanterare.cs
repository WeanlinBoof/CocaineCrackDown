using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

using LiteNetLib;
using LiteNetLib.Utils;

using Nez;

namespace CocaineCrackDown.Nätverk {
    public class VärdServerHanterare  : GlobalManager {
        public EventBasedNetListener lyssnare;
        public static NetManager Server;
       public VärdServerHanterare() {
            EventBasedNetListener lyssnare = new EventBasedNetListener();
            Server = new NetManager(lyssnare);
            
       }
        public void Anslut() {
            Server.Start(StandigaVarden.PORTEN);

            lyssnare.ConnectionRequestEvent += request => {
                if(Server.ConnectedPeersCount < 10) {
                    request.Accept();
                }
                else {
                    request.Reject();
                }
            };

            lyssnare.PeerConnectedEvent += peer => {
                Console.WriteLine("We got connection: {0}" , peer.EndPoint); // Show peer ip
            };
            NetDataWriter writer = new NetDataWriter();                 // Create writer class
            writer.Put("Hello client!");                                // Put some string
            Server.SendToAll(writer,DeliveryMethod.Sequenced);
        }
        public override void Update() {
            base.Update();
            Server.PollEvents();

        }
    }
}
