using System;

using Nez;
using CocaineCrackDown.Entiteter;
using CocaineCrackDown.Nätverk;
using Lidgren.Network;

namespace CocaineCrackDown.Hanterare {
    public class SpelarHanterare : GlobalManager {
        public Spelare LokalaSpelare { get; set; }

        public Spelare[] AndraSpelare { get; set; }

        public Spelare Spelare { get; set; }

        public INätverkHanterare NätHanterare { get; set; }

        public bool IsHost => Host(NätHanterare);

        public bool Host(INätverkHanterare nätHanterare) {
            if(nätHanterare is ServerNätverkHanterare) {
                return true;
            }
            if(nätHanterare is KlientNätverkHanterare) {
                return false;
            }
            if(nätHanterare is null) {
                //singelplayer 

            }
            return false;

        }
//spelare är redan inne
        public Spelare GetPlayer(ulong iD) {
            throw new NotImplementedException();
        }
//inte local
        public Spelare AddPlayer(ulong iD , INätverkHanterare serverNätverkHanterare , bool v) {
            throw new NotImplementedException();
        }
//local
        public Spelare AddPlayer(INätverkHanterare serverNätverkHanterare , bool v) {
            throw new NotImplementedException();
        }
    }
}
