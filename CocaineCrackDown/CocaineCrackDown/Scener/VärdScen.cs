
using CocaineCrackDown.Hanterare.Nät;

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
        public ServerNätverkHanterare SNH;
        public VärdScen() { }     
        public override void Initialize() {
            SNH = new ServerNätverkHanterare();
            BruhUi();
            Table.Add(new Label("ok").SetFontScale(5));

            Table.Row().SetPadTop(20);

            TextButton KörPå = Table.Add(new TextButton("Klicka" , Skin.CreateDefaultSkin())).SetFillX().SetMinHeight(30).GetElement<TextButton>();


            KörPå.OnClicked += TextFält;
        }
        private void TextFält(Button obj) {
            SNH.Enabled = true;
            SNH.Anslut();

            Core.StartSceneTransition(new TextureWipeTransition(() => new NätLobby(SNH)) { TransitionTexture = Core.Content.Load<Texture2D>("nez/textures/textureWipeTransition/wink") });


        }
    }
    }

