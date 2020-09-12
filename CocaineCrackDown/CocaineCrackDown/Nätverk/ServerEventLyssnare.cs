using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

using LiteNetLib;
using LiteNetLib.Utils;

namespace CocaineCrackDown.Nätverk {
    public class ServerEventLyssnare : EventBasedNetListener {
        private readonly NetPacketProcessor NetpacketProcessor = new NetPacketProcessor();

        public ServerEventLyssnare() {
            NetpacketProcessor.RegisterNestedType(SpelarData.Serialize , SpelarData.Deserialize);
            //Subscribe to packet receiving
            NetpacketProcessor.SubscribeReusable<SpelarDataPacket , NetPeer>(OnSpelarDataReceived);

        }
        
        private void OnSpelarDataReceived(SpelarDataPacket spelarDataPacket , NetPeer netPeer) {
            Console.WriteLine("[Server] ReceivedPacket:\n" + spelarDataPacket.SpelarDatan);
        }
        public new void OnNetworkReceive(NetPeer peer , NetPacketReader reader , DeliveryMethod deliveryMethod) {
            Console.WriteLine("[Server] received data. Processing...");
            NetpacketProcessor.ReadAllPackets(reader , peer);
        } 
    }
    
}
