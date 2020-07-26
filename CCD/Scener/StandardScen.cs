using Nez;
using Nez.UI;
using Nez.Sprites;
using Nez.Textures;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace CocaineCrackDown.Scener {

    public abstract class StandardScen : Scene {
        public abstract Table Table { get; set; }

        public StandardScen() {
        }
        public void SetupScene() {
            SetDesignResolution(640, 360, SceneResolutionPolicy.NoBorderPixelPerfect);

            Screen.SetSize(1280, 720);

            UICanvas UICanvas = CreateEntity("ui-canvas").AddComponent(new UICanvas());

            Table = UICanvas.Stage.AddElement(new Table());

            Table.SetFillParent(true).Top().PadLeft(50).PadTop(50);

        }
    }

}

/*Scene sceneEtt = Scene.CreateWithDefaultRenderer(Color.PaleTurquoise);
Texture2D dougNormal = sceneEtt.Content.Load<Texture2D>("doug");
Entity dougEnhet = sceneEtt.CreateEntity("player", new Vector2(Screen.Width / 2, Screen.Height / 2));
dougEnhet.AddComponent(new SpriteRenderer(dougNormal));
Scene = sceneEtt;*/
