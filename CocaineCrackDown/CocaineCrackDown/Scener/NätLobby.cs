﻿using System;
using System.Text;

using CocaineCrackDown.Entiteter;
using CocaineCrackDown.Nätverk;

using LiteNetLib;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using Nez;
using Nez.UI;

namespace CocaineCrackDown.Scener {

    public class NätLobby : GrundScen, IUpdatable {
        public INätHanterare NätHaterare { get; set; }           
        public TextField textField;
        public TextFieldStyle textFields;
        public TextButton KörPå;
        public Label Chat;
        public TextField Meddelade;
        public float tids = 0f;
        public string Stäng { get; set; } = "Stang";
        public string Svara { get; set; } = "Svara";   
        public string NyttMeddelade { get; set; } = "Nytt Meddelade";
        public string MottagetMeddelande { get; set; } = "";
        public string MeddelandeSicka { get; set; } = "";
        public bool Enabled { get; }
        public int UpdateOrder { get; }

        public NätLobby(INätHanterare NH) {
            NätHaterare = NH;   

        }
        public override void Initialize() {
            BruhUi();
            Stäng = "Stang";
            Svara = "Svara";
            NyttMeddelade = "Nytt Meddelade";
            MottagetMeddelande = " ";
            MeddelandeSicka = " ";
            textFields = TextFieldStyle.Create(Color.White , Color.White , Color.Black , Color.Black);
            textField = new TextField(MeddelandeSicka , textFields);
            TextFieldStyle tfs = TextFieldStyle.Create(Color.Black , Color.Black , Color.White , Color.White);
            Meddelade = new TextField(MottagetMeddelande, tfs);
            Chat = new Label("", Skin.CreateDefaultSkin()).SetText(NyttMeddelade).SetFontScale(7.0f);
            Table.Add(Chat);
            Table.Row().SetPadTop(20);
            Table.Add(Meddelade);
            Table.Row().SetPadTop(20);
            Table.Add(textField);
            Table.Row().SetPadTop(20);
            KörPå = Table.Add(new TextButton(Svara , Skin.CreateDefaultSkin())).SetMinHeight(30).GetElement<TextButton>();
            KörPå.OnClicked += TextFält;

        }
        public override void OnStart() {
            base.OnStart();
         
            
        }
        void IUpdatable.Update() {
            base.Update();
            if(MottagetMeddelande != NätHaterare.MottagenString) {
                MottagetMeddelande = NätHaterare.MottagenString;
                Meddelade.SetText(MottagetMeddelande);
                
            }
        }
        protected void Koppplafrån() {

        }

        private void TextFält(Button obj) {
            MeddelandeSicka = textField.GetText();
            NätHaterare.SickaString(MeddelandeSicka);
            Console.WriteLine(MeddelandeSicka);
        }
    }

}