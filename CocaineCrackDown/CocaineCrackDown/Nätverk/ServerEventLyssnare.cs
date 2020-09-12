using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

using LiteNetLib;
using LiteNetLib.Utils;

using Nez;

namespace CocaineCrackDown.Nätverk {
    public class ServerEventLyssnare  : INetEventListener {

        private readonly NetPacketProcessor _netPacketProcessor = new NetPacketProcessor();

        public ServerEventLyssnare() {
            //Subscribe to packet receiving
            _netPacketProcessor.SubscribeReusable<SpelarDataPacket , NetPeer>(OnSpelarDataReceived);

        }

        private void OnSpelarDataReceived(SpelarDataPacket arg1 , NetPeer arg2) {
            throw new NotImplementedException();
        }

        public void OnPeerConnected(NetPeer peer) {
            throw new NotImplementedException();
        }

        public void OnPeerDisconnected(NetPeer peer , DisconnectInfo disconnectInfo) {
            throw new NotImplementedException();
        }

        public void OnNetworkError(IPEndPoint endPoint , SocketError socketError) {
            throw new NotImplementedException();
        }

        public void OnNetworkReceive(NetPeer peer , NetPacketReader reader , DeliveryMethod deliveryMethod) {
            throw new NotImplementedException();
        }

        public void OnNetworkReceiveUnconnected(IPEndPoint remoteEndPoint , NetPacketReader reader , UnconnectedMessageType messageType) {
            throw new NotImplementedException();
        }

        public void OnNetworkLatencyUpdate(NetPeer peer , int latency) {
            throw new NotImplementedException();
        }

        public void OnConnectionRequest(ConnectionRequest request) {
            throw new NotImplementedException();
        }
    }
/*public override void Update() {
    base.Update();
    Server.PollEvents();
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
        Server.SendToAll(writer , DeliveryMethod.Sequenced);
    }*/
}
