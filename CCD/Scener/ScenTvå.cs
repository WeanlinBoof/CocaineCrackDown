using Nez;
using Nez.UI;

namespace CocaineCrackDown.Scener {
    class ScenTvå : StandardScen {

        public ScenTvå() { }
        public override void Initialize() {
            SetupScene();


        }

    }

}





/*Scene sceneEtt = Scene.CreateWithDefaultRenderer(Color.PaleTurquoise);
Texture2D dougNormal = sceneEtt.Content.Load<Texture2D>("doug");
Entity dougEnhet = sceneEtt.CreateEntity("player", new Vector2(Screen.Width / 2, Screen.Height / 2));
dougEnhet.AddComponent(new SpriteRenderer(dougNormal));
Scene = sceneEtt;*/
