using LiteNetLib;

namespace CocaineCrackDown.Nätverk {
    public interface INätHanterare {
        EventBasedNetListener Lyssnare { get; set; }
        NetManager Hanterare{ get; set; }
        void SickaString(string str);
        void Anslut(string ip);
        public string MottagenString { get; set; }
    }
    
}
