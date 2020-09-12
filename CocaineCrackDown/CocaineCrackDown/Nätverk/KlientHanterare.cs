using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

using LiteNetLib;
using LiteNetLib.Utils;

using Nez;

namespace CocaineCrackDown.Nätverk {
    public class KlientHanterare : GlobalManager, INätHanterare {
        KlientEventLyssnare lyssnare;
        NetManager Klient;
        public KlientHanterare() {
            lyssnare = new KlientEventLyssnare();
            Klient = new NetManager(lyssnare);
        }
        public void Anslut(string ip) {
            Klient.Start();
            Klient.Connect(ip , StandigaVarden.PORTEN , "");
            lyssnare.PeerConnectedEvent += peer => {
                Console.WriteLine("We got connection: {0}" , peer.EndPoint); // Show peer ip
            };
            lyssnare.NetworkReceiveEvent += (fromPeer , dataReader , deliveryMethod) => {
                Console.WriteLine("We got: {0}" , dataReader.GetString(100));
                dataReader.Recycle();
            };
        }
        public override void Update() {
            base.Update();
            Klient.PollEvents();
        }
        public void SickaString(string str) {
            NetDataWriter writer = new NetDataWriter();
            writer.Put(str);
            Klient.SendToAll(writer , DeliveryMethod.Sequenced);
        }
    }
}
