﻿

using CocaineCrackDown.Nätverk;

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
    public class VärdScen : GrundScen {

        public override Table Table { get; set; }
        public VärdHanterare VH;
        public VärdScen() {

        }
        public override void Initialize() {
            VH = new VärdHanterare();
            Core.RegisterGlobalManager(VH);
            BruhUi();
            Table.Add(new Label("ok").SetFontScale(5));

            Table.Row().SetPadTop(20);

            TextButton KörPå = Table.Add(new TextButton("Klicka" , Skin.CreateDefaultSkin())).SetFillX().SetMinHeight(30).GetElement<TextButton>();


            KörPå.OnClicked += TextFält;
        }
        private void TextFält(Button obj) {
            VH.Anslut();
            Core.StartSceneTransition(new TextureWipeTransition(() => new NätLobby(VH){NätHaterare = VH}) { TransitionTexture = Core.Content.Load<Texture2D>("nez/textures/textureWipeTransition/wink") });
        }
    }
}

