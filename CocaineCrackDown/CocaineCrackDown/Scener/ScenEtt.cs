using CocaineCrackDown.Entiteter.spelare;
using CocaineCrackDown.Komponenter;

using Microsoft.Xna.Framework;

using Nez;
using Nez.Timers;

using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Text;
using System.Threading;

namespace CocaineCrackDown.Scener {
    public class ScenEtt : GrundScen {
        public Doug spelareEtt;
        public ScenEtt() : base() { }


        public override void Initialize() {
            //ta ej bort 
            SeneStandard();
            AddEntity(EntitetsFabrik.CreateEntity(EntitetTyp.Himmel));
            AddEntity(EntitetsFabrik.CreateEntity(EntitetTyp.Bakgrund));
            AddEntity(EntitetsFabrik.CreateEntity(EntitetTyp.Mark));
            spelareEtt = AddEntity(new Doug() { Position = new Vector2(SceneRenderTargetSize.X / 2,SceneRenderTargetSize.Y / 2) });
            //Core.Schedule(1, !död, this, TestaerHälsoPoäng);
            //Core.Schedule(1, !död, this, TestaerHälsoPoängskada);

        }
        public override void Update() {
            base.Update();
 

        }



    }
}
