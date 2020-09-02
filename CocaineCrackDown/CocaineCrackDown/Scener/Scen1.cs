
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Nez;
using Nez.Timers;
using Nez.UI;

using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Text;
using System.Threading;

namespace CocaineCrackDown.Scener {
    public class Scen1 : GrundScen {

        public override Table Table { get; set; }

        public Scen1() { }
        
        public override void Initialize() { 
            BruhUi();
            Table.Add(new Label("Main Menu").SetFontScale(5));

            Table.Row().SetPadTop(20);

            Table.Add(new Label("Host Eller Klient?").SetFontScale(2));

            Table.Row().SetPadTop(40);

            TextButton KnappFörVärd = Table.Add(new TextButton("Host" , Skin.CreateDefaultSkin())).SetFillX().SetMinHeight(30).GetElement<TextButton>();

            KnappFörVärd.OnClicked += VärdKnapp;

            Table.Row().SetPadTop(40);

            TextButton KnappFörKlient = Table.Add(new TextButton("Klient" , Skin.CreateDefaultSkin())).SetFillX().SetMinHeight(30).GetElement<TextButton>();

            KnappFörKlient.OnClicked += KlientKnapp;
        }

        private void VärdKnapp(Button obj) {
            Core.StartSceneTransition(new TextureWipeTransition(() => new VärdScen()) { TransitionTexture = Core.Content.Load<Texture2D>("nez/textures/textureWipeTransition/wink") });
        }
        private void KlientKnapp(Button obj) {
            Core.StartSceneTransition(new TextureWipeTransition(() => new KlientScen()) { TransitionTexture = Core.Content.Load<Texture2D>("nez/textures/textureWipeTransition/crissCross") });

        }
    }
}
