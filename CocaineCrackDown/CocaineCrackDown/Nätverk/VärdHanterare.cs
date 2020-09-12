using System;

using LiteNetLib;
using LiteNetLib.Utils;

using Nez;

namespace CocaineCrackDown.Nätverk {
    public class VärdHanterare : GlobalManager , INätHanterare {
        private readonly ServerEventLyssnare lyssnare;
        private readonly NetManager Server;
        public VärdHanterare() {
            lyssnare = new ServerEventLyssnare();
            Server = new NetManager(lyssnare);
        }
        public override void Update() {
            base.Update();
            Server.PollEvents();

        }
        public void Anslut(string ip = "localhost") {
            Server.Start(StandigaVarden.PORTEN);
            lyssnare.ConnectionRequestEvent += request => {
                if(Server.ConnectedPeersCount < 16) {
                    request.Accept();
                }
                else {
                    request.Reject();
                }
            };
            lyssnare.NetworkReceiveEvent += (fromPeer , dataReader , deliveryMethod) => {
                Console.WriteLine("We got: {0}" , dataReader.GetString(100));
                dataReader.Recycle();
            };
            lyssnare.PeerConnectedEvent += peer => {
                Console.WriteLine("We got connection: {0}" , peer.EndPoint); // Show peer ip
            };
        }
        public void SickaString(string str) {
            NetDataWriter writer = new NetDataWriter();               
            writer.Put(str);                              
            Server.SendToAll(writer , DeliveryMethod.Sequenced);
        }
    }
    
}
