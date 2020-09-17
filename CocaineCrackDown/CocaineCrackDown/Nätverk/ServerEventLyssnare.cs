using System;

using LiteNetLib;
using LiteNetLib.Utils;

namespace CocaineCrackDown.Nätverk {

    public class ServerEventLyssnare : EventBasedNetListener {

        private readonly NetPacketProcessor NetpaketProcessor = new NetPacketProcessor();

        public ServerEventLyssnare() {
            NetpaketProcessor.RegisterNestedType(SpelarData.Serialize , SpelarData.Deserialize);

            //Subscribe to packet receiving
            NetpaketProcessor.SubscribeReusable<SpelarDataPacket , NetPeer>(SpelarDataMottagen);
        }

        public virtual void SpelarDataMottagen(SpelarDataPacket spelarDataPacket , NetPeer netPeer) {
            Console.WriteLine("[Server] ReceivedPacket:\n" + spelarDataPacket.SpelarDatan);
        }
    }
}