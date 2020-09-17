using System;

using CocaineCrackDown.Nätverk;

using Nez;
namespace CocaineCrackDown.Hanterare {
    public class ServerSpelarHanterare : GrundSpelarHanterare {
        public ServerSpelarHanterare() {
         NätHanterare = Core.GetGlobalManager<VärdHanterare>();

        }
    }

}
