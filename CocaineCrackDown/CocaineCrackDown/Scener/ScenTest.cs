using CocaineCrackDown.Entiteter;

using Microsoft.Xna.Framework;

using Nez;

namespace CocaineCrackDown.Scener {
    public class ScenTest : GrundScen {
        public override void Initialize() {
            base.Initialize();
            //AddEntity(new Spelare("doug"));
            AddEntity(new Karta("testnr1"));
            AddEntity(new Randy());
        }
    }
}
