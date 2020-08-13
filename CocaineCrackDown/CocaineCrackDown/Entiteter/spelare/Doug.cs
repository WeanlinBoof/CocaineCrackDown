using CocaineCrackDown.Komponenter;
using CocaineCrackDown.Komponenter.Spelare;

using Nez;

using System;
using System.Collections.Generic;
using System.Text;

namespace CocaineCrackDown.Entiteter.spelare {
   public class Doug : Entity{

        public override void OnAddedToScene() {
            base.OnAddedToScene();
            Name = "Doug";
            AddComponent<DougSpelareEtt>();

        }


    }
}
