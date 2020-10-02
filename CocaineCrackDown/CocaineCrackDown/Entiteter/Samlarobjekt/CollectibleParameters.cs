
namespace CocaineCrackDown.Entiteter {

    public class CollectibleParameters {
        public string Namn { get; set; }
        internal CollectibleParameters() { }
        public SamlarobjektTyp Typ { get; set; }
        public VapenParameters Vapen { get; set; }
        public float ChansAttTappa { get; set; }
        public Ovanlighet Ovanlighet { get; set; }
    }
}
