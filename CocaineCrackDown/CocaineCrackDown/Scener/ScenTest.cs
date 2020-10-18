using CocaineCrackDown.Entiteter;
using CocaineCrackDown.Nätverk;

using Microsoft.Xna.Framework;

using Nez;

namespace CocaineCrackDown.Scener {
    public class ScenTest : GrundScen {  
        public KlientHanterare Klient { get; internal set; }
        public VärdHanterare Server { get; internal set; }
        public bool IsHost { get; internal set; }
        public override void Initialize() {
            base.Initialize();
            AddEntity(new TiledMap("testnr1"));

            if(IsHost) {
                AddEntity(new Doug());
            }
            else {
                AddEntity(new Randy());
            }
        }
    }
}
