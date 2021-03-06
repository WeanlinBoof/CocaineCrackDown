﻿using Microsoft.Xna.Framework.Input;

using Nez;

namespace CocaineCrackDown.Client.Managers {
    /// <summary>
    /// Inmatnings Hanteraren
    /// </summary>
    public class InmatningsHanterare : GlobalManager {
        /// <summary>
        /// AvändKnapp
        /// </summary>
        public VirtualButton AvändKnapp { get;  set; }
        /// <summary>
        /// AttackKnapp
        /// </summary>
        public VirtualButton AttackKnapp { get;  set; }
        /// <summary>
        /// HoppKnapp
        /// </summary>
        public VirtualButton HoppKnapp { get;  set; }
        /// <summary>
        /// UppKnapp
        /// </summary>
        public VirtualButton UppKnapp { get;  set; }
        /// <summary>
        /// HögerKnapp
        /// </summary>
        public VirtualButton HögerKnapp { get;  set; }
        /// <summary>
        /// VänsterKnapp
        /// </summary>
        public VirtualButton VänsterKnapp { get;  set; }
        /// <summary>
        /// NedKnapp
        /// </summary>
        public VirtualButton NedKnapp { get;  set; }
        /// <summary>
        /// RörelseAxelX
        /// </summary>
        public VirtualIntegerAxis RörelseAxelX { get; set; }
        /// <summary>
        /// RörelseAxelY
        /// </summary>
        public VirtualIntegerAxis RörelseAxelY { get;  set; }
        /// <summary>
        /// Välj Knappen
        /// </summary>       
        public VirtualButton VäljKnapp { get;  set; }
        /// <summary>
        /// Blockerar All Interaktion/Använd Typ Av Saker
        /// </summary>
        public bool ÄrUpptagen { get; set; }
        /// <summary>
        /// Blockerar All Inmatning 
        /// </summary>
        public bool ÄrLåst { get; set; }

        public InmatningsHanterare() {
            AvändKnapp = new VirtualButton();
            AvändKnapp.AddKeyboardKey(Keys.A).AddKeyboardKey(Keys.Enter).AddGamePadButton(0, Buttons.A);

            AttackKnapp = new VirtualButton();
            AttackKnapp.AddKeyboardKey(Keys.Z).AddGamePadButton(0, Buttons.X);

            HoppKnapp = new VirtualButton();
            HoppKnapp.AddKeyboardKey(Keys.X).AddGamePadButton(0, Buttons.A);

            UppKnapp = new VirtualButton();
            UppKnapp.AddKeyboardKey(Keys.Up).AddGamePadButton(0, Buttons.DPadUp);

            HögerKnapp = new VirtualButton();
            HögerKnapp.AddKeyboardKey(Keys.Right).AddGamePadButton(0, Buttons.DPadRight);

            VänsterKnapp = new VirtualButton();
            VänsterKnapp.AddKeyboardKey(Keys.Left).AddGamePadButton(0, Buttons.DPadLeft);

            NedKnapp = new VirtualButton();
            NedKnapp.AddKeyboardKey(Keys.Down).AddGamePadButton(0, Buttons.DPadDown);

            RörelseAxelX = new VirtualIntegerAxis();
            RörelseAxelX.AddKeyboardKeys(VirtualInput.OverlapBehavior.TakeNewer, Keys.Left, Keys.Right).AddGamePadLeftStickX().AddGamePadDPadLeftRight();

            RörelseAxelY = new VirtualIntegerAxis();
            RörelseAxelY.AddKeyboardKeys(VirtualInput.OverlapBehavior.TakeNewer, Keys.Up, Keys.Down).AddGamePadLeftStickY().AddGamePadDPadUpDown();

            VäljKnapp = new VirtualButton();
            VäljKnapp.AddKeyboardKey(Keys.Enter).AddGamePadButton(0, Buttons.A).AddGamePadButton(0, Buttons.Start);
        }

        public bool ÄrRörelseTillgänglig() {
            return !ÄrUpptagen && !ÄrLåst;
        }

        public override void Update() { }
    }
}
