using System;
using System.Collections.Generic;
using System.Text;

using CocaineCrackDown.Entiteter;
using CocaineCrackDown.Nätverk;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
        public bool ReadyStart;
        private bool otherReady;
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
            /*TextButton Kör = Table.Add(new TextButton("byebye borski" , Skin.CreateDefaultSkin())).SetFillX().SetMinHeight(30).GetElement<TextButton>();
              Kör.OnClicked += Koppplafrån;*/
            Skin skin = Skin.CreateDefaultSkin();
            CheckBox checkbox = Table.Add(new CheckBox("Ready", skin)).GetElement<CheckBox>();
			checkbox.IsChecked = false;
			checkbox.OnChanged += isChecked => { ReadyStart = isChecked; };

        }
         private void bruh(ITimer time){
            if(IsHost) {
                Core.StartSceneTransition(new TextureWipeTransition(() => new ScenTest(true){Server = Server}) { TransitionTexture = Core.Content.Load<Texture2D>("nez/textures/textureWipeTransition/wink") });
            }
            if(!IsHost) {
                Core.StartSceneTransition(new TextureWipeTransition(() => new ScenTest(false){Klient = Klient}) { TransitionTexture = Core.Content.Load<Texture2D>("nez/textures/textureWipeTransition/wink") });

            }
         }
        protected void SickaMeddelade(Button obj) {
            string MeddelandeSicka = textField.GetText();
            if(IsHost) {
                Server.SickaString(MeddelandeSicka);
            }
            if(!IsHost) {
                Klient.SickaString(MeddelandeSicka);
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
                otherReady = Klient.KlientReady; 
            }
            if(!IsHost && MottagetMeddelande != Klient.msg) {
                MottagetMeddelande = Klient.msg;
                Chat = Chat.SetText(MottagetMeddelande);
                otherReady = Server.ServerReady;

            }
            if(ReadyStart) {
                    Console.WriteLine("Ready");

            }
            if(!ReadyStart) {
                    Console.WriteLine("Not Ready");

            }
            if(ReadyStart && otherReady) {             
                Core.Schedule(5,false,bruh);
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