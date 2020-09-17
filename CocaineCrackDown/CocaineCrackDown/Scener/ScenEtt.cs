using System;
using CocaineCrackDown.Entiteter;
using Nez;

namespace CocaineCrackDown.Scener {
    public class ScenEtt : StandardScenFormat {

        public override void OnStart() {
            base.OnStart();
            Entity doug = CreateEntity("doug");
            doug.AddComponent<Doug>();
        }
    }
}
