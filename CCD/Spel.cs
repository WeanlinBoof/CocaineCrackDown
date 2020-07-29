using CocaineCrackDown.Scener;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Nez;
using Nez.Sprites;
using Nez.Console;
using Nez.Farseer;

namespace CocaineCrackDown {
    public class Spel : Core {
        public Spel() : base() {
            //640, 360, false, "Cocaine CrackDown"
        }
        protected override void Initialize() {
            base.Initialize();
            Window.AllowUserResizing = true;
            Scene.CreateWithDefaultRenderer(Color.PeachPuff);
            Scene = new ScenEtt();
            DebugRenderEnabled = true;
            DebugConsole.ConsoleKey = Microsoft.Xna.Framework.Input.Keys.Tab;
            
        }
    }
}
