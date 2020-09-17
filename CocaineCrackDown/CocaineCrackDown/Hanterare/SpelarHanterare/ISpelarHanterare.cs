using System;

using CocaineCrackDown.Entiteter;
using CocaineCrackDown.Nätverk;

namespace CocaineCrackDown.Hanterare {
    public interface ISpelarHanterare {
        public Spelare LokalaSpelare { get; set; }

        public Spelare[] AndraSpelare { get; set; }

        public Spelare Spelare { get; set; }

        public INätHanterare NätHanterare { get; set; }

        public bool IsHost => Host(NätHanterare);

        public bool Host(INätHanterare nätHanterare);
    }
}
