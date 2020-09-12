using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

using LiteNetLib;

using Nez;

namespace CocaineCrackDown.Nätverk {
    public class KlientHanterare : GlobalManager {
        KlientEventLyssnare lyssnare;
        NetManager Klient;
        public KlientHanterare() {
            lyssnare = new KlientEventLyssnare();
            lyssnare.NetworkReceiveEvent += (fromPeer , dataReader , deliveryMethod) => {
                Console.WriteLine("We got: {0}" , dataReader.GetString(100));
                dataReader.Recycle();
            };
            Klient = new NetManager(lyssnare);
        }
        public void Anslut(string ip) {
            Klient.Start();
            Klient.Connect(ip , StandigaVarden.PORTEN , "");
        }
        public override void Update() {
            base.Update();
            Klient.PollEvents();
        }
    }
}
