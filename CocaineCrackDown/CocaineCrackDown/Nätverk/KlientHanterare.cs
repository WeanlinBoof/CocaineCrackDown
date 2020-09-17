﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

using LiteNetLib;
using LiteNetLib.Utils;

using Nez;

namespace CocaineCrackDown.Nätverk {
    public class KlientHanterare : GlobalManager, INätHanterare {
        public EventBasedNetListener Lyssnare { get; set; }
        public NetManager Hanterare { get; set; }
        public string MottagenString { get; set; }

        public KlientHanterare() {
            Lyssnare = new KlientEventLyssnare();
            Hanterare = new NetManager(Lyssnare);
            Lyssnare.PeerConnectedEvent += peer => {
                MottagenString = $"Connected To {peer.EndPoint}";
                Console.WriteLine($"Connected To {peer.EndPoint}"); // Show peer ip
            };
            Lyssnare.NetworkReceiveEvent += (fromPeer , dataReader , deliveryMethod) => {
                MottagenString = dataReader.GetString();
                Console.WriteLine($"We got: {dataReader.GetString(100)}");
                dataReader.Recycle();
            };

        }
        public void Anslut(string ip) {
            Hanterare.Start();
            Hanterare.Connect(ip , StandigaVarden.PORTEN , "");
 
        }
        public override void Update() {
            base.Update();
            Hanterare.PollEvents();
        }
        public void SickaString(string str) {
            NetDataWriter writer = new NetDataWriter();
            writer.Put(str);
            Hanterare.SendToAll(writer , DeliveryMethod.Sequenced);
        }

    }
}
