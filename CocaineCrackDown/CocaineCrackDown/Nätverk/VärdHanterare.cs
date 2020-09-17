using System;

using LiteNetLib;
using LiteNetLib.Utils;

using Nez;

namespace CocaineCrackDown.Nätverk {
    public class VärdHanterare : GlobalManager , INätHanterare {

        public EventBasedNetListener Lyssnare { get; set; }
        public NetManager Hanterare { get; set; }

        public VärdHanterare() {
            Lyssnare = new ServerEventLyssnare();
            Hanterare = new NetManager(Lyssnare);
        }
        public override void Update() {
            base.Update();
            Hanterare.PollEvents();

        }
        public void Anslut(string ip = "localhost") {
            Hanterare.Start(StandigaVarden.PORTEN);
            Lyssnare.ConnectionRequestEvent += request => {
                if(Hanterare.ConnectedPeersCount < 16) {
                    request.Accept();
                }
                else {
                    request.Reject();
                }
            };
            Lyssnare.NetworkReceiveEvent += (fromPeer , dataReader , deliveryMethod) => {
                Console.WriteLine("We got: {0}" , dataReader.GetString(100));
                dataReader.Recycle();
            };
            Lyssnare.PeerConnectedEvent += peer => {
                Console.WriteLine("We got connection: {0}" , peer.EndPoint); // Show peer ip
            };
        }
        public void SickaString(string str) {
            NetDataWriter writer = new NetDataWriter();               
            writer.Put(str);                              
            Hanterare.SendToAll(writer , DeliveryMethod.Sequenced);
        }
    }
    
}
