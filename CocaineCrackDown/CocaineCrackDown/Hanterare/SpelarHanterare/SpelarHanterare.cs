using System;

using Nez;
using CocaineCrackDown.Entiteter;
using CocaineCrackDown.Nätverk;

namespace CocaineCrackDown.Hanterare {
    public class GrundSpelarHanterare : GlobalManager, ISpelarHanterare {
        public Spelare LokalaSpelare { get; set; }

        public Spelare[] AndraSpelare { get; set; }

        public Spelare Spelare { get; set; }

        public INätHanterare NätHanterare { get; set; }

        public bool IsHost => Host(NätHanterare);

        public bool Host(INätHanterare nätHanterare) {
            if(nätHanterare is VärdHanterare) {
                return true;
            }
            if(nätHanterare is KlientHanterare) {
                return false;
            }
            if(nätHanterare is null) {
                //singelplayer 

            }
            return false;

        }
    }
}
