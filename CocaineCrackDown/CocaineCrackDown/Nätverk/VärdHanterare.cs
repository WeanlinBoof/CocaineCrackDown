﻿using System;

using LiteNetLib;
using LiteNetLib.Utils;

using Nez;

namespace CocaineCrackDown.Nätverk {
    public class VärdHanterare : GlobalManager , INätHanterare {
        public EventBasedNetListener Lyssnare { get; set; }
        public NetManager Hanterare { get; set; }
        public string MottagenString { get; set; }

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
//fixa user identefier för att kunna veta vem som är vad i ui
            Lyssnare.NetworkReceiveEvent += (fromPeer , dataReader , deliveryMethod) => {
                MottagenString = dataReader.GetString(100);
                Console.WriteLine($"We got: {fromPeer} {dataReader.GetString(100)}");
                dataReader.Recycle();
                
            };
        
            Lyssnare.PeerConnectedEvent += peer => {
                MottagenString = $"We got connection: {peer}";
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
