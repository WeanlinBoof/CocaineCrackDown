using Nez;
using Nez.UI;

namespace CocaineCrackDown.Scener {
    class ScenEtt : StandardScen {
        private Table table;

        public override Table Table {
            get { return table; }
            set { table = value; }
        }

        public ScenEtt() { }

        public override void Initialize() {
            SetupScene();

            Table.Add(new Label("Cocaine Crack Down").SetFontScale(5));

            Table.Row().SetPadTop(20);

            Table.Add(new Label("woooooo").SetFontScale(3));

            Table.Row().SetPadTop(40);

            TextButton playButton = Table.Add(new TextButton("Spela", Skin.CreateDefaultSkin())).SetFillX().SetMinHeight(30).GetElement<TextButton>();

            playButton.OnClicked += PlayButton_onClicked;
        }

        private void PlayButton_onClicked(Button obj) {
            Core.Scene = new ScenTvå();
        }
    }

}





/*Scene sceneEtt = Scene.CreateWithDefaultRenderer(Color.PaleTurquoise);
Texture2D dougNormal = sceneEtt.Content.Load<Texture2D>("doug");
Entity dougEnhet = sceneEtt.CreateEntity("player", new Vector2(Screen.Width / 2, Screen.Height / 2));
dougEnhet.AddComponent(new SpriteRenderer(dougNormal));
Scene = sceneEtt;*/
