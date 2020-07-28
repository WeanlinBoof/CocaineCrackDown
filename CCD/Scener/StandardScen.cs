using Nez;
using Nez.UI;
using Nez.Sprites;
using Nez.Textures;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace CocaineCrackDown.Scener {

    public class StandardScen : Scene {

        public StandardScen() {
        }
        public void SetupScene() {
            SetDesignResolution(640, 360, SceneResolutionPolicy.NoBorderPixelPerfect);

            Screen.SetSize(1280, 720);

        }
    }

}

/*Scene sceneEtt = Scene.CreateWithDefaultRenderer(Color.PaleTurquoise);
Texture2D dougNormal = sceneEtt.Content.Load<Texture2D>("doug");
Entity dougEnhet = sceneEtt.CreateEntity("player", new Vector2(Screen.Width / 2, Screen.Height / 2));
dougEnhet.AddComponent(new SpriteRenderer(dougNormal));
Scene = sceneEtt;*/
