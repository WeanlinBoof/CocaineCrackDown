using System;
using System.Collections.Generic;

using CocaineCrackDown.Entiteter;

using LiteNetLib;

namespace CocaineCrackDown.Hanterare {

        //TODO FIX
    public class SpelHanterare {
        public Dictionary<NetPeer , Spelare> SpelarLista = new Dictionary<NetPeer , Spelare>();

        public void JoinGame(NetPeer peer , Spelare spelare) {
            throw new NotImplementedException();
        }

        public void LeaveGame(NetPeer peer, Spelare spelare) {
            throw new NotImplementedException();
            //pollevent medelar hosten att en spelare lämnade   
        }

        public void LäggTillSpelare(NetPeer peer , Spelare otherSpelare) {
            SpelarLista.Add(peer , otherSpelare);
        }
        //referera denna inom pollevents i värd hanterare
        public void TabortSpelare(NetPeer peer) {
            SpelarLista[peer].Destroy();
            SpelarLista.Remove(peer);
        }
    }
}