using LiteNetLib.Utils;
using LiteNetLib;

namespace CocaineCrackDown.Nätverk {
    public interface INätHanterare {
        public EventBasedNetListener Lyssnare { get; set; }
        public NetManager Hanterare { get; set; }
        void SickaString(string str);
        void Anslut(string ip);
    }
    
}
