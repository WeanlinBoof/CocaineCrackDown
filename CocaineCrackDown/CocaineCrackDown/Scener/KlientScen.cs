﻿
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

        

        public KlientScen() { }
        
        public TextField textField;
        

        public string ip = "";
        public override void Initialize() { 


            BruhUi();

            Table.Add(new Label("ok").SetFontScale(5));

            Table.Row().SetPadTop(20);
            
            TextFieldStyle textFields = TextFieldStyle.Create(Color.White, Color.White, Color.Black, Color.DarkGray);

            textField = new TextField("", textFields);

            Table.Add(textField);

            Table.Row().SetPadTop(20);

            TextButton KörPå = Table.Add(new TextButton("Klicka" , Skin.CreateDefaultSkin())).SetFillX().SetMinHeight(30).GetElement<TextButton>();
        

            KörPå.OnClicked += TextFält;



        }


       /* public override void Update() {
            base.Update();

            KeyboardState state = Keyboard.GetState();

            if(state.IsKeyDown(Keys.Enter)){
                TextFält(textField);
            }

        } */

        private void TextFält(Button obj){
            ip = textField.GetText();
            
            Core.Scene = new Scen1();

            Console.WriteLine(ip);
        }
    }
}