using Nez;
using Nez.UI;

namespace CocaineCrackDown.Scener {
    class ScenTvå : StandardScen {
        private Table table;

        public override Table Table {
            get { return table; }
            set { table = value; }
        }

        public ScenTvå() { }
        public override void Initialize() {
            SetupScene();

            Table.Add(new Label("BRUH jag venne e trot").SetFontScale(5));

            Table.Row().SetPadTop(20);

            Table.Add(new Label("bruhhh").SetFontScale(3));

            Table.Row().SetPadTop(40);

            TextButton playButton = Table.Add(new TextButton("naxt", Skin.CreateDefaultSkin())).SetFillX().SetMinHeight(30).GetElement<TextButton>();

            playButton.OnClicked += PlayButton_onClicked;


            Table.Row().SetPadTop(10);

            TextField textField = Table.Add(new TextField("bruh yext", Skin.CreateDefaultSkin())).SetFillX().SetMinHeight(30).GetElement<TextField>();
            Table.Row().SetPadTop(10);

            CheckBox checkBox = Table.Add(new CheckBox("Bruh ckiucj me", Skin.CreateDefaultSkin())).SetFillX().SetMinHeight(30).GetElement<CheckBox>();
        }

        private void PlayButton_onClicked(Button obj) {
            Core.Scene = new ScenEtt();
        }
    }

}





/*Scene sceneEtt = Scene.CreateWithDefaultRenderer(Color.PaleTurquoise);
Texture2D dougNormal = sceneEtt.Content.Load<Texture2D>("doug");
Entity dougEnhet = sceneEtt.CreateEntity("player", new Vector2(Screen.Width / 2, Screen.Height / 2));
dougEnhet.AddComponent(new SpriteRenderer(dougNormal));
Scene = sceneEtt;*/
