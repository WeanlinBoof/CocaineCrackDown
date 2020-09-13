using CocaineCrackDown.Entiteter;

namespace CocaineCrackDown.Scener {
    public class ScenTest : GrundScen {
        public override void Initialize() {
            base.Initialize();
            //AddEntity(new Spelare("doug"));
            AddEntity(new TiledMap());
        }
    }
}
