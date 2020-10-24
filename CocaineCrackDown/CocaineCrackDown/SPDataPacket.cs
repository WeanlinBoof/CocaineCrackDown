namespace CocaineCrackDown {
    public class SPDataPacket {
        public SpelarData SpelarData { get; set; }

        public override string ToString() {
            return SpelarData.ToString();
        }
    }
}
