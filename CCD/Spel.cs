using CocaineCrackDown.Scener;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Nez;
using Nez.Sprites;

namespace CocaineCrackDown {
    public class Spel : Core {
        public Spel() : base() {
            //480,320,false,"CocaineCrackDown"
        }
        protected override void Initialize() {
            base.Initialize();
            Window.AllowUserResizing = true;
            Scene = new ScenEtt();

        }
    }
}
