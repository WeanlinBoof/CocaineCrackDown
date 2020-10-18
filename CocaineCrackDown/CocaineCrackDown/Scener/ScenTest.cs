using CocaineCrackDown.Entiteter;
using CocaineCrackDown.Nätverk;

using Microsoft.Xna.Framework;

using Nez;

namespace CocaineCrackDown.Scener {
    public class ScenTest : GrundScen {  
        public ScenTest(bool ishost){
            IsHost = ishost;
        }
        public KlientHanterare Klient { get; internal set; }
        public VärdHanterare Server { get; internal set; }
        protected bool IsHost;
        public override void Initialize() {
            base.Initialize();
            AddEntity(new TiledMap("testnr1"));

            if(IsHost) {
                AddEntity(new Doug());
            }

            if(!IsHost) {
                AddEntity(new Randy());
            }
        }
    }
}
