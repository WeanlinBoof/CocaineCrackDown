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
        private INätHanterare NätHaterare;       
        private string Chat = "Nytt Meddelade";
        public string MottagetMeddelande = "inget just nu";
        public TextField textField;
        public string MeddelandeSicka = "";
        public TextFieldStyle textFields;
        public TextButton KörPå;
        public string Stäng = "Stang";
        public string Svara = "Svara";
        public float tids = 0f;
        public NätLobby(INätHanterare NH) {
            NätHaterare = NH; 
        }
        public override void Initialize() {
            base.Initialize();

            //AddEntity(new TiledMap("testnr1"));
            //AddEntity(new Randy());

            BruhUi();

            textFields = TextFieldStyle.Create(Color.White , Color.Yellow , Color.DimGray , Color.Transparent);

            textField = new TextField(MeddelandeSicka , textFields);
            ShowDialog(Chat, MottagetMeddelande, Stäng,Svara);
        }
        public override void OnStart() {
            base.OnStart();
         
            
        }
        public override void Update() {
            base.Update();

            if(MottagetMeddelande != null) { 
                MottagetMeddelande = NätHaterare.MottagenString;
                ShowDialog(Chat, MottagetMeddelande, Stäng,Svara);
                tids = Time.DeltaTime;
                if(tids > 0.5f) {
                    MottagetMeddelande = null;
                    tids = 0f;

                }

            }
            
            
        }
        protected void Koppplafrån() {

        }


         
    public Dialog ShowDialog(string title, string MeddelandeText, string Stäng,string Svara)
	{
            Skin skin = Skin.CreateDefaultSkin();

            WindowStyle style = new WindowStyle
			{
				Background = new PrimitiveDrawable(new Color(50, 50, 50)),
				StageBackground = new PrimitiveDrawable(new Color(0, 0, 0, 150))
			};

            Dialog dialog = new Dialog(title, style);
			dialog.GetTitleLabel().GetStyle().Background = new PrimitiveDrawable(new Color(55, 100, 100));
			dialog.Pad(20, 5, 5, 5);
			dialog.AddText(MeddelandeText);
            dialog.AddButton(new TextButton(Stäng , skin)).OnClicked += butt => StängUtanSvar(butt , dialog); 
            dialog.Add(textField);
            dialog.AddButton(new TextButton(Svara , skin)).OnClicked += TextFält;
			dialog.Show(Table.GetStage());

			return dialog;
		}

        private static void StängUtanSvar(Button butt , Dialog dialog) {
            dialog.Hide();
        }

        private void TextFält(Button obj) {
            MeddelandeSicka = textField.GetText();
            NätHaterare.SickaString(MeddelandeSicka);
            Console.WriteLine(MeddelandeSicka);
        }
    }

}