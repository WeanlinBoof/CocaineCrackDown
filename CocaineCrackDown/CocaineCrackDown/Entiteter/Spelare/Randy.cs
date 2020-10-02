namespace CocaineCrackDown.Entiteter {
    public class Randy : Spelare {
        public Randy(string namn = "randy") : base(namn) {
        }

        public Randy(SpelarData spelarData , string namn = "randy") : base(spelarData , namn) {
        }
    }
}
