using System;
using System.Collections.Generic;
using System.Text;

using CocaineCrackDown.Entiteter;
using CocaineCrackDown.Nätverk;

using Lidgren.Network;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using Nez;
using Nez.UI;

namespace CocaineCrackDown.Scener {

    public class NätLobby : GrundScen {
        public INätverkHanterare NätHaterare { get; set; }
        public TextField textField;
        public TextFieldStyle textFields;
        public TextButton KörPå;
        public Label Chat;
        public TextField Meddelade;
        public float tids = 0f;
        public string Stäng;
        public string Svara;
        public string NyttMeddelade;
        public string MottagetMeddelande;
        public string MeddelandeSicka;
        public SpelarData SpelarUtData;
        public SpelarData SpelarInData;
        public List<SpelarData> SpelarIDLista;
        public Randy Randy;
        public Doug Doug;
        bool IsHost;
        public NätLobby(INätverkHanterare NH,bool ishost) {
            NätHaterare = NH;
            SpelarIDLista = new List<SpelarData>();
            IsHost = ishost;
        }
        public override void Initialize() {
            BruhUi();
            
            AddEntity(new TiledMap("testnr1"));
            if(IsHost) {
                Doug = AddEntity(new Doug());
                Doug.LokalSpelare = true;
                SpelarUtData = new SpelarData(1,Doug.Name,Doug.Position,Doug.atlasAnimationsKomponent.Attackerar);
                Doug.LokalSpelarData = SpelarUtData;
            }
            if(!IsHost) {
                Randy = AddEntity(new Randy());
                Randy.LokalSpelare = true;
                SpelarUtData = new SpelarData(2,Randy.Name,Randy.Position,Randy.atlasAnimationsKomponent.Attackerar);
                Randy.LokalSpelarData = SpelarUtData;

                
            }
        }
        public override void OnStart() {
            base.OnStart();
         
            
        }
        public override void Update() {
            base.Update();
            NätHaterare.SkickaMeddelande(SpelarUtData);
            if(NätHaterare.InkomandeData != null) {
                if(FindEntity("randy") != null && FindEntity("doug") != null ) {
                    UppdateraInkomandeDataSpelar(NätHaterare.InkomandeData);
                }
                else {
                    AddSpelare(NätHaterare.InkomandeData);
                }
            }
            
        }

        private void UppdateraInkomandeDataSpelar(SpelarData spelarData) {
            if(IsHost) {
                Randy.NySpelarData = spelarData;
            }
            if(!IsHost) {
                Doug.NySpelarData = spelarData;
            }
        }

        protected void Koppplafrån() {

        }
        private void AddSpelare(SpelarData spelarData){
            if(IsHost) {
                Randy = AddEntity(new Randy(spelarData));
                Randy.LokalSpelare = false;
            }
            if(!IsHost) {
                Doug = AddEntity(new Doug(spelarData));
                Doug.LokalSpelare = false;
            }
        }
    }

}