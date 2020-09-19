using System;
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
        public NätLobby(INätverkHanterare NH) {
            NätHaterare = NH;
        }
        public override void Initialize() {
            BruhUi();


        }
        public override void OnStart() {
            base.OnStart();
         
            
        }
        protected void Koppplafrån() {

        }
    }

}