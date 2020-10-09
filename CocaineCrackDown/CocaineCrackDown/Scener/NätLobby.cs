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
        private TextField textField;
        private TextFieldStyle textFields;
        private TextButton KörPå;
        private Label Chat;
        private TextField Meddelade;
        private float tids = 0f;
        private string Stäng;
        private string Svara;
        private string NyttMeddelade;
        private string MottagetMeddelande;
        private bool IsHost;
        public NätLobby(bool ishost) {
            IsHost = ishost;
        }

        public KlientHanterare KlientHanterare { get; internal set; }
        public VärdHanterare VärdHanterare { get; internal set; }

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
        }

        private void SickaMeddelade(Button obj) {
            string MeddelandeSicka = textField.GetText();
            if(IsHost) {
                VärdHanterare.NetTest(MeddelandeSicka);
            }
            if(!IsHost) {
                KlientHanterare.NetTest(MeddelandeSicka);
            }
            Console.WriteLine(MeddelandeSicka);
        }

        public override void OnStart() {
            base.OnStart();
         
            
        }
        public override void Update() {
            base.Update();
            if(IsHost && MottagetMeddelande != VärdHanterare.msg) {
                MottagetMeddelande = VärdHanterare.msg;
                Chat = Chat.SetText(MottagetMeddelande);
            }
            if(!IsHost && MottagetMeddelande != KlientHanterare.msg) {
                MottagetMeddelande = KlientHanterare.msg;
                Chat = Chat.SetText(MottagetMeddelande);
            }
        }


        protected void Koppplafrån() {

        }
     
        
    }

}