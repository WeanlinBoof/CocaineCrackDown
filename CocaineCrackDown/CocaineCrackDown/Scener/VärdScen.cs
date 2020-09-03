﻿
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Nez;
using Nez.Timers;
using Nez.UI;

using RedGrin;

using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Text;
using System.Threading;

namespace CocaineCrackDown.Scener {
    public class VärdScen : GrundScen {

        public override Table Table { get; set; }

        public VärdScen() { }     
        public override void Initialize() { 
            BruhUi();
            Table.Add(new Label("ok").SetFontScale(5));

            Table.Row().SetPadTop(20);

            TextButton KörPå = Table.Add(new TextButton("Klicka" , Skin.CreateDefaultSkin())).SetFillX().SetMinHeight(30).GetElement<TextButton>();


            KörPå.OnClicked += TextFält;
        }
        private void TextFält(Button obj) {

            NetworkManager.Self.Initialize(Konfig);
            NetworkManager.Self.Start(NetworkRole.Server);
            Core.StartSceneTransition(new TextureWipeTransition(() => new NätLobby()) { TransitionTexture = Core.Content.Load<Texture2D>("nez/textures/textureWipeTransition/wink") });


        }
    }
    }

