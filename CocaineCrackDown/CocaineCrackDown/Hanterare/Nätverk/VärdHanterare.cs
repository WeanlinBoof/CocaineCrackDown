using System;

using System.Net;
using System.Net.Sockets;

using CocaineCrackDown.Entiteter;

using LiteNetLib;
using LiteNetLib.Utils;

using Nez;

namespace CocaineCrackDown.Nätverk {
    public class VärdHanterare : GlobalManager, INetEventListener, INetLogger {
        //FUCKING KOLLA IN REFERENCER SOM DU HAR I GOOGLECHROME ELLER LETA EFTER MER/BÄTTRE REFERENSER
        private NetManager server;
        public string msg;

        private NetDataWriter writer;

        private NetPeer lokalPeer;
        private Spelare lokalSpelare;

        private NetPeer otherPeer;
        private Spelare otherSpelare;
        

        public override void OnEnabled() {
            NetDebug.Logger = this;

            writer = new NetDataWriter();

            server = new NetManager(this);

        }
        public void Anslut(){
            server.Start(StandigaVarden.PORTEN);
        }
        public override void Update() {
            server.PollEvents();
        }

        public override void OnDisabled() {
            NetDebug.Logger = null;
            server?.Stop();
        }

        public void OnPeerConnected(NetPeer peer) {
            Console.WriteLine("[SERVER] We have new peer " + peer.EndPoint);
        }

        public void OnNetworkError(IPEndPoint endPoint , SocketError socketErrorCode) {
            Console.WriteLine("[SERVER] error " + socketErrorCode);
        }

        public void OnNetworkReceiveUnconnected(IPEndPoint remoteEndPoint , NetPacketReader reader , UnconnectedMessageType messageType) {
            if(messageType == UnconnectedMessageType.Broadcast) {
                Console.WriteLine("[SERVER] Received discovery request. Send discovery response");
                NetDataWriter resp = new NetDataWriter();
                resp.Put(1);
                server.SendUnconnectedMessage(resp , remoteEndPoint);
            }
        }

        public void OnNetworkLatencyUpdate(NetPeer peer , int latency) {
        }

        public void OnConnectionRequest(ConnectionRequest request) {
            request.AcceptIfKey(StandigaVarden.SPELNAMN);
        }

        public void OnPeerDisconnected(NetPeer peer , DisconnectInfo disconnectInfo) {
            Console.WriteLine("[SERVER] peer disconnected " + peer.EndPoint + ", info: " + disconnectInfo.Reason);
            if(peer == lokalPeer) {
                lokalPeer = null;
            }
        }

        public void OnNetworkReceive(NetPeer peer , NetPacketReader reader , DeliveryMethod deliveryMethod) {
            msg = reader.GetString();
            Console.WriteLine($"{peer.Id}: {msg}");
            reader.Recycle();
        }

        public void WriteNet(NetLogLevel level , string str , params object[] args) {
            Console.WriteLine(str , args);
        }
        public void NetTest(string str){
            NetDataWriter skriv = new NetDataWriter();
            skriv.Put(str);
            server.SendToAll(skriv,DeliveryMethod.ReliableOrdered);
        }
    }
}