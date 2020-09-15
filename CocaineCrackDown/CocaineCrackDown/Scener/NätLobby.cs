using System;
using System.Text;

using CocaineCrackDown.Entiteter;
using CocaineCrackDown.Nätverk;

using LiteNetLib;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using Nez;
using Nez.UI;

namespace CocaineCrackDown.Scener {

    public class NätLobby : GrundScen {
        INätHanterare NätHaterare;
        public NätLobby(INätHanterare NH) {
            NätHaterare = NH; 
        }
        public override void Initialize() {
            base.Initialize();

            AddEntity(new TiledMap("testnr1"));
            AddEntity(new Randy());

            BruhUi();

            Table.Add(new Label("ok").SetFontScale(5));

            Table.Row().SetPadTop(20);

            TextFieldStyle textFields = TextFieldStyle.Create(Color.White , Color.White , Color.Black , Color.DarkGray);

            textField = new TextField("" , textFields);

            Table.Add(textField);

            Table.Row().SetPadTop(20);

            TextButton KörPå = Table.Add(new TextButton("Klicka" , Skin.CreateDefaultSkin())).SetFillX().SetMinHeight(30).GetElement<TextButton>();


            KörPå.OnClicked += TextFält;
        }
        public override void OnStart() {
            base.OnStart();
         
            
        }
        public override void Update() {
            base.Update();
        
        }
        protected void Koppplafrån() {
        }
        public TextField textField;
        public string text;

        private void TextFält(Button obj) {
            text = textField.GetText();
            Console.WriteLine(text);
            NätHaterare.SickaString(text);
        }
    }
}