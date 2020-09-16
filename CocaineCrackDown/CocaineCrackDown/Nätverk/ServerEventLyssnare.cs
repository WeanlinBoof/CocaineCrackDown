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
        private readonly NetPacketProcessor NetpaketProcessor = new NetPacketProcessor();
        public ServerEventLyssnare() {
            NetpaketProcessor.RegisterNestedType(SpelarData.Serialize , SpelarData.Deserialize);
            //Subscribe to packet receiving
            NetpaketProcessor.SubscribeReusable<SpelarDataPacket , NetPeer>(OnSpelarDataReceived);
        }

      private void OnSpelarDataReceived(SpelarDataPacket spelarDataPacket , NetPeer netPeer) {
            Console.WriteLine("[Server] ReceivedPacket:\n" + spelarDataPacket.SpelarDatan);
        }
    }
    
}
 