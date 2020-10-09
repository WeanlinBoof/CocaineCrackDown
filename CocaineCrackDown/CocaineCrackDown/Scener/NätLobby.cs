using System;
using System.Collections.Generic;
using System.Text;

using CocaineCrackDown.Entiteter;
using CocaineCrackDown.Nätverk;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using Nez;
using Nez.UI;

namespace CocaineCrackDown.Scener {

    public class NätLobby : GrundScen {
        protected TextField textField;
        protected TextFieldStyle textFields;
        private TextButton KörPå;
        private Label Chat;
        private TextField Meddelade;
        protected float tids = 0f;
        private string Stäng;
        private string Svara;
        private string NyttMeddelade;
        protected string MottagetMeddelande;
        protected bool IsHost;
        public NätLobby(bool ishost) {
            IsHost = ishost;
        }
        public KlientHanterare Klient { get; internal set; }
        public VärdHanterare Server { get; internal set; }

        public override void Initialize() {
            BruhUi();

            Chat = new Label("inget mottaget").SetFontScale(3);

            Table.Add(Chat);

            Table.Row().SetPadTop(20);

            textFields = TextFieldStyle.Create(Color.White, Color.White, Color.Black, Color.DarkGray);

            textField = new TextField("", textFields);

            Table.Add(textField);

            Table.Row().SetPadTop(20);

            KörPå = Table.Add(new TextButton("skicka" , Skin.CreateDefaultSkin())).SetFillX().SetMinHeight(30).GetElement<TextButton>();
            KörPå.OnClicked += SickaMeddelade;

            Table.Row().SetPadRight(50);
            
           TextButton Kör = Table.Add(new TextButton("byebye borski" , Skin.CreateDefaultSkin())).SetFillX().SetMinHeight(30).GetElement<TextButton>();
            Kör.OnClicked += Koppplafrån;

        }

        protected void SickaMeddelade(Button obj) {
            string MeddelandeSicka = textField.GetText();
            if(IsHost) {
                Server.NetTest(MeddelandeSicka);
            }
            if(!IsHost) {
                Klient.NetTest(MeddelandeSicka);
            }
            Console.WriteLine(MeddelandeSicka);
        }

        public override void OnStart() {
            base.OnStart();
         
            
        }
        public override void Update() {
            base.Update();
            if(IsHost && MottagetMeddelande != Server.msg) {
                MottagetMeddelande = Server.msg;
                Chat = Chat.SetText(MottagetMeddelande);
            }
            if(!IsHost && MottagetMeddelande != Klient.msg) {
                MottagetMeddelande = Klient.msg;
                Chat = Chat.SetText(MottagetMeddelande);
            }
        }

        protected void Koppplafrån(Button obj) {
            if(IsHost) {
                Server.SetEnabled(false);
            }
            if(!IsHost) {
                Klient.SetEnabled(false);
            }
        }
     
        
    }

}