namespace CocaineCrackDown.Entiteter {
    public class Doug : Spelare {
        public Doug(string namn = "doug") : base(namn) {
        }

        public Doug(SpelarData spelarData , string namn = "doug") : base(spelarData , namn) {
        }
    }
}
