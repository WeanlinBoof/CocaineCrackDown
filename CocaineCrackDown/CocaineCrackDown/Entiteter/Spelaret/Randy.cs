namespace CocaineCrackDown.Entiteter {
    public class Randy : Spelare {
        public Randy(string namn = "randy", bool lokal = false) : base(namn) {
            Lokal = lokal;
        }
    }
}
