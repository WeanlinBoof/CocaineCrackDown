using CocaineCrackDown.Entiteter;

namespace CocaineCrackDown.Verktyg.Events {
    public class SpelareDödadEventParameter : SpelEventParameter {
        public Spelare Killed { get; set; }
        public Spelare Killer { get; set; }
    }

}
