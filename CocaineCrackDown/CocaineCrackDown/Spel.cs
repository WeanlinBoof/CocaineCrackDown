using CocaineCrackDown.Entiteter.spelare;
using CocaineCrackDown.Scener;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Nez;
using Nez.Console;

using System;
using CocaineCrackDown.Komponenter.Spelare;
using CocaineCrackDown.Komponenter;

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
            DebugConsole.ConsoleKey = Keys.F10;
            //lägger scen ett som start scen
            NewScenee1();

        }


        private static void NewScenee1() {
            Scene = new ScenEtt();
        }
    }
}
