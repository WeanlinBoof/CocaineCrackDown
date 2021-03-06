﻿using System;
using System.Net;
using System.Net.Sockets;

using CocaineCrackDown.Entiteter;

using LiteNetLib;
using LiteNetLib.Utils;

using Nez;

namespace CocaineCrackDown.Nätverk {

    public class KlientHanterare : GlobalManager, INetEventListener {
        public string msg;
        public bool KlientReady;
        public NetManager klient;
        public SpelarData recivedSpelarData;
        private NetDataWriter writer;
        
        public NetPeer lokalPeer;

        public Spelare lokalSpelare;

        public NetPeer otherPeer;
        public Spelare otherSpelare;

        public KlientHanterare() {
        }
        public void SickaSpelarData(SpelarData spelarData){
            NetDataWriter skriv = new NetDataWriter();
            skriv.Put(spelarData.X);
            skriv.Put(spelarData.Y);
            skriv.Put(spelarData.Attack);
            klient.SendToAll(skriv ,DeliveryMethod.ReliableOrdered);
        }
        private void OnSpelarDataRecived(SPDataPacket spData , NetPeer peer) {
            recivedSpelarData = spData.SpelarData;
        }

        public void SickaString(string str){
            NetDataWriter skriv = new NetDataWriter();
            skriv.Put(str);
            klient.SendToAll(skriv,DeliveryMethod.ReliableOrdered);
        }
        public void SickaBool(bool bOl){
            NetDataWriter skriv = new NetDataWriter();
            skriv.Put(bOl);
            klient.SendToAll(skriv,DeliveryMethod.ReliableOrdered);
        }
        public void Anslut(string ip){
            klient.Connect(ip, StandigaVarden.PORTEN, StandigaVarden.SPELNAMN);
        }
        public override void OnEnabled() {
            klient = new NetManager(this);
            klient.Start();
            lokalPeer = klient.FirstPeer;
        }

        public override void Update() {
            klient.PollEvents();
        }

        public override void OnDisabled() {
            klient?.Stop();
        }

        public void OnPeerConnected(NetPeer peer) {
            Console.WriteLine("[CLIENT] We connected to " + peer.EndPoint);
        }

        public void OnNetworkError(IPEndPoint endPoint , SocketError socketErrorCode) {
            Console.WriteLine("[CLIENT] We received error " + socketErrorCode);
        }

        public void OnNetworkReceive(NetPeer peer , NetPacketReader reader , DeliveryMethod deliveryMethod) {
            recivedSpelarData = new SpelarData {
                X = reader.GetFloat() ,
                Y = reader.GetFloat() ,
                Attack = reader.GetBool()
            };
            reader.Recycle();
            
        }

        public void OnNetworkReceiveUnconnected(IPEndPoint remoteEndPoint , NetPacketReader reader , UnconnectedMessageType messageType) {
            if(messageType == UnconnectedMessageType.BasicMessage && klient.ConnectedPeersCount == 0 && reader.GetInt() == 1) {
                Console.WriteLine("[CLIENT] Received discovery response. Connecting to: " + remoteEndPoint);
            }
        }

        public void OnNetworkLatencyUpdate(NetPeer peer , int latency) {
        }

        public void OnConnectionRequest(ConnectionRequest request) {
        }

        public void OnPeerDisconnected(NetPeer peer , DisconnectInfo disconnectInfo) {
            Console.WriteLine("[CLIENT] We disconnected because " + disconnectInfo.Reason);
        }
    }
}