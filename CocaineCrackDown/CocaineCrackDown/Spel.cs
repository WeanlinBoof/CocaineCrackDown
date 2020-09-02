using CocaineCrackDown.Scener;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Nez;
using Nez.Console;

using System;

namespace CocaineCrackDown {
    public class Spel : Core {
        public Spel() {
        }
        protected override void Initialize() {
            //fixar Grafix mer eller mindre
            base.Initialize();
            //Man kan dra kanter för att ändra storlek på spel fönster
            Window.AllowUserResizing = true;
            //fönster titel blir detta
            Window.Title = "Cocaine CrackDown";
            //debug nez console
            DebugRenderEnabled = true;
            //gör knappen  till f10 för att öppna den
            DebugConsole.ConsoleKey = Keys.Tab;
            //RegisterGlobalManager(new NätVärd());
            //lägger scen ett som start scen
            NyScen1();
        }

        private static void NyScen1() {
            Scene = new Scen1();
        }
    }
}
