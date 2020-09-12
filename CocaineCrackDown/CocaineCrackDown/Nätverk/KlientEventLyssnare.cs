
using LiteNetLib;
using LiteNetLib.Utils;

namespace CocaineCrackDown.Nätverk {
    public class KlientEventLyssnare : EventBasedNetListener {
        private readonly NetPacketProcessor NetpaketProcessor = new NetPacketProcessor();
        public KlientEventLyssnare() {
            NetpaketProcessor.RegisterNestedType(SpelarData.Serialize , SpelarData.Deserialize);
        }
        public new void OnPeerConnected(NetPeer peer) {
            SpelarDataPacket sp = new SpelarDataPacket {
                SpelarDatan = new SpelarData() ,
            };
             peer.Send(NetpaketProcessor.Write(sp) , DeliveryMethod.Sequenced);
            //or you can use             NetpaketProcessor.Send(peer , sp , DeliveryMethod.ReliableOrdered);

        }
    }
}
