using System;

using CocaineCrackDown.Nätverk;

using Nez;

namespace CocaineCrackDown.Hanterare {
    public class KlientSpelareHanterare : GrundSpelarHanterare {
        public KlientSpelareHanterare() {
            NätHanterare = Core.GetGlobalManager<KlientHanterare>();
        }
    }
}
