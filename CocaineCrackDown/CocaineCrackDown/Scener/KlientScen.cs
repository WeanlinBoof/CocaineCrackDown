﻿

using CocaineCrackDown.Nätverk;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Nez;
using Nez.Timers;
using Nez.UI;


using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Text;
using System.Threading;

namespace CocaineCrackDown.Scener {
    public class KlientScen : GrundScen {

        public KlientHanterare KlientHanterare;


        public KlientScen() {

        }
        
        public TextField textField;
        public string ip;
        public override void Initialize() {
            BruhUi();
            KlientHanterare = new KlientHanterare();
            Core.RegisterGlobalManager(KlientHanterare);
            Table.Add(new Label("ip pls").SetFontScale(5));

            Table.Row().SetPadTop(20);
            
            TextFieldStyle textFields = TextFieldStyle.Create(Color.White, Color.White, Color.Black, Color.DarkGray);

            textField = new TextField("", textFields);

            Table.Add(textField);

            Table.Row().SetPadTop(20);

            TextButton KörPå = Table.Add(new TextButton("Klicka" , Skin.CreateDefaultSkin())).SetFillX().SetMinHeight(30).GetElement<TextButton>();
        

            KörPå.OnClicked += TextFält;
        }
        private void TextFält(Button obj) {
            ip = textField.GetText();
            Console.WriteLine(ip);
            KlientHanterare.Anslut(ip);
            Core.StartSceneTransition(new TextureWipeTransition(() => new ScenTest(){Klient = KlientHanterare,IsHost = false}) { TransitionTexture = Core.Content.Load<Texture2D>("nez/textures/textureWipeTransition/wink") });


        }
    }

}
